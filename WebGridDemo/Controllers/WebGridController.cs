using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using WebGridDemo.Models;

namespace WebGridDemo.Controllers
{
    public class WebGridController : Controller
    {
        // GET: WebGrid
        public ActionResult Index()
        {
            return View(GetProducts());
        }


        public List<ProductModel> GetProducts()
        {
            try
            {
                List<ProductModel> lst = new List<ProductModel>();

                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["NorthwindConn"].ToString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT Productid,productname,unitprice FROM Products", conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while(rdr.Read())
                    {
                        ProductModel obj = new ProductModel();

                        obj.ProductID = rdr.GetInt32(0);

                        obj.ProductName = rdr.GetString(1);

                        obj.UnitPrice = rdr.GetDecimal(2);

                        lst.Add(obj);
                    }
                }

                return lst;
            }
            catch (Exception)
            {

            }

            return null;
        }

        [HttpPost]
        public ActionResult DeleteProduct(int ProductID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["NorthwindConn"].ToString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Products WHERE ProductID=@ProductID", conn);
                    cmd.Parameters.AddWithValue("@ProductID", ProductID);
                    int rowsAffected =cmd.ExecuteNonQuery();
                    if(rowsAffected>0)
                        return Json(new { result = "true" });
                    else
                        return Json(new { result = "false" });
                }
            }
            catch(Exception)
            { }

            return Json(new { result = "false" });
        }


        [HttpPost]
        public ActionResult InsertProduct(string ProductName,float UnitPrice)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["NorthwindConn"].ToString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO PRODUCTS(ProductName,UnitPrice) VALUES(@ProductName,@UnitPrice)", conn);
                    cmd.Parameters.AddWithValue("@ProductName", ProductName);
                    cmd.Parameters.AddWithValue("@UnitPrice", UnitPrice);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        return Json(new { result = "true" });
                    else
                        return Json(new { result = "false" });
                }
            }
            catch (Exception)
            { }

            return Json(new { result = "false" });
        }
    }
}