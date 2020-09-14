using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Employees.Models;
using Newtonsoft.Json;
using System.Net.Http;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.Design;
using Microsoft.AspNetCore.Http;
using System;
//using System.Web.Mvc;

namespace Employees.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index(int?page,int? pagesize,string? sortItem)
        {

            List<Empolyee> emplist = new List<Empolyee>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://emplistapi-258220.appspot.com"))
                {
                   
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    emplist = JsonConvert.DeserializeObject<List<Empolyee>>(apiResponse);
                    emplist = emplist.OrderBy(a => a.name.first).ToList();
                    var pageNumber = page ?? 1;
                    var sizeNumber = pagesize ?? 2;

                   ViewData["pagesize"] = sizeNumber;
                    PopulateJobTitleDropdownlist(emplist);
                    PopulatePageSizeDropdownlist(sizeNumber);
                    PopulateSortItemsDropdownlist(sortItem);
                    var onePageOfemplist = emplist.ToPagedList(pageNumber, sizeNumber);
                    ViewBag.onePageOfemplist = onePageOfemplist;
                    var urlParameters = new { action = "Index", controller = "Home"};
                    string stringUrl = Url.RouteUrl(urlParameters);
                 //   int totalCount = emplist.Count();
                 //   Paginator paginator = new Paginator(stringUrl, page ?? 1, totalCount);
                 //   ViewData["pagination"] = paginator;


                    ;
                  //  System.IO.File.WriteAllText(@"C:\temp\strcoc.txt", "b" + pagesize.ToString());

                }
            }
            return View();
        }
        public void PopulatePageSizeDropdownlist(object selectedFileType = null)
        {
            
            List<SelectListItem> list = new List<SelectListItem>();
            
            list.Add(new SelectListItem() { Text = "2", Value = "2"});
            list.Add(new SelectListItem() { Text = "3", Value = "3" });
            list.Add(new SelectListItem() { Text = "4", Value = "4" });
            list.Add(new SelectListItem() { Text = "5", Value = "5" });
            ViewData["pagesize"] = new SelectList(list, "Value", "Text", selectedFileType);

        }
        public void PopulateSortItemsDropdownlist(object selectedFileType = null)
        {

            List<SelectListItem> list = new List<SelectListItem>();

            list.Add(new SelectListItem() { Text = "Name", Value = "Name" });
            list.Add(new SelectListItem() { Text = "JobTitle", Value = "JobTitle" });
            list.Add(new SelectListItem() { Text = "HasImage", Value = "HasImage" });
          
            ViewData["SortItem"] = new SelectList(list, "Value", "Text", selectedFileType);

        }

        public void PopulateJobTitleDropdownlist(List<Empolyee> emplist )
        {
            var jobtitle = from employee in emplist
                           group employee by employee.jobTitle into depGroup
                           orderby depGroup.Key ascending
                           select depGroup.Key;
            ViewBag.jobtitlelist = jobtitle;
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var a in jobtitle)

            {
                if (a != null)
                {
                    list.Add(new SelectListItem()
                    {
                        Text = a.ToString(),
                        Value = a.ToString()
                    });
                }
            }

            ViewData["jobTitle"] = new SelectList(list, "Value", "Text");

        }
       
        [HttpGet]
        public ActionResult  CreateEmployee(string first,string last,string url,string title)
        {
            string strmessage = "";
          // System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", first+last+title);

            strmessage = "New employee "+first + " " + last + " " + "is added to database";
           
            return Json(new { m=strmessage });
        }


        [HttpGet]
        public async Task<IActionResult> Index(int? page, int? pagesize,string? SortItem ,IFormCollection form)
        {
          // System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", "get" + pagesize.ToString());
            List<Empolyee> emplist = new List<Empolyee>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://emplistapi-258220.appspot.com"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    emplist = JsonConvert.DeserializeObject<List<Empolyee>>(apiResponse);
                    var pageNumber = page ?? 1;
                    var sizeNumber = pagesize ?? 5;
                    var Item = SortItem ?? "Name";
                    if (Item == "Name")

                    { emplist = emplist.OrderBy(a => a.name.first).ThenByDescending(a => a.name.last).ToList(); }
                    if (Item == "HasImage")
                    { emplist = emplist.OrderByDescending(a => a.photoURL).ToList(); }
                    if (Item == "JobTitle")
                    { emplist = emplist.OrderByDescending(a => a.jobTitle).ToList(); }
                   
                    var option = new CookieOptions();
                    option.Expires = System.DateTime.Now.AddDays(10);
                   // if (pagesize.HasValue)
                   // { Response.Cookies.Append("cookiesize", pagesize.ToString(), option); }

                    var onePageOfemplist=emplist.ToPagedList(pageNumber, sizeNumber); ;
                   // if (page.HasValue && !pagesize.HasValue)
                   
                   // {  onePageOfemplist = emplist.ToPagedList(pageNumber,Convert.ToInt32( Request.Cookies["cookiesize"].ToString())); }
                    ViewBag.onePageOfemplist = onePageOfemplist;
                    PopulateJobTitleDropdownlist(emplist);
                    PopulatePageSizeDropdownlist(sizeNumber);
                    PopulateSortItemsDropdownlist(SortItem);
                   ViewData["pagesizeNo"] = pagesize;
                    ViewData["item"] = SortItem;
                }
            }
            return View();
        }


      

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
