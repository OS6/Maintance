using OnlineHotel.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineHotel.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Admin/Home/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BangDuyetPhong()
        {
            var dao = new BANGDUYETPHONG_DAO();
            var model = dao.SelectAll();
            return View(model);
        }

       

	}
}