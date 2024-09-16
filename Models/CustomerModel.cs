using System.ComponentModel.DataAnnotations;

namespace CoffeeShopManagment.Models
{
    public class CustomerModel
    {
        public int CustomerID { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        public string HomeAddress { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        [Phone]
        public string MobileNo { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        public string GSTNo { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        public string CityName { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        public string PinCode { get; set; }
        [Required]
        public decimal NetAmount { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        public int UserID { get; set; }
    }
    public class CustomerDropDownModel
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
    }
}
