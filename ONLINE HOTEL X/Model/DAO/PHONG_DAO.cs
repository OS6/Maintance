using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using Model.VIEWMODEL;

namespace Model.DAO
{
    public class PHONG_DAO
    {
        OnlineHotelDbContext db = null;
        public PHONG_DAO()
        {
            db = new OnlineHotelDbContext();
        }

        public List<PHONG_VIEWMODEL> ListFeatureRooms()
        {
            var model = from a in db.PHONGs
                        join b in db.LOAIPHONGs
                        on a.MaLP equals b.MaLP
                        where a.Feature == true
                        select new PHONG_VIEWMODEL()
                        {
                            MaPhong = a.MaPhong,
                            MaLP = b.MaLP,
                            TenPhong = a.TenPhong,
                            TinhTrang = a.TinhTrang,
                            ThongTin = b.ThongTin,
                            Href = a.Href,
                            Feature = a.Feature,
                            Picture = b.HinhAnh,
                            DonGia = b.DonGia
                        };
            model.OrderBy(x => x.MaLP);
            return model.ToList();
        }

        public List<PHONG_VIEWMODEL> ListRooms(int type)
        {
            var model = from a in db.PHONGs
                        join b in db.LOAIPHONGs
                        on a.MaLP equals b.MaLP
                        where b.MaLP == type
                        select new PHONG_VIEWMODEL()
                        {
                            MaPhong = a.MaPhong,
                            MaLP = b.MaLP,
                            TenPhong = a.TenPhong,
                            TinhTrang = a.TinhTrang,
                            ThongTin = b.ThongTin,
                            Href = a.Href,
                            Feature = a.Feature,
                            Picture = b.HinhAnh,
                            DonGia = b.DonGia
                        };
            model.OrderBy(x => x.MaLP);
            return model.ToList();
        }

        public List<PHONG_VIEWMODEL> SearchRooms(bool available)
        {
            var model = from a in db.PHONGs
                        join b in db.LOAIPHONGs
                        on a.MaLP equals b.MaLP
                        where a.TinhTrang == available
                        select new PHONG_VIEWMODEL()
                        {
                            MaPhong = a.MaPhong,
                            MaLP = b.MaLP,
                            TenPhong = a.TenPhong,
                            TinhTrang = a.TinhTrang,
                            ThongTin = b.ThongTin,
                            Href = a.Href,
                            Feature = a.Feature,
                            Picture = b.HinhAnh,
                            DonGia = b.DonGia
                        };
            model.OrderBy(x => x.MaLP);
            return model.ToList();
        }
    }
}
