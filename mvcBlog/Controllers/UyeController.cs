using mvcBlog.Models;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace mvcBlogDB.Controllers
{
    public class UyeController : Controller
    {
        mvcblogDB db = new mvcblogDB();
        // GET: Uye
        public ActionResult Index(int id)
        {
            var uye = db.Uyes.Where(u => u.id == id).SingleOrDefault();
            if (Convert.ToInt32(Session["uyeid"])!=id)
            {
                return HttpNotFound();
            }
            return View(uye);
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Uye uye)
        {
            //kullanıcı adı ile kullanıcıyı çektik
            var login = db.Uyes.Where(u => u.kullaniciAdi == uye.kullaniciAdi).SingleOrDefault();
            if (login!= null && login.kullaniciAdi==uye.kullaniciAdi && login.email==uye.email && login.sifre==uye.sifre)
            {
                Session["uyeid"] = login.id;
                Session["kullaniciadi"] = login.kullaniciAdi;
                Session["yetkiid"] = login.yetki_id;
               return  RedirectToAction("Index", "Home");
            }

            else
            {
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session["uyeid"] = null;
            Session.Abandon();
           return RedirectToAction("Index", "Home");
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Uye uye , HttpPostedFileBase Foto)
        {
            if (ModelState.IsValid)
            {
                if (Foto!=null)
                {
                    WebImage img = new WebImage(Foto.InputStream);
                    FileInfo fotoInfo = new FileInfo(Foto.FileName);

                    string newFoto = Guid.NewGuid().ToString() + fotoInfo.Extension;
                    img.Resize(200, 200);
                    img.Save("~/Uploads/UyeFoto/" + newFoto);
                    uye.foto = "/Uploads/UyeFoto/" + newFoto;
                }
                else
                {
                    ModelState.AddModelError("fotograf", "Fotoğraf Seçin");
                }
                uye.yetki_id = 2;
                db.Uyes.Add(uye);
                db.SaveChanges();
                Session["uyeid"] = uye.id;
                Session["kullaniciadi"] = uye.kullaniciAdi;

                return RedirectToAction("Index", "Home");
            }
            return View(uye);
        }

        public ActionResult Edit(int id)
        {
            var uye = db.Uyes.Where(u => u.id == id).SingleOrDefault();
            if (uye.id!=Convert.ToInt32(Session["uyeid"]))
            {
                return HttpNotFound();
            }
            return View(uye);
        }
        [HttpPost]
        public ActionResult Edit(Uye uye,int id, HttpPostedFileBase Foto)
        {
            if (ModelState.IsValid)
            {
                var uyes = db.Uyes.Where(u => u.id == id).SingleOrDefault();
                if (Foto != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(uyes.foto)))
                    {
                        System.IO.File.Delete(Server.MapPath(uyes.foto));
                    }
                    WebImage img = new WebImage(Foto.InputStream);
                    FileInfo fotoInfo = new FileInfo(Foto.FileName);

                    string newFoto = Guid.NewGuid().ToString() + fotoInfo.Extension;
                    img.Resize(200, 200);
                    img.Save("~/Uploads/UyeFoto/" + newFoto);
                    uyes.foto = "/Uploads/UyeFoto/" + newFoto;
                }
                uyes.adSoyad = uye.adSoyad;
                uyes.email = uye.email;
                uyes.kullaniciAdi = uye.kullaniciAdi;
                uyes.sifre = uye.sifre;
              //  db.Uyes.Add(uyes);
                db.SaveChanges();
                Session["kullaniciadi"] = uye.kullaniciAdi;

                return RedirectToAction("Index", "Uye",new { id=uyes.id});
            }
            return View(uye);
        }

        public ActionResult UyeProfil(int id)
        {
            var uye = db.Uyes.Where(u => u.id == id).SingleOrDefault();
            return View(uye);
        }
    }
}