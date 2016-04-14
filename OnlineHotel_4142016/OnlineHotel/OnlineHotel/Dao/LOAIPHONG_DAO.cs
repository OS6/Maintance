using OnlineHotel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineHotel.Dao
{
    public class LOAIPHONG_DAO
    {
            OnlineHotelDbContext db = null;
            public LOAIPHONG_DAO()
            {
                db = new OnlineHotelDbContext();
            }

            public ICollection<LOAIPHONG> SelectList()
            {
                var listLOAIPHONG_ = (from p in db.LOAIPHONGs orderby p.TenLP descending select p).Take(6).ToList();
                return listLOAIPHONG_;
            }
            public string SelectDetail(int id)
            {
                var loaiphong_ = (from p in db.LOAIPHONGs where p.MaLP == id select p.ChiTiet).ToString();
                return loaiphong_;
            }
            public LOAIPHONG Select(int id)
            {
                var loaiphong_ = db.LOAIPHONGs.Find(id);
                return loaiphong_;
            }
    }
}