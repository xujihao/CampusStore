using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;

namespace ECommerce
{
    /// <summary>
    /// Summary description for Goods
    /// </summary>
    /// 
    public class RecentGoods{
        public string goodsId;
        public string goodsType;
        public string goodsName;
        public string publishTime;
        public RecentGoods(string id,string type, string name, string time)
        {
            this.goodsId = id;
            this.goodsType = type;
            this.goodsName = name;
            this.publishTime = time;
        }
    }
    public class PopulateGoods
    {
        public string goodsId;
        public string goodsType;
        public string goodsName;
        public string starlevel;
        public PopulateGoods(string id,string type, string name, string sl)
        {
            this.goodsId = id;
            this.goodsType = type;
            this.goodsName = name;
            this.starlevel = sl;
        }
    }
    public class TypeGoods
    {
        public string goodsid;
        public string goodsname;
        public string publishtime;
        public string goodscount;
        public TypeGoods(string gid, string gname, string time, string gcount)
        {
            this.goodsid = gid;
            this.goodsname = gname;
            this.publishtime = time;
            this.goodscount = gcount;
        }
    }
    public class Goods : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string constr = WebConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(constr);
            string sql = "select top 20 goods.id,goodType.name,goods.name,publishTime"+
                " from goods,goodType where goods.goodType=goodType.id order by publishTime desc";
            context.Response.ContentType = "application/json";
            string index = context.Request.QueryString["index"].ToString();
            if (index == "1")
            {
                using (sqlconn)
                {
                    sqlconn.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(sql, sqlconn);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    List<RecentGoods> l = new List<RecentGoods>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string temp = dt.Rows[i][3].ToString();
                        temp=DateTime.Parse(temp).ToShortDateString();
                        l.Add(new RecentGoods(dt.Rows[i][0].ToString(),dt.Rows[i][1].ToString(),dt.Rows[i][2].ToString(),
                            temp));
                    }
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string json = js.Serialize(l);
                    context.Response.Write(json);
                }
            }
            else if (index == "2")
            {
                sql = "select top 20 avg(starlevel) as 'starlevel',gId from comment group by gId order by starlevel desc";
                using (sqlconn)
                {
                    sqlconn.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(sql, sqlconn);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    string subsql = "";
                    SqlDataAdapter subsda = new SqlDataAdapter();
                    DataTable subdt = new DataTable();
                    List<PopulateGoods> l = new List<PopulateGoods>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        subsql="select goodType.name,goods.name from goodType,goods where goods.goodType=goodType.id and goods.id="
                            +dt.Rows[i][1].ToString();
                        subsda = new SqlDataAdapter(subsql, sqlconn);
                        subdt = new DataTable();
                        subsda.Fill(subdt);
                        for (int j = 0; j < subdt.Rows.Count; j++)
                        {
                            l.Add(new PopulateGoods(dt.Rows[i][1].ToString(),subdt.Rows[j][0].ToString(), subdt.Rows[j][1].ToString(), dt.Rows[i][0].ToString()));
                        }
                    }
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string json = js.Serialize(l);
                    context.Response.Write(json);
                }
            }
            else
            {
                string type = context.Request.QueryString["type"];
                sql="select id from goodType where name='"+type+"'";
                SqlDataAdapter sda = new SqlDataAdapter(sql, sqlconn);
                DataTable dt=new DataTable();
                sda.Fill(dt);
                string gid=dt.Rows[0][0].ToString();
                sql = "select id,name,publishTime,gCount from goods where goodType=" + gid;
                sda = new SqlDataAdapter(sql, sqlconn);
                DataTable dt1 = new DataTable();
                sda.Fill(dt1);
                List<TypeGoods> l = new List<TypeGoods>();
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    l.Add(new TypeGoods(dt1.Rows[i][0].ToString(), dt1.Rows[i][1].ToString(),
                        dt1.Rows[i][2].ToString(), dt1.Rows[i][3].ToString()));
                }
                JavaScriptSerializer js = new JavaScriptSerializer();
                string json = js.Serialize(l);
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