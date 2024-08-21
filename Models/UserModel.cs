using System.ComponentModel.DataAnnotations;

namespace CoffeeShopManagment.Models
{
    public class UserModel
    {
        [Required(ErrorMessage = "This Field is Compulsory")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        public string Password { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        [Phone]
        public string MobileNo { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        public string Address { get; set; }
        [Required(ErrorMessage = "This Field is Compulsory")]
        public bool IsActive { get; set; }
    }

    public class UserDropDownModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }
}
