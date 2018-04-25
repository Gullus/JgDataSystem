using JgLibDataModel;
using JgLibHelper;
using JgMaschineAspCore;
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
    public class BedienerController : Controller
    {
        private JgMaschineDb db = new JgMaschineDb();

        public async Task<ActionResult> Index()
        {
            return View(await db.TabBedienerSet.ToListAsync());
        }

        private void BedienerHelper(bool isCreate)
        {
            ViewBag.FormHelper = new TFormHelper() { IsCreate = isCreate };
        }

        [HttpGet]
        public ActionResult Create()
        {
            BedienerHelper(true);
            return View(new TabBediener() { Id = Guid.NewGuid() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TabBediener bediener)
        {
            if (ModelState.IsValid)
            {
                await db.TabBedienerSet.AddAsync(bediener);
                await db.DbSave(User, bediener);
                return RedirectToAction("Index");
            }

            BedienerHelper(true);
            return View(bediener);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(Guid id)
        {
            var bediener = await db.TabBedienerSet.FindAsync(id);

            if (bediener != null)
            {
                BedienerHelper(false);
                return View("Create", bediener);
            }

            return StatusCode(StatusCodes.Status404NotFound);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit1(Guid id)
        {
            var bediener = await db.TabBedienerSet.FindAsync(id);
            await TryUpdateModelAsync(bediener);

            if (ModelState.IsValid)
            {
                await db.DbSave(User, bediener);
                return RedirectToAction("Index");
            }

            BedienerHelper(false);
            return View("Create", bediener);
        }

        // GET: TabBediener/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id != null)
            {
                var bediener = await db.TabBedienerSet.FindAsync(id);
                if (bediener != null)
                    return View(bediener);
            }

            return StatusCode(500);
        }

        // POST: TabBediener/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            var bediener = await db.TabBedienerSet.FindAsync(id);
            db.TabBedienerSet.Remove(bediener);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> IndexMeldungen()
        {
            ViewBag.ListeMaschinen = new SelectList(await db.TabMaschineSet.Where(w => w.IstAktiv).Select(s => new { s.Id, s.MaschineName }).ToListAsync(), "id", "MaschineName");
            return View();
        }

        public async Task<IActionResult> IndexMeldungenPartial(IFormCollection Erg)
        {
            var datVon = Convert.ToDateTime(Erg["TxtDatumVon"]);
            var datBis = Convert.ToDateTime(Erg["TxtDatumBis"]).AddDays(1);

            var lMeldungen = new List<ScannerMeldung>();
            if (Erg.Keys.Contains("CbAnmeldung"))
                lMeldungen.Add(ScannerMeldung.ANMELDUNG);
            if (Erg.Keys.Contains("CbReparatur"))
                lMeldungen.Add(ScannerMeldung.REPASTART);
            if (Erg.Keys.Contains("CbWartung"))
                lMeldungen.Add(ScannerMeldung.WARTSTART);
            if (Erg.Keys.Contains("CbCoilwechsel"))
                lMeldungen.Add(ScannerMeldung.COILSTART);

            var anmeldungen = db.TabMeldungSet.Include(i => i.EBediener).Include(e => e.EMaschine)
                .Where(w => (w.ZeitMeldung >= datVon) && (w.ZeitMeldung < datBis));

            if (lMeldungen.Count != 0)
                anmeldungen = anmeldungen.Where(w => lMeldungen.Contains(w.Meldung));

            if (Erg["IdMaschine"] != "-- Alle Maschinen --")
            {
                var idMaschine = Guid.Parse(Erg["IdMaschine"]);
                anmeldungen = anmeldungen.Where(w => w.IdMaschine == idMaschine);
            }

            return PartialView(await anmeldungen.OrderBy(o => o.ZeitMeldung).ToListAsync());
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
