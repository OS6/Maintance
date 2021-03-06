﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using XoSoKienThiet.BUS;
using System.Data.Entity;
using XoSoKienThiet.DTO;
using XoSoKienThiet.REPORT;
using DevExpress.XtraReports.UI;

namespace XoSoKienThiet.PRESENT
{
    public partial class frmPhieuTraVe : DevExpress.XtraEditors.XtraForm
    {
        DOITAC_BUS _DOITAC_BUS = null;
        NHANVIEN_BUS _NHANVIEN_BUS = null;
        CT_PHIEUNHANVE_BUS _CT_PHIEUNHANVE_BUS = null;
        PHIEUTRAVE_BUS _PHIEUTRAVE_BUS = null;
        CT_PHIEUTRAVE_BUS _CT_PHIEUTRAVE_BUS = null;
        string _MaDoiTac = "", _MaPhieuNhanVe = "", _MaPhieuTraVe = "";
        bool _InPhieu = false;
        rptIn_PhieuTraVe _Report = null;
        ReportPrintTool _Tool = null;
        public frmPhieuTraVe()
        {
            InitializeComponent();
            _DOITAC_BUS = new DOITAC_BUS();
            _NHANVIEN_BUS = new NHANVIEN_BUS();
            _PHIEUTRAVE_BUS = new PHIEUTRAVE_BUS();
            _CT_PHIEUTRAVE_BUS = new CT_PHIEUTRAVE_BUS();
            _CT_PHIEUNHANVE_BUS = new CT_PHIEUNHANVE_BUS();
            #region #Load Mã phiếu nhận vé
            XoSoKienThiet.DTO.XoSoKienThietDbContext dbContext = new XoSoKienThiet.DTO.XoSoKienThietDbContext();
            // Call the Load method to get the data for the given DbSet from the database.
            dbContext.PHIEUNHANVE_DOITAC_VIEWs.Load();
            // This line of code is generated by Data Source Configuration Wizard
            bindingPhieuNhanVe.DataSource = dbContext.PHIEUNHANVE_DOITAC_VIEWs.Local.ToBindingList();
            #endregion
        }
        private void frmPhieuTraVe_Load(object sender, EventArgs e)
        {
            #region Init lookup nhân viên
            var ListNhanVien = _NHANVIEN_BUS.Select();
            lkNguoiLap.Properties.DataSource = ListNhanVien;
            lkNguoiLap.Properties.DisplayMember = "Ten";
            lkNguoiLap.Properties.ValueMember = "MaNhanVien";

            lkNguoiLap.Properties.ForceInitialize();
            lkNguoiLap.Properties.PopulateColumns();

            lkNguoiLap.Properties.Columns["MaCoCauToChuc"].Visible = false;
            lkNguoiLap.Properties.Columns["MaTaiKhoan"].Visible = false;
            lkNguoiLap.Properties.Columns["DiaChi"].Visible = false;
            lkNguoiLap.Properties.Columns["Email"].Visible = false;
            lkNguoiLap.Properties.Columns["SDT"].Visible = false;
            #endregion
        }

        private void btnDong_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        void LoadgcBASE(bool Type = false) //fasle: load lên những loai vé chưa trả, ngược lại, load loại vé trả
        {
            List<CT_PHIEUNHANVE_VIEW> ListNhan = new List<CT_PHIEUNHANVE_VIEW>();
            if (Type == false)
            {
                ListNhan = _CT_PHIEUNHANVE_BUS.SelectNotPayView(_MaPhieuNhanVe);
            }
            else
            {
                ListNhan = _CT_PHIEUNHANVE_BUS.SelectReCeiveView(_MaPhieuNhanVe);
            }
            List<CT_PHIEUTRAVE_VIEW> ListTra = new List<CT_PHIEUTRAVE_VIEW>();

            foreach (var item in ListNhan)
            {
                CT_PHIEUTRAVE_VIEW phieutrave_view = new CT_PHIEUTRAVE_VIEW();
                phieutrave_view.MaDoiTac = item.MaDoiTac;
                phieutrave_view.MaDotPhatHanh = item.MaDotPhatHanh;
                phieutrave_view.MaLoaiVe = item.MaLoaiVe;
                phieutrave_view.Ten = item.Ten;
                phieutrave_view.NgayPhatHanh = item.NgayPhatHanh;
                phieutrave_view.MenhGia = item.MenhGia;
                phieutrave_view.SoVeNhan = item.SoLuongNhan;
                phieutrave_view.HoanThanh = false;
                ListTra.Add(phieutrave_view);
            }
            gcBASE.DataSource = ListTra;
        }

        private void lkMaPhieuNhanVe_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                txtTenDoiTac.Text = lkMaPhieuNhanVe.Properties.GetDataSourceValue("Ten", lkMaPhieuNhanVe.ItemIndex).ToString();
                _MaDoiTac = lkMaPhieuNhanVe.Properties.GetDataSourceValue("MaDoiTac", lkMaPhieuNhanVe.ItemIndex).ToString();
                _MaPhieuNhanVe = lkMaPhieuNhanVe.Properties.GetDataSourceValue("MaPhieuNhanVe", lkMaPhieuNhanVe.ItemIndex).ToString();
                LoadgcBASE();
            }
            catch (Exception)
            {
            }
        }
        private void btnThemPhieu_Click(object sender, EventArgs e)
        {
            _MaPhieuTraVe = "";
            gvBASE.Columns["HoanThanh"].Visible = true;
            lkMaPhieuNhanVe.ReadOnly = false;
            deNgayLap.ReadOnly = false;
            lkNguoiLap.ReadOnly = false;
            txtTenDoiTac.Text = "";
            txtTongSoTien.Text = "0";
            txtTongSoVe.Text = "0";
            lkMaPhieuNhanVe.EditValue = null;
            lkNguoiLap.EditValue = null;
            deNgayLap.Text = "";
            _InPhieu = false;

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            LoadgcBASE();
            gvBASE.Columns["HoanThanh"].Visible = true;
            _InPhieu = false;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            LoadgcBASE(true);// load danh sách vé đã trả
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {

        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            int TongVeTra = 0;
            decimal TongTra = 0;
            DialogResult result = XtraMessageBox.Show("Bạn chắc chắn muốn thêm phiếu trả vé", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            List<CT_PHIEUTRAVE_VIEW> ListReport = new List<CT_PHIEUTRAVE_VIEW>();
            List<CT_PHIEUTRAVE_VIEW> List_CT_PhieuTraVe = new List<CT_PHIEUTRAVE_VIEW>();
            if (result == DialogResult.Yes)
            {
                string NguoiLap = "";
                try
                {
                    NguoiLap = lkNguoiLap.EditValue.ToString();
                }
                catch (Exception)
                {
                }
                if (_MaPhieuTraVe == "")
                {
                    string Error = _PHIEUTRAVE_BUS.CheckBeforeInsert(_MaPhieuNhanVe, NguoiLap, deNgayLap.Text, txtTongSoVe.Text, txtTongSoTien.Text);
                    if (Error != "")
                    {
                        XtraMessageBox.Show(Error, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        _MaPhieuTraVe = _PHIEUTRAVE_BUS.Insert(_MaPhieuNhanVe, NguoiLap, deNgayLap.Text, txtTongSoVe.Text, txtTongSoTien.Text);
                        var DataInGV = (List<CT_PHIEUTRAVE_VIEW>)gvBASE.DataSource;
                        int i = 0;
                        //Kiểm tra lỗi
                        foreach (var item in DataInGV)
                        {
                            if (gvBASE.GetRowCellValue(i, gvBASE.Columns["HoanThanh"]).ToString() == "True")
                            {
                                string ErrorCT = _CT_PHIEUTRAVE_BUS.CheckBeforeInsert(_MaPhieuTraVe, item.MaDoiTac, item.MaDotPhatHanh, item.MaLoaiVe, item.SoVeNhan.ToString(), item.SoVeTra.ToString(), item.SoTienPhaiTra.ToString());
                                if (ErrorCT != "")
                                {
                                    XtraMessageBox.Show("Chưa nhập đầy đủ thông tin trả vé", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                            i++;
                        }

                        //Insert CT
                        i = 0;
                        foreach (var item in DataInGV)
                        {
                            if (gvBASE.GetRowCellValue(i, gvBASE.Columns["HoanThanh"]).ToString() == "True")
                            {
                                _CT_PHIEUTRAVE_BUS.Insert(_MaPhieuTraVe, item.MaDoiTac, item.MaDotPhatHanh, item.MaLoaiVe, item.SoVeNhan.ToString(), item.SoVeTra.ToString(), item.SoTienPhaiTra.ToString());
                                _CT_PHIEUNHANVE_BUS.UpdateDaTra(_MaPhieuNhanVe, item.MaDoiTac, item.MaDotPhatHanh, item.MaLoaiVe);

                            }
                            i++;
                            ListReport.Add(item);
                        }
                    }
                }
                else
                {
                    var DataInGV = (List<CT_PHIEUTRAVE_VIEW>)gvBASE.DataSource;
                    int i = 0;
                    //Kiểm tra lỗi
                    foreach (var item in DataInGV)
                    {
                        if (gvBASE.GetRowCellValue(i, gvBASE.Columns["HoanThanh"]).ToString() == "True")
                        {
                            string ErrorCT = _CT_PHIEUTRAVE_BUS.CheckBeforeInsert(_MaPhieuTraVe, item.MaDoiTac, item.MaDotPhatHanh, item.MaLoaiVe, item.SoVeNhan.ToString(), item.SoVeTra.ToString(), item.SoTienPhaiTra.ToString());
                            if (ErrorCT != "")
                            {
                                XtraMessageBox.Show("Chưa nhập đầy đủ thông tin trả vé", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        i++;
                    }
                    //Insert CT
                    i = 0;
                    foreach (var item in DataInGV)
                    {
                        if (gvBASE.GetRowCellValue(i, gvBASE.Columns["HoanThanh"]).ToString() == "True")
                        {
                            _CT_PHIEUTRAVE_BUS.Insert(_MaPhieuTraVe, item.MaDoiTac, item.MaDotPhatHanh, item.MaLoaiVe, item.SoVeNhan.ToString(), item.SoVeTra.ToString(), item.SoTienPhaiTra.ToString());
                            _CT_PHIEUNHANVE_BUS.UpdateDaTra(_MaPhieuNhanVe, item.MaDoiTac, item.MaDotPhatHanh, item.MaLoaiVe);
                        }
                        i++;
                        ListReport.Add(item);
                    }
                }

                gcBASE.DataSource = _CT_PHIEUTRAVE_BUS.SelectView(_MaPhieuTraVe);
                var DataGV = (List<CT_PHIEUTRAVE_VIEW>)gvBASE.DataSource;
                foreach (var item in DataGV)
                {
                    TongVeTra += item.SoVeTra.Value;
                    TongTra += item.SoTienPhaiTra.Value;
                }
                txtTongSoTien.Text = TongVeTra.ToString();
                txtTongSoVe.Text = TongTra.ToString();
                gvBASE.Columns["HoanThanh"].Visible = false;
                lkMaPhieuNhanVe.ReadOnly = true;
                deNgayLap.ReadOnly = true;
                lkNguoiLap.ReadOnly = true;
                _DOITAC_BUS.Update_Debt(_MaDoiTac, TongTra.ToString());
                _PHIEUTRAVE_BUS.Update(_MaPhieuTraVe, TongVeTra, TongTra);

                _InPhieu = true;
                _Report = new rptIn_PhieuTraVe();
                _Report.DataSource = ListReport;
            }
        }

        private void gvBASE_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (gvBASE.FocusedColumn.FieldName == "HoanThanh")
            {
                if (e.Value.ToString() == "True")
                {
                    gvBASE.Columns["SoVeTra"].OptionsColumn.AllowEdit = true;
                }
                else
                {
                    gvBASE.Columns["SoVeTra"].OptionsColumn.AllowEdit = false;
                }
            }
            int SoTienPhaiTra = 0;
            int SoLuongNhan = 0;
            if (gvBASE.FocusedColumn.FieldName == "SoVeTra")
            {
                SoLuongNhan = Convert.ToInt32(gvBASE.GetRowCellValue(gvBASE.FocusedRowHandle, gvBASE.Columns["SoVeNhan"]));
                try
                {
                    if (e.Value != "" && Convert.ToInt32(e.Value) <= SoLuongNhan)
                    {
                        int MenhGia = Convert.ToInt32(gvBASE.GetRowCellValue(gvBASE.FocusedRowHandle, gvBASE.Columns["MenhGia"]));
                        float TiLeHoaHong = _DOITAC_BUS.GetPercentage(_MaDoiTac);
                        SoTienPhaiTra = Convert.ToInt32((SoLuongNhan - Convert.ToInt32(e.Value)) * MenhGia * (1 - TiLeHoaHong));
                        gvBASE.SetRowCellValue(gvBASE.FocusedRowHandle, gvBASE.Columns["SoTienPhaiTra"], SoTienPhaiTra);
                    }
                    else
                    {
                        gvBASE.SetRowCellValue(gvBASE.FocusedRowHandle, gvBASE.Columns["SoTienPhaiTra"], 0);
                    }
                }
                catch (Exception)
                {
                }

            }
        }

        private void gvBASE_InvalidValueException(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
        {
            if (gvBASE.FocusedColumn.FieldName == "SoVeTra")
            {
                if (e.Value as String == "")
                {
                    e.ErrorText = "Số vé trả không được rỗng";
                }
            }
        }

        private void gvBASE_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (gvBASE.FocusedColumn.FieldName == "SoVeTra")
            {
                int SoVeTra = 0;
                if (!Int32.TryParse(e.Value as String, out SoVeTra))
                {
                    e.Valid = false;
                    e.ErrorText = "Số vé trả là số nguyên dương";
                }
                else
                {
                    if (SoVeTra > Convert.ToInt32(gvBASE.GetRowCellValue(gvBASE.FocusedRowHandle, gvBASE.Columns["SoVeNhan"])))
                    {
                        e.Valid = false;
                        e.ErrorText = "Số vé trả không lớn hơn số vé nhận";
                    }
                }
            }
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            if (!_InPhieu)
            {
                XtraMessageBox.Show("Phải điền thông tin đầy đủ và lưu phiếu trước khi in!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                _Report.rptLabelTenDoiTac.Text = "Tên đối tác: " + txtTenDoiTac.Text;
                _Report.rptLabelNgay.Text = "Tp. Hồ Chí Minh, ngày " + DateTime.Now.Day.ToString() + " tháng " + DateTime.Now.Month.ToString() + " năm " + DateTime.Now.Year.ToString();
                _Report.rptLabelTongSoVe.Text = "Tổng số vé: " + txtTongSoVe.Text;
                _Report.rptLabelTongSoTien.Text = "Tổng số tiền: " + txtTongSoTien.Text;
                _Tool = new ReportPrintTool(_Report);
                _Tool.ShowPreview();
            }
        }
    }
}