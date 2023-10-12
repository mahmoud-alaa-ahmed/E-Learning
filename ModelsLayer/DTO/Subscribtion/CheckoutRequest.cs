using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTO.Subscribtion
{
    public class CheckoutRequest
    {
        [Required] 
        public string SubscriptionId { get; set; }
    }
}
