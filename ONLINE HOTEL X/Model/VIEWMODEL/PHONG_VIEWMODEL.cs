using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.VIEWMODEL
{
    public class PHONG_VIEWMODEL
    {
        public int MaPhong { get; set; }
        public int MaLP { get; set; }
        public string TenPhong { get; set; }
        public bool? TinhTrang { get; set; }
        public string ThongTin { get; set; }
        public string Href { get; set; }
        public bool? Feature { get; set; }
        public string Picture { get; set; }
        public int? DonGia { get; set; }
    }
}
