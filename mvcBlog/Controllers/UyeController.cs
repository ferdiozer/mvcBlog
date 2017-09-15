﻿using mvcBlogDB.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace mvcBlog.Controllers
{
    public class UyeController : Controller
    {
        mvcblogDB db = new mvcblogDB();
        // GET: Uye
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Uye uye)
        {
            var login = db.Uyes.Where(u => u.kullaniciAdi == uye.kullaniciAdi).SingleOrDefault();
            if (login.kullaniciAdi==uye.kullaniciAdi && login.email==uye.email && login.sifre==uye.sifre)
            {

            }
            return View();
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
    }
}