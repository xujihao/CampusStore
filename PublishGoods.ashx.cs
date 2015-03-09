using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.SessionState;

namespace ECommerce
{
    /// <summary>
    /// Summary description for PublishGoods
    /// </summary>
    public class PublishGoods : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string constr = WebConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(constr);
            string name = context.Request.Form.Get("goodsname");
            string type = context.Request.Form.Get("goodstype");
            string price = context.Request.Form.Get("goodsprice");
            string gcount = context.Request.Form.Get("goodscount");
            string describe = context.Request.Form.Get("goodsdescribe");
            string username = context.Session["username"].ToString();
            string pubtime = DateTime.Now.ToShortDateString();
            string sql = "insert into goods values('" + name + "','" + pubtime + "'," + price + ",'" + describe
                + "'," + type + "," + gcount+",'"+username+"')";
            try
            {
                using (sqlconn)
                {
                    sqlconn.Open();
                    SqlCommand com = new SqlCommand(sql, sqlconn);
                    com.ExecuteNonQuery();
                    context.Response.Write("success");
                }
            }
            catch (Exception e)
            {
                context.Response.Write("failed");
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