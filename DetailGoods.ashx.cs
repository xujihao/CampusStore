using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using System.Data;
using System.Web.Configuration;
using System.Web.SessionState;

namespace ECommerce
{
    /// <summary>
    /// Summary description for DetailGoods
    /// </summary>
    /// 
    public class DImage{
        public string imgurl;
        public DImage(string url){
            this.imgurl=url;
        }
    }
    public class DUser
    {
        public string username;
        public string email;
        public string phone;
        public string qq;
        public DUser(string name, string email, string p, string q)
        {
            this.username = name;
            this.email = email;
            this.phone = p;
            this.qq = q;
        }
    }
    public class DGoods
    {
        public string goodsname;
        public string goodstype;
        public string publishTime;
        public string goodsprice;
        public string goodsdescribe;
        public string goodscount;
        public List<DImage> images;
        public DUser userinfo;
        public DGoods(string name,string type,string pubtime,
            string price,string des,string count,List<DImage> l,DUser userinfo){
            this.goodsname = name;
            this.goodsprice = price;
            this.goodstype = type;
            this.publishTime = pubtime;
            this.goodsdescribe = des;
            this.goodscount = count;
            this.images = l;
            this.userinfo = userinfo;
        }
    }
    public class DetailGoods : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string constr = WebConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(constr);
            string id = context.Request.QueryString["id"];
            string sql = "select goods.name,goodtype.name,publishTime,price,describe,gCount,username from goods,goodType where goodType.id=goodType and goods.id=" + id;
            using (sqlconn)
            {
                sqlconn.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, sqlconn);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                sql = "select url from image where gId=" + id;
                sda = new SqlDataAdapter(sql, sqlconn);
                DataTable dt1 = new DataTable();
                sda.Fill(dt1);
                List<DImage> l = new List<DImage>();
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    string temp = dt1.Rows[i][0].ToString();
                   string []arr;
                    arr=temp.Split('\\');
                    temp = "http://" + "192.168.1.110/Test/" + arr[arr.Length - 2] + "/" + arr[arr.Length - 1];
                    l.Add(new DImage(temp));
                }
                string username = dt.Rows[0]["username"].ToString();
                sql = "select username,email,phone,qq from usertable where username='" + username + "'";
                sda = new SqlDataAdapter(sql, sqlconn);
                DataTable dt2 = new DataTable();
                sda.Fill(dt2);
                DUser du = new DUser(dt2.Rows[0][0].ToString(), dt2.Rows[0][1].ToString(),
                    dt2.Rows[0][2].ToString(), dt2.Rows[0][3].ToString());
                DGoods g = new DGoods(dt.Rows[0][0].ToString(), dt.Rows[0][1].ToString(), dt.Rows[0][2].ToString(), dt.Rows[0][3].ToString(),
                    dt.Rows[0][4].ToString(), dt.Rows[0][5].ToString(), l,du);
                JavaScriptSerializer js = new JavaScriptSerializer();
                string json = js.Serialize(g); 
                context.Response.ContentType = "application/json";
                context.Response.Write(json);
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