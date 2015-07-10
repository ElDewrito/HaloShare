using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using HaloShare.Models;
using HaloShare.Security;

namespace HaloShare.Controllers
{
    [Authorize]
    [RoutePrefix("manage")]
    public class ManageController : Controller
    {
       
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(object model)
        {
            if (ModelState.IsValid)
            {

            }
            return View(model);
        }
        
        [Route("")]
        [HttpPost]
        public ActionResult Refresh()
        {


            return RedirectToAction("Index");
        }
    }
}