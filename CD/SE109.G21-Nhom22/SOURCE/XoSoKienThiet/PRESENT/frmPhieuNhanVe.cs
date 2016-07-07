using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XoSoKienThiet.BUS;
using System.Data.Entity;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using XoSoKienThiet.DTO;
using XoSoKienThiet.REPORT;
using DevExpress.XtraReports.UI;

namespace XoSoKienThiet.PRESENT
{
    public partial class frmPhieuNhanVe : DevExpress.XtraEditors.XtraForm
    {

        NHANVIEN_BUS _NHANVIEN_BUS = null;
        CT_PHIEUDANGKYVE_BUS _CT_PHIEUDANGKYVE_BUS = null;
        DOITAC_BUS _DOITAC_BUS = null;
        CT_PHIEUNHANVE_BUS _CT_PHIEUNHANVE_BUS = null;
        LOAIVE_BUS _LOAIVE_BUS = null;
        PHIEUNHANVE_BUS _PHIEUNHANVE_BUS = null;
        PHIEUDANGKYVE_BUS _PHIEUDANGKYVE_BUS = null;
        string _MaPhieuNhanVe = "";
        string _MaPhieuDangKy = "";
        int _Type = 0;
        bool _InPhieu = false;
        rptIn_PhieuNhanVe _Report = null;
        ReportPrintTool _Tool = null;
        string _MaDoiTac = "";
        public frmPhieuNhanVe()
        {
            InitializeComponent();
            _NHANVIEN_BUS = new NHANVIEN_BUS();
            _CT_PHIEUDANGKYVE_BUS = new CT_PHIEUDANGKYVE_BUS();
            _DOITAC_BUS = new DOITAC_BUS();
            _CT_PHIEUNHANVE_BUS = new CT_PHIEUNHANVE_BUS();
            _LOAIVE_BUS = new LOAIVE_BUS();
            _PHIEUNHANVE_BUS = new PHIEUNHANVE_BUS();
            _PHIEUDANGKYVE_BUS = new PHIEUDANGKYVE_BUS();

            #region #Hiển thị txtTenDoiTac theo MaPhieuDangKy
            //XoSoKienThiet.DTO.XoSoKienThietDbContext dbContext = new XoSoKienThiet.DTO.XoSoKienThietDbContext();
           // dbContext.PHIEUDANGKYVE_DOITAC_VIEWs.Load();
            bindingMaPhieuDangKy.DataSource = _PHIEUDANGKYVE_BUS.SelectLKView();
            #endregion
        }

