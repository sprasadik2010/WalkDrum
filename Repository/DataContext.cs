using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

using MySql;
using MySql.Data;
using MySql.Data.MySqlClient;

using WalkDrum.Models;

namespace WalkDrum.Repository
{
    public class DataContext
    {
        public int savewalkin(WalkInInfo wi)
        {
            Location loc = new Location()
            {
                CityName = wi.location.CityName,
                CountryName = wi.location.CountryName
            };
            int lid = savelocation(loc);
            loc.id = lid;

            CompanyInfo cinfo = new CompanyInfo() 
            {
                CompanyName = wi.companyinfo.CompanyName,
                Email = wi.companyinfo.Email,
                Phone = wi.companyinfo.Phone 
            };
            int cid = savecompanyinfo(cinfo);
            cinfo.id = cid;

            WalkInInfo winfo = new WalkInInfo()
            {
                companyinfo = cinfo,
                location = loc,
                venue = wi.venue,
                dtfrom = wi.dtfrom,
                dtto = wi.dtto,
                timefrom = wi.timefrom,
                timeto = wi.timeto,
                jobtitle = wi.jobtitle,
                jobdescription = wi.jobdescription,
                salary = wi.salary,
                experience = wi.experience,
                contactperson = wi.contactperson,
                createdon = DateTime.Now
            };
            int wid = savewalkininfo(winfo);
            return wid;
        }

        public int updatejobviews(int wid)
        {
            updateviews(wid);
            return 1;
        }
        private int savelocation(Location loc)
        {
            var saveloc = new queries().savelinfo(loc);
            return MySqlEvents.executeinsert(saveloc); 
        }
        private int savecompanyinfo(CompanyInfo cinfo)
        {
            var savecinfo = new queries().savecinfo(cinfo);
            return MySqlEvents.executeinsert(savecinfo);
        }
        private int updateviews(int wid)
        {
            var savecinfo = new queries().updateviews(wid);
            return MySqlEvents.executeinsert(savecinfo);
        }
        private int savewalkininfo(WalkInInfo winfo)
        {
            var savewinfo = new queries().savewinfo(winfo);
            return MySqlEvents.executeinsert(savewinfo);
        }

        public List<WalkInInfo> GetWalkInInfos()
        {
            var getwinfosquery = new queries().getwinfos();
            return MySqlEvents.executeselect<List<WalkInInfo>>(getwinfosquery);
        }
        public WalkInInfo GetWalkInInfo(int id)
        {
            var getwinfoquery = new queries().getwinfo(id);
            return MySqlEvents.executeselect<WalkInInfo>(getwinfoquery);
        }
    }
    public class MySqlEvents
    {
        static string constr = System.Configuration.ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
        static MySqlConnection con = new MySqlConnection(constr);
        static MySqlCommand com = new MySqlCommand("",con);
        public static int executeinsert(string query)
        {
            com.CommandText = query;
            if (con.State == ConnectionState.Closed) { con.Open(); }
            var reader = com.ExecuteNonQuery();
            var lastid = com.LastInsertedId;
            con.Close();
            return Convert.ToInt32(lastid);
        }
        public static T executeselect<T>(string query)
        {
            com.CommandText = query;
            if (con.State == System.Data.ConnectionState.Closed) { con.Open(); }
            DataSet ds = new DataSet();
            MySqlDataAdapter myda = new MySqlDataAdapter();
            myda.SelectCommand = com;
            myda.Fill(ds);
            con.Close();
            return (T)(object)DatatoObj<T>(ds, typeof(T).ToString());
            //return (T)(object)null; 
        }

        private static T DatatoObj<T>(DataSet ds, string retType)
        {
            if (retType == typeof(List<WalkInInfo>).ToString())
            {
                List<WalkInInfo> winfos = new List<WalkInInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    winfos.Add(new WalkInInfo
                    {
                        id = Convert.ToInt32(dr["id"]),
                        jobtitle = dr["jobtitle"].ToString(),
                        companyinfo = executeselect<CompanyInfo>(new queries().getcinfo(Convert.ToInt32(dr["companyinfo"]))),
                        location = executeselect<Location>(new queries().getlinfo(Convert.ToInt32(dr["location"]))),
                        venue = dr["venue"].ToString(),
                        jobdescription = dr["jobdescription"].ToString(),
                        contactperson = dr["contactperson"].ToString(),
                        //dtfrom = DateTime.ParseExact(dr["datefrom"].ToString(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                        //dtto = DateTime.ParseExact(dr["dateto"].ToString(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                        //timefrom = DateTime.ParseExact(dr["timefrom"].ToString(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                        //timeto = DateTime.ParseExact(dr["timeto"].ToString(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                        dtfrom = Convert.ToDateTime(dr["datefrom"]),
                        dtto = Convert.ToDateTime(dr["dateto"]),
                        timefrom = Convert.ToDateTime(dr["timefrom"]),
                        timeto = Convert.ToDateTime(dr["timeto"]),

                        salary = dr["salary"].ToString(),
                        experience = dr["experience"].ToString(),
                        createdon = Convert.ToDateTime(dr["createdon"]),
                        views = Convert.ToInt32(dr["views"])
                    });
                }
                return (T)(object)winfos;
            }
            else if (retType == typeof(WalkInInfo).ToString())
            {
                WalkInInfo winfo = new WalkInInfo();
                DataRow dr = ds.Tables[0].Rows[0];
                winfo.id = Convert.ToInt32(dr["id"]);
                winfo.jobtitle = dr["jobtitle"].ToString();
                winfo.companyinfo = executeselect<CompanyInfo>(new queries().getcinfo(Convert.ToInt32(dr["companyinfo"])));
                winfo.location = executeselect<Location>(new queries().getlinfo(Convert.ToInt32(dr["location"])));
                winfo.venue = dr["venue"].ToString();
                winfo.jobdescription = dr["jobdescription"].ToString();
                winfo.contactperson = dr["contactperson"].ToString();

                return (T)(object)winfo;
            }
            else if (retType == typeof(List<CompanyInfo>).ToString())
            {
                List<CompanyInfo> cinfos = new List<CompanyInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    cinfos.Add(new CompanyInfo
                    {
                        id = Convert.ToInt32(dr["id"]),
                        CompanyName = dr["companyname"].ToString(),
                        Email = dr["email"].ToString(),
                        Phone = dr["phone"].ToString()
                    });
                }
                return (T)(object)cinfos;
            }
            else if (retType == typeof(CompanyInfo).ToString())
            {
                CompanyInfo winfo = new CompanyInfo();
                DataRow dr = ds.Tables[0].Rows[0];
                winfo.id = Convert.ToInt32(dr["id"]);
                winfo.CompanyName = dr["companyname"].ToString();
                winfo.Email = dr["email"].ToString();
                winfo.Phone = dr["phone"].ToString();
                return (T)(object)winfo;
            }
            else if (retType == typeof(List<Location>).ToString())
            {
                List<Location> linfos = new List<Location>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    linfos.Add(new Location
                    {
                        id = Convert.ToInt32(dr["id"]),
                        CityName = dr["cityname"].ToString(),
                        CountryName = dr["countryname"].ToString()
                    });
                }
                return (T)(object)linfos;
            }
            else if (retType == typeof(Location).ToString())
            {
                Location linfo = new Location();
                DataRow dr = ds.Tables[0].Rows[0];
                linfo.id = Convert.ToInt32(dr["id"]);
                linfo.CityName = dr["cityname"].ToString();
                linfo.CountryName = dr["countryname"].ToString();
                return (T)(object)linfo;
            }
            else
            {
                return (T)(object)null;
            }
        }
    }
}

