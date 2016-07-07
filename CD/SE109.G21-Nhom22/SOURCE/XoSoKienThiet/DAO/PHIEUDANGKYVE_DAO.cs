﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XoSoKienThiet.DTO;

namespace XoSoKienThiet.DAO
{
    public class PHIEUDANGKYVE_DAO
    {
        XoSoKienThietDbContext _Context = null;
        public PHIEUDANGKYVE_DAO()
        {
            _Context = new XoSoKienThietDbContext();
        }

        public List<PHIEUDANGKYVE> Select_ID(string maphieudangky)
        {
            var _MaPhieuDangKy = new SqlParameter("@MaPhieuDangKy", SqlDbType.NChar, 10)
            {
                Value = maphieudangky
            };
            return _Context.Database.SqlQuery<PHIEUDANGKYVE>("PHIEUDANGKYVE_Sel_ID @MaPhieuDangKy ", _MaPhieuDangKy).ToList();
        }

        public List<PHIEUDANGKYVE> Select()
        {
            return _Context.Database.SqlQuery<PHIEUDANGKYVE>("PHIEUDANGKYVE_Sel").ToList();
        }
        public List<PHIEUDANGKYVE_DOITAC_VIEW> SelectLKView()
        {
            return _Context.Database.SqlQuery<PHIEUDANGKYVE_DOITAC_VIEW>("PHIEUDANGKYVE_DOITAC_VIEW_Sel ").ToList();
        }

        public void Update(string maphieudangky, int tongsovedangky)
        {
             var MaPhieuDangKy = new SqlParameter("@MaPhieuDangKy", SqlDbType.NChar, 10)
            {
                Value =maphieudangky
            };
             var TongSoVeDangKy = new SqlParameter("@TongSoVeDangKy", SqlDbType.Int)
             {
                 Value = tongsovedangky
             };
             _Context.Database.ExecuteSqlCommand("PHIEUDANGKYVE_Upd @MaPhieuDangKy, @TongSoVeDangKy",MaPhieuDangKy, TongSoVeDangKy);
        }
        public string Insert(PHIEUDANGKYVE phieudangkyve)
        {
            var _MaDoiTac = new SqlParameter("@MaDoiTac", SqlDbType.NChar, 10)
            {
                Value = phieudangkyve.MaDoiTac
            };
            var _NgayLap = new SqlParameter("@NgayLap", SqlDbType.DateTime)
            {
                Value = phieudangkyve.NgayLap
            };
            var _MaNhanVienLap = new SqlParameter("@MaNhanVienLap", SqlDbType.NChar, 10)
            {
                Value = phieudangkyve.MaNhanVienLap
            };
            var _TongSoVeDangKy = new SqlParameter("@TongSoVeDangKy", SqlDbType.Int)
            {
                Value = phieudangkyve.TongSoVeDangKy
            };
            var _MaPhieuDangKyVe = new SqlParameter("@MaPhieuDangKyVe", SqlDbType.NChar, 10)
            {
                Direction = ParameterDirection.Output
            };
            _Context.Database.ExecuteSqlCommand("PHIEUDANGKYVE_Ins @MaDoiTac, @MaNhanVienLap,@NgayLap, @TongSoVeDangKy,@MaPhieuDangKyVe output",
                                                                        _MaDoiTac, _MaNhanVienLap, _NgayLap, _TongSoVeDangKy, _MaPhieuDangKyVe);
            return _MaPhieuDangKyVe.Value.ToString();
        }

        public bool IsYourCompany(string maphieudangky)
        {
            var MaPhieuDangKyVe = new SqlParameter("@MaPhieuDangKyVe", SqlDbType.NChar, 10)
            {
                Value = maphieudangky
            };
            var IsYourCpmpany = new SqlParameter("@IsYourCompany", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            _Context.Database.ExecuteSqlCommand("PHIEUDANGKYVE_IsYourCompany @MaPhieuDangKyVe, @IsYourCompany output",
                                                                        MaPhieuDangKyVe, IsYourCpmpany);
            return  Convert.ToBoolean(IsYourCpmpany.Value.ToString());
        }
    }
}
