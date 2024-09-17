using CoffeeShopManagment.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace CoffeeShopManagment.Controllers
{
    [CheckAccess]
    public class BillsController : Controller
    {
        private IConfiguration configuration;

        public BillsController(IConfiguration _configuration)
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
                sqlCommand.CommandText = "PR_Bills_SelectAll";
                SqlDataReader reader = sqlCommand.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                return View(dataTable);
            }
        }

        public IActionResult Form(int? BillID)
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            List<UserDropDownModel> userList = new List<UserDropDownModel>();
            List<OrderDropDownModel> orderList = new List<OrderDropDownModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Load User Dropdown
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_User_DropDown";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        foreach (DataRow data in dataTable.Rows)
                        {
                            UserDropDownModel userDropDownModel = new UserDropDownModel
                            {
                                UserID = Convert.ToInt32(data["UserID"]),
                                UserName = data["UserName"].ToString()
                            };
                            userList.Add(userDropDownModel);
                        }
                    }
                }

                // Load Order Dropdown
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Order_DropDown";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        foreach (DataRow data in dataTable.Rows)
                        {
                            OrderDropDownModel orderDropDownModel = new OrderDropDownModel
                            {
                                OrderID = Convert.ToInt32(data["OrderID"])
                            };
                            orderList.Add(orderDropDownModel);
                        }
                    }
                }

                BillsModel billsModel = null;
                if (BillID.HasValue)
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "PR_Bills_SelectByPK";
                        command.Parameters.Add("@BillID", SqlDbType.Int).Value = BillID.Value;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                billsModel = new BillsModel
                                {
                                    BillID = Convert.ToInt32(reader["BillID"]),
                                    BillNumber = reader["BillNumber"].ToString(),
                                    BillDate = Convert.ToDateTime(reader["BillDate"]),
                                    OrderID = Convert.ToInt32(reader["OrderID"]),
                                    TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                                    Discount = Convert.ToDecimal(reader["Discount"]),
                                    NetAmount = Convert.ToDecimal(reader["NetAmount"]),
                                    UserID = Convert.ToInt32(reader["UserID"])
                                };
                            }
                        }
                    }
                }

                // Pass dropdowns and model to View
                ViewBag.UserList = userList.Count > 0 ? userList : new List<UserDropDownModel>();
                ViewBag.OrderList = orderList.Count > 0 ? orderList : new List<OrderDropDownModel>();

                return View("Form", billsModel);
            }
        }

        public IActionResult BillSave(BillsModel model)
        {
            //if (ModelState.IsValid)
            //{
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                if (model.BillID == 0)
                {
                    command.CommandText = "PR_Bills_Insert";
                }
                else
                {
                    command.CommandText = "PR_Bills_Update";
                    command.Parameters.Add("@BillID", SqlDbType.Int).Value = model.BillID;
                }
                command.Parameters.Add("@BillNumber", SqlDbType.VarChar).Value = model.BillNumber;
                command.Parameters.Add("@BillDate", SqlDbType.DateTime).Value = model.BillDate;
                command.Parameters.Add("@OrderID", SqlDbType.Int).Value = model.OrderID;
                command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = model.TotalAmount;
                command.Parameters.Add("@Discount", SqlDbType.Decimal).Value = model.Discount;
                command.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = model.NetAmount;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = model.UserID;
                command.ExecuteNonQuery();
                return RedirectToAction("Index");
            }
            //}

            // Reload dropdowns if ModelState is not valid
            LoadDropdowns();
            return View("Form", model);
        }

        public IActionResult BillDelete(int BillID)
        {
            try
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Bills_Delete";
                    command.Parameters.Add("@BillID", SqlDbType.Int).Value = BillID;
                    command.ExecuteNonQuery();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Error Message"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        private void LoadDropdowns()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Load User Dropdown
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_User_DropDown";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        List<UserDropDownModel> userList = new List<UserDropDownModel>();
                        foreach (DataRow data in dataTable.Rows)
                        {
                            UserDropDownModel userDropDownModel = new UserDropDownModel
                            {
                                UserID = Convert.ToInt32(data["UserID"]),
                                UserName = data["UserName"].ToString()
                            };
                            userList.Add(userDropDownModel);
                        }
                        ViewBag.UserList = userList.Count > 0 ? userList : new List<UserDropDownModel>();  // Initialize the list if empty
                    }
                }

                // Load Order Dropdown
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Order_DropDown";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        List<OrderDropDownModel> orderList = new List<OrderDropDownModel>();
                        foreach (DataRow data in dataTable.Rows)
                        {
                            OrderDropDownModel orderDropDownModel = new OrderDropDownModel
                            {
                                OrderID = Convert.ToInt32(data["OrderID"])
                            };
                            orderList.Add(orderDropDownModel);
                        }
                        ViewBag.OrderList = orderList.Count > 0 ? orderList : new List<OrderDropDownModel>();  // Initialize the list if empty
                    }
                }
            }
        }
    }
}
