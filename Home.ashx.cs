using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Data;

namespace ECommerce
{
    public class ProductType
    {
        public string typevalue;
        public string typeid;
        public ProductType(string id,string value)
        {
            this.typeid = id;
            this.typevalue = value;
        }
    }
    /// <summary>
    /// Summary description for Home
    /// </summary>
    public class Home : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/Json";
            string type = context.Request.QueryString["type"];
            string sql="";
            List<ProductType> l=new List<ProductType>();
            string constr = WebConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(constr);
            using (sqlconn)
            {
                sqlconn.Open();
                if (type == "alllist")
                {
                     sql= "select * from goodType";
                }
                SqlDataAdapter sda = new SqlDataAdapter(sql, sqlconn);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                JavaScriptSerializer js = new JavaScriptSerializer();             
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                  l.Add(new ProductType(dt.Rows[i][0].ToString(),dt.Rows[i][1].ToString()));
                }
                string json=js.Serialize(l);
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