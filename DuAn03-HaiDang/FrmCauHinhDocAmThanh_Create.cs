using DuAn03_HaiDang.DAO;
using DuAn03_HaiDang.Model;
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
    public partial class FrmCauHinhDocAmThanh_Create : Form
    {
        private SoundDAO soundDAO;
        private SoundIntConfigDAO soundIntConfigDAO;
        private SoundReadConfigDAO soundReadConfigDAO;
        int indexCbbFileTypeSelect = 0;
        int rowIndex = -1;
        int idReadNSConfig = 0;
        int idChuyen = 0;
        int configType = 1;
        public delegate void SEND();
        public SEND sender;
        public FrmCauHinhDocAmThanh_Create(int _idReadNSConfig, int _idChuyen, int _configType)
        {
            InitializeComponent();
            soundDAO = new SoundDAO();
            soundIntConfigDAO = new SoundIntConfigDAO();
            soundReadConfigDAO = new SoundReadConfigDAO();
            this.idReadNSConfig = _idReadNSConfig;
            this.idChuyen = _idChuyen;
            this.configType = _configType;
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmCauHinhDocAmThanh_Create_Load(object sender, EventArgs e)
        {
            try
            {
                cbbLoaiFile.SelectedIndex = indexCbbFileTypeSelect;
                LoadSoundsForCBB();
                LoadIntConfigsForCBB();
                cbbIntConfig.Visible = true;
                cbbSound.Visible = false;
                if (idReadNSConfig > 0)
                    LoadInfoUpdate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: "+ex.Message);
            }
        }

        private void LoadSoundsForCBB()
        {
            try
            {
                var list = soundDAO.GetListSelectItem();
                if (list != null && list.Count > 0)
                {
                    cbbSound.DataSource = list;
                    cbbSound.DisplayMember = "Text";
                    cbbSound.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        private void LoadIntConfigsForCBB()
        {
            try
            {
                var list = soundIntConfigDAO.GetListSelectItem();
                if (list != null && list.Count > 0)
                {
                    cbbIntConfig.DataSource = list;
                    cbbIntConfig.DisplayMember = "Text";
                    cbbIntConfig.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void cbbLoaiFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbbLoaiFile.SelectedIndex == 0)
                {
                    cbbIntConfig.Visible = true;
                    cbbSound.Visible = false;
                }
                else
                {
                    cbbIntConfig.Visible = false;
                    cbbSound.Visible = true;
                }
                indexCbbFileTypeSelect = cbbLoaiFile.SelectedIndex;
            }
            catch (Exception ex)
            {
                
            }
        }

        private void butAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridViewRow drow = new DataGridViewRow();

                DataGridViewCell cell = new DataGridViewTextBoxCell();
                cell.Value = cbbLoaiFile.SelectedIndex;
                drow.Cells.Add(cell);

                cell = new DataGridViewTextBoxCell();
                cell.Value = cbbLoaiFile.Text;
                drow.Cells.Add(cell);

                ModelSelect modelSelect=null;
                if(indexCbbFileTypeSelect==0)
                    modelSelect = (ModelSelect)cbbIntConfig.SelectedItem;
                else
                    modelSelect = (ModelSelect)cbbSound.SelectedItem;
                if(modelSelect!=null)
                {
                    cell = new DataGridViewTextBoxCell();
                    cell.Value = modelSelect.Value;
                    drow.Cells.Add(cell);

                    cell = new DataGridViewTextBoxCell();
                    cell.Value = modelSelect.Text;
                    drow.Cells.Add(cell);
                }

                int thuThuDoc = 1;
                int.TryParse(txtThuTuDoc.Text, out thuThuDoc);
                cell = new DataGridViewTextBoxCell();
                cell.Value = thuThuDoc.ToString();
                drow.Cells.Add(cell);

                cell = new DataGridViewTextBoxCell();
                cell.Value = chkIsActiveOfDetail.Checked.ToString();
                drow.Cells.Add(cell);

                dgListItem.Rows.Add(drow);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void dgListItem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                rowIndex = e.RowIndex;
                cbbLoaiFile.Text = dgListItem.Rows[rowIndex].Cells["FileType"].Value.ToString();
                int.TryParse(dgListItem.Rows[rowIndex].Cells["IndexTypeSelect"].Value.ToString(), out indexCbbFileTypeSelect);
                if (indexCbbFileTypeSelect == 0)
                {
                    cbbIntConfig.Visible = true;
                    cbbSound.Visible = false;
                    cbbIntConfig.Text = dgListItem.Rows[rowIndex].Cells["ValueFile"].Value.ToString();
                }
                else
                {
                    cbbIntConfig.Visible = false;
                    cbbSound.Visible = true;
                    cbbSound.Text = dgListItem.Rows[rowIndex].Cells["ValueFile"].Value.ToString();
                }
                txtThuTuDoc.Text = dgListItem.Rows[rowIndex].Cells["ThuTuDoc"].Value.ToString();
                bool isActive = false;
                bool.TryParse(dgListItem.Rows[rowIndex].Cells["IsActiveOfDetail"].Value.ToString(), out isActive);
                chkIsActiveOfDetail.Checked = isActive;
            }
            catch (Exception ex)
            {
                 MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void butDeleteItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (rowIndex != -1)
                    dgListItem.Rows.RemoveAt(rowIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void butUpdateItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (rowIndex != -1)
                {
                    
                    dgListItem.Rows[rowIndex].Cells["FileType"].Value = cbbLoaiFile.Text;
                    dgListItem.Rows[rowIndex].Cells["IndexTypeSelect"].Value = indexCbbFileTypeSelect;
                    if (indexCbbFileTypeSelect == 0)
                    {
                        ModelSelect modelSelect = (ModelSelect)cbbIntConfig.SelectedItem;
                        dgListItem.Rows[rowIndex].Cells["ValueFile"].Value = modelSelect.Text;
                        dgListItem.Rows[rowIndex].Cells["IdObj"].Value = modelSelect.Value;
                    }
                    else
                    {
                        ModelSelect modelSelect = (ModelSelect)cbbSound.SelectedItem;
                        dgListItem.Rows[rowIndex].Cells["ValueFile"].Value = modelSelect.Text;
                        dgListItem.Rows[rowIndex].Cells["IdObj"].Value = modelSelect.Value;
                    }
                    
                    dgListItem.Rows[rowIndex].Cells["ThuTuDoc"].Value = txtThuTuDoc.Text;
                    dgListItem.Rows[rowIndex].Cells["IsActiveOfDetail"].Value = chkIsActiveOfDetail.Checked;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        public SoundReadConfig BuildModel()
        {
            SoundReadConfig model = null;
            try
            {
                if (string.IsNullOrEmpty(txtName.Text))
                    MessageBox.Show("Lỗi: Tên cấu hình không được để trống");
                else
                {
                    model = new SoundReadConfig();
                    model.Id = idReadNSConfig;
                    model.Name = txtName.Text;
                    model.Description = txtDescription.Text;
                    model.IsActive = chkIsActive.Checked;
                    model.IdChuyen = idChuyen;
                    model.ConfigType = configType;

                    if (dgListItem.Rows != null && dgListItem.Rows.Count > 0)
                    {
                        foreach (DataGridViewRow row in dgListItem.Rows)
                        {
                            SoundReadItem item = new SoundReadItem();
                            item.IntType = int.Parse(row.Cells["IndexTypeSelect"].Value.ToString());
                            if (item.IntType == 0)
                                item.IdIntConfig = int.Parse(row.Cells["IdObj"].Value.ToString());
                            else
                                item.IsSound = int.Parse(row.Cells["IdObj"].Value.ToString());
                            item.OrderIndex = int.Parse(row.Cells["ThuTuDoc"].Value.ToString());
                            item.IsActive = bool.Parse(row.Cells["IsActiveOfDetail"].Value.ToString());
                            model.listItem.Add(item);
                        }
                    }
                }
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
                SoundReadConfig model = BuildModel();
                if (model != null)
                {
                    int result = 0;
                    if (model.Id == 0)
                    {
                        result = soundReadConfigDAO.AddObj(model);
                    }
                    else
                    {
                        result = soundReadConfigDAO.UpdateObj(model);
                    }
                    if (result > 0)
                    {
                        MessageBox.Show("Lưu dữ liệu thành công.");
                        this.sender();
                        this.Close();
                    }
                    else
                        MessageBox.Show("Lưu dữ liệu thất bại.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void LoadInfoUpdate()
        {
            try
            {
                if(idReadNSConfig>0)
                {
                    SoundReadConfig model = soundReadConfigDAO.GetInfoById(idReadNSConfig);
                    if (model != null)
                    {
                        txtName.Text = model.Name;
                        txtDescription.Text = model.Description;
                        chkIsActive.Checked = model.IsActive;
                        if (model.listItem != null && model.listItem.Count > 0)
                        {
                            foreach (SoundReadItem item in model.listItem)
                            {
                                DataGridViewRow drow = new DataGridViewRow();

                                DataGridViewCell cell = new DataGridViewTextBoxCell();
                                cell.Value = item.IntType;
                                drow.Cells.Add(cell);
                                
                                cell = new DataGridViewTextBoxCell();
                                if (item.IntType == 0)
                                    cell.Value = "Đọc số";
                                else
                                    cell.Value = "Đọc âm thanh";
                                drow.Cells.Add(cell);

                                ModelSelect modelSelect = null;
                                if (item.IntType == 0)
                                {
                                    cell = new DataGridViewTextBoxCell();
                                    cell.Value = item.IdIntConfig;
                                    drow.Cells.Add(cell);

                                    cell = new DataGridViewTextBoxCell();
                                    if (cbbIntConfig.Items != null && cbbIntConfig.Items.Count > 0)
                                    {
                                        foreach (ModelSelect sl in cbbIntConfig.Items)
                                        {
                                            if (sl.Value == item.IdIntConfig)
                                            {
                                                cell.Value = sl.Text;
                                                break;
                                            }
                                        }
                                    }                                    
                                    drow.Cells.Add(cell);
                                }
                                else
                                {
                                    cell = new DataGridViewTextBoxCell();
                                    cell.Value = item.IsSound;
                                    drow.Cells.Add(cell);

                                    cell = new DataGridViewTextBoxCell();
                                    if (cbbSound.Items != null && cbbSound.Items.Count > 0)
                                    {
                                        foreach (ModelSelect sl in cbbSound.Items)
                                        {
                                            if (sl.Value == item.IsSound)
                                            {
                                                cell.Value = sl.Text;
                                                break;
                                            }
                                        }
                                    }    
                                    drow.Cells.Add(cell);
                                }
                               
                                cell = new DataGridViewTextBoxCell();
                                cell.Value = item.OrderIndex;
                                drow.Cells.Add(cell);

                                cell = new DataGridViewTextBoxCell();
                                cell.Value = item.IsActive;
                                drow.Cells.Add(cell);

                                dgListItem.Rows.Add(drow);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }
    }
}
