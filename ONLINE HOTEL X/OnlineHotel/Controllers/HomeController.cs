using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineHotel.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            var _PHONG_DAO = new PHONG_DAO();
            ViewBag.FeatureRooms = _PHONG_DAO.ListFeatureRooms();
            return View();
        }

        public ActionResult ListRooms()
        {
            var _PHONG_DAO = new PHONG_DAO();
            ViewBag.ListRooms1 = _PHONG_DAO.ListRooms(4);
            ViewBag.ListRooms2 = _PHONG_DAO.ListRooms(5);
            ViewBag.ListRooms3 = _PHONG_DAO.ListRooms(6);
            return View();
        }
        public ActionResult Booking()
        {
            return View();
        }

        public ActionResult SearchRooms()
        {
            var _PHONG_DAO = new PHONG_DAO();
            ViewBag.SearchRooms = _PHONG_DAO.SearchRooms(true);
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Contact(CONTACT contact)
        {
            if (ModelState.IsValid)
            {
                var dao = new CONTACT_DAO();
                int ID = dao.Insert(contact);
                if (ID > 0)
                {
                    //SetAlert("Thêm account thành công.", "success");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Liên hệ không thành công.");
                }
            }
            return View("Contact");
        }
	}
}