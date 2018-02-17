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
        public async Task<ActionResult> IndexMeldungProdokoll(string Prog)
        {
            var meldungen = new ScannerMeldung[] { ScannerMeldung.WARTSTART, ScannerMeldung.REPASTART, ScannerMeldung.COILSTART };
            var lMeldungen = db.TabMeldungSet
                .Where(w => (meldungen.Contains(w.Meldung)));

            if (Prog == "OFFEN")
                lMeldungen = lMeldungen.Where(w => w.Status == StatusMeldung.Offen);
            else
            {
                var auswahlBis = DateTime.Now.Date.AddDays(-10);
                lMeldungen = lMeldungen.Where(w => w.ZeitMeldung >= auswahlBis);
            }

            lMeldungen = lMeldungen.Include(i => i.EBediener)
               .Include(m => m.EMaschine)
               .Include(s => s.EMaschine.EStandort)
               .OrderBy(o => o.EMaschine.EStandort.StandortName).ThenBy(o => o.ZeitMeldung);

            ViewBag.Prog = Prog;
            return View(await lMeldungen.ToListAsync());
        }

        [Authorize]
        public async Task<ActionResult> IndexMeldungProdokollEdit(Guid? Id, string Prog)
        {
            if (Id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var meldung = await db.TabMeldungSet.Include(i => i.EMaschine)
                .Include(f => f.EMaschine.EStandort).Include(b => b.EBediener)
                .FirstOrDefaultAsync(f => f.Id == Id);

            if (meldung == null)
                return HttpNotFound();

            ViewBag.Prog = Prog;
            return View(meldung);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> IndexMeldungProdokollEdit(Guid Id, string Prog)
        {
            if (ModelState.IsValid)
            {
                var meldung = await db.TabMeldungSet.FindAsync(Id);
                TryUpdateModel(meldung);
                await db.SaveChangesAsync();

                return RedirectToAction("IndexMeldungProdokoll", new { Prog });
            }

            var mel = new TabMeldung();
            TryUpdateModel(mel);
            return View(mel);
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
