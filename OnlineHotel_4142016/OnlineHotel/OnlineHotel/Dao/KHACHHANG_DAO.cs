using OnlineHotel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineHotel.Dao
{
    public class KHACHHANG_DAO
    {
        OnlineHotelDbContext db = null;
        public KHACHHANG_DAO()
        {
            db = new OnlineHotelDbContext();
        }

        public int Insert(KHACHHANG khachhang)
        {
            db.KHACHHANGs.Add(khachhang);
            return db.SaveChanges();
        }
        public ICollection<KHACHHANG> SelectList()
        {
            var listKHACHHANG_ = (from p in db.KHACHHANGs select p).ToList();
            return listKHACHHANG_;
        }
        public KHACHHANG Select(int id)
        {
            var khachhang_ = db.KHACHHANGs.Find(id);
            return khachhang_;
        }

        public int GetNowID()
        {
            int id = db.KHACHHANGs.Max(p => p.MaKH);
            return id;
        }

        public KHACHHANG Select()
        {
            //var khachhang_ = db.KHACHHANGs.Find();
            //return khachhang_;
            return null;
        }
    }
}