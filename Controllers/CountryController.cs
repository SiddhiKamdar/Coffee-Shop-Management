using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using CoffeeShopManagment.Models;

namespace CoffeeShopManagment.Controllers
{
    public class CountryController : Controller
    {
        private readonly IConfiguration _configuration;

        #region configuration
        public CountryController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Index
        public IActionResult Index()
        {
            string connectionstr = _configuration.GetConnectionString("ConnectionString");
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionstr))
            {
                conn.Open();
                using (SqlCommand objCmd = conn.CreateCommand())
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "PR_LOC_Country_SelectAll";
                    SqlDataReader objSDR = objCmd.ExecuteReader();
                    dt.Load(objSDR);
                }
            }
            return View("Index", dt);
        }
        #endregion

        #region Delete
        public IActionResult Delete(int CountryID)
        {
            string connectionstr = _configuration.GetConnectionString("ConnectionString");
            using (SqlConnection conn = new SqlConnection(connectionstr))
            {
                conn.Open();
                using (SqlCommand sqlCommand = conn.CreateCommand())
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "PR_LOC_Country_Delete";
                    sqlCommand.Parameters.AddWithValue("@CountryID", CountryID);
                    sqlCommand.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Index");
        }
        #endregion
    }
}
