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


namespace HaloShare.Controllers
{
    [RoutePrefix("gametypes")]
    public class GameTypeController : BaseController
    {
        [Route("")]
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

            var maps = GameTypeService.SearchVariants(q, sort, order == "ascending" ? true : false, typeId, staffPick, authorId);

            return View(maps.ToPagedList(page ?? 1, 15));
        }

        [Route(@"{slug:slug}")]
        public ActionResult Details(int slug)
        {
            ViewBag.Token = TokenService.CreateDateTimeToken();
            return View(GameTypeService.GetVariant(slug));
        }

        [Authorize]
        [Route(@"edit/{id:int}")]
        public ActionResult Edit(int id)
        {
            var variant = GameTypeService.GetVariant(id);

            if (!(variant.AuthorId == User.Identity.GetUserId<int>() || User.IsInRole("Mod")))
                return new HttpUnauthorizedResult();

            return View(variant);
        }

        [Authorize]
        [HttpPost]
        [Route(@"edit/{id:int}")]
        public ActionResult Edit(int id, Models.GameTypeVariant model)
        {
            if (ModelState.IsValid)
            {
                var variant = GameTypeService.GetVariant(id);

                if (!(variant.AuthorId == User.Identity.GetUserId<int>() || User.IsInRole("Mod")))
                    return new HttpUnauthorizedResult();

                variant.Title = model.Title;
                variant.Description = model.Description;
                variant.ShortDescription = model.ShortDescription;

                string path = System.IO.Path.Combine(Server.MapPath("~/Content/Files/GameType/"), variant.File.FileName);
                using (FileStream stream = System.IO.File.Open(path, FileMode.Open))
                {
                    VariantLib.GameVariant type = new VariantLib.GameVariant(stream);

                    type.VariantDescription = variant.ShortDescription;
                    type.VariantName = variant.Title;
                    type.Save();
                }

                if (User.IsInRole("Mod"))
                {
                    variant.IsStaffPick = model.IsStaffPick;
                }

                GameTypeService.Save();

                SetAlert(string.Format("The game variant is saved.", variant.Title), AlertType.Success);
                return RedirectToAction("Details", new { slug = variant.Slug });
            }
            return View(model);
        }

        [Authorize]
        [Route(@"delete/{id:int}")]
        public ActionResult Delete(int id)
        {
            var variant = GameTypeService.GetVariant(id);

            if (!(variant.AuthorId == User.Identity.GetUserId<int>() || User.IsInRole("Mod")))
                return new HttpUnauthorizedResult();

            return View(variant);
        }

        [Authorize]
        [HttpPost]
        [Route(@"delete/{id:int}")]
        public ActionResult Delete(int id, GameTypeVariant model)
        {
            var variant = GameTypeService.GetVariant(id);

            if (!(variant.AuthorId == User.Identity.GetUserId<int>() || User.IsInRole("Mod")))
                return new HttpUnauthorizedResult();

            variant.IsDeleted = true;
            GameTypeService.Save();

            SetAlert(string.Format("The game variant {0} is deleted.", variant.Title), AlertType.Success);
            return RedirectToAction("Index");
        }

        [Authorize]
        [Route("rate")]
        public ActionResult Rate(int id, int rating)
        {
            return Json(GameTypeService.Rate(id, User.Identity.GetUserId<int>(), rating));
        }

        [Authorize]
        [Route("action/reply")]
        public ActionResult Reply(ViewModels.ReplyRequest model)
        {
            var variant = GameTypeService.GetVariant(model.Id);

            if (ModelState.IsValid)
            {
                int id = GameTypeService.Reply(model, User.Identity.GetUserId<int>());

                return new RedirectResult(Url.Action("Details", new { slug = variant.Slug }) + "#reactions-" + id);
            }
            SetAlert("<strong>PARDON OUR DUST!</strong> Something went wrong when trying to reply, please try again.", AlertType.Danger);

            return RedirectToAction("Details", new { slug = variant.Slug });
        }
    }
}