using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaloShare.Areas.Admin.Controllers
{
    [Authorize(Roles = "Mod, Admin")]
    public class HomeController : HaloShare.Controllers.BaseController
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View(new Models.HomeIndexViewModel
            {
                AccountCount = UserService.Count(),
                ReactionCount = ReactionService.Count(),
                UploadCount = FileService.Count(),
                DownloadCount = FileService.DownloadCount(),
                GameVariants = GameTypeService.SearchVariants("", "release", false, null, null, null).Take(5),
                ForgeVariants = GameMapService.SearchVariants("", "release", false, null, null, null).Take(5),
            });
        }
    }
}