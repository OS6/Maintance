using OnlineHotel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineHotel.Dao
{
    public class BANGDUYETPHONG_DAO
    {
        OnlineHotelDbContext db = null;
        public BANGDUYETPHONG_DAO()
        {
            db = new OnlineHotelDbContext();
        }

        public int Insert(BANGDUYETPHONG bangduyet)
        {
            db.BANGDUYETPHONGs.Add(bangduyet);
            return db.SaveChanges();
        }
        public List<BANGDUYETPHONG> SelectAll()
        {
            return db.BANGDUYETPHONGs.Select(p => p).ToList();
        }
    }
}