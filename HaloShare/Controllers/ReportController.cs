using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PagedList;
using PagedList.Mvc;

namespace HaloShare.Controllers
{
    public class ReportController : BaseController
    {
        [Authorize(Roles = "Staff")]
        public ActionResult Index(int? page, bool? handled)
        {
            ViewBag.handled = handled;
            return View(ReportService.GetReports(handled).ToPagedList(page ?? 1, 50));
        }

        [Authorize(Roles = "Staff")]
        public ActionResult SetHandled(int id)
        {
            var report = ReportService.Get(id);
            report.IsHandled = true;
            ReportService.Save();
            return RedirectToAction("Index");
        }

        // GET: Report
        [HttpPost]
        public ActionResult Submit(string url, string description)
        {
            if (!string.IsNullOrWhiteSpace(url) && url.StartsWith("/"))
            {
                var model = new Models.Report();
                model.AuthorId = User.Identity.GetUserId<int>();
                model.ReportedOn = DateTime.UtcNow;
                model.Url = url;
                model.Description = description;
                ReportService.AddReport(model);
                ReportService.Save();

                return Json(new
                {
                    Code = 0
                });
            }
           
            return Json(new
            {
                Code = 1
            });
        }
    }
}