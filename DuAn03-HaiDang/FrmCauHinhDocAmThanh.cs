using DuAn03_HaiDang.DAO;
using QuanLyNangSuat;
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
    public partial class FrmCauHinhDocAmThanh : Form
    {
        private SoundReadConfigDAO soundReadConfigDAO;
        private int idChuyen = 0;
        private int idSoundReadConfig = 0;
        private int configType = 1;
        public delegate void SEND();
        public SEND sender;
        public FrmCauHinhDocAmThanh(int _idChuyen, string tenChuyen, int _configType)
        {
            InitializeComponent();
            soundReadConfigDAO = new SoundReadConfigDAO();
            this.idChuyen = _idChuyen;
            this.Text += " của " + tenChuyen;
            this.configType = _configType;
        }

        private void butClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmCauHinhDocAmThanh_Load(object sender, EventArgs e)
        {
            try
            {
                LoadDataToGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: "+ex.Message);
            }
        }

        private void LoadDataToGridView()
        {
            try
            {
                dgListConfig.Rows.Clear();
                soundReadConfigDAO.LoadConfigDataToGridView(dgListConfig, idChuyen, configType);
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        private void butAdd_Click(object sender, EventArgs e)
        {
            FrmCauHinhDocAmThanh_Create form = new FrmCauHinhDocAmThanh_Create(0, idChuyen, configType);
            form.sender = new FrmCauHinhDocAmThanh_Create.SEND(LoadDataToGridView);
            form.ShowDialog();
        }

        private void butDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Bạn có muốn xoá dữ liệu đã chọn?", "Xoá dữ liệu", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (dgListConfig.Rows != null && dgListConfig.Rows.Count > 0)
                    {
                        bool isSelectRow = false;
                        int result= 0;
                        foreach (DataGridViewRow row in dgListConfig.Rows)
                        {
                            bool isDeleted = false;
                            bool.TryParse(row.Cells["chonXoa"].Value.ToString(), out isDeleted);
                            if (isDeleted)
                            {
                                isSelectRow = true;
                                int Id = 0;
                                int.TryParse(row.Cells["Id"].Value.ToString(), out Id);
                                if (Id > 0)
                                    result = soundReadConfigDAO.DeleteObj(Id);
                            }
                        }
                        if (!isSelectRow)
                            MessageBox.Show("Vui lòng chọn dữ liệu muốn xoá.");
                        else
                            LoadDataToGridView();
                    }
                }

            }
            catch (Exception ex)
            {                
                MessageBox.Show("Lỗi: "+ex.Message);
            }            
        }

        private void dgListConfig_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                
                int rowIndex = e.RowIndex;
                int columnIndex = e.ColumnIndex;
                idSoundReadConfig = int.Parse(dgListConfig.Rows[rowIndex].Cells["Id"].Value.ToString());                
                if (columnIndex == 2)
                {                    
                    FrmCauHinhDocAmThanh_Create form = new FrmCauHinhDocAmThanh_Create(idSoundReadConfig, idChuyen, configType);
                    form.sender = new FrmCauHinhDocAmThanh_Create.SEND(LoadDataToGridView);
                    form.ShowDialog();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            var f = new frmChonChuyen(idChuyen);
            f.StartPosition = FormStartPosition.CenterScreen;
            f.ShowDialog();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                LoadDataToGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}
