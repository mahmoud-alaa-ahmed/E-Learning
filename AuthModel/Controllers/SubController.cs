using DataLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTO.Subscribtion;
using ModelLayer.Models;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace AuthModel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class SubController : ControllerBase
    {
        private readonly IAuth _auth;
        private readonly IUnit _unit;
        private readonly IConfiguration _con;

        public SubController(IConfiguration con,IAuth auth,IUnit unit)
        {
            _auth = auth;
            _unit = unit;
            _con=con;
            StripeConfiguration.ApiKey = _con.GetSection("StripeKeys:SK").Value;
        }
        [Authorize]
        [HttpPost("checkout")]
         public async Task<IActionResult> CreateCheckOut([FromBody]CheckoutRequest req)
        {
           

            var options = new SessionCreateOptions
            {
               
                SuccessUrl = "http://localhost:4200/paymentproceed",
                CancelUrl = "http://localhost:4200/",
                Mode = "subscription",
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Price = req.SubscriptionId,
                            // For metered billing, do not pass quantity
                         Quantity = 1,
                    },
                },
            };

            var service = new SessionService();
            try
            {
                var session = await service.CreateAsync(options);
                return Ok(new { SessionId = session.Id });
            }catch(StripeException ex) 
            {
                return BadRequest(ex.StripeError.Message);
            }
           

        }
        [Authorize]
        [HttpPost("user")]
        public async Task<IActionResult> UserServices([FromBody] UserServiceRequest req )
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user=await _auth.FindUserAsync(userId);
            if (user == null)
                return NotFound(new { message = "Invalid User" });
            try
            {
                var options = new Stripe.BillingPortal.SessionCreateOptions
                {
                    Customer = user.CustomerId,
                    ReturnUrl = req.ReturnUrl,
                };
                var service = new Stripe.BillingPortal.SessionService();
                var session = await service.CreateAsync(options);
                
                return Ok(new {url=session.Url});
            }catch(StripeException ex)
            {
                return BadRequest(ex.StripeError.Message);
            }
        }
        
        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook()
        {
            
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            
              
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                  json,
                 Request.Headers["Stripe-Signature"],
                 _con.GetSection("StripeKeys:WH").Value
                 );

                // Handle the event
                if (stripeEvent.Type == Events.CustomerSubscriptionCreated)
                {
                    var subscription = stripeEvent.Data.Object as Subscription;
                    if (subscription is not null)
                    {
                        var Usersubscription = new UserSubscribtion()
                        {
                            SubscriptionId = subscription.Id,
                            CustomerId = subscription.CustomerId,
                            Status = subscription.Status,
                            EndDate = subscription.CurrentPeriodEnd
                        };
                        await _unit.UserSubscribtion.AddAsync(Usersubscription);
                        await _unit.SaveDataAsync();
                        return Ok("Created");    
                    }
                }
                else if (stripeEvent.Type == Events.CustomerSubscriptionUpdated)
                {
                    var subSession = stripeEvent.Data.Object as Stripe.Subscription;
                    if (subSession is null)
                        return BadRequest();
                    var userSubscripe = await _unit.UserSubscribtion.GetByIdAsync(user => user.SubscriptionId == subSession.Id);
                    if (userSubscripe is null)
                        return NotFound(new { message = "User subscription not found to update!" });
                     userSubscripe.Status = subSession.Status;
                    userSubscripe.EndDate = subSession.CurrentPeriodEnd;
                    _unit.UserSubscribtion.Edit(userSubscripe);
                   await _unit.SaveDataAsync();
                }
                else if (stripeEvent.Type == Events.CustomerCreated)
                {
                    var customer = stripeEvent.Data.Object as Customer;
                    if(customer is null)
                        return BadRequest();
                    var user = await _auth.FindUserByEmailAsync(customer.Email);
                    if (user is null)
                        return NotFound("Invalid Email or User");
                    user.CustomerId = customer.Id;
                    var update = await _auth.UpdataUserData(user);
                    if (!update.Succeeded)
                        return BadRequest(new { message = "failed to update user data" });
                    return Ok(new { message = "User data has been Updated!" });
                }
                // ... handle other event types
                else
                {
                    // Unexpected event type
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }
                return Ok();
            }
            catch (StripeException e)
            {
                Console.WriteLine(e.StripeError.Message);
                return BadRequest();
            }
        }

        [HttpGet("test")]
        public IActionResult Getprodct()
        {
            StripeConfiguration.ApiKey = "sk_test_51NMXHVCJZ3JWdwv6vCl00HQBYqqRFLNCLvKJHWl5Iv43PtYeiexIHCIIXx6lhKGTHcsbIPiwgD6HyZemNBM497OA00BMPBHlUZ";

            var options = new ProductListOptions
            {
                Limit = 3,
            };
            var service = new ProductService();
            StripeList<Product> products = service.List(
              options);
            return Ok(products);
        }


    }
}
