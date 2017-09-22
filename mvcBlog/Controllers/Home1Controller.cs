using mvcBlog.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace mvcBlogDB.Controllers
{
    public class Home1Controller : Controller
    {
        mvcblogDB db = new mvcblogDB();     // DB    

        // GET: Home
        public ActionResult Index(int Page = 1)
        {
            var makales = db.Makales.OrderByDescending(m => m.id).ToPagedList(Page, 5);
            return View(makales);
        }

        public ActionResult KategoriMakale(int id)
        {
            var makales = db.Makales.Where(m => m.kategori_id == id).ToList();
            return View(makales);
        }
        public ActionResult MakaleDetay(int id)
        {
            var makales = db.Makales.Where(x => x.id == id).SingleOrDefault();
            if (makales == null)
            {
                return HttpNotFound();
            }
            return View(makales);
        }
        public ActionResult Hakkimizda()
        {
            return View();
        }
        public ActionResult Iletisim()
        {
            return View();
        }

        public ActionResult BlogAra(string Ara = null)
        {
            var aranan = db.Makales.Where(m => m.baslik.Contains(Ara) || m.icerik.Contains(Ara)).ToList();
            return View(aranan.OrderByDescending(m => m.tarih));
        }

        public ActionResult KategoriPartial()
        {
            return View(db.Kategoris.ToList());
        }

        public JsonResult YorumYap(string yorum, int makaleid)
        {
            var uyeid = Session["uyeid"];
            if (yorum == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            db.Yorums.Add(new Yorum
            {
                icerik = yorum,
                makale_id = makaleid,
                uye_id = Convert.ToInt32(uyeid),
                tarih = DateTime.Now
            });
            db.SaveChanges();

            return Json(false, JsonRequestBehavior.AllowGet);

        }
        public ActionResult SonYorumlar()
        {
            return View(db.Yorums.OrderByDescending(y => y.id).Take(5));
        }
        public ActionResult PopulerMakaleler()
        {
            return View(db.Makales.OrderByDescending(m => m.okunma).Take(5));
        }
        public ActionResult YorumSil(int id)
        {
            int uyeid = Convert.ToInt32(Session["uyeid"]);
            var yorum = db.Yorums.Where(y => y.id == id).SingleOrDefault();
            var makale = db.Makales.Where(m => m.id == yorum.makale_id).SingleOrDefault();
            if (yorum.uye_id == uyeid)
            {
                db.Yorums.Remove(yorum);
                db.SaveChanges();
                return RedirectToAction("MakaleDetay", "Home", new { id = makale.id });
            }
            else
            {
                return HttpNotFound();
            }
        }
        public ActionResult OkunmaArttir(int Makaleid)
        {
            var makale = db.Makales.Where(x => x.id == Makaleid).SingleOrDefault();
            makale.okunma += 1;
            db.SaveChanges();
            return View();
        }
    }
}