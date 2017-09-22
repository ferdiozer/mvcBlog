using System.Web.Mvc;
using mvcBlog.Models;
using System.Linq;

namespace mvcBlogDB.Controllers
{
    public class AdminController : Controller
    {
        mvcblogDB db = new mvcblogDB();
        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.MakaleSayisi = db.Makales.Count();
            ViewBag.YorumSayisi = db.Yorums.Count();
            ViewBag.UyeSayisi = db.Uyes.Count();
            ViewBag.KategoriSayisi = db.Kategoris.Count();
            return View();
        }
    }
}