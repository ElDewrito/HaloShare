using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using Microsoft.AspNet.Identity;
using HaloShare.Models;
using System.IO;

namespace HaloShare.Areas.Admin.Controllers
{
    [Authorize(Roles = "Mod, Admin")]
    public class GameVariantController : HaloShare.Controllers.BaseController
    {
        // GET: Admin/GameVariant
        public ActionResult Index(string q, int? page, int? typeId, bool? staffPick, string sort = "release", string order = "descending", string author = "")
        {
            ViewBag.selectedMapId = typeId;
            ViewBag.typeId = GameTypeService.GetTypeSelectlist(typeId);
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

            var game = GameTypeService.SearchVariants(q, sort, order == "ascending" ? true : false, typeId, staffPick, authorId, false);

            return View(game.ToPagedList(page ?? 1, 50));
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var variant = GameTypeService.GetVariant(id);
            if (variant == null)
                return HttpNotFound();

            return View(variant);
        }

        [HttpPost]
        public ActionResult Details(int id, GameTypeVariant model)
        {
            var variant = GameTypeService.GetVariant(id);
            variant.Title = model.Title;
            variant.ShortDescription = model.ShortDescription;
            variant.Description = model.Description;
            variant.IsStaffPick = model.IsStaffPick;
            if (ModelState.IsValid)
            {
                string path = System.IO.Path.Combine(Server.MapPath("~/Content/Files/GameType/"), variant.File.FileName);
                using (FileStream stream = System.IO.File.Open(path, FileMode.Open))
                {
                    VariantLib.GameVariant game = new VariantLib.GameVariant(stream);

                    game.VariantDescription = variant.ShortDescription;
                    game.VariantName = variant.Title;
                    game.Save();
                }

                GameTypeService.Save();

                this.SetAlert(string.Format("The variant '{0}' has been updated.", variant.Title), AlertType.Success);
                return RedirectToAction("Index");
            }
            return View(variant);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var variant = GameTypeService.GetVariant(id);
            if (variant == null)
                return HttpNotFound();

            return View(variant);
        }

        [HttpPost]
        public ActionResult Delete(int id, GameTypeVariant model)
        {
            var variant = GameTypeService.GetVariant(id);
            variant.IsDeleted = !variant.IsDeleted;
            GameTypeService.Save();

            this.SetAlert(string.Format("The variant '{0}' has been deleted.", variant.Title), AlertType.Success);
            return RedirectToAction("Index");
        }
    }
}