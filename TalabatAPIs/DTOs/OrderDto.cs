using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities.Order_Aggregate;

namespace TalabatAPIs.DTOs
{
    public class OrderDto
    {
        
        [Required]
        public string BasketId { get; set; }
        [Required]
        public int DelivertMethodId { get; set; }
        [Required]
        public AddressDto ShippingAddress { get; set; }
    }
}