        private void frmPhieuNhanVe_Load(object sender, EventArgs e)
        {
            #region #Load danh sách nhân viên
            var _ListNhanVien = _NHANVIEN_BUS.Select();
            lkNguoiLap.Properties.DataSource = _ListNhanVien;
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
        void LoadgcBASE(bool Type = false) // load danh sách loại vé đăng ký mà chưa nhận nếu phiếu đăng ký là của đại lý
        // load danh sách phiếu đã đăng ký chưa nhận của rồi điền số vé nhận vào nếu là công ty      
        {
            List<CT_PHIEUDANGKYVE_VIEW> ListDangKy = new List<CT_PHIEUDANGKYVE_VIEW>();
            if (Type == false)
            {
                ListDangKy = _CT_PHIEUDANGKYVE_BUS.SelectViewNotReCeive(_MaPhieuDangKy);
            }
            else
            {
                ListDangKy = _CT_PHIEUDANGKYVE_BUS.SelectViewReCeive(_MaPhieuDangKy);
            }
            List<CT_PHIEUNHANVE_VIEW> ListNhan = new List<CT_PHIEUNHANVE_VIEW>();

            int SoVeNhan = 0;
            float TiLeHoaHong = 0;
            decimal ThanhTien = 0, MenhGia = 0;

            foreach (var item in ListDangKy)
            {
                //Gán những thuộc tính có sẵn
                CT_PHIEUNHANVE_VIEW phieunhanve_view = new CT_PHIEUNHANVE_VIEW();
                phieunhanve_view.MaDoiTac = item.MaDoiTac;
                phieunhanve_view.MaDotPhatHanh = item.MaDotPhatHanh;
                phieunhanve_view.MaLoaiVe = item.MaLoaiVe;
                phieunhanve_view.Ten = item.Ten;
                phieunhanve_view.NgayPhatHanh = item.NgayPhatHanh;
                phieunhanve_view.MenhGia = item.MenhGia;
                phieunhanve_view.SoLuongDangKy = item.SoVeDangKy;
                phieunhanve_view.DaTra = false;
                ListNhan.Add(phieunhanve_view);

                if (_PHIEUDANGKYVE_BUS.IsYourCompany(_MaPhieuDangKy) == true)
                {
                    // do nothing
                    gvBASE.Columns["SoLuongNhan"].OptionsColumn.AllowEdit = true;
                }
                else
                {
                    // Tính thành tiền và số lượng nhận
                    if (_DOITAC_BUS.IsYourCompany(item.MaDoiTac) == false)
                    {
                        try
                        {
                            SoVeNhan = Convert.ToInt32(item.SoVeDangKy * _CT_PHIEUNHANVE_BUS.PercentageAmountofTicketReceive(item.MaDoiTac, item.MaDotPhatHanh, item.MaLoaiVe));
                        }
                        catch (Exception)
                        {
                            XtraMessageBox.Show("Loại vé chưa được nhận", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        SoVeNhan = Convert.ToInt32(item.SoVeDangKy * _DOITAC_BUS.GetPercentageConsume(lkMaPhieuDangKy.Properties.GetDataSourceValue("MaDoiTac", lkMaPhieuDangKy.ItemIndex).ToString()));
                    }
                    TiLeHoaHong = _DOITAC_BUS.GetPercentage(lkMaPhieuDangKy.Properties.GetDataSourceValue("MaDoiTac", lkMaPhieuDangKy.ItemIndex).ToString());
                    MenhGia = _LOAIVE_BUS.GetPrice(item.MaLoaiVe);
                    ThanhTien = Convert.ToDecimal((1 - Convert.ToDecimal(TiLeHoaHong)) * MenhGia * SoVeNhan);
                    phieunhanve_view.SoLuongNhan = SoVeNhan;
                    phieunhanve_view.ThanhTien = ThanhTien;
                }
            }
            gcBASE.DataSource = ListNhan;
        }
        private void lkMaPhieuDangKy_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                _MaDoiTac = lkMaPhieuDangKy.Properties.GetDataSourceValue("MaDoiTac", lkMaPhieuDangKy.ItemIndex).ToString();
                _MaPhieuDangKy = lkMaPhieuDangKy.Properties.GetDataSourceValue("MaPhieuDangKy", lkMaPhieuDangKy.ItemIndex).ToString();
                txtTenDoiTac.Text = _DOITAC_BUS.GetTenDoiTac(lkMaPhieuDangKy.Properties.GetDataSourceValue("MaDoiTac", lkMaPhieuDangKy.ItemIndex).ToString());
                LoadgcBASE();
            }
            catch (Exception)
            {
            }
        }


        private void gvBASE_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            int SoLuongNhan = 0;
            float ThanhTien = 0;
            if (gvBASE.FocusedColumn.FieldName == "DaTra")
            {
                if (e.Value.ToString() == "True")
                {
                    gvBASE.SetRowCellValue(gvBASE.FocusedRowHandle, gvBASE.Columns["DaTra"], true);
                    SoLuongNhan = Convert.ToInt32(gvBASE.GetRowCellValue(gvBASE.FocusedRowHandle, gvBASE.Columns["SoLuongNhan"]));
                    ThanhTien = Convert.ToInt32(gvBASE.GetRowCellValue(gvBASE.FocusedRowHandle, gvBASE.Columns["ThanhTien"]));
                }
                else
                {
                    gvBASE.SetRowCellValue(gvBASE.FocusedRowHandle, gvBASE.Columns["DaTra"], false);
                    Convert.ToInt32(gvBASE.GetRowCellValue(gvBASE.FocusedRowHandle, gvBASE.Columns["SoLuongNhan"]));
                    SoLuongNhan = -Convert.ToInt32(gvBASE.GetRowCellValue(gvBASE.FocusedRowHandle, gvBASE.Columns["SoLuongNhan"]));
                    ThanhTien = -Convert.ToInt32(gvBASE.GetRowCellValue(gvBASE.FocusedRowHandle, gvBASE.Columns["ThanhTien"]));
                }
            }
            txtTongSoVe.Text = (Convert.ToInt32(txtTongSoVe.Text) + SoLuongNhan).ToString();
            txtTongSoTien.Text = (Convert.ToInt32(txtTongSoTien.Text) + ThanhTien).ToString();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            _Type = 0;
            _InPhieu = false;
            gvBASE.Columns["DaTra"].Visible = true;
            LoadgcBASE();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {

            int TongSoVe = 0;
            decimal TongTien = 0;
            List<CT_PHIEUNHANVE_VIEW> ListReport = new List<CT_PHIEUNHANVE_VIEW>();
            List<CT_PHIEUNHANVE_VIEW> List_CT_PhieuTraVe = new List<CT_PHIEUNHANVE_VIEW>();
            #region Công ty nhận vé từ công ty cung ứng
            if (_PHIEUDANGKYVE_BUS.IsYourCompany(_MaPhieuDangKy) == true)
            {
                DialogResult result = XtraMessageBox.Show("Bạn có chắc chắn muốn thêm phiếu nhận vé ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

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
                    if (_MaPhieuNhanVe == "")
                    {
                        string Error = _PHIEUNHANVE_BUS.CheckBeforeInsert(_MaPhieuDangKy, txtTongSoVe.Text, deNgayLap.Text, NguoiLap, txtTongSoTien.Text);
                        if (Error != "")
                        {
                            XtraMessageBox.Show(Error, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            _MaPhieuNhanVe = _PHIEUNHANVE_BUS.Insert(_MaPhieuDangKy, txtTongSoVe.Text, deNgayLap.Text, lkNguoiLap.GetColumnValue("MaNhanVien").ToString(), txtTongSoTien.Text);
                            var DataInGV = (List<CT_PHIEUNHANVE_VIEW>)gvBASE.DataSource;
                            int i = 0;
                            //Kiểm tra lỗi

                            foreach (var item in DataInGV)
                            {
                                if (gvBASE.GetRowCellValue(i, gvBASE.Columns["DaTra"]).ToString() == "True")
                                {
                                    string ErrorCT = _CT_PHIEUNHANVE_BUS.CheckBeforeInsert(_MaPhieuNhanVe, item.MaDoiTac, item.MaDotPhatHanh, item.MaLoaiVe, item.SoLuongDangKy.ToString(), item.SoLuongNhan.ToString(), item.ThanhTien.ToString());
                                    _CT_PHIEUDANGKYVE_BUS.UpdateDaNhan(_MaPhieuDangKy, item.MaDoiTac, item.MaDotPhatHanh, item.MaLoaiVe);
                                    if (ErrorCT != "")
                                    {
                                        XtraMessageBox.Show("Chưa nhập đầy đủ thông tin nhận vé", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                }
                                i++;
                            }
                            //Insert CT
                            i = 0;
                            foreach (var item in DataInGV)
                            {
                                if (gvBASE.GetRowCellValue(i, gvBASE.Columns["DaTra"]).ToString() == "True")
                                {
                                    _CT_PHIEUNHANVE_BUS.Insert(_MaPhieuNhanVe, item.MaDoiTac, item.MaDotPhatHanh, item.MaLoaiVe, item.SoLuongDangKy.ToString(), item.SoLuongNhan.ToString(), item.ThanhTien.ToString());
                                    _CT_PHIEUDANGKYVE_BUS.UpdateDaNhan(_MaPhieuDangKy, item.MaDoiTac, item.MaDotPhatHanh, item.MaLoaiVe);
                                }
                                i++;
                                ListReport.Add(item);
                            }
                        }
                    }
                    else
                    {
                        var DataInGV = (List<CT_PHIEUNHANVE_VIEW>)gvBASE.DataSource;
                        //Kiểm tra lỗi
                        int i = 0;
                        foreach (var item in DataInGV)
                        {
                            if (gvBASE.GetRowCellValue(i, gvBASE.Columns["DaTra"]).ToString() == "True")
                            {
                                string ErrorCT = _CT_PHIEUNHANVE_BUS.CheckBeforeInsert(_MaPhieuNhanVe, item.MaDoiTac, item.MaDotPhatHanh, item.MaLoaiVe, item.SoLuongDangKy.ToString(), item.SoLuongNhan.ToString(), item.ThanhTien.ToString());
                                if (ErrorCT != "")
                                {
                                    XtraMessageBox.Show("Chưa nhập đầy đủ thông tin nhận vé", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                            i++;
                        }
                        //Insert CT
                        i = 0;
                        foreach (var item in DataInGV)
                        {
                            if (gvBASE.GetRowCellValue(i, gvBASE.Columns["DaTra"]).ToString() == "True")
                            {
                                _CT_PHIEUNHANVE_BUS.Insert(_MaPhieuNhanVe, item.MaDoiTac, item.MaDotPhatHanh, item.MaLoaiVe, item.SoLuongDangKy.ToString(), item.SoLuongNhan.ToString(), item.ThanhTien.ToString());
                                _CT_PHIEUDANGKYVE_BUS.UpdateDaNhan(_MaPhieuDangKy, item.MaDoiTac, item.MaDotPhatHanh, item.MaLoaiVe);
                            }
                            i++;
                            ListReport.Add(item);
                        }
                    }
                    //  gcBASE.DataSource = List_CT_PhieuNhanVe;
                    gvBASE.Columns["DaTra"].Visible = false;
                    gcBASE.DataSource = _CT_PHIEUNHANVE_BUS.SelectView(_MaPhieuNhanVe);
                    var DataGV = (List<CT_PHIEUNHANVE_VIEW>)gvBASE.DataSource;
                    foreach (var item in DataGV)
                    {
                        TongTien += item.ThanhTien.Value;
                        TongSoVe += item.SoLuongDangKy.Value;
                    }
                    txtTongSoTien.Text = TongTien.ToString();
                    txtTongSoVe.Text = TongSoVe.ToString();
                    var phieunhanve = _PHIEUNHANVE_BUS.Select(_MaPhieuNhanVe);
                    txtTongSoTien.Text = TongTien.ToString();
                    txtTongSoVe.Text = TongSoVe.ToString();
                    _PHIEUNHANVE_BUS.Update(_MaPhieuNhanVe, TongSoVe.ToString(), TongTien.ToString());
                }
            }
            #endregion
            #region Đại lý nhận vé từ công ty
            else
            {
                DialogResult result = XtraMessageBox.Show("Bạn có chắc chắn muốn thêm phiếu nhận vé ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

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
                    if (_MaPhieuNhanVe == "")
                    {
                        string Error = _PHIEUNHANVE_BUS.CheckBeforeInsert(_MaPhieuDangKy, txtTongSoVe.Text, deNgayLap.Text, NguoiLap, txtTongSoTien.Text);
                        if (Error != "")
                        {
                            XtraMessageBox.Show(Error, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            _MaPhieuNhanVe = _PHIEUNHANVE_BUS.Insert(_MaPhieuDangKy, txtTongSoVe.Text, deNgayLap.Text, lkNguoiLap.GetColumnValue("MaNhanVien").ToString(), txtTongSoTien.Text);
                            var DataInGV = (List<CT_PHIEUNHANVE_VIEW>)gvBASE.DataSource;
                            int i = 0;
                            //Kiểm tra lỗi
                            foreach (var item in DataInGV)
                            {
                                if (gvBASE.GetRowCellValue(i, gvBASE.Columns["DaTra"]).ToString() == "True")
                                {
                                    string ErrorCT = _CT_PHIEUNHANVE_BUS.CheckBeforeInsert(_MaPhieuNhanVe, item.MaDoiTac, item.MaDotPhatHanh, item.MaLoaiVe, item.SoLuongDangKy.ToString(), item.SoLuongNhan.ToString(), item.ThanhTien.ToString());
                                    _CT_PHIEUDANGKYVE_BUS.UpdateDaNhan(_MaPhieuDangKy, item.MaDoiTac, item.MaDotPhatHanh, item.MaLoaiVe);
                                    if (ErrorCT != "")
                                    {
                                        XtraMessageBox.Show("Chưa nhập đầy đủ thông tin nhận vé", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                }
                                i++;
                            }
                            //Insert CT
                            i = 0;
                            foreach (var item in DataInGV)
                            {
                                if (gvBASE.GetRowCellValue(i, gvBASE.Columns["DaTra"]).ToString() == "True")
                                {
                                    _CT_PHIEUNHANVE_BUS.Insert(_MaPhieuNhanVe, item.MaDoiTac, item.MaDotPhatHanh, item.MaLoaiVe, item.SoLuongDangKy.ToString(), item.SoLuongNhan.ToString(), item.ThanhTien.ToString());
                                    _CT_PHIEUDANGKYVE_BUS.UpdateDaNhan(_MaPhieuDangKy, item.MaDoiTac, item.MaDotPhatHanh, item.MaLoaiVe);

                                }
                                i++;
                                ListReport.Add(item);
                            }
                        }
                    }
                    else
                    {
                        var DataInGV = (List<CT_PHIEUNHANVE_VIEW>)gvBASE.DataSource;
                        //Kiểm tra lỗi
                        int i = 0;
                        foreach (var item in DataInGV)
                        {
                            if (gvBASE.GetRowCellValue(i, gvBASE.Columns["DaTra"]).ToString() == "True")
                            {
                                string ErrorCT = _CT_PHIEUNHANVE_BUS.CheckBeforeInsert(_MaPhieuNhanVe, item.MaDoiTac, item.MaDotPhatHanh, item.MaLoaiVe, item.SoLuongDangKy.ToString(), item.SoLuongNhan.ToString(), item.ThanhTien.ToString());
                                if (ErrorCT != "")
                                {
                                    XtraMessageBox.Show("Chưa nhập đầy đủ thông tin nhận vé", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                            i++;
                        }
                        //Insert CT
                        i = 0;
                        foreach (var item in DataInGV)
                        {
                            if (gvBASE.GetRowCellValue(i, gvBASE.Columns["DaTra"]).ToString() == "True")
                            {
                                _CT_PHIEUNHANVE_BUS.Insert(_MaPhieuNhanVe, item.MaDoiTac, item.MaDotPhatHanh, item.MaLoaiVe, item.SoLuongDangKy.ToString(), item.SoLuongNhan.ToString(), item.ThanhTien.ToString());
                                _CT_PHIEUDANGKYVE_BUS.UpdateDaNhan(_MaPhieuDangKy, item.MaDoiTac, item.MaDotPhatHanh, item.MaLoaiVe);
                            }
                            i++;
                            ListReport.Add(item);
                        }
                    }
                    //  gcBASE.DataSource = List_CT_PhieuNhanVe;
                    gvBASE.Columns["DaTra"].Visible = false;
                    gcBASE.DataSource = _CT_PHIEUNHANVE_BUS.SelectView(_MaPhieuNhanVe);
                    var DataGV = (List<CT_PHIEUNHANVE_VIEW>)gvBASE.DataSource;
                    foreach (var item in DataGV)
                    {
                        TongTien += item.ThanhTien.Value;
                        TongSoVe += item.SoLuongDangKy.Value;
                    }
                    txtTongSoTien.Text = TongTien.ToString();
                    txtTongSoVe.Text = TongSoVe.ToString();
                    var phieunhanve = _PHIEUNHANVE_BUS.Select(_MaPhieuNhanVe);
                    _DOITAC_BUS.Update_Debt(_MaDoiTac, TongTien.ToString());
                    _PHIEUNHANVE_BUS.Update(_MaPhieuNhanVe, TongSoVe.ToString(), TongTien.ToString());
                }
            #endregion
                _InPhieu = true;
                _Report = new rptIn_PhieuNhanVe();
                _Report.DataSource = gcBASE.DataSource;
            }
        }

        private void btnThemPhieu_Click(object sender, EventArgs e)
        {
            lkMaPhieuDangKy.EditValue = null;
            deNgayLap.Text = "";
            lkNguoiLap.EditValue = null;
            txtTongSoVe.Text = "0";
            txtTongSoTien.Text = "0";
            lkMaPhieuDangKy.ReadOnly = false;
            deNgayLap.ReadOnly = false;
            lkNguoiLap.ReadOnly = false;
            txtTenDoiTac.Text = "";
            gvBASE.Columns["DaTra"].Visible = true;
            gcBASE.DataSource = null;
            _MaPhieuNhanVe = "";
        }
        private void gvBASE_CellValueChanging_1(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (gvBASE.FocusedColumn.FieldName == "SoLuongNhan")
            {
                try
                {
                    int MenhGia;
                    float TiLeHoaHong;
                    decimal ThanhTien = 0;
                    TiLeHoaHong = _DOITAC_BUS.GetPercentage(Convert.ToString(gvBASE.GetRowCellValue(gvBASE.FocusedRowHandle, gvBASE.Columns["MaDoiTac"])));
                    MenhGia = _LOAIVE_BUS.GetPrice(Convert.ToString(gvBASE.GetRowCellValue(gvBASE.FocusedRowHandle, gvBASE.Columns["MaLoaiVe"])));
                    ThanhTien = Convert.ToDecimal((1-TiLeHoaHong )* MenhGia * Convert.ToInt32(e.Value.ToString()));
                    gvBASE.SetRowCellValue(gvBASE.FocusedRowHandle, gvBASE.Columns["ThanhTien"], ThanhTien);
                }
                catch (Exception)
                {
                    gvBASE.SetRowCellValue(gvBASE.FocusedRowHandle, gvBASE.Columns["ThanhTien"], 0);
                }
            }

        }

        private void gvBASE_InvalidValueException(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
        {
            if (gvBASE.FocusedColumn.FieldName == "SoLuongNhan")
            {
                if (e.Value as String == "")
                {
                    e.ErrorText = "Số vé nhận không được rỗng";
                }
            }
        }

        private void gvBASE_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (gvBASE.FocusedColumn.FieldName == "SoLuongNhan")
            {
                int SoVeNhan = 0;
                if (!Int32.TryParse(e.Value as String, out SoVeNhan))
                {
                    e.Valid = false;
                    e.ErrorText = "Số vé nhận là số nguyên dương";
                }
                else
                {
                    if (SoVeNhan > Convert.ToInt32(gvBASE.GetRowCellValue(gvBASE.FocusedRowHandle, gvBASE.Columns["SoLuongDangKy"])))
                    {
                        e.Valid = false;
                        e.ErrorText = "Số vé nhận không lớn hơn số vé đăng ký";
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
