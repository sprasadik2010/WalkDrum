using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MySql;
using MySql.Data;
using MySql.Data.MySqlClient;
using WalkDrum.Models;

namespace WalkDrum.Repository
{
    public class queries
    {
        #region WALKIN INFO QUERIES
        public string savewinfo(WalkInInfo wi)
        {
            var query =
            "INSERT INTO walkdrum.wiinfo" +
            "(companyinfo," +
            "location," +
            "jobtitle," +
            "jobdescription," +
            "salary," +
            "experience," +
            "venue," +
            "datefrom," +
            "dateto," +
            "timefrom," +
            "timeto," +
            "contactperson," +
            "createdon)" +
            "VALUES" +
            "(" +
            "'" + wi.companyinfo.id + "'," +
            "'" + wi.location.id + "'," +
            "'" + wi.jobtitle + "'," +
            "'" + wi.jobdescription + "'," +
            "'" + wi.salary + "'," +
            "'" + wi.experience + "'," +
            "'" + wi.venue + "'," +
            "'" + wi.dtfrom.ToString("yyyy-MM-dd") + "'," +
            "'" + wi.dtto.ToString("yyyy-MM-dd") + "'," +
            "'" + wi.timefrom.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
            "'" + wi.timeto.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
            "'" + wi.contactperson + "'," +
            "'" + wi.createdon.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
            ");";

            return query;
        }
        public string getwinfo(int wiid)
        {
            return
            "SELECT * from walkdrum.wiinfo where id=" + wiid;
        }
        public string getwinfos()
        {
            return
            "SELECT * from walkdrum.wiinfo";
        }
        #endregion
        #region COMPANY INFO QUERIES
        public string savecinfo(CompanyInfo ci)
        {
            var query = 
            "INSERT INTO walkdrum.companyinfo" +
            "(companyname," +
            "email," +
            "phone)" +
            "VALUES" +
            "(" +
            "'" + ci.CompanyName + "'," +
            "'" + ci.Email + "'," +
            "'" + ci.Phone + "'" +
            ");";

            return query;
        }
        public string getcinfo(int wiid)
        {
            return
            "SELECT * from walkdrum.companyinfo where id=" + wiid;
        }
        public string getcinfos()
        {
            return
            "SELECT * from walkdrum.companyinfo";
        }
        #endregion
        #region LOCATION INFO QUERIES
        public string savelinfo(Location li)
        {
            var query = 
            "INSERT INTO walkdrum.location" +
            "(countryname," +
            "cityname)" +
            "VALUES" +
            "(" +
            "'" + li.CountryName + "'," +
            "'" + li.CityName + "'" +
            ");";

            return query;
        }
        public string getlinfo(int liid)
        {
            return
            "SELECT * from walkdrum.location where id=" + liid;
        }
        public string getlinfos()
        {
            return
            "SELECT * from walkdrum.location";
        }
        public string updateviews(int wid)
        {
            return
            "UPDATE walkdrum.wiinfo SET views = views + 1 WHERE id =" + wid;
        }
        #endregion
    }
}