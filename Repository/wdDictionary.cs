using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WalkDrum.Repository
{
    public class wdDictionary
    {
        public string getdisplayname(string key)
        {
            return collection()[key];
        }

        private Dictionary<string, string> collection()
        {
            var myDict = new Dictionary<string, string>
                {
                    { "CompanyName", "Company Name" },
                    { "Email", "E-Mail" },
                    { "Phone", "Phone" },
                    { "CountryName", "Country Name" },
                    { "CityName", "City Name" },
                    { "dtfrom", "Date From" },
                    { "dtto", "Date To" },
                    { "timefrom", "Time From" },
                    { "timeto", "Time To" },
                    { "venue", "Venue" },
                    { "jobtitle", "Job Title" },
                    { "jobdescription", "Job Description" },
                    { "salary", "Salary" },
                    { "experience", "Experience" },
                    { "contactperson", "Contact Person" }
            };
            return myDict;
        }
    }
}