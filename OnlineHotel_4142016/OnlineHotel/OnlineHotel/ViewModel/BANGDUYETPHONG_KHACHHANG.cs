using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineHotel.ViewModel
{
    public class BANGDUYETPHONG_KHACHHANG
    {
        public int MaKH { get; set; }

        [StringLength(50)]
        public string HoTen { get; set; }

    }
}