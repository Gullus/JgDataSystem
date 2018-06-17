using JgLibDataModel;
using JgLibHelper;
using JgMaschineAspCore;
using JgMaschineAspCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        //[Authorize]
        public async Task<ActionResult> Index()
        {
            var maschine = db.TabMaschineSet.Include(i => i.EStandort).OrderBy(o => o.EStandort.StandortName).OrderBy(o => o.MaschineName);
            return View(await maschine.ToListAsync());
        }

        private async Task MaschineHelper(bool isCreate)
        {
            var standorte = db.TabStandortSet.Select(s => new { id = s.Id, Value = s.StandortName });
            ViewBag.Standort = new SelectList(await standorte.ToListAsync(), "id", "Value");
            ViewBag.FormHelper = new TFormHelper() { IsCreate = isCreate };
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await MaschineHelper(true);
            return View(new TabMaschine() { Id = Guid.NewGuid() });
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Create(TabMaschine maschine)
        {
            if (ModelState.IsValid)
            {
                await db.TabMaschineSet.AddAsync(maschine);
                await db.DbSave(User, maschine);
                return RedirectToAction("Index");
            }

            await MaschineHelper(true);
            return View(maschine);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Edit(Guid id)
        {
            var maschine = await db.TabMaschineSet.FindAsync(id);

            if (maschine != null)
            {
                await MaschineHelper(false);
                return View("Create", maschine);
            }

            return StatusCode(StatusCodes.Status404NotFound);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit1(Guid id)
        {
            var maschine = await db.TabMaschineSet.FindAsync(id);
            await TryUpdateModelAsync(maschine);

            if (ModelState.IsValid)
            {
                await db.DbSave(User, maschine);
                return RedirectToAction("Index");
            }

            await MaschineHelper(false);
            return View("Create", maschine);
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
        public async Task<ActionResult> IndexMeldungProtokollEdit(Guid? id, string Prog)
        {
            if (id != null)
            {
                var meldung = await db.TabMeldungSet.Include(i => i.EMaschine)
                    .Include(f => f.EMaschine.EStandort).Include(b => b.EBediener)
                    .FirstOrDefaultAsync(f => f.Id == id);

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
        public async Task<ActionResult> IndexMeldungProtokollEdit(Guid id, string Prog)
        {
            if (ModelState.IsValid)
            {
                var meldung = await db.TabMeldungSet.FindAsync(id);
                await TryUpdateModelAsync(meldung);
                await db.SaveChangesAsync();

                return RedirectToAction("IndexMeldungProtokoll", new { Prog });
            }

            var mel = new TabMeldung();
            await TryUpdateModelAsync(mel);
            return View(mel);
        }

        [Authorize]
        public async Task<ActionResult> IndexBauteileProMaschine(Guid id)
        {
            var listeMaschinen = db.TabMaschineSet.Where(w => w.IstAktiv)
                .Select(s => new { s.Id, Value = s.MaschineName })
                .OrderBy(o => o.Value);

            return View(new SelectList(await listeMaschinen.ToListAsync(), "Id", "Value", id));
        }

        public async Task<ActionResult> IndexBauteileProMaschinePartial(Guid IdMaschine, DateTime TxtDatumVon, DateTime TxtDatumBis)
        {
            var datBis = TxtDatumBis.Date.AddDays(1);
            var bauteile = db.TabBauteilSet
                .Where(w => (w.IdMaschine == IdMaschine) && (w.StartFertigung >= TxtDatumVon.Date) && (w.StartFertigung < datBis))
                .OrderBy(o => o.StartFertigung);

            return View(await bauteile.ToListAsync());
        }

        public async Task<ActionResult> AnzeigeMaschineStatus(Guid? id)
        {
            if (id != null)
            {
                var lMaschinen = await db.TabMaschineSet
                    .Where(w => w.IstAktiv)
                    .Select(s => new { s.Id, Value = s.MaschineName })
                    .OrderBy(o => o.Value).ToListAsync();

                return View(new SelectList(lMaschinen, "Id", "Value", id));
            }

            return StatusCode(500);
        }

        public async Task<PartialViewResult> AnzeigeMaschineStatusPartial(Guid? id, string DatumAenderung = null)
        {
            var maschine = await db.TabMaschineSet.Include(i => i.EStandort).FirstOrDefaultAsync(f => f.Id == id);

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
