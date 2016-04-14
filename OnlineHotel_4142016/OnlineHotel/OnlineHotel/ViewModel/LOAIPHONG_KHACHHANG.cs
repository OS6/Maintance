using OnlineHotel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace OnlineHotel.ViewModel
{
    public class LOAIPHONG_KHACHHANG
    {
        public int idLP { get; set; }

        [StringLength(50)]
        public string TenLP { get; set; }

        public int? DonGia { get; set; }

        [StringLength(500)]
        public string GioiThieu { get; set; }

        [StringLength(250)]
        public string HinhAnh { get; set; }

        [StringLength(1000)]
        public string ChiTiet { get; set; }
        [StringLength(50)]
        public string HoTen { get; set; }

        [StringLength(10)]
        public string CMND { get; set; }

        [StringLength(13)]
        public string SDT { get; set; }

       // [Column(TypeName = "smalldatetime")]
       // public DateTime? NgayBatDauThue { get; set; }

       // [Column(TypeName = "smalldatetime")]
       // public DateTime? NgayKetThucThue { get; set; }
         [StringLength(13)]
        public string NgayBatDauThue { get; set; }
         [StringLength(13)]
        public string NgayKetThucThue { get; set; }
        public LOAIPHONG_KHACHHANG()
        {
        }
    }
}