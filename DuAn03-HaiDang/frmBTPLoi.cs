using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DuAn03_HaiDang.KeyPad_Chuyen.dao;
using DuAn03_HaiDang.KeyPad_Chuyen.pojo;
using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.DAO;
using DuAn03_HaiDang.POJO;
using DuAn03_HaiDang.Model;

namespace DuAn03_HaiDang
{
    public partial class frmBTPLoi : FormBase
    {
        public frmBTPLoi()
        {
            InitializeComponent();
        }

        private void frmBTPLoi_Load(object sender, EventArgs e)
        {
            LoadDSChuyen();
        }
        ChuyenDAO chuyenDAO = new ChuyenDAO();
        private void LoadDSChuyen()
        {

           List<Chuyen> listChuyen = new List<Chuyen>();
           Chuyen chuyenf = new Chuyen();
            chuyenf.MaChuyen = "";
            chuyenf.TenChuyen = "(None)";
            listChuyen.Add(chuyenf);
            listChuyen.AddRange(chuyenDAO.GetListChuyenInfByListId(AccountSuccess.strListChuyenId));   
            // Load thong tin vao combobox chuyen san xuat cua tad thanh pham
            cboChuyen.DataSource = listChuyen;
            cboChuyen.DisplayMember = "TenChuyen";
            cboChuyen.SelectedIndex = 0;

            
        }
        Chuyen_SanPhamDAO chuyen_sanphamDAO = new Chuyen_SanPhamDAO();
        NangXuatDAO nangxuatDAO = new NangXuatDAO();
        DataTable dtSanPham = new DataTable();
        List<SanPhamCuaChuyen> listSPCuaChuyen = new List<SanPhamCuaChuyen>();
        private void LoadDSSanPham()
        {
            dtSanPham.Clear();
            Chuyen chuyen = ((Chuyen)cboChuyen.SelectedItem);
            listSPCuaChuyen.Clear();
            if (chuyen.MaChuyen != "")
            {
                //load sanpham cua chuyen       
                dtSanPham = chuyen_sanphamDAO.DSPhanCongCuaChuyen_BTPLoi(chuyen.MaChuyen);
                if (dtSanPham.Rows.Count > 0)
                {
                    foreach (DataRow drow in dtSanPham.Rows)
                    {
                        SanPhamCuaChuyen spcuachuyen = new SanPhamCuaChuyen();
                        spcuachuyen.STT = drow["STT"].ToString();
                        spcuachuyen.TenSanPham = drow["TenSanPham"].ToString();
                        spcuachuyen.NangXuatSanXuat = float.Parse(drow["NangXuatSanXuat"].ToString());
                        spcuachuyen.SanLuongKeHoach = int.Parse(drow["SanLuongKeHoach"].ToString());
                        spcuachuyen.LuyKeTH = int.Parse(drow["LuyKeTH"].ToString());
                        spcuachuyen.BTPLoi = int.Parse(drow["BTPLoi"].ToString());
                        cboSanPham.Items.Add(spcuachuyen);
                    }

                }
                else
                {
                    MessageBox.Show("Bạn chưa phân công mặt hàng cho chuyền này hay chưa nhập thông tin trong ngày cho chuyền, Vui lòng thực hiện thao tác Phân Công Cho Chuyên", "Lỗi Thực Hiện", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SanPhamCuaChuyen spcuachuyen = new SanPhamCuaChuyen { STT = "", TenSanPham = "(None)", LuyKeTH = 0, SanLuongKeHoach = 0, MaSanPham = "", NangXuatSanXuat = 0 };
                    cboSanPham.Items.Add(spcuachuyen);
                }
            }
            else
            {
                SanPhamCuaChuyen spcuachuyen = new SanPhamCuaChuyen { STT = "", TenSanPham = "(None)", LuyKeTH = 0, SanLuongKeHoach = 0, MaSanPham = "", NangXuatSanXuat = 0 };
                cboSanPham.Items.Add(spcuachuyen);

            }
            cboSanPham.DisplayMember = "TenSanPham";
            cboSanPham.SelectedIndex = 0;
        }

        private void butClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboChuyen_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboSanPham.DataSource = null;
            cboSanPham.Items.Clear();
            cboSanPham.Refresh();
            LoadDSSanPham();
        }

        private void butUpdate_Click(object sender, EventArgs e)
        {
            SanPhamCuaChuyen spcuachuyen = ((SanPhamCuaChuyen)cboSanPham.SelectedItem);
            NangXuat nx = new NangXuat();
            nx.STTChuyen_SanPham = spcuachuyen.STT;
            nx.Ngay = DateTime.Now.Day + "/" + DateTime.Now.Month + "/"+DateTime.Now.Year ;
            int btploi = 0;
            try
            {
                btploi = txtBTPLoi.Value;
            }
            catch (Exception)
            { }
            if (btploi > 0)
            {
                nx.BTPLoi = btploi;
                int kq = nangxuatDAO.SuaThongTinBTPLoi(nx);
                if (kq > 0)
                {
                    txtBTPLoi.Value = 0;
                    LoadDSChuyen();
                    MessageBox.Show("Thay đổi thông tin bán thành phẩm lỗi thành công", "Thay đổi thành công", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
                else
                {
                    MessageBox.Show("Không thể thay đổi thông tin bán thành phẩm lỗi của chuyền", "Thay đổi thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Lỗi: Luỹ kê Kiểm đạt mới phải lớn hơn luỹ kế Kiểm đạt cũ", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
            SanPhamCuaChuyen spcuachuyen = ((SanPhamCuaChuyen)cboSanPham.SelectedItem);
            txtBTPLoi.Value = spcuachuyen.BTPLoi;
        }
    }
}
