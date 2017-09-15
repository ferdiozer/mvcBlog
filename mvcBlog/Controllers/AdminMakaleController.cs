using mvcBlog.Models;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace mvcBlogDB.Controllers
{
    public class AdminMakaleController : Controller
    {
        mvcblogDB db = new mvcblogDB();

        // GET: AdminMakale
        public ActionResult Index()
        {
            var makales = db.Makales.ToList();
            return View(makales);
        }

        // GET: AdminMakale/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminMakale/Create
        public ActionResult Create()
        {
            ViewBag.kategori_id = new SelectList(db.Kategoris, "id", "ad");
            return View();
        }

        // POST: AdminMakale/Create
        [HttpPost]
        public ActionResult Create(Makale makale,string etiketler,HttpPostedFileBase Foto)
        {
            if (ModelState.IsValid)
            {

                if (Foto != null)
                {
                    WebImage img = new WebImage(Foto.InputStream);
                    FileInfo fotoInfo = new FileInfo(Foto.FileName);

                    string newFoto = Guid.NewGuid().ToString() + fotoInfo.Extension;
                    img.Resize(800, 350);
                    img.Save("~/Uploads/MakaleFoto/" + newFoto);
                    makale.foto = "/Uploads/MakaleFoto/" + newFoto;
                }
                if (etiketler != null)
                {
                    string[] etiketDizi = etiketler.Split(',');
                    foreach (var item in etiketDizi)
                    {
                        var yeniEtiket = new Etiket
                        {
                            ad = item
                        };
                        db.Etikets.Add(yeniEtiket);
                        makale.Etikets.Add(yeniEtiket);

                    }
                }
                db.Makales.Add(makale);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
                        
                return View(makale);         
        }

        // GET: AdminMakale/Edit/5
        public ActionResult Edit(int id)
        {
            var makales = db.Makales.Where(m => m.id == id).SingleOrDefault();
            if (makales==null)
            {
                return HttpNotFound();
            }
            ViewBag.kategori_id = new SelectList(db.Kategoris, "id", "ad",makales.kategori_id);
            return View(makales);
        }

        // POST: AdminMakale/Edit/5
        [HttpPost]
        public ActionResult Edit(int id,HttpPostedFileBase Foto, Makale makale)
        {
            try
            {
               // ViewBag.kategori_id = new SelectList(db.Kategoris, "id", "ad", makale.kategori_id);
                var makales = db.Makales.Where(m => m.id == id).SingleOrDefault();
                if (Foto!=null)
                {
                    if (System.IO.File.Exists(Server.MapPath(makales.foto)))
                    {
                        System.IO.File.Delete(Server.MapPath(makales.foto));
                    }
                    WebImage img = new WebImage(Foto.InputStream);
                    FileInfo fotoInfo = new FileInfo(Foto.FileName);

                    string newFoto = Guid.NewGuid().ToString() + fotoInfo.Extension;
                    img.Resize(800, 350);
                    img.Save("~/Uploads/MakaleFoto/" + newFoto);
                    makales.foto = "/Uploads/MakaleFoto/" + newFoto;

                }

                makales.baslik = makale.baslik;
                makales.icerik = makale.icerik;
                makales.kategori_id = makale.kategori_id;
                db.SaveChanges();
                return RedirectToAction("Index");
              //  return View();
               
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminMakale/Delete/5
        public ActionResult Delete(int id)
        {
            var makale = db.Makales.Where(m => m.id == id).SingleOrDefault();
            if (makale==null)
            {
                return HttpNotFound();
            }
            return View(makale);
        }

        // POST: AdminMakale/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var makale = db.Makales.Where(m => m.id == id).SingleOrDefault();
                if (makale == null)
                {
                    return HttpNotFound();
                }
                if (System.IO.File.Exists(Server.MapPath(makale.foto)))
                {
                    System.IO.File.Delete(Server.MapPath(makale.foto));
                }
                foreach (var item in makale.Yorums.ToList())
                {
                    db.Yorums.Remove(item);
                }
                foreach (var item in makale.Etikets.ToList())
                {
                    db.Etikets.Remove(item);
                }
                db.Makales.Remove(makale);
                db.SaveChanges();


                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
