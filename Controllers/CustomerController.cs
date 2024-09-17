using CoffeeShopManagment.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace CoffeeShopManagment.Controllers
{
    [CheckAccess]
    public class CustomerController : Controller
    {
        private IConfiguration configuration;

        public CustomerController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public IActionResult Index()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand sqlCommand = connection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_Customer_SelectAll";
            SqlDataReader reader = sqlCommand.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            return View(dataTable);
        }
        public IActionResult Form(int? customerID)
        {
            // Fetch UserList for the dropdown
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection2 = new SqlConnection(connectionString);
            connection2.Open();
            SqlCommand command2 = connection2.CreateCommand();
            command2.CommandType = CommandType.StoredProcedure;
            command2.CommandText = "PR_User_DropDown";
            SqlDataReader reader2 = command2.ExecuteReader();
            DataTable dataTable2 = new DataTable();
            dataTable2.Load(reader2);
            List<UserDropDownModel> userList = new List<UserDropDownModel>();
            foreach (DataRow data in dataTable2.Rows)
            {
                UserDropDownModel userDropDownModel = new UserDropDownModel();
                userDropDownModel.UserID = Convert.ToInt32(data["UserID"]);
                userDropDownModel.UserName = data["UserName"].ToString();
                userList.Add(userDropDownModel);
            }
            ViewBag.UserList = userList;

            // Fetch customer data if it's an edit (customerID is provided)
            CustomerModel customerModel = new CustomerModel();
            if (customerID.HasValue)
            {
                SqlCommand sqlCommand = new SqlCommand("PR_Customers_SelectByPK", connection2);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@CustomerID", customerID.Value);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    customerModel.CustomerID = Convert.ToInt32(reader["CustomerID"]);
                    customerModel.CustomerName = reader["CustomerName"].ToString();
                    customerModel.HomeAddress = reader["HomeAddress"].ToString();
                    customerModel.Email = reader["Email"].ToString();
                    customerModel.MobileNo = reader["MobileNo"].ToString();
                    customerModel.GSTNo = reader["GST NO"].ToString();
                    customerModel.CityName = reader["CityName"].ToString();
                    customerModel.PinCode = reader["PinCode"].ToString();
                    customerModel.NetAmount = Convert.ToDecimal(reader["NetAmount"]);
                    customerModel.UserID = Convert.ToInt32(reader["UserID"]);
                }
            }

            return View(customerModel);
        }

        public IActionResult CustomerSave(CustomerModel model)
        
        {
            //if (ModelState.IsValid)
            //{
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                if (model.CustomerID == 0)
                {
                    command.CommandText = "PR_Customers_Insert";
                }
                else
                {
                    command.CommandText = "PR_Customer_Update";
                    command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = model.CustomerID;
                }
                command.Parameters.Add("@CustomerName", SqlDbType.VarChar).Value = model.CustomerName;
                command.Parameters.Add("@HomeAddress", SqlDbType.VarChar).Value = model.HomeAddress;
                command.Parameters.Add("@Email", SqlDbType.VarChar).Value = model.Email;
                command.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = model.MobileNo;
                command.Parameters.Add("@GSTNo", SqlDbType.VarChar).Value = model.GSTNo;
                command.Parameters.Add("@CityName", SqlDbType.VarChar).Value = model.CityName;
                command.Parameters.Add("@PinCode", SqlDbType.VarChar).Value = model.PinCode;
                command.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = model.NetAmount;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = model.UserID;
                command.ExecuteNonQuery();
                return RedirectToAction("Index");
            //}
            return View("Form", model);
        }
        public IActionResult CustomerDelete(int CustomerID)
        {
            try
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Customer_Delete";
                command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = CustomerID;
                command.ExecuteNonQuery();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error Message"] = ex.Message;
                return RedirectToAction("Index");
            }

        }
       
    }
}
