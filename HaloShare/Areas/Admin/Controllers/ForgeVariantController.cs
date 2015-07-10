using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaloShare.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using PagedList.Mvc;

namespace HaloShare.Areas.Admin.Controllers
{
    [Authorize(Roles = "Mod, Admin")]
    public class ForgeVariantController : HaloShare.Controllers.BaseController
    {
        // GET: Admin/ForgeVariant
        public ActionResult Index(string q, int? page, int? typeId, bool? staffPick, string sort = "release", string order = "descending", string author = "")
        {
            ViewBag.selectedMapId = typeId;
            ViewBag.typeId = GameMapService.GetMapSelectlist(typeId);
            ViewBag.q = q;
            ViewBag.order = order;
            ViewBag.sort = sort;
            ViewBag.staffPick = staffPick;


            int? authorId = null;
            if (!string.IsNullOrWhiteSpace(author))
            {
                var user = UserManager.FindByName(author);
                authorId = user.Id;
                ViewBag.author = user.UserName;
            }

            var maps = GameMapService.SearchVariants(q, sort, order == "ascending" ? true : false, typeId, staffPick, authorId, false);

            return View(maps.ToPagedList(page ?? 1, 50));
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var variant = GameMapService.GetVariant(id);
            if (variant == null)
                return HttpNotFound();

            return View(variant);
        }

        [HttpPost]
        public ActionResult Details(int id, GameMapVariant model)
        {
            var variant = GameMapService.GetVariant(id);
            variant.Title = model.Title;
            variant.ShortDescription = model.ShortDescription;
            variant.Description = model.Description;
            variant.MinPlayers = model.MinPlayers;
            variant.MaxPlayers = model.MaxPlayers;
            variant.IsStaffPick = model.IsStaffPick;
            if (ModelState.IsValid)
            {
                string path = System.IO.Path.Combine(Server.MapPath("~/Content/Files/Forge/"), variant.File.FileName);
                using (FileStream stream = System.IO.File.Open(path, FileMode.Open))
                {
                    VariantLib.ForgeVariant forge = new VariantLib.ForgeVariant(stream);

                    forge.VariantDescription = variant.ShortDescription;
                    forge.VariantName = variant.Title;
                    forge.Save();
                }

                GameMapService.Save();

                this.SetAlert(string.Format("The variant '{0}' has been updated.", variant.Title), AlertType.Success);
                return RedirectToAction("Index");
            }
            return View(variant);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var variant = GameMapService.GetVariant(id);
            if (variant == null)
                return HttpNotFound();

            return View(variant);
        }

        [HttpPost]
        public ActionResult Delete(int id, GameMapVariant model)
        {
            var variant = GameMapService.GetVariant(id);
            variant.IsDeleted = !variant.IsDeleted;
            GameMapService.Save();

            this.SetAlert(string.Format("The variant '{0}' has been deleted.", variant.Title), AlertType.Success);
            return RedirectToAction("Index");
        }
    }
}