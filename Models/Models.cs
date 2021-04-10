using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WalkDrum.Models
{
    public class CompanyInfo
    {
        public int id { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
    public class Location
    {
        public int id { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }
    }
    public class WalkInInfo
    {
        public int id { get; set; }
        public CompanyInfo companyinfo { get; set; }
        public Location location { get; set; }
        public string venue { get; set; }
        public DateTime dtfrom { get; set; }
        public DateTime dtto { get; set; }
        public DateTime timefrom { get; set; }
        public DateTime timeto { get; set; }
        public string jobtitle { get; set; }
        public string jobdescription { get; set; }
        public string salary { get; set; }
        public string experience { get; set; }
        public string contactperson { get; set; }
        public DateTime createdon { get; set; }
        public int views { get; set; }

    }
    public class WalkInInfoLink
    {
        public int id { get; set; }
        public Location location { get; set; }
        public DateTime dtfrom { get; set; }
        public DateTime dtto { get; set; }
        public DateTime timefrom { get; set; }
        public DateTime timeto { get; set; }
        public string jobtitle { get; set; }

        public Uri link { get; set; }
        public DateTime createdon { get; set; }
        public int views { get; set; }

    }

}