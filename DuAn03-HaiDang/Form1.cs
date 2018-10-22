using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DuAn03_HaiDang.DAO;
using DuAn03_HaiDang.POJO;
using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.KeyPad_Chuyen.pojo;
using DuAn03_HaiDang.KeyPad_Chuyen.dao;
using System.Configuration;
using DuAn03_HaiDang.Model;
using PMS.Business;
namespace DuAn03_HaiDang
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void QuanLyThongTin_Load(object sender, EventArgs e)
        {

            LoadFloorToCbb();

            //quan ly san pham
            btnLuu.Enabled = false;
            btnHuy.Enabled = false;
            txtTenMatHang.Enabled = false;
            txtDinhNghia.Enabled = false;
            txtDonGia.Enabled = false;
            txtMaMatHang.Visible = false;
            txtDonGiaCM.Enabled = false;

            //end quan ly san pham

            //start phan quyen

            btnThemTK.Enabled = true;
            btnSuaTK.Enabled = true;
            btnXoaTK.Enabled = true;
            btnLuuTK.Enabled = false;
            btnHuyTK.Enabled = false;
            txtTenNhanVien.Enabled = false;
            txtTenNhanVien.Text = "";
            txtTaiKhoan.Enabled = false;
            txtTaiKhoan.Text = "";
            txtMatKhau.Enabled = false;
            txtMatKhau.Text = "";
            cbbHienMatKau.Enabled = false;
            rdbBTP.Enabled = false;
            rdbThanhPham.Enabled = false;
            rdbThanhPhamVaBTP.Enabled = false;
            chkQuyenThaoTac.Enabled = false;

            //end phan quyen
            if (AccountSuccess.ThaoTac == 1)
            {
                btnThem.Enabled = true;
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
            }
            else
            {
                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
            }
        }
        ///////Start Quản lý Mặt Hàng/////////
        string sukienqlsanpham = "";
        SanPhamDAO sanphamDAO = new SanPhamDAO();
        ChuyenDAO chuyenDAO = new ChuyenDAO();
        private void LoadMatHangRaDataGridView(string IdFloor)
        {
            dgThongTinMatHang.Rows.Clear();
            dgThongTinMatHang.Refresh();
            sanphamDAO.LoadOBJToDataGirdview(dgThongTinMatHang, IdFloor);
        }
        FloorDAO floorDAO = new FloorDAO();
        private void LoadListTeam(string idfloor)
        {
            cbbChuyen.DataSource = null;
            cbbChuyen.Refresh();
            DataTable dtchuyen = new DataTable();
            List<Chuyen> listChuyen = new List<Chuyen>();
            Chuyen chuyen1 = new Chuyen();
            chuyen1.MaChuyen = "0";
            chuyen1.TenChuyen = "(None)";
            listChuyen.Add(chuyen1);
            var listChuyenInDB = chuyenDAO.GetListChuyenInfByListId(AccountSuccess.strListChuyenId);
            if (listChuyenInDB != null && listChuyenInDB.Count > 0)
                listChuyen.AddRange(listChuyenInDB);
            cbbChuyen.DataSource = listChuyen;
            cbbChuyen.DisplayMember = "TenChuyen";
        }
        private void LoadFloorToCbb()
        {
            var listFloor = BLLFloor.GetFloorForComBoBox();
            cboFloor.DataSource = listFloor.SelectList;
            cboFloor.DisplayMember = "Name";
            cboFloor.ValueMember = "IdFloor";
            cboFloor.SelectedValue = listFloor.DefaultValue;

            cboFloorProduct.DataSource = listFloor.SelectList;
            cboFloorProduct.DisplayMember = "Name";
            cboFloorProduct.ValueMember = "IdFloor";
            cboFloorProduct.SelectedValue =listFloor.DefaultValue;
        }
        private void dgThongTinMatHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int row = e.RowIndex;
                txtMaMatHang.Text = dgThongTinMatHang.Rows[row].Cells["MaHang"].Value.ToString();
                txtTenMatHang.Text = dgThongTinMatHang.Rows[row].Cells["TenMatHang"].Value.ToString();
                txtDonGia.Text = dgThongTinMatHang.Rows[row].Cells["DonGia"].Value.ToString();
                txtDinhNghia.Text = dgThongTinMatHang.Rows[row].Cells["DinhNghia"].Value.ToString();
                txtDonGiaCM.Text = dgThongTinMatHang.Rows[row].Cells["DonGiaCM"].Value.ToString();
            }
            catch
            { }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = true;
            btnHuy.Enabled = true;
            txtTenMatHang.Enabled = true;
            txtDinhNghia.Enabled = true;
            txtDonGia.Enabled = true;
            txtTenMatHang.Text = "";
            txtDinhNghia.Text = "";
            txtDonGia.Text = "";
            txtDonGiaCM.Enabled = true;
            txtDonGiaCM.Text = "";
            sukienqlsanpham = "them";
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtTenMatHang.Text != "")
            {
                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnLuu.Enabled = true;
                btnHuy.Enabled = true;
                txtTenMatHang.Enabled = true;
                txtDinhNghia.Enabled = true;
                txtDonGia.Enabled = true;
                txtDonGiaCM.Enabled = true;
                sukienqlsanpham = "sua";
            }
            else
            {
                MessageBox.Show("Vui lòng chọn tên Mặt Hàng bạn muốn thay đổi thông tin", "Lỗi xử lý", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            var floor = ((PMS.Data.Floor)cboFloorProduct.SelectedItem);
            SanPham obj = new SanPham();
            if ((int)cboFloorProduct.SelectedValue == 0)
                MessageBox.Show("Lỗi: Bạn chưa chọn Lầu, Vui lòng thực hiện thao tác này.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (txtTenMatHang.Text != "")
            {
                obj.TenSanPham = txtTenMatHang.Text;
                obj.DinhNghia = txtDinhNghia.Text;
                obj.MaSanPham = txtMaMatHang.Text;
                obj.Floor = floor.IdFloor.ToString();
                float donGia = 0;
                float donGiaCM = 0;
                float.TryParse(txtDonGia.Text, out donGia);
                float.TryParse(txtDonGiaCM.Text, out donGiaCM);
                obj.DonGia = donGia;
                obj.DonGiaCM = donGiaCM;
                if (sukienqlsanpham == "them")
                {
                    DataTable dtcheckAvailable = new DataTable();
                    dtcheckAvailable = sanphamDAO.TimKiemOBJ(obj.TenSanPham, floor.IdFloor.ToString());
                    if (dtcheckAvailable.Rows.Count <= 0)
                    {
                        int kq = 0;
                        kq = sanphamDAO.ThemOBJ(obj);
                        if (kq != -1)
                        {
                            MessageBox.Show("Thêm mặt hàng thành công.");
                            btnThem.Enabled = true;
                            btnSua.Enabled = true;
                            btnXoa.Enabled = true;
                            btnLuu.Enabled = false;
                            btnHuy.Enabled = false;
                            txtTenMatHang.Enabled = false;
                            txtDinhNghia.Enabled = false;
                            txtDonGia.Enabled = false;
                            txtDonGiaCM.Enabled = false;
                            LoadMatHangRaDataGridView(floor.IdFloor.ToString());
                        }
                        else
                            MessageBox.Show("Quá trình thêm mặt hàng thất bại.\n Lỗi: " + dbclass.error);
                    }
                    else
                        MessageBox.Show("Tên Mặt Hàng này đã tồn tại. Vui lòng kiểm tra lại, hay chọn tên Mặt Hàng khác");
                }
                else
                {

                    if (obj.MaSanPham != "")
                    {
                        int kq = 0;
                        kq = sanphamDAO.SuaThongTinOBJ(obj);
                        if (kq != -1)
                        {
                            MessageBox.Show("Thay đổi mặt hàng thành công.");
                            btnThem.Enabled = true;
                            btnSua.Enabled = true;
                            btnXoa.Enabled = true;
                            btnLuu.Enabled = false;
                            btnHuy.Enabled = false;
                            txtTenMatHang.Enabled = false;
                            txtDinhNghia.Enabled = false;
                            txtDonGia.Enabled = false;
                            txtDonGiaCM.Enabled = false;
                            LoadMatHangRaDataGridView(floor.IdFloor.ToString());
                        }
                        else
                            MessageBox.Show("Quá trình thay đổi mặt hàng thất bại.\n Lỗi: " + dbclass.error);
                    }
                    else
                        MessageBox.Show("Lỗi: không lấy được mã mặt hàng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
                MessageBox.Show("Lỗi: bạn chưa nhập tên cho mặt hàng, Vui lòng thực hiện thao tác này.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            var floor = ((PMS.Data.Floor)cboFloorProduct.SelectedItem);
            if (txtMaMatHang.Text != "")
            {
                var rs = BLLCommodity.Delete(int.Parse(txtMaMatHang.Text));
                if (rs.IsSuccess)
                {
                    MessageBox.Show(rs.Messages[0].msg, rs.Messages[0].Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadMatHangRaDataGridView(floor.IdFloor.ToString());
                }
                else
                    MessageBox.Show(rs.Messages[0].msg, rs.Messages[0].Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                MessageBox.Show("Vui lòng chọn tên mặt hàng bạn muốn xoá", "Lỗi xử lý", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnLuu.Enabled = false;
            btnHuy.Enabled = false;
            txtTenMatHang.Enabled = false;
            txtTenMatHang.Text = "";
            txtDinhNghia.Enabled = false;
            txtDonGia.Enabled = false;
            txtDinhNghia.Text = "";
            txtDonGia.Text = "";
            txtDonGiaCM.Enabled = false;
            txtDonGiaCM.Text = "";
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            timkiem();
        }
        private void timkiem()
        {
            DataTable dt = new DataTable();
            var floor = ((PMS.Data.Floor)cboFloorProduct.SelectedItem);
            if (txtNoiDungTimKiem.Text == "")
            {
                MessageBox.Show("Vui lòng nhập nội dung tìm kiếm", "Lỗi tìm kiếm");
            }
            else
            {
                dt = sanphamDAO.TimKiemOBJ(txtNoiDungTimKiem.Text, floor.IdFloor.ToString());
                if (dt != null && dt.Rows.Count == 0)
                {
                    MessageBox.Show("Mặt hàng không tồn tại", "Lỗi tìm kiếm");
                }
                else
                {
                    DataRow row = dt.Rows[0];
                    txtTenMatHang.Text = row["TenSanPham"].ToString();
                    txtMaMatHang.Text = row["MaSanPham"].ToString();
                    txtDinhNghia.Text = row["DinhNghia"].ToString();
                    txtDonGia.Text = row["DonGia"].ToString();
                    txtDonGiaCM.Text = row["DonGiaCM"].ToString();
                }

            }
        }

        private void txtNoiDungTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                timkiem();
            }
        }
        //////End Quản lý Mặt Hàng///////////



        ////// Start Phân Quyền/////////////

        string sukienphanquyen = "";
        TaiKhoanDAO taikhoanDAO = new TaiKhoanDAO();

        private void LoadTaiKhoanRaDataGridView()
        {
            var floor = ((PMS.Data.Floor)cboFloor.SelectedItem);
            Chuyen chuyen = ((Chuyen)cbbChuyen.SelectedItem);
            dgThongTinTaiKhoan.Rows.Clear();
            dgThongTinTaiKhoan.Refresh();
            taikhoanDAO.LoadOBJToDataGirdview(dgThongTinTaiKhoan, chuyen.MaChuyen, floor.IdFloor.ToString());
        }


        private void dgThongTinTaiKhoan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int row = e.RowIndex;
                txtTaiKhoan.Text = dgThongTinTaiKhoan.Rows[row].Cells[0].Value.ToString();
                txtMatKhau.Text = dgThongTinTaiKhoan.Rows[row].Cells[1].Value.ToString();
                txtTenNhanVien.Text = dgThongTinTaiKhoan.Rows[row].Cells[2].Value.ToString();
                int QuyenTP = int.Parse(dgThongTinTaiKhoan.Rows[row].Cells[3].Value.ToString());
                int QuyenBTP = int.Parse(dgThongTinTaiKhoan.Rows[row].Cells[4].Value.ToString());
                if (QuyenTP == 1 && QuyenBTP == 0)
                {
                    rdbThanhPham.Checked = true;
                }
                else if (QuyenTP == 0 && QuyenBTP == 1)
                {
                    rdbBTP.Checked = true;
                }
                else
                {
                    rdbThanhPhamVaBTP.Checked = true;
                }
            }
            catch
            { }
        }

        private void btnThemTK_Click(object sender, EventArgs e)
        {
            btnThemTK.Enabled = false;
            btnSuaTK.Enabled = false;
            btnXoaTK.Enabled = false;
            btnLuuTK.Enabled = true;
            btnHuyTK.Enabled = true;
            txtTenNhanVien.Enabled = true;
            txtMatKhau.Enabled = true;
            txtTaiKhoan.Enabled = true;
            cbbHienMatKau.Enabled = true;
            rdbThanhPham.Enabled = true;
            rdbBTP.Enabled = true;
            rdbThanhPhamVaBTP.Enabled = true;
            chkQuyenThaoTac.Enabled = true;
            txtTenNhanVien.Text = "";
            txtTaiKhoan.Text = "";
            txtMatKhau.Text = "";
            rdbThanhPham.Checked = true;
            rdbBTP.Checked = false;
            rdbThanhPhamVaBTP.Checked = false;
            sukienphanquyen = "them";
        }

        private void btnSuaTK_Click(object sender, EventArgs e)
        {
            if (txtTaiKhoan.Text != "")
            {
                btnThemTK.Enabled = false;
                btnSuaTK.Enabled = false;
                btnXoaTK.Enabled = false;
                btnLuuTK.Enabled = true;
                btnHuyTK.Enabled = true;
                txtTenNhanVien.Enabled = true;
                txtTaiKhoan.Enabled = true;
                txtMatKhau.Enabled = true;
                cbbHienMatKau.Enabled = true;
                rdbThanhPham.Enabled = true;
                rdbBTP.Enabled = true;
                rdbThanhPhamVaBTP.Enabled = true;
                chkQuyenThaoTac.Enabled = true;
                sukienphanquyen = "sua";
            }
            else
            {
                MessageBox.Show("Vui lòng chọn tên tài khoản bạn muốn thay đổi thông tin", "Lỗi xử lý", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnLuuTK_Click(object sender, EventArgs e)
        {
            Floor floor = ((Floor)cboFloor.SelectedItem);
            Chuyen chuyen = ((Chuyen)cbbChuyen.SelectedItem);
            TaiKhoan obj = new TaiKhoan();
            obj.TenChuTK = txtTenNhanVien.Text;
            obj.TenTaiKhoan = txtTaiKhoan.Text;
            obj.MatKhau = txtMatKhau.Text;
            obj.Floor = floor.IdFloor.ToString();
            if (rdbThanhPham.Checked == true)
            {
                obj.ThanhPhan = 1;
                obj.BTP = 0;
            }
            else if (rdbBTP.Checked == true)
            {
                obj.ThanhPhan = 0;
                obj.BTP = 1;
            }
            else
            {
                obj.ThanhPhan = 1;
                obj.BTP = 1;
            }
            if (chkQuyenThaoTac.Checked)
            {
                obj.ThaoTac = 1;
            }
            else
            {
                obj.ThaoTac = 0;
            }
            if (sukienphanquyen == "them")
            {
                int kq = 0;
                kq = taikhoanDAO.ThemOBJ(obj);
                if (kq != -1)
                {
                    MessageBox.Show("Thêm tài khoản thành công.");
                    btnThemTK.Enabled = true;
                    btnSuaTK.Enabled = true;
                    btnXoaTK.Enabled = true;
                    btnLuuTK.Enabled = false;
                    btnHuyTK.Enabled = false;
                    txtTenNhanVien.Enabled = false;
                    txtTaiKhoan.Enabled = false;
                    txtMatKhau.Enabled = false;
                    cbbHienMatKau.Enabled = false;
                    rdbBTP.Enabled = false;
                    rdbThanhPham.Enabled = true;
                    rdbThanhPhamVaBTP.Enabled = false;
                    LoadTaiKhoanRaDataGridView();
                }
                else
                {
                    MessageBox.Show("Quá trình thêm tài khoản thất bại.\n Lỗi: " + dbclass.error);
                }
            }
            else
            {


                int kq = 0;
                kq = taikhoanDAO.SuaThongTinOBJ(obj);
                if (kq != -1)
                {
                    MessageBox.Show("Thay đổi thông tin tài khoản thành công.");
                    btnThemTK.Enabled = true;
                    btnSuaTK.Enabled = true;
                    btnXoaTK.Enabled = true;
                    btnLuuTK.Enabled = false;
                    btnHuyTK.Enabled = false;
                    txtTenNhanVien.Enabled = false;
                    txtTaiKhoan.Enabled = false;
                    txtMatKhau.Enabled = false;
                    cbbHienMatKau.Enabled = false;
                    rdbBTP.Enabled = false;
                    rdbThanhPham.Enabled = true;
                    rdbThanhPhamVaBTP.Enabled = false;
                    LoadTaiKhoanRaDataGridView();
                }
                else
                {
                    MessageBox.Show("Quá trình thay đổi tài khoản thất bại.\n Lỗi: " + dbclass.error);
                }

            }
        }

        private void btnXoaTK_Click(object sender, EventArgs e)
        {
            if (txtTaiKhoan.Text != "")
            {
                int kq = 0;
                kq = taikhoanDAO.XoaOBJ(txtTaiKhoan.Text);
                if (kq > 0)
                {
                    MessageBox.Show("Xoá tài khoản thành công", "Xử lý thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadTaiKhoanRaDataGridView();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn tài khoản bạn muốn xoá", "Lỗi xử lý", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void btnHuyTK_Click(object sender, EventArgs e)
        {
            btnThemTK.Enabled = true;
            btnSuaTK.Enabled = true;
            btnXoaTK.Enabled = true;
            btnLuuTK.Enabled = false;
            btnHuyTK.Enabled = false;
            txtTenNhanVien.Enabled = false;
            txtTenNhanVien.Text = "";
            txtTaiKhoan.Enabled = false;
            txtTaiKhoan.Text = "";
            txtMatKhau.Enabled = false;
            txtMatKhau.Text = "";
            cbbHienMatKau.Enabled = false;
            rdbBTP.Enabled = false;
            rdbThanhPham.Enabled = false;
            rdbThanhPham.Checked = true;
            rdbThanhPhamVaBTP.Enabled = false;

        }



        private void cbbHienMatKau_CheckedChanged(object sender, EventArgs e)
        {
            if (cbbHienMatKau.Checked == true)
            {
                txtMatKhau.UseSystemPasswordChar = false;
            }
            else
            {
                txtMatKhau.UseSystemPasswordChar = true;
            }
        }

        private void cbbChuyen_SelectionChangeCommitted(object sender, EventArgs e)
        {
            LoadTaiKhoanRaDataGridView();
        }

        private void cboFloor_SelectedValueChanged(object sender, EventArgs e)
        {
            var floor = ((PMS.Data.Floor)cboFloor.SelectedItem);
            LoadListTeam(floor.IdFloor.ToString());
        }

        private void cboFloorProduct_SelectedValueChanged(object sender, EventArgs e)
        {
            var floor = ((PMS.Data.Floor)cboFloorProduct.SelectedItem);
            LoadMatHangRaDataGridView(floor.IdFloor.ToString());
            DataTable dt = new DataTable();
            dt = sanphamDAO.DSOBJ(floor.IdFloor.ToString());
            int somang = dt.Rows.Count;
            List<string> mathangs = new List<string>();
            string[] mangmh = new string[somang];
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                mathangs.Add(dt.Rows[i][1].ToString());
            }
            for (int i = 0; i < mathangs.Count; i++)
            {
                mangmh[i] = mathangs[i];
            }
            txtNoiDungTimKiem.AutoCompleteCustomSource.AddRange(mangmh);
        }



        /// End Phân Quyền//////////////////


    }
}
