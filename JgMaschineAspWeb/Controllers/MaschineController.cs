using JgLibDataModel;
using JgLibHelper;
using JgMaschineAspWeb.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JgMaschineAspWeb.Controllers
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
                maschine.Aenderung = DateTime.Now;
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

        [Authorize]
        public async Task<ActionResult> IndexBauteileProMaschine(Guid? Id)
        {
            if (Id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var listeMaschinen = await db.TabMaschineSet.Where(w => w.IstAktiv)
                .OrderBy(o => o.MaschineName).ToListAsync();

            var maschine = listeMaschinen.FirstOrDefault(f => f.Id == Id);

            if (maschine == null)
                return HttpNotFound();

            var datStart = DateTime.Now.Date;
            var datEnde = datStart.AddDays(1);

            var bauteile = db.TabBauteilSet
                .Where(w => (w.IdMaschine == Id) && (w.StartFertigung >= datStart) && (w.StartFertigung < datEnde))
                .OrderBy(o => o.StartFertigung);

            ViewBag.ListeMaschinen = new SelectList(listeMaschinen, "Id", "MaschineName", maschine.Id);
            return View(await bauteile.ToListAsync());
        }

        public async Task<ActionResult> IndexBauteileProMaschinePartial(Guid IdMaschine, DateTime TxtDatumVon, DateTime TxtDatumBis)
        {
            var datBis = TxtDatumBis.Date.AddDays(1);
            var bauteile = db.TabBauteilSet
                .Where(w => (w.IdMaschine == IdMaschine) && (w.StartFertigung >= TxtDatumVon.Date) && (w.StartFertigung < datBis))
                .OrderBy(o => o.StartFertigung);

            return View(await bauteile.ToListAsync());
        }

        public async Task<ActionResult> AnzeigeMaschineStatus(Guid? Id)
        {
            if (Id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var lMaschinen = await db.TabMaschineSet
                .Where(w => w.IstAktiv)
                .OrderBy(o => o.MaschineName).ToListAsync();

            var maschine = lMaschinen.FirstOrDefault(f => f.Id == Id);

            if (maschine == null)
                return HttpNotFound();

            return View(new SelectList(lMaschinen, "Id", "MaschineName", maschine.Id));
        }

        public async Task<PartialViewResult> AnzeigeMaschineStatusPartial(Guid? Id, string DatumAenderung = null)
        {
            var maschine = await db.TabMaschineSet.Include(i => i.EStandort).FirstOrDefaultAsync(f => f.Id == Id);

            if (DatumAenderung != null)
            {
                if (maschine.StatusMaschineAenderung.ToString("dd.MM.yyyy HH:mm:ss") == DatumAenderung)
                    return PartialView(null);
            }

            if (maschine.StatusMaschine == null)
                return PartialView("AnzeigeMaschineStatusFehler");

            var meldStatus = Helper.ByteDatenXmlInObjekt<JgMaschinenStatusMeldungen>(maschine.StatusMaschine);

            // Alle Meldungen mit einem mal vom Server holen

            var idisHelfer = meldStatus.IdListeHelfer.Select(s => s).ToList();
            var idisMeldungen = new List<Guid>(idisHelfer);
            if (meldStatus.IdBediener != null)
                idisMeldungen.Add((Guid)meldStatus.IdBediener);
            if (meldStatus.IdMeldung != null)
                idisMeldungen.Add((Guid)meldStatus.IdMeldung);

            var lMeldungen = await db.TabMeldungSet.Include(i => i.EBediener).Where(w => idisMeldungen.Contains(w.Id)).ToListAsync();

            var anzeigeStatus = new JgMaschineStatusAnzeige()
            {
                Maschine = maschine,
                ListeHelfer = new List<TabMeldung>(lMeldungen.Where(w => idisHelfer.Contains(w.Id))), 
                Information = meldStatus.Information
            };

            if (meldStatus.IdAktivBauteil != null)
                anzeigeStatus.Bauteil = await db.TabBauteilSet.FindAsync(meldStatus.IdAktivBauteil);

            if (meldStatus.IdBediener != null)
                anzeigeStatus.Bediener = lMeldungen.FirstOrDefault(f => f.Id == meldStatus.IdBediener);

            if (meldStatus.IdMeldung != null)
                anzeigeStatus.Meldung = lMeldungen.FirstOrDefault(f => f.Id == meldStatus.IdMeldung);

            return PartialView(anzeigeStatus);
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
