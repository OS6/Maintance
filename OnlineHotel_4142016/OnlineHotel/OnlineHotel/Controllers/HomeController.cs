using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineHotel.Dao;
using OnlineHotel.ViewModel;
using OnlineHotel.Models;

namespace OnlineHotel.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Home()
        {
            var dao = new LOAIPHONG_DAO();
            var model = dao.SelectList();
            return View(model);
        }

        public ActionResult ListRooms()
        {
            return View();
        }
        
        public ActionResult Booking(int id)
        {
            var dao = new LOAIPHONG_KHACHHANG_DAO();
            var model = dao.Select(id);
            return View(model);
        }
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Contact(CONTACT model)
        {
            var dao = new CONTACT_DAO();
            dao.Insert(model);
            return View("Contact");
        }

        [HttpPost]

        public ActionResult Booking(LOAIPHONG_KHACHHANG model)
        {
            KHACHHANG khachhang = new KHACHHANG();
            khachhang.HoTen = model.HoTen;
            khachhang.SDT = model.SDT;
            khachhang.CMND = model.CMND;
            var daoKH = new KHACHHANG_DAO();
            daoKH.Insert(khachhang);
            BANGDUYETPHONG bangduyet = new BANGDUYETPHONG();
            bangduyet.MaKH = daoKH.GetNowID();

            bangduyet.NgayBatDauThue = Convert.ToDateTime(model.NgayBatDauThue);
            bangduyet.NgayKetThucThue = Convert.ToDateTime(model.NgayKetThucThue);
            var daoBD = new BANGDUYETPHONG_DAO();
            daoBD.Insert(bangduyet);
            return View("Home");
        }

    }
}