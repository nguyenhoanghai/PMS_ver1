using DuAn03_HaiDang.DAO;
using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.POJO;
using PMS.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DuAn03_HaiDang
{
    public partial class FrmSanLuongCat : FormBase
    {
        FloorDAO floorDAO = new FloorDAO();
        ToCatDAO toCatDAO = new ToCatDAO();
        HangDAO hangDAO = new HangDAO();
        SanLuongCatDAO sanLuongCatDAO = new SanLuongCatDAO();
        string sukien = "";
        //int IdFloor = 0;
        //bool IsAll = false;
        public FrmSanLuongCat()
        {
            InitializeComponent();


        }
        private void FrmSanLuongCat_Load(object sender, EventArgs e)
        {
            LoadFloorToCbb();
            LoadToCatToCbb();
            LoadDSSamPham();
            dtpNgayNhap.Value = DateTime.Now;
            btnLuu.Enabled = false;
            btnHuy.Enabled = false;
            txtSanLuong.Enabled = false;
        }
        private void LoadFloorToCbb()
        {
            try
            {
                var list = BLLFloor.GetFloorForComBoBox();
                cboFloor.DataSource = list.SelectList;                
                cboFloor.DisplayMember = "Name";
                cboFloor.ValueMember = "IdFloor";
                cboFloor.SelectedValue = list.DefaultValue;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã có lỗi xảy ra: " + ex.Message, "Lỗi xử lý", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadToCatToCbb()
        {
            try
            {
                DataTable dt = new DataTable();
                List<ToCat> list = new List<ToCat>();

                var floor = ((PMS.Data.Floor)cboFloor.SelectedItem);
                int IdFloor = 0;
                bool IsAll = false;
                if (floor != null)
                    int.TryParse(floor.IdFloor.ToString(), out IdFloor);
                dt = toCatDAO.DSOBJ(IdFloor, IsAll);

                if (dt != null && dt.Rows.Count != 0)
                {
                    foreach (DataRow drow in dt.Rows)
                    {
                        string ten = drow["TenToCat"].ToString();
                        if (!string.IsNullOrEmpty(ten))
                        {
                            ToCat toCat = new ToCat();
                            toCat.IdToCat = int.Parse(drow["IdToCat"].ToString());
                            toCat.TenToCat = ten;
                            list.Add(toCat);

                        }
                    }
                }
                cbbToCat.DataSource = list;
                cbbToCat.DisplayMember = "TenToCat";
            }
            catch (Exception ex)
            {

                MessageBox.Show("Đã có lỗi xảy ra: " + ex.Message, "Lỗi xử lý", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDSSamPham()
        {
            List<Hang> listHang = new List<Hang>();
            DataTable dt = new DataTable();
            dt = hangDAO.DSHang(AccountSuccess.IdFloor, 1);
            if (dt.Rows.Count != 0)
            {
                Hang hang1 = new Hang();
                hang1.MaHang = "";
                hang1.TenHang = "(None)";
                listHang.Add(hang1);
                foreach (DataRow drow in dt.Rows)
                {
                    Hang hang = new Hang();
                    hang.MaHang = drow["MaSanPham"].ToString();
                    hang.TenHang = drow["TenSanPham"].ToString();
                    listHang.Add(hang);
                }

                cbbSanPham.DataSource = listHang;
                cbbSanPham.DisplayMember = "TenHang";
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = true;
            btnHuy.Enabled = true;
            txtSanLuong.Enabled = true;
            txtSanLuong.Enabled = true;
            sukien = "them";
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnLuu.Enabled = false;
            btnHuy.Enabled = false;
            txtSanLuong.Enabled = false;
            txtSanLuong.Text = "";


        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {

                ToCat toCat = ((ToCat)cbbToCat.SelectedItem);
                Hang hang = ((Hang)cbbSanPham.SelectedItem);
                var messageError = "";
                if (string.IsNullOrEmpty(txtSanLuong.Text))
                    messageError = "Sản lượng không được để trống.";
                if (hang == null)
                    messageError = "Bạn chưa chọn mặt hàng.";
                if (toCat == null)
                    messageError = "Bạn chưa chọn tổ cắt.";
                int sanLuong = 0;
                int.TryParse(txtSanLuong.Text, out sanLuong);
                if (sanLuong <= 0)
                    messageError = "Nhập sai định dạng số sản lượng hoặc Số sản lượng phải lớn hơn 0";
                int IdHang = 0;
                int.TryParse(hang.MaHang, out IdHang);
                if (IdHang == 0)
                {
                    messageError = "Lỗi khi chọn Mặt Hàng, mã Mặt Hàng không hợp lệ";
                }
                if (string.IsNullOrEmpty(messageError))
                {
                    SanLuongCat sanLuongCat = new SanLuongCat();
                    sanLuongCat.IdToCat = toCat.IdToCat;
                    sanLuongCat.IdSanPham = IdHang;
                    sanLuongCat.SanLuong = sanLuong;
                    sanLuongCat.NgayNapSL = dtpNgayNhap.Value;
                    sanLuongCat.ThoiGianNapSL = dtpNgayNhap.Value.TimeOfDay;

                    int kq = -1;

                    if (sukien == "them")
                    {
                        kq = sanLuongCatDAO.ThemOBJ(sanLuongCat);
                        if (kq != -1)
                        {
                            MessageBox.Show("Thêm sản lượng cắt thành công.");
                            btnThem.Enabled = true;
                            btnSua.Enabled = true;
                            btnXoa.Enabled = true;
                            btnLuu.Enabled = false;
                            btnHuy.Enabled = false;
                            txtSanLuong.Enabled = false;
                            LoadSanLuongRaDataGridView(toCat.IdToCat, IdHang, dtpNgayNhap.Value);
                        }
                        else
                        {
                            MessageBox.Show("Quá trình thêm sản lượng cắt thất bại.\n Lỗi: " + dbclass.error);
                        }
                    }
                    else
                    {
                        int Id = 0;
                        int.TryParse(txtIdSanLuongCat.Text, out Id);
                        sanLuongCat.Id = Id;
                        kq = sanLuongCatDAO.SuaThongTinOBJ(sanLuongCat);
                        if (kq != -1)
                        {
                            MessageBox.Show("Thay đổi sản lượng cắt thành công.");
                            btnThem.Enabled = true;
                            btnSua.Enabled = true;
                            btnXoa.Enabled = true;
                            btnLuu.Enabled = false;
                            btnHuy.Enabled = false;
                            txtSanLuong.Enabled = false;
                            LoadSanLuongRaDataGridView(toCat.IdToCat, IdHang, dtpNgayNhap.Value);
                        }
                        else
                        {
                            MessageBox.Show("Quá trình thay đổi sản lượng cắt thất bại.\n Lỗi: " + dbclass.error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Lỗi nhập liệu: " + messageError, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã có lỗi xảy ra: " + ex.Message, "Lỗi xử lý", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                ToCat toCat = ((ToCat)cbbToCat.SelectedItem);
                Hang hang = ((Hang)cbbSanPham.SelectedItem);
                int IdHang = 0;
                int.TryParse(hang.MaHang, out IdHang);

                if (!string.IsNullOrEmpty(txtIdSanLuongCat.Text))
                {
                    int Id = 0;
                    int.TryParse(txtIdSanLuongCat.Text, out Id);
                    int kq = -1;
                    kq = sanLuongCatDAO.XoaOBJ(Id);
                    if (kq != -1)
                    {
                        MessageBox.Show("Xoá mặt hàng thành công", "Xử lý thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadSanLuongRaDataGridView(toCat.IdToCat, IdHang, dtpNgayNhap.Value);
                    }
                }
                else
                {
                    MessageBox.Show("Lỗi thao tác: Bạn chưa chọn tổ cắt muốn xoá.", "Lỗi thao tác", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Đã có lỗi xảy ra: " + ex.Message, "Lỗi xử lý", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtSanLuong.Text != "")
            {
                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnLuu.Enabled = true;
                btnHuy.Enabled = true;
                txtSanLuong.Enabled = true;
                sukien = "sua";
            }
            else
            {
                MessageBox.Show("Vui lòng chọn tổ cắt muốn thay đổi thông tin", "Lỗi thao tác", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cboFloor_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                LoadToCatToCbb();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã có lỗi xảy ra: " + ex.Message, "Lỗi xử lý", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSanLuongRaDataGridView(int IdToCat, int IdSanPham, DateTime NgayNhap)
        {
            try
            {
                dgThongTinSanLuongCat.Rows.Clear();
                dgThongTinSanLuongCat.Refresh();
                sanLuongCatDAO.LoadOBJToDataGirdview(dgThongTinSanLuongCat, IdToCat, IdSanPham, NgayNhap);
            }
            catch (Exception ex)
            {

                MessageBox.Show("Đã có lỗi xảy ra: " + ex.Message, "Lỗi xử lý", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void cbbSanPham_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                ToCat toCat = ((ToCat)cbbToCat.SelectedItem);
                Hang hang = ((Hang)cbbSanPham.SelectedItem);
                int IdHang = 0;
                int.TryParse(hang.MaHang, out IdHang);
                if (toCat != null && hang != null)
                    LoadSanLuongRaDataGridView(toCat.IdToCat, IdHang, dtpNgayNhap.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã có lỗi xảy ra: " + ex.Message, "Lỗi xử lý", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbbToCat_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                ToCat toCat = ((ToCat)cbbToCat.SelectedItem);
                Hang hang = ((Hang)cbbSanPham.SelectedItem);
                int IdHang = 0;
                if (toCat != null && hang != null)
                {
                    int.TryParse(hang.MaHang, out IdHang);
                    LoadSanLuongRaDataGridView(toCat.IdToCat, IdHang, dtpNgayNhap.Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã có lỗi xảy ra: " + ex.Message, "Lỗi xử lý", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
