using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Models
{
    //[PrimaryKey(nameof(UserId) ,nameof(SubscribtionId))]
    public class UserSubscribtion
    {

        [Key, DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.None)]
        public string CustomerId { get; set; }
        public string SubscriptionId { get; set; }
        [Required]
        public string Status { get; set; }
        public DateTime? EndDate { get; set; } 
       
    }
}
