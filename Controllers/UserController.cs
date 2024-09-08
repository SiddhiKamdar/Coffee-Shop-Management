using CoffeeShopManagment.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace CoffeeShopManagment.Controllers
{
    public class UserController : Controller
    {
        private readonly IConfiguration configuration;

        public UserController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public IActionResult Index()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCommand = connection.CreateCommand();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = "PR_User_SelectAll";
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    return View(dataTable);
                }
            }
        }

        public IActionResult Form(int? userId)
        {
            if (userId.HasValue)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "PR_User_SelectByPK";
                        command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId.Value;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                UserModel userModel = new UserModel
                                {
                                    UserId = Convert.ToInt32(reader["UserId"]),
                                    UserName = reader["UserName"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Password = reader["Password"].ToString(),
                                    MobileNo = reader["MobileNo"].ToString(),
                                    Address = reader["Address"].ToString(),
                                    IsActive = Convert.ToBoolean(reader["IsActive"])
                                };

                                return View(userModel);
                            }
                        }
                    }
                }
            }
            return View(new UserModel());
        }

        [HttpPost]
        public IActionResult UserSave(UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        if (userModel.UserId == 0)
                        {
                            command.CommandText = "PR_User_Insert";
                        }
                        else
                        {
                            command.CommandText = "PR_User_Update";
                            command.Parameters.Add("@UserId", SqlDbType.Int).Value = userModel.UserId;
                        }

                        command.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userModel.UserName;
                        command.Parameters.Add("@Email", SqlDbType.VarChar).Value = userModel.Email;
                        command.Parameters.Add("@Password", SqlDbType.VarChar).Value = userModel.Password;
                        command.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = userModel.MobileNo;
                        command.Parameters.Add("@Address", SqlDbType.VarChar).Value = userModel.Address;
                        command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = userModel.IsActive;

                        command.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("Index");
            }
            return View("Form", userModel);
        }

        [HttpPost]
        public IActionResult UserDelete(int userId)
        {
            try
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_User_Delete";
                    command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                    command.ExecuteNonQuery();
                }
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
