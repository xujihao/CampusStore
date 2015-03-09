using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace ECommerce
{
    /// <summary>
    /// Summary description for Image
    /// </summary>
    public class Image : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpPostedFile file = HttpContext.Current.Request.Files["fileAddPic"];
            try
            {
                string filepath = context.Server.MapPath("img").ToString();
                filepath = filepath + "\\" + file.FileName;
                file.SaveAs(filepath);
                string constr = WebConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
                SqlConnection sqlconn = new SqlConnection(constr);
                string sql = "select max(id) from goods";
                using (sqlconn)
                {
                    sqlconn.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(sql,sqlconn);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    string gid = dt.Rows[0][0].ToString();
                    sql = "insert into image values('" + filepath + "'," + gid + ")";
                    SqlCommand com = new SqlCommand(sql, sqlconn);
                    com.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}