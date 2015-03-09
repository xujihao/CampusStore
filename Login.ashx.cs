using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Configuration;
using System.Web.SessionState;

namespace ECommerce
{
    /// <summary>
    /// Summary description for Login
    /// </summary>
    public class Login : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string username = context.Request.Form.Get("username");
            string password = context.Request.Form.Get("password");

            string constr = WebConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(constr);
            using (sqlconn)
            {
                sqlconn.Open();
                string sql = "select username,password from usertable where username='" + username + "' and password='" + password + "'";
                SqlDataAdapter sda = new SqlDataAdapter(sql, sqlconn);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    string uname=dt.Rows[0][0].ToString();
                    context.Session.Add("username", uname);
                    context.Response.Write("true");
                }
                else
                {
                    context.Response.Write("false");
                }
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