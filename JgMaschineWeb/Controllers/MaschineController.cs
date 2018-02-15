using JgLibDataModel;
using JgLibHelper;
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

        [Authorize]
        public async Task<ActionResult> Index()
        {
            var maschine = db.TabMaschineSet.Include(i => i.EStandort).OrderBy(o => o.EStandort.StandortName).OrderBy(o => o.MaschineName);
            return View(await maschine.ToListAsync());
        }

        [Authorize]
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

        [Authorize]
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

        [Authorize]
        public async Task<ActionResult> IndexMeldungProdokoll()
        {
            var auswahlBis = DateTime.Now.Date.AddDays(-5);
            var meldungen = new ScannerMeldung[] { ScannerMeldung.WARTSTART, ScannerMeldung.REPASTART };
            var lMeldungen = db.TabMeldungSet
                .Include(i => i.EBediener)
                .Include(m => m.EMaschine)
                .Include(s => s.EMaschine.EStandort)
                .Where(w => (w.ZeitMeldung >= auswahlBis) && (meldungen.Contains(w.Meldung)))
                .OrderBy(o => o.EMaschine.EStandort.StandortName).ThenBy(o => o.EMaschine.MaschineName);

            return View(await lMeldungen.ToListAsync());
        }

        [Authorize]
        public async Task<ActionResult> IndexMeldungProdokollEdit(Guid? Id)
        {

            return View();
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
