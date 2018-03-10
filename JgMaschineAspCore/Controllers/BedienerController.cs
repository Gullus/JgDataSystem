using JgLibDataModel;
using JgLibHelper;
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

        //// GET: TabBediener/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: TabBediener/Create
        //// Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        //// finden Sie unter https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "Id,Vorname,Nachname,NummerAusweis,Aenderung,Modifikation")] TabBediener tabBediener)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        tabBediener.Id = Guid.NewGuid();
        //        db.TabBedieners.Add(tabBediener);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    return View(tabBediener);
        //}

        public async Task<ActionResult> Edit(Guid? Id)
        {
            if (Id != null)
            {
                var bediener = await db.TabBedienerSet.FindAsync(Id);

                if (bediener != null)
                    return View(bediener);
            }

            return StatusCode(500);
        }

        // [Bind(Include = "Id,Vorname,Nachname,NummerAusweis,Aenderung,Modifikation")] TabBediener tabBediener)

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid Id)
        {
            if (ModelState.IsValid)
            {
                var bediener = await db.TabBedienerSet.FindAsync(Id);
                await TryUpdateModelAsync(bediener);
                bediener.Aenderung = DateTime.Now;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            var bed = new TabBediener();
            await TryUpdateModelAsync(bed);
            return View(bed);
        }

        // GET: TabBediener/Delete/5
        public async Task<ActionResult> Delete(Guid? Id)
        {
            if (Id != null)
            {
                var bediener = await db.TabBedienerSet.FindAsync(Id);
                if (bediener != null)
                    return View(bediener);
            }

            return StatusCode(500);
        }

        // POST: TabBediener/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid Id)
        {
            var bediener = await db.TabBedienerSet.FindAsync(Id);
            db.TabBedienerSet.Remove(bediener);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> IndexMeldungen()
        {
            ViewBag.ListeMaschinen = new SelectList(await db.TabMaschineSet.Where(w => w.IstAktiv).Select(s => new { s.Id, s.MaschineName }).ToListAsync(), "Id" , "MaschineName");
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
