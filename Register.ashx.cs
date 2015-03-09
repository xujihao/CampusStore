using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Web.Script.Serialization;

namespace ECommerce
{
    /// <summary>
    /// Summary description for Register
    /// </summary>
    /// 
    public class School
    {
        public int schoolId;
        public string schoolName;
        public School(int id, string name)
        {
            this.schoolId = id;
            this.schoolName = name;
        }
    }
    public class Register : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        { 
            string constr = WebConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(constr);
            string sql="select * from school";
            if (context.Request.QueryString["query"] == "schools")
            {
                context.Response.ContentType = "application/json";               
                List<School> l=new List<School>();               
                using (sqlconn)
                {
                    sqlconn.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(sql, sqlconn);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        l.Add(new School(Int32.Parse(dt.Rows[i][0].ToString()), dt.Rows[i][1].ToString()));
                    }
                    string json = js.Serialize(l);
                    context.Response.Write(json);
                }
            }
            else
            {
                context.Response.ContentType = "text/plain";
                string username = context.Request.Form.Get("username");
                string password = context.Request.Form.Get("password");
                string school = context.Request.Form.Get("school");
                string email = context.Request.Form.Get("email");
                string phone = context.Request.Form.Get("phone");
                string qq = context.Request.Form.Get("qq");
                sql = "insert into usertable values('"+username+"','"+password+"',"+"'用户','"+email
                    +"',"+school+",'"+phone+"',"+qq+")";
                try
                {
                    using (sqlconn)
                    {
                        sqlconn.Open();
                        SqlCommand sqlcom = new SqlCommand(sql, sqlconn);
                        sqlcom.ExecuteNonQuery();
                        context.Response.Write("true");
                    }
                }
                catch (Exception e)
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