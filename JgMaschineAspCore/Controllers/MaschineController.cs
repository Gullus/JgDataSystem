using JgLibDataModel;
using JgLibHelper;
using JgMaschineAspCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<ActionResult> Details(Guid? Id)
        {
            if (Id != null)
            {
                var maschine = await db.TabMaschineSet.FindAsync(Id);

                if (maschine != null)
                    return View(maschine);
            }

            return StatusCode(500);
        }

        [Authorize]
        public async Task<ActionResult> Edit(Guid? Id)
        {
            if (Id != null)
            {
                var maschine = await db.TabMaschineSet.FindAsync(Id);
                if (maschine != null)
                {
                    ViewBag.Standort = new SelectList(await db.TabStandortSet.ToListAsync(), "Id", "StandortName", maschine.Id);
                    return View(maschine);
                }
            }

            return StatusCode(500);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid Id)
        {
            if (ModelState.IsValid)
            {
                var maschine = await db.TabMaschineSet.FindAsync(Id);
                await TryUpdateModelAsync(maschine);
                maschine.Aenderung = DateTime.Now;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            var ma = new TabMaschine();
            await TryUpdateModelAsync(ma);
            ViewBag.Standort = new SelectList(await db.TabStandortSet.ToListAsync(), "Id", "StandortName", ma.Id);
            return View(ma);
        }

        [Authorize]
        public async Task<ActionResult> IndexMeldungProtokoll(string Prog)
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
        public async Task<ActionResult> IndexMeldungProtokollEdit(Guid? Id, string Prog)
        {
            if (Id != null)
            {
                var meldung = await db.TabMeldungSet.Include(i => i.EMaschine)
                    .Include(f => f.EMaschine.EStandort).Include(b => b.EBediener)
                    .FirstOrDefaultAsync(f => f.Id == Id);

                if (meldung != null)
                {
                    ViewBag.Prog = Prog;
                    return View(meldung);
                }
            }

            return StatusCode(500);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> IndexMeldungProtokollEdit(Guid Id, string Prog)
        {
            if (ModelState.IsValid)
            {
                var meldung = await db.TabMeldungSet.FindAsync(Id);
                await TryUpdateModelAsync(meldung);
                await db.SaveChangesAsync();

                return RedirectToAction("IndexMeldungProdokoll", new { Prog });
            }

            var mel = new TabMeldung();
            await TryUpdateModelAsync(mel);
            return View(mel);
        }

        [Authorize]
        public async Task<ActionResult> IndexBauteileProMaschine(Guid? Id)
        {
            if (Id != null)
            {
                var listeMaschinen = db.TabMaschineSet.Where(w => w.IstAktiv)
                    .OrderBy(o => o.MaschineName);

                ViewBag.ListeMaschinen = new SelectList(await listeMaschinen.ToListAsync(), "Id", "MaschineName", Id);
                return View();
            }

            return StatusCode(500);
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
            if (Id != null)
            {
                var lMaschinen = await db.TabMaschineSet
                    .Where(w => w.IstAktiv)
                    .OrderBy(o => o.MaschineName).ToListAsync();

                return View(new SelectList(lMaschinen, "Id", "MaschineName", Id));
            }

            return StatusCode(500);
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
