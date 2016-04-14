using OnlineHotel.Models;
using OnlineHotel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineHotel.Dao
{
    public class LOAIPHONG_KHACHHANG_DAO
    {

        LOAIPHONG_KHACHHANG LoaiPhong_KH_VM = null; 
        public LOAIPHONG_KHACHHANG_DAO()
        {
            LoaiPhong_KH_VM = new LOAIPHONG_KHACHHANG();
        }
        public int Insert(int idLP, KHACHHANG khachhang)
        {
            if(khachhang != null)
            {
                KHACHHANG_DAO daoKH = new KHACHHANG_DAO();
                return daoKH.Insert(khachhang); 
            }
            return -1;
        }
        public LOAIPHONG_KHACHHANG Select(int id)
        {
            var model = new LOAIPHONG_KHACHHANG();
            var phong = new LOAIPHONG();
            var dao = new LOAIPHONG_DAO();
            phong = dao.Select(id);
            model.idLP = phong.MaLP;
            model.TenLP = phong.TenLP;
            model.DonGia = phong.DonGia;
            model.ChiTiet = phong.ChiTiet;
            model.HinhAnh = phong.HinhAnh;
            return model;
        }
    }
}