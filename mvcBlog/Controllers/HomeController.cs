using mvcBlog.Models;
using System.Linq;
using System.Web.Mvc;

namespace mvcBlogDB.Controllers
{
    public class HomeController : Controller
    {
        mvcblogDB db = new mvcblogDB();     // DB    

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Hakkimizda()
        {
            return View();
        }
        public ActionResult Iletisim()
        {
            return View();
        }

        public ActionResult KategoriPartial()
        {
            return View(db.Kategoris.ToList());
        }
    }
}