using JgLibDataModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JgMaschineWeb.Controllers
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
            if (Id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var bediener = await db.TabBedienerSet.FindAsync(Id);
            if (bediener == null)
                return HttpNotFound();

            return View(bediener);
        }

        // [Bind(Include = "Id,Vorname,Nachname,NummerAusweis,Aenderung,Modifikation")] TabBediener tabBediener)

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id, NummerAusweis,Aenderung,Modifikation")] Guid Id)
        {
            if (ModelState.IsValid)
            {
                var bediener = await db.TabBedienerSet.FindAsync(Id);
                TryUpdateModel(bediener);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            var bed = new TabBediener();
            TryUpdateModel(bed);
            return View(bed);
        }

        // GET: TabBediener/Delete/5
        public async Task<ActionResult> Delete(Guid? Id)
        {
            if (Id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var bediener = await db.TabBedienerSet.FindAsync(Id);
            if (bediener == null)
                return HttpNotFound();

            return View(bediener);
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
