using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShopManagment.Models
{
    public class CountryModel
    {
        public int? CountryID { get; set; }

        [Required]
        [DisplayName("Country Name")]
        public string CountryName { get; set; }

        [Required]
        [DisplayName("Country Code")]
        public string CountryCode { get; set; }
    }
}
