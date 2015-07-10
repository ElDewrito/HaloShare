using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaloShare.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View(new ViewModels.HomeViewModel
            {
                Maps = GameMapService.SearchVariants("", "release", false, null, true, null).Take(3),
                Types = GameTypeService.SearchVariants("", "release", false, null, true, null).Take(3)
            });
        }

        public ActionResult Terms()
        {
            return View();
        }
    }
}