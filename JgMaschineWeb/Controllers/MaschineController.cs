using JgLibDataModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JgMaschineWeb.Controllers
{
    public class MaschineController : Controller
    {
        private JgMaschineDb db = new JgMaschineDb();

        public async Task<ActionResult> Index()
        {
            var maschine = db.TabMaschineSet.Include(i => i.EStandort).OrderBy(o => o.EStandort.StandortName).OrderBy(o => o.MaschineName);
            return View(await maschine.ToListAsync());
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TabMaschine tabMaschine = db.TabMaschineSet.Find(id);
            if (tabMaschine == null)
            {
                return HttpNotFound();
            }
            return View(tabMaschine);
        }

        public async Task<ActionResult> Edit(Guid? Id)
        {
            if (Id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                var maschine = await db.TabMaschineSet.FindAsync(Id);
            if (maschine == null)
                return HttpNotFound();

            ViewBag.Standort = new SelectList(await db.TabStandortSet.ToListAsync(), "Id", "StandortName", maschine.Id);
            return View(maschine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid Id)
        {
            if (ModelState.IsValid)
            {
                var maschine = await db.TabMaschineSet.FindAsync(Id);
                TryUpdateModel(maschine);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            var ma = new TabMaschine();
            TryUpdateModel(ma);
            ViewBag.Standort = new SelectList(await db.TabStandortSet.ToListAsync(), "Id", "StandortName", ma.Id);
            return View(ma);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
