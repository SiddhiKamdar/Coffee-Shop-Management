using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShopManagment.Models
{
    public class StateModel
    {
        public int? StateID { get; set; }

        [Required]
        [DisplayName("State Name")]
        public string StateName { get; set; }

        [Required]
        [DisplayName("State Code")]
        public string StateCode { get; set; }

        [Required]
        [DisplayName("Country Name")]
        public int CountryID { get; set; }
    }
}
