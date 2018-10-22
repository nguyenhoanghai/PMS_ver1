using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DuAn03_HaiDang.POJO;
using DuAn03_HaiDang.DAO;

namespace DuAn03_HaiDang
{
    public partial class FrmBaoHetHang : FormBase
    {
        BaoHetHangDAO baohethangDAO = new BaoHetHangDAO();
        public FrmBaoHetHang()
        {
            InitializeComponent();
        }

        private void butHuyThayDoiHienThiDen_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void butLuuHienThiDen_Click(object sender, EventArgs e)
        {
            if (dataGridBaoHetHang.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridBaoHetHang.Rows.Count-1; i++)
                {
                    if (dataGridBaoHetHang.Rows[i].Cells[0].Value == null)
                    {
                        
                        BaoHetHang bhh = new BaoHetHang();
                        bhh.SoLuongCon = 0;
                        try
                        {
                                bhh.SoLuongCon = int.Parse(dataGridBaoHetHang.Rows[i].Cells[1].Value.ToString());
                        }
                        catch (Exception)
                        {
                                
                        }
                        bhh.SoLanBao = 0;
                        try
                        {
                            bhh.SoLanBao = int.Parse(dataGridBaoHetHang.Rows[i].Cells[2].Value.ToString());   
                        }
                        catch (Exception)
                        {
                                                             
                        }                            
                        baohethangDAO.ThemOBJ(bhh);
                        
                        
                    }
                    else
                    {
                        BaoHetHang bhh = new BaoHetHang { STT = int.Parse(dataGridBaoHetHang.Rows[i].Cells[0].Value.ToString()), SoLuongCon = int.Parse(dataGridBaoHetHang.Rows[i].Cells[1].Value.ToString()), SoLanBao = int.Parse(dataGridBaoHetHang.Rows[i].Cells[2].Value.ToString()) };
                        baohethangDAO.SuaThongTinOBJ(bhh);
                    }
                }
                dataGridBaoHetHang.Rows.Clear();
                dataGridBaoHetHang.Refresh();
                baohethangDAO.LoadOBJToDataGirdview(dataGridBaoHetHang);
            }
        }

        private void FrmBaoHetHang_Load(object sender, EventArgs e)
        {
            baohethangDAO.LoadOBJToDataGirdview(dataGridBaoHetHang);
        }

        private void dataGridBaoHetHang_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            int STT = int.Parse(e.Row.Cells[0].Value.ToString());
            baohethangDAO.XoaOBJ(STT);

        }
    }
}
