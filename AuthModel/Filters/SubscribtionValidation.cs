using DataLayer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace AuthModel.Filters
{
    public class SubscribtionValidation : ActionFilterAttribute
    {
        public IUnit unitOfWork { get; set; }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                // UserId not found in the claim
                context.Result = new UnauthorizedResult();
                return;
            }

            // Check if the user has a valid subscription
            //var userSubscription = await unitOfWork.UserSubscribtion.FindAnyAsync(a => a.UserId == userId);

            //if (userSubscription == null || userSubscription.EndDate > DateTime.Now)
            //{
            //    // User does not have a valid subscription or no subscription found
            //    context.Result = new ObjectResult("Invalid subscription or user not subscribed.")
            //    {
            //        StatusCode = (int)HttpStatusCode.Forbidden
            //    };
            //    return;
            //}

            // Continue with the action execution
        
            await base.OnActionExecutionAsync(context, next);
        }
     

    }


}
