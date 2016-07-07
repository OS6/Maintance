﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XoSoKienThiet.DAO;
using XoSoKienThiet.DTO;

namespace XoSoKienThiet.BUS
{
    public class PHIEUDANGKYVE_BUS
    {
        PHIEUDANGKYVE_DAO _PHIEUDANGKYVE_DAO = null;
        CheckError _CheckError = null;
        public PHIEUDANGKYVE_BUS()
        {
            _PHIEUDANGKYVE_DAO = new PHIEUDANGKYVE_DAO();
            _CheckError = new CheckError();
        }

        public List<PHIEUDANGKYVE> Select()
        {
            return _PHIEUDANGKYVE_DAO.Select();
        }
        public List<PHIEUDANGKYVE_DOITAC_VIEW> SelectLKView()
        {
            return _PHIEUDANGKYVE_DAO.SelectLKView();
        }
        public PHIEUDANGKYVE Select_ID(string maphieudangky)
        {
            return _PHIEUDANGKYVE_DAO.Select_ID(maphieudangky).SingleOrDefault();
        }
        public void Update(string maphieudangky, int tongsovedangky)
        {
            _PHIEUDANGKYVE_DAO.Update(maphieudangky, tongsovedangky);
        }
        public string Insert(string madoitac, string manhanvienlap, string ngaylap, string tongsovedk)
        {
            DateTime _NgayLap;
            _NgayLap = Convert.ToDateTime(ngaylap);
            int _TongSoVeDangKy = int.Parse(tongsovedk);
            PHIEUDANGKYVE _PHIEUDANGKYVE = new PHIEUDANGKYVE(madoitac, manhanvienlap, _NgayLap, _TongSoVeDangKy);
            return _PHIEUDANGKYVE_DAO.Insert(_PHIEUDANGKYVE);
        }
        public bool IsYourCompany(string maphieudangky)
        {
            return _PHIEUDANGKYVE_DAO.IsYourCompany(maphieudangky);
        }
        public string CheckErrorBeforeInsert(string madoitac, string manhanvienlap, string ngaylap, string tongsovedk)
        {
            _CheckError = new CheckError();

            if (madoitac == "")
            {
                _CheckError.CheckErrorAvailable("Đối tác");
            }

            if (manhanvienlap == "<Null>") // do debug = "" no khong nhay vao if
            {
                _CheckError.CheckErrorAvailable("Nhân viên lập");
            }

            if(ngaylap == "")
            {
                _CheckError.CheckErrorAvailable("Ngày lập");
            }
            if (_CheckError.IsError())
            {
                return _CheckError.GetError();
            }
            else
            {
                return "";
            }
        }
    }
}
