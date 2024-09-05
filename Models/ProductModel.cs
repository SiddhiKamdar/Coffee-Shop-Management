using System.ComponentModel.DataAnnotations;

namespace CoffeeShopManagment.Models
{
    public class ProductModel
    {
        public int? ProductId { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        public decimal ProductPrice { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        public string ProductCode { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        public string Description { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        public int UserId { get; set; }
    }
    public class ProductDropDownModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
    }

}
