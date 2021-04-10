using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.WebSockets;
using System.Xml;
using System.Xml.Linq;
using WalkDrum.Models;
using WalkDrum.Repository;

namespace WalkDrum.Controllers
{
    public class HomeController : Controller
    {

    
        public ActionResult Index()
        {
            addviewer();
            return View(GetWalkIns());
        }

    public ActionResult SearchResults(
    string keyword = null,
    string location = null,
    string datefrom = null,
    string dateto = null)
        {
            var AllWalkins = GetWalkIns();

            AllWalkins = (!string.IsNullOrEmpty(keyword.Trim() + location.Trim())) ?
            AllWalkins.Where(wi =>
            {
                List<string> keynloc = new List<string>();
                if (!string.IsNullOrEmpty(keyword.Trim()))
                    keynloc.Add(keyword.Trim());
                if (!string.IsNullOrEmpty(location.Trim()))
                    keynloc.Add(location.Trim());
                var allText = (wi.companyinfo.CompanyName +
                wi.location.CityName +
                wi.location.CountryName +
                wi.experience +
                wi.jobdescription +
                wi.jobtitle +
                wi.venue).ToLower();
                return keynloc.All(c => allText.Contains(c.ToLower()));
            }).ToList() :
            AllWalkins;


            AllWalkins = (!string.IsNullOrEmpty(datefrom)) ?
                AllWalkins.Where(wi => wi.dtfrom >= Convert.ToDateTime(
                    DateTime.ParseExact(datefrom.Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList() : AllWalkins;

            AllWalkins = (!string.IsNullOrEmpty(dateto)) ?
                AllWalkins.Where(wi => wi.dtto <= Convert.ToDateTime(
                    DateTime.ParseExact(dateto.Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList() : AllWalkins;


            return View(AllWalkins.OrderByDescending(w => w.dtto));
        }

        public ActionResult addwalkin(WalkInInfo wi)
        {
            if (Request.HttpMethod == "POST")
            {
                if (String.IsNullOrEmpty(wi.jobtitle)) { ViewBag.elementid = "jobtitle"; ViewBag.invalidfield = "jobtitle"; }
                else if (String.IsNullOrEmpty(wi.jobdescription)) { ViewBag.elementid = "jobdescription"; ViewBag.invalidfield = "jobdescription"; }
                else if (String.IsNullOrEmpty(wi.salary)) { ViewBag.elementid = "salary"; ViewBag.invalidfield = "salary"; }
                else if (String.IsNullOrEmpty(wi.experience)) { ViewBag.elementid = "experience"; ViewBag.invalidfield = "experience"; }

                else if (String.IsNullOrEmpty(wi.companyinfo.CompanyName)) { ViewBag.elementid = "companyinfo_CompanyName"; ViewBag.invalidfield = "CompanyName"; }
                else if (String.IsNullOrEmpty(wi.companyinfo.Email)) { ViewBag.elementid = "companyinfo_Email"; ViewBag.invalidfield = "Email"; }
                else if (String.IsNullOrEmpty(wi.companyinfo.Phone)) { ViewBag.elementid = "companyinfo_Phone"; ViewBag.invalidfield = "Phone"; }

                else if (String.IsNullOrEmpty(wi.dtfrom.ToString())) { ViewBag.elementid = "dtfrom"; ViewBag.invalidfield = "dtfrom"; }
                else if (String.IsNullOrEmpty(wi.dtto.ToString())) { ViewBag.elementid = "dtto"; ViewBag.invalidfield = "dtto"; }

                else if (String.IsNullOrEmpty(wi.timefrom.ToString())) { ViewBag.elementid = "timefrom"; ViewBag.invalidfield = "timefrom"; }
                else if (String.IsNullOrEmpty(wi.timeto.ToString())) { ViewBag.elementid = "timeto"; ViewBag.invalidfield = "timeto"; }

                else if (String.IsNullOrEmpty(wi.venue)) { ViewBag.elementid = "venue"; ViewBag.invalidfield = "venue"; }

                else if (String.IsNullOrEmpty(wi.location.CountryName)) { ViewBag.elementid = "location_CountryName"; ViewBag.invalidfield = "CountryName"; }
                else if (String.IsNullOrEmpty(wi.location.CityName)) { ViewBag.elementid = "location_CityName"; ViewBag.invalidfield = "CityName"; }
                
                else if (String.IsNullOrEmpty(wi.contactperson)) { ViewBag.elementid = "contactperson"; ViewBag.invalidfield = "Contact Person"; }
                else { ViewBag.invalidfield = null; }
                if (ViewBag.invalidfield == null)
                {
                    new DataContext().savewalkin(wi);
                }
                else {
                    ViewBag.focusscript = "$('#" + ViewBag.elementid + "').focus();$('#" + ViewBag.elementid + "').attr('placeholder', 'Please enter a valid " + new wdDictionary().getdisplayname(ViewBag.invalidfield) + "');";
                    ViewBag.focusscript += "$('#" + ViewBag.elementid + "').addClass('highlight');";
                    ViewBag.focusscript += "$('#" + ViewBag.elementid + "').keyup(function(){$('#" + ViewBag.elementid + "').removeClass('highlight')});";
                }

            }

            return View();
        }


        public ActionResult JobDetails(int id = 0)
        {
            WalkInInfo wi = GetWalkIns().Where(w => w.id == id).FirstOrDefault();
            new DataContext().updatejobviews(id);
            return View(wi);
        }
        public ActionResult ContactUs(
            string yourname = null, 
            string youremail = null, 
            string message = null)
        {
            if (yourname != null && message != null)
            {
                StringBuilder msg = new StringBuilder();

                msg.Append("Hi Sivaprasad, <br/>");
                msg.Append("There is a message from ");
                msg.Append(yourname + " (" + youremail + ")</br></br>");
                msg.Append("MESSAGE:<br/>");
                msg.Append(message);
                //string from = "info.walkdrum@gmail.com";// System.Web.HttpContext.Current.Session["UserEmail"].ToString();
                //string to = "sprasadik@gmail.com";


                //var client = new SmtpClient("relay-hosting.secureserver.net", 25);
                //client.UseDefaultCredentials = true;
                ////client.Credentials = new System.Net.NetworkCredential("info.walkdrum@gmail.com", "w@1kdrum2020");
                //client.EnableSsl = false;
                //try { 
                //    client.Send(from, to, "Walkdrum Contact Us", msg.ToString()); 
                //} catch (Exception) { }
                contacusmessage(msg.ToString());
                ViewBag.result = "success";
            }
            ModelState.Clear();
            return View();
        }
        public PartialViewResult WalkinInfo(int wid)
        {
            return PartialView();
        }
        public PartialViewResult CompanyInfo(int ciid)
        {
            return PartialView();
        }
        public PartialViewResult Location(int lid)
        {
            return PartialView();
        }
        public PartialViewResult Venue(int vid)
        {
            return PartialView();
        }
        public PartialViewResult DateRange()
        {
            return PartialView();
        }
        public PartialViewResult TimeRange()
        {
            return PartialView();
        }


        public PartialViewResult _JobDetails(int jID)
        {
            return PartialView(GetWalkIns().Where(wi => wi.id == jID).FirstOrDefault());
        }

        public PartialViewResult _SearchResults(
            string keyword = null,
            string location = null,
            string datefrom = null,
            string dateto = null)
        {
            ViewBag.keyword = keyword;
            ViewBag.location = location;
            ViewBag.datefrom = datefrom;
            ViewBag.dateto = dateto;
            var AllWalkins = GetWalkIns();

            //AllWalkins = (!string.IsNullOrEmpty(keyword)) ?
            //    AllWalkins.Where(wi => (wi.companyinfo.CompanyName +
            //    wi.experience +
            //    wi.jobdescription +
            //    wi.venue).Contains(keyword)).ToList() : AllWalkins;

            //AllWalkins = (!string.IsNullOrEmpty(location)) ?
            //    AllWalkins.Where(wi => (wi.companyinfo +
            //    wi.experience +
            //    wi.jobdescription +
            //    wi.venue).Contains(location)).ToList() : AllWalkins;

            AllWalkins = (!string.IsNullOrEmpty(keyword.Trim() + location.Trim())) ?
            AllWalkins.Where(wi =>
            {
                List<string> keynloc = new List<string>();
                if (!string.IsNullOrEmpty(keyword.Trim()))
                    keynloc.Add(keyword.Trim());
                if (!string.IsNullOrEmpty(location.Trim()))
                    keynloc.Add(location.Trim());
                var allText = (wi.companyinfo.CompanyName +
                wi.location.CityName +
                wi.location.CountryName +
                wi.experience +
                wi.jobdescription +
                wi.jobtitle +
                wi.venue +
                wi.contactperson).ToLower();
                return keynloc.All(c => allText.Contains(c.ToLower()));
            }).ToList() : 
            AllWalkins;

            if (string.IsNullOrEmpty(datefrom)) { datefrom = DateTime.MinValue.ToString("yyyy-MM-dd"); }
            if (string.IsNullOrEmpty(dateto)) { dateto = DateTime.MaxValue.ToString("yyyy-MM-dd"); }

            DateTime dtFrom = DateTime.ParseExact(datefrom.Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime dtTo = DateTime.ParseExact(dateto.Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture);

            AllWalkins = (!string.IsNullOrEmpty(datefrom)) ?
                AllWalkins.Where(wi => dtFrom <= wi.dtto && wi.dtfrom <= dtTo).ToList() : AllWalkins;
            
            //AllWalkins = (!string.IsNullOrEmpty(datefrom)) ?
            //    AllWalkins.Where(wi => wi.dtfrom >= dtFrom).ToList() : AllWalkins;

            //AllWalkins = (!string.IsNullOrEmpty(dateto)) ?
            //    AllWalkins.Where(wi => wi.dtto >= dtTo).ToList() : AllWalkins;


            return PartialView(AllWalkins.OrderByDescending(w => w.dtto));
        }
        public PartialViewResult _SearchResultLinks(
            string keyword = null,
            string location = null,
            string datefrom = null,
            string dateto = null)
        {
            ViewBag.keyword = keyword;
            ViewBag.location = location;
            ViewBag.datefrom = datefrom;
            ViewBag.dateto = dateto;
            var AllWalkinLinks = new List<WalkInInfoLink>();
            AllWalkinLinks.Add(
                new WalkInInfoLink
                {
                    jobtitle = "Walk-In Interview For Trade Operations - 3 April - Mashreq, Bangalore",
                    dtfrom = new DateTime(2021, 4, 3),
                    dtto = new DateTime(2021, 4, 3),
                    timefrom = new DateTime(2021, 4, 3, 10, 0, 0),
                    timeto = new DateTime(2021, 4, 3, 13, 0, 0),
                    link = new Uri("https://www.naukri.com/job-listings-walk-in-interview-for-trade-operations-3-april-mashreq-bangalore-mashreq-global-services-private-limited-bangalore-bengaluru-2-to-7-years-310321005476?src=seo_srp&sid=16173885459265877&xp=1&px=1")
                }
                );

            return PartialView(AllWalkinLinks);
        }

        public List<WalkInInfo> GetWalkIns()
        {
            List<WalkInInfo> lstWIns = new DataContext().GetWalkInInfos();
            return lstWIns;
        }

        public void addviewer()
        {
            string xmlpath = HostingEnvironment.MapPath(@"~/Content\views.xml");
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlpath);
            XmlNode node = doc.DocumentElement.SelectSingleNode(@"/viewcount/count");
            int count = Convert.ToInt32(node.InnerText);
            node.InnerText = (++count).ToString();
            doc.Save(xmlpath);
        }

        public void contacusmessage(string message)
        {
            //string xmlpath = HostingEnvironment.MapPath(@"~/Content\contacts.xml");
            //XmlDocument doc = new XmlDocument();
            //doc.Load(xmlpath);
            //XmlNode node = doc.DocumentElement.SelectSingleNode(@"/contacts/contacts/");
            //int count = Convert.ToInt32(node.InnerText);
            //node.InnerText = (++count).ToString();
            //doc.Save(xmlpath);

            string xmlpath = HostingEnvironment.MapPath(@"~/Content\contacts.xml");
            XDocument doc = XDocument.Load(xmlpath);
            //XmlElement contact = doc.CreateElement(@"/contacts/contact");

            XElement contact = new XElement("contact",
                        new XElement("date", DateTime.Now.ToString("MMM dd, yyyy hh:mm:ss tt")),
                        new XElement("message", message));

            doc.Element("contacts").Add(contact);
            doc.Save(xmlpath);





        }

    }
}