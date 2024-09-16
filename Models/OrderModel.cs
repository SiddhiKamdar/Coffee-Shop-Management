using System.ComponentModel.DataAnnotations;

namespace CoffeeShopManagment.Models
{
    public class OrderModel
    {
        [Required(ErrorMessage = "This Field is Compulsory")]
        public int OrderID { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        public DateTime OrderDate { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        public int CustomerID { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        public string PaymentMode { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        public decimal TotalAmount { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        public string ShippingAddress { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        public int UserID { get; set; }
    }
    public class OrderDropDownModel
    {
        public int OrderID { get; set; }
    }

}
