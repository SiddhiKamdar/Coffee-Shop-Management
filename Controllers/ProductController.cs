﻿using CoffeeShopManagment.Models;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Data;
using System.Data.SqlClient;

namespace CoffeeShopManagment.Controllers
{
    [CheckAccess]
    public class ProductController : Controller
    {
        private IConfiguration configuration;

        public ProductController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        #region ExportExcel
        public ActionResult ExportToExcel()
        {

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            DataTable dataTable = GetProductsData();

            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Products");

                for (int col = 1; col <= dataTable.Columns.Count; col++)
                {
                    worksheet.Cells[1, col].Value = dataTable.Columns[col - 1].ColumnName;
                }

                for (int row = 1; row <= dataTable.Rows.Count; row++)
                {
                    for (int col = 1; col <= dataTable.Columns.Count; col++)
                    {
                        worksheet.Cells[row + 1, col].Value = dataTable.Rows[row - 1][col - 1];
                    }
                }

                var stream = new MemoryStream(package.GetAsByteArray());
                string fileName = "Products.xlsx";
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                return File(stream, contentType, fileName);
            }
        }
        #endregion

        #region ProductTable
        private DataTable GetProductsData()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();


                using (SqlCommand sqlCommand = connection.CreateCommand())
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "PR_Product_SelectAll";


                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        return dataTable;
                    }
                }
            }
        }
        #endregion

        #region ProductList
        public IActionResult Index()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand sqlCommand = connection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_Product_SelectAll";
            SqlDataReader reader = sqlCommand.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            return View(dataTable);
        }
        #endregion
        #region ProductForm
        public IActionResult Form()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection2 = new SqlConnection(connectionString);
            connection2.Open();
            SqlCommand command2 = connection2.CreateCommand();
            command2.CommandType = System.Data.CommandType.StoredProcedure;
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
            return View();
        }
        #endregion
        #region ProductSave
        public IActionResult ProductSave(ProductModel productModel)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand checkUserCommand = connection.CreateCommand())
                    {
                        checkUserCommand.CommandType = CommandType.Text;
                        checkUserCommand.CommandText = "SELECT COUNT(*) FROM [dbo].[User] WHERE UserID = @UserId";
                        checkUserCommand.Parameters.Add("@UserId", SqlDbType.Int).Value = productModel.UserId;

                        int userCount = (int)checkUserCommand.ExecuteScalar();
                        if (userCount == 0)
                        {
                            ModelState.AddModelError("", "Invalid UserId");
                            return View("Index", GetProductsData());
                        }
                    }

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        if (productModel.ProductId == null)
                        {
                            command.CommandText = "PR_Product_Insert";
                        }
                        else
                        {
                            command.CommandText = "PR_Product_Update";
                            command.Parameters.Add("@ProductId", SqlDbType.Int).Value = productModel.ProductId;
                        }
                        command.Parameters.Add("@ProductName", SqlDbType.VarChar).Value = productModel.ProductName;
                        command.Parameters.Add("@ProductCode", SqlDbType.VarChar).Value = productModel.ProductCode;
                        command.Parameters.Add("@ProductPrice", SqlDbType.Decimal).Value = productModel.ProductPrice;
                        command.Parameters.Add("@Description", SqlDbType.VarChar).Value = productModel.Description;
                        command.Parameters.Add("@UserId", SqlDbType.Int).Value = productModel.UserId;

                        command.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("Index");
            }

            DataTable dataTable = GetProductsData();
            return View("Index", dataTable);
        }
        #endregion
        #region ProductEdit
        public IActionResult Edit(int ProductId)
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Product_SelectByPK";
                    command.Parameters.Add("@ProductId", SqlDbType.Int).Value = ProductId;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ProductModel productModel = new ProductModel
                            {
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                                ProductName = reader["ProductName"].ToString(),
                                ProductCode = reader["ProductCode"].ToString(),
                                ProductPrice = Convert.ToDecimal(reader["ProductPrice"]),
                                Description = reader["Description"].ToString(),
                                UserId = Convert.ToInt32(reader["UserId"])
                            };

                            LoadUserDropdown();

                            return View("Form", productModel);
                        }
                    }
                }
            }

            return RedirectToAction("Index");
        }
        #endregion
        #region LoadDD
        private void LoadUserDropdown()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
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
                            UserDropDownModel userDropDownModel = new UserDropDownModel();
                            userDropDownModel.UserID = Convert.ToInt32(data["UserID"]);
                            userDropDownModel.UserName = data["UserName"].ToString();
                            userList.Add(userDropDownModel);
                        }
                        ViewBag.UserList = userList;
                    }
                }
            }
        }
        #endregion
        #region ProductDelete

        public IActionResult ProductDelete(int ProductID)
        {
            try
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Product_Delete";
                command.Parameters.Add("@ProductID", SqlDbType.Int).Value = ProductID;
                command.ExecuteNonQuery();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error Message"] = ex.Message;
                return RedirectToAction("Index");
            }

        }
        #endregion
    }

}
