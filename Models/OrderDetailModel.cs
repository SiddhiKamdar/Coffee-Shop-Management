using System.ComponentModel.DataAnnotations;

namespace CoffeeShopManagment.Models
{
    public class OrderDetailModel
    {
        [Required(ErrorMessage = "This Field is Compulsory")]
        public int OrderDetailID { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        public int OrderID { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        public int ProductID { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        public decimal TotalAmount { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        public int UserID { get; set; }
    }
}
