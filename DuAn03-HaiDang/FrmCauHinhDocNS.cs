using DuAn03_HaiDang.DAO;
using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.KeyPad_Chuyen.dao;
using DuAn03_HaiDang.KeyPad_Chuyen.pojo;
using DuAn03_HaiDang.Model;
using DuAn03_HaiDang.POJO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace DuAn03_HaiDang
{
    public partial class FrmCauHinhDocNS : Form
    {
        private ChuyenDAO chuyenDAO;
        private SoundTimeConfigDAO soundTimeConfig;
        int idTimeConfig = 0;
        public FrmCauHinhDocNS()
        {
            InitializeComponent();
            try
            {
                chuyenDAO = new ChuyenDAO();
                soundTimeConfig = new SoundTimeConfigDAO();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: "+ex.Message);
            }
        }        

        private void FrmCauHinhDocNS_Load(object sender, EventArgs e)
        {
            try
            {
                CreateGirdView();
                LoadDataGridView();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void CreateGirdView()
        {
            try
            {
                if (!string.IsNullOrEmpty(AccountSuccess.strListChuyenId))
                {
                    var listChuyen = chuyenDAO.GetListChuyenInfByListId(AccountSuccess.strListChuyenId);
                    if (listChuyen != null && listChuyen.Count > 0)
                    {
                        dgReadSoundConfig.Rows.Clear();
                        PropertyInfo[] props = typeof(Chuyen).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        foreach (var chuyen in listChuyen)
                        {

                            DataGridViewRow dgrow = new DataGridViewRow();
                            //DataGridViewCell cell;

                            //for (int j = 0; j < props.Count(); j++)
                            //{
                            //    cell = new DataGridViewTextBoxCell();
                            //    cell.Value = props[j].GetValue(chuyen, null);
                            //    dgrow.Cells.Add(cell);
                            //}
                            DataGridViewCell cellIdChuyen = new DataGridViewTextBoxCell();
                            cellIdChuyen = new DataGridViewButtonCell();
                            cellIdChuyen.Value = chuyen.MaChuyen;
                            dgrow.Cells.Add(cellIdChuyen);

                            DataGridViewCell cellTenChuyen = new DataGridViewTextBoxCell();
                            cellTenChuyen = new DataGridViewButtonCell();
                            cellTenChuyen.Value = chuyen.TenChuyen;
                            dgrow.Cells.Add(cellTenChuyen);

                            DataGridViewCell cellFileAmThanhnh = new DataGridViewTextBoxCell();
                            cellFileAmThanhnh = new DataGridViewButtonCell();
                            cellFileAmThanhnh.Value = chuyen.Sound;
                            dgrow.Cells.Add(cellFileAmThanhnh);

                            DataGridViewCell cellThuTuDoc = new DataGridViewTextBoxCell();
                            cellThuTuDoc = new DataGridViewButtonCell();
                            cellThuTuDoc.Value = chuyen.STTReadNS;
                            dgrow.Cells.Add(cellThuTuDoc);

                            DataGridViewCell cellThayDoiThuTu = new DataGridViewTextBoxCell();
                            cellThayDoiThuTu = new DataGridViewButtonCell();
                            cellThayDoiThuTu.Value = "Thay đổi thứ tự";
                            dgrow.Cells.Add(cellThayDoiThuTu);

                            DataGridViewCell cellThayDoiFileDoc = new DataGridViewTextBoxCell();
                            cellThayDoiFileDoc = new DataGridViewButtonCell();
                            cellThayDoiFileDoc.Value = "Thay đổi file đọc";
                            dgrow.Cells.Add(cellThayDoiFileDoc);

                            DataGridViewCell cellCauHinhDocAmThanh = new DataGridViewTextBoxCell();
                            cellCauHinhDocAmThanh = new DataGridViewButtonCell();
                            cellCauHinhDocAmThanh.Value = "Cấu hình âm thanh";
                            dgrow.Cells.Add(cellCauHinhDocAmThanh);

                            dgReadSoundConfig.Rows.Add(dgrow);
                        }
                    }
                }
            }
            catch (Exception ex)
            {                
                throw ex;
            }    
        }

        private void dgReadSoundConfig_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int row = e.RowIndex;
                int column = e.ColumnIndex;
                string columnName= dgReadSoundConfig.Columns[column].Name;
                int idChuyen=0;
                string tenChuyen=string.Empty;
                int thuTuDoc= 0;
                string fileAmThanh = string.Empty;
                int.TryParse(dgReadSoundConfig.Rows[row].Cells["MaChuyen"].Value.ToString(), out idChuyen); 
                tenChuyen = dgReadSoundConfig.Rows[row].Cells["TenChuyen"].Value.ToString();
                fileAmThanh = dgReadSoundConfig.Rows[row].Cells["Sound"].Value.ToString();
                int.TryParse(dgReadSoundConfig.Rows[row].Cells["STTReadNS"].Value.ToString(), out thuTuDoc); 
                switch (columnName)
                {
                    case "ThayDoiThuTu":
                        {
                            FrmThayDoiThuTuDocChuyen form = new FrmThayDoiThuTuDocChuyen(idChuyen, tenChuyen, thuTuDoc);
                            form.sender = new FrmThayDoiThuTuDocChuyen.SEND(CreateGirdView);
                            form.ShowDialog();
                            break;
                        }
                    case "ThayDoiFileAmThanh":
                        {
                            FrmThayDoiFileAmThanh form = new FrmThayDoiFileAmThanh(idChuyen, tenChuyen, fileAmThanh);
                            form.sender = new FrmThayDoiFileAmThanh.SEND(CreateGirdView);
                            form.ShowDialog();
                            break;
                        }
                    case "CauHinhDocAmThanh":
                        {
                            FrmCauHinhDocAmThanh form = new FrmCauHinhDocAmThanh(idChuyen, tenChuyen, 1);
                            form.sender = new FrmCauHinhDocAmThanh.SEND(CreateGirdView);
                            form.ShowDialog();
                            break;
                        }
                    default:{
                        break;
                    }

                }
                //replace_special_word(join_unit(txtn.Text)).ToUpper().Trim();                           
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        public SoundTimeConfig BuildModel()
        {
            SoundTimeConfig model = null;
            try
            {
                model = new SoundTimeConfig();
                TimeSpan time = new TimeSpan(0, 0, 0);
                try
                {
                    DateTime datetime = DateTime.Parse(timeThoiGianDoc.EditValue.ToString());
                    time = datetime.TimeOfDay;
                }
                catch
                { TimeSpan.TryParse(timeThoiGianDoc.EditValue.ToString(), out time); }
                model.Time = TimeSpan.Parse(time.Hours.ToString() + ":" + time.Minutes.ToString() + ":00");
                int soLanDoc=1;
                int.TryParse(txtSoLanDoc.Text, out soLanDoc);
                model.SoLanDoc = soLanDoc;
                model.IsActive = chkIsActive.Checked;
                model.Id = idTimeConfig;
                // Cau hinh doc am thanh nang suat co configtype=1
                model.ConfigType = 1;
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            return model;
        }

        private void butSave_Click(object sender, EventArgs e)
        {
            try
            {
                SoundTimeConfig model = BuildModel();
                int result = 0;
                if (idTimeConfig == 0)
                {
                    result = soundTimeConfig.AddObj(model);
                }
                else
                {
                    result = soundTimeConfig.UpdateObj(model);
                }
                if (result > 0)
                {
                    MessageBox.Show("Lưu dữ liệu thành công.");
                    LoadDataGridView();
                    idTimeConfig = 0;
                   // if (MessageBox.Show("Bạn có muốn khởi động lại chương trình?", "Khởi động chương trình", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                     //   Application.Restart();
                }
                else
                {
                    MessageBox.Show("Lưu dữ liệu thất bại.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void butDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (idTimeConfig > 0)
                {
                    var result = soundTimeConfig.DeleteObj(idTimeConfig);
                    if (result > 0)
                    {
                        MessageBox.Show("Xoá dữ liệu thành công.");
                        LoadDataGridView();
                        idTimeConfig = 0;
                    }
                    else
                        MessageBox.Show("Xoá dữ liệu thất bại");
                }
                else
                    MessageBox.Show("Bạn chưa chọn dữ liệu muốn xoá.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void LoadDataGridView()
        {
            try
            {
                soundTimeConfig.LoadMailToDataGirdview(dgListTime, 1);
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        private void dgListTime_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int rowIndex = e.RowIndex;
                txtSoLanDoc.Text = dgListTime.Rows[rowIndex].Cells["SoLanDoc"].Value.ToString();
                var time = TimeSpan.Parse(dgListTime.Rows[rowIndex].Cells["Time"].Value.ToString());               
                timeThoiGianDoc.EditValue = time;
                idTimeConfig = int.Parse(dgListTime.Rows[rowIndex].Cells["Id"].Value.ToString());
                chkIsActive.Checked = bool.Parse(dgListTime.Rows[rowIndex].Cells["IsActive"].Value.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }


        private void ClearInput()
        {
            try
            {
                TimeSpan time = DateTime.Now.TimeOfDay;
                timeThoiGianDoc.EditValue = TimeSpan.Parse(time.Hours + ":" + time.Minutes + ":00");
                chkIsActive.Checked = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
