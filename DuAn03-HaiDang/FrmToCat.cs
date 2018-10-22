using DuAn03_HaiDang.DAO;
using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.POJO;
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
    public partial class FrmToCat : Form
    {
        FloorDAO floorDAO = new FloorDAO();
        ToCatDAO toCatDAO = new ToCatDAO();
        string sukien;
        public FrmToCat()
        {
            InitializeComponent();
            LoadFloorToCbb();
        }
        private void LoadFloorToCbb()
        {
            List<Floor> listFloor = new List<Floor>();
            listFloor.Add(new Floor()
            {
                IdFloor = 0,
                Name = "Chọn lầu của chuyền"
            });
            var listFloorInDB = floorDAO.GetListFloor();
            if (listFloorInDB != null && listFloorInDB.Count > 0)
                listFloor.AddRange(listFloorInDB);
            cboFloor.DataSource = listFloor;
            cboFloor.DisplayMember = "Name";
            cboFloor.ValueMember = "IdFloor";

            var fl = listFloor.Where(x => x.IsDefault).FirstOrDefault();
            cboFloor.SelectedValue = fl != null ? fl.IdFloor : 0;
        }

        private void FrmToCat_Load(object sender, EventArgs e)
        {
            LoadFloorToCbb();
            btnLuu.Enabled = false;
            btnHuy.Enabled = false;
            txtTenToCat.Enabled = false;
            txtMoTa.Enabled = false;
            
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = true;
            btnHuy.Enabled = true;
            txtTenToCat.Enabled = true;
            txtMoTa.Enabled = true;
            txtTenToCat.Text = "";
            txtMoTa.Text = "";            
            sukien = "them";                                   
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                
                Floor floor = ((Floor)cboFloor.SelectedItem);
                int IdFloor = 0;
                bool IsAll = false;
                if (floor != null)
                    int.TryParse(floor.IdFloor.ToString(), out IdFloor);
                var messageError = "";
                if (string.IsNullOrEmpty(txtTenToCat.Text))
                    messageError = "Tên tổ cắt không được để trống";
                if (floor == null)
                    messageError = "Bạn chưa chọn lầu cho tổ cắt";
                if (string.IsNullOrEmpty(messageError))
                {
                    ToCat toCat = new ToCat();
                    toCat.IdFloor = floor.IdFloor;
                    toCat.TenToCat = txtTenToCat.Text;
                    toCat.DinhNghia = txtMoTa.Text;
                    int kq = -1;
                    
                    if (sukien== "them")                
                    {                        
                        kq = toCatDAO.ThemOBJ(toCat);
                        if (kq != -1)
                        {
                            MessageBox.Show("Thêm tổ cắt thành công.");
                            btnThem.Enabled = true;
                            btnSua.Enabled = true;
                            btnXoa.Enabled = true;
                            btnLuu.Enabled = false;
                            btnHuy.Enabled = false;
                            txtTenToCat.Enabled = false;
                            txtMoTa.Enabled = false;
                            LoadMatHangRaDataGridView(IdFloor, IsAll);
                        }
                        else
                        {
                            MessageBox.Show("Quá trình thêm tổ cắt thất bại.\n Lỗi: " + dbclass.error);
                        }
                    }
                    else
                    {
                        int Id = 0;
                        int.TryParse(txtIdToCat.Text, out Id);
                        toCat.IdToCat = Id;
                        kq = toCatDAO.SuaThongTinOBJ(toCat);
                        if (kq != -1)
                        {
                            MessageBox.Show("Thay đổi tổ cắt thành công.");
                            btnThem.Enabled = true;
                            btnSua.Enabled = true;
                            btnXoa.Enabled = true;
                            btnLuu.Enabled = false;
                            btnHuy.Enabled = false;
                            txtTenToCat.Enabled = false;
                            txtMoTa.Enabled = false;                            
                            LoadMatHangRaDataGridView(IdFloor, IsAll);
                        }
                        else
                        {
                            MessageBox.Show("Quá trình thay đổi tổ cắt thất bại.\n Lỗi: " + dbclass.error);
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
                Floor floor = ((Floor)cboFloor.SelectedItem);
                int IdFloor = 0;
                bool IsAll = false;
                if (floor != null)
                    int.TryParse(floor.IdFloor.ToString(), out IdFloor);
                if (!string.IsNullOrEmpty(txtIdToCat.Text))
                {
                    int Id = 0;
                    int.TryParse(txtIdToCat.Text, out Id);
                    int kq = -1;
                    kq = toCatDAO.XoaOBJ(Id);
                    if (kq != -1)
                    {
                        MessageBox.Show("Xoá mặt hàng thành công", "Xử lý thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadMatHangRaDataGridView(IdFloor, IsAll);
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
            if (txtTenToCat.Text != "")
            {
                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnLuu.Enabled = true;
                btnHuy.Enabled = true;
                txtTenToCat.Enabled = true;
                txtMoTa.Enabled = true;                
                sukien = "sua";
            }
            else
            {
                MessageBox.Show("Vui lòng chọn tổ cắt muốn thay đổi thông tin", "Lỗi thao tác", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnLuu.Enabled = false;
            btnHuy.Enabled = false;
            txtTenToCat.Enabled = false;
            txtTenToCat.Text = "";
            txtMoTa.Enabled = false;            
            txtMoTa.Text = "";
            
        }

        private void LoadMatHangRaDataGridView(int IdFloor, bool isall)
        {
            try
            {
                dgThongTinToCat.Rows.Clear();
                dgThongTinToCat.Refresh();
                toCatDAO.LoadOBJToDataGirdview(dgThongTinToCat, IdFloor, isall);
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Đã có lỗi xảy ra: " + ex.Message, "Lỗi xử lý", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void dgThongTinToCat_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int row = e.RowIndex;
                txtIdToCat.Text = dgThongTinToCat.Rows[row].Cells["IdToCat"].Value.ToString();
                txtTenToCat.Text = dgThongTinToCat.Rows[row].Cells["TenToCat"].Value.ToString();
                txtMoTa.Text = dgThongTinToCat.Rows[row].Cells["DinhNghia"].Value.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã có lỗi xảy ra: " + ex.Message, "Lỗi xử lý", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboFloor_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                Floor floor = ((Floor)cboFloor.SelectedItem);
                int IdFloor = 0;
                bool IsAll = false;
                if (floor != null)
                    int.TryParse(floor.IdFloor.ToString(), out IdFloor);
                LoadMatHangRaDataGridView(IdFloor, IsAll);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã có lỗi xảy ra: " + ex.Message, "Lỗi xử lý", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            

        }

      

        
    }
}
