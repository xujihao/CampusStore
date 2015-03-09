using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data.SqlClient;
using System.Data;
using System.Web.Configuration;
using System.Web.Script.Serialization;

namespace ECommerce
{
    /// <summary>
    /// Summary description for Mine
    /// </summary>
    /// 
    public class MineInfo
    {
        public string username;
        public string password;
        public string email;
        public string phone;
        public string qq;
        public MineInfo(string un,string ps,string ea,string ph,string qq){
            this.username=un;
            this.password=ps;
            this.email=ea;
            this.phone=ph;
            this.qq=qq;
        }
    }
    public class Saled
    {
        public string goodsname;
        public string publishtime;
        public string goodsprice;
        public string goodsid;
        public Saled(string name, string time, string price,string id)
        {
            this.goodsid = id;
            this.goodsname = name;
            this.publishtime = time;
            this.goodsprice = price;
        }
    }
    public class Mine : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string id = context.Request.QueryString["id"];
            string constr = WebConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(constr);
            string username = context.Session["username"].ToString();
            if (id == "mine") {                
                string sql = "select username,password,email,phone,qq from usertable where username='" + username + "'";
                using (sqlconn)
                {
                    sqlconn.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(sql, sqlconn);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    MineInfo m = new MineInfo(dt.Rows[0][0].ToString(), dt.Rows[0][1].ToString(), dt.Rows[0][2].ToString(),
                        dt.Rows[0][3].ToString(), dt.Rows[0][4].ToString());
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string json = js.Serialize(m);
                    context.Response.Write(json);
                }
            }
            else
            {
                string sql = "select name,publishTime,price,id from goods where username='" + username + "'";
                using (sqlconn)
                {
                    sqlconn.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(sql, sqlconn);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    List<Saled> l = new List<Saled>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int index=dt.Rows[i][1].ToString().IndexOf("0:");
                        string temp = dt.Rows[i][1].ToString().Substring(0, index);
                        l.Add(new Saled(dt.Rows[i][0].ToString(), temp, 
                            dt.Rows[i][2].ToString(),dt.Rows[i][3].ToString()));
                    }
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string json = js.Serialize(l);
                    context.Response.Write(json);
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