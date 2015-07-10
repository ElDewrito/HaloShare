using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HaloShare.Models;
using Microsoft.AspNet.Identity;
using PagedList;

namespace HaloShare.Controllers
{
    [RoutePrefix("forge")]
    public class ForgeController : BaseController
    {
        [Route("")]
        public ActionResult Index(string q, int? page, int? typeId, bool? staffPick, string sort = "release", string order = "descending", string author = "")
        {
            ViewBag.selectedMapId = typeId;
            ViewBag.typeId = GameMapService.GetMapSelectlist(typeId);
            ViewBag.q = q;
            ViewBag.order = order;
            ViewBag.sort = sort;
            ViewBag.staffPick = staffPick;
            

            int? authorId = null;
            if(!string.IsNullOrWhiteSpace(author)){
                var user =  UserManager.FindByName(author);
                authorId = user.Id;
                ViewBag.author = user.UserName;
            }

            var maps = GameMapService.SearchVariants(q, sort, order == "ascending" ? true : false, typeId, staffPick, authorId);

            return View(maps.ToPagedList(page ?? 1, 15));
        }

        [Route(@"{slug:slug}")]
        public ActionResult Details(int slug)
        {
            ViewBag.Token = TokenService.CreateDateTimeToken();
            return View(GameMapService.GetVariant(slug));
        }

        [Authorize]
        [Route(@"edit/{id:int}")]
        public ActionResult Edit(int id)
        {
            var variant = GameMapService.GetVariant(id);

            if (!(variant.AuthorId == User.Identity.GetUserId<int>() || User.IsInRole("Mod")))
                return new HttpUnauthorizedResult();
            
            return View(variant);
        }

        [Authorize]
        [HttpPost]
        [Route(@"edit/{id:int}")]
        public ActionResult Edit(int id, Models.GameMapVariant model)
        {
            if (ModelState.IsValid)
            {
                var variant = GameMapService.GetVariant(id);

                if (!(variant.AuthorId == User.Identity.GetUserId<int>() || User.IsInRole("Mod")))
                    return new HttpUnauthorizedResult();

                variant.Title = model.Title;
                variant.Description = model.Description;
                variant.ShortDescription = model.ShortDescription;
                variant.MinPlayers = model.MinPlayers;
                variant.MaxPlayers = model.MaxPlayers;

                string path = System.IO.Path.Combine(Server.MapPath("~/Content/Files/Forge/"), variant.File.FileName);
                using (FileStream stream = System.IO.File.Open(path, FileMode.Open))
                {
                    VariantLib.ForgeVariant forge = new VariantLib.ForgeVariant(stream);

                    forge.VariantDescription = variant.ShortDescription;
                    forge.VariantName = variant.Title;
                    forge.Save();
                }

                if (User.IsInRole("Mod"))
                {
                    variant.IsStaffPick = model.IsStaffPick;
                }

                GameMapService.Save();
                
                SetAlert(string.Format("The forge variant is saved.", variant.Title), AlertType.Success);
                return RedirectToAction("Details", new { slug = variant.Slug });
            }
            return View(model);
        }

        [Authorize]
        [Route(@"delete/{id:int}")]
        public ActionResult Delete(int id)
        {
            var variant = GameMapService.GetVariant(id);

            if (!(variant.AuthorId == User.Identity.GetUserId<int>() || User.IsInRole("Mod")))
                return new HttpUnauthorizedResult();

            return View(variant);
        }

        [Authorize]
        [HttpPost]
        [Route(@"delete/{id:int}")]
        public ActionResult Delete(int id, GameMapVariant model)
        {
            var variant = GameMapService.GetVariant(id);

            if (!(variant.AuthorId == User.Identity.GetUserId<int>() || User.IsInRole("Mod")))
                return new HttpUnauthorizedResult();

            variant.IsDeleted = true;
            GameMapService.Save();

            SetAlert(string.Format("The forge variant {0} is deleted.", variant.Title), AlertType.Success);
            return RedirectToAction("Index");
        }

        [Authorize]
        [Route("action/rate")]
        public ActionResult Rate(int id, int rating)
        {
            return Json(GameMapService.Rate(id, User.Identity.GetUserId<int>(), rating));
        }

        [Authorize]
        [Route("action/reply")]
        public ActionResult Reply(ViewModels.ReplyRequest model)
        {
            var variant = GameMapService.GetVariant(model.Id);
            if (ModelState.IsValid)
            {
                int id = GameMapService.Reply(model, User.Identity.GetUserId<int>());

                return new RedirectResult(Url.Action("Details", new { slug = variant.Slug }) + "#reactions-" + id);
            }
            SetAlert("<strong>PARDON OUR DUST!</strong> Something went wrong when trying to reply, please try again.", AlertType.Danger);

            return RedirectToAction("Details", new { slug = variant.Slug });
        }
    }
}