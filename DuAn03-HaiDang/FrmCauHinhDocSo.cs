using DuAn03_HaiDang.DAO;
using DuAn03_HaiDang.POJO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DuAn03_HaiDang.Model;
using PMS.Business;

namespace DuAn03_HaiDang
{
    public partial class FrmCauHinhDocSo : Form
    {
        private SoundIntConfigDAO soundIntConfigDAO;
      //  private ErrorDAO errorDAO;
        private int idConfig = 0;
        public FrmCauHinhDocSo()
        {
            InitializeComponent();
            soundIntConfigDAO = new SoundIntConfigDAO();
       //     errorDAO = new ErrorDAO();
        }

        private void butHuyMailSend_Click(object sender, EventArgs e)
        {
            if (idConfig == 0)
            {
                EnableControlSend(true, false, false, false, false);
                ClearInput();
            }
            else
            {
                if (butSaveMailSend.Enabled)
                    EnableControlSend(false, true, false, true, true);
                else
                {
                    EnableControlSend(true, false, false, false, false);
                    ClearInput();
                }
            }            
            //this.Close();
            EnableInputMSend(false);
        }

        private void LoadCbbSoundInt()
        {
            try
            {
                List<ModelSelect> listModelSelect = new List<ModelSelect>();
                listModelSelect.Add(new ModelSelect() { Value=0, Text="<<Chọn cấu hình đọc số>>", Code=""});
                var listSelect = soundIntConfigDAO.GetListSelectItem();
                listModelSelect.AddRange(listSelect);
                cbbSoundInt.DataSource = listModelSelect;
                cbbSoundInt.DisplayMember = "Text";
                
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        private SoundIntConfig BuilModel()
        {
            SoundIntConfig model = null;
            try
            {
                if (string.IsNullOrEmpty(txtCode.Text))
                    MessageBox.Show("Lỗi: Mã cấu hình không được để trống.");
                else if (string.IsNullOrEmpty(txtName.Text))
                    MessageBox.Show("Lỗi: Tên cấu hình không được để trống.");
                else if (string.IsNullOrEmpty(txtFormula.Text))
                    MessageBox.Show("Lỗi: Công thức không được để trống");
                else
                {
                    model = new SoundIntConfig();
                    model.Code = "[|" + txtCode.Text + "]";
                    model.IsActive = chkIsActive.Checked;
                    model.Id = idConfig;
                    model.Name = txtName.Text;
                    model.Description = txtDescription.Text;
                    model.Formula = txtFormula.Text;
                    model.IsProductivity = chkIsXetNangSuat.Checked;
                }
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            return model;
        }

        private void butSaveMailSend_Click(object sender, EventArgs e)
        {
            try
            {
                SoundIntConfig model = BuilModel();
                if (model != null)
                {
                    int result = 0;
                    if (idConfig == 0)
                    {
                        result = soundIntConfigDAO.AddObj(model);
                    }
                    else
                    {
                        result = soundIntConfigDAO.UpdateObj(model);
                    }
                    if (result > 0)
                    {
                        MessageBox.Show("Lưu dữ liệu thành công.");
                        LoadDataToGridView();
                        idConfig = 0;
                        ClearInput();
                        EnableControlSend(true, false, false, false, false);
                        EnableInputMSend(false);
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

        private void butDeleteMailSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (idConfig > 0)
                {
                    var result =soundIntConfigDAO.DeleteObj(idConfig);
                    if (result > 0)
                    {
                        MessageBox.Show("Xoá dữ liệu thành công");
                        LoadDataToGridView();
                        EnableControlSend(true, false, false, false, false);
                        EnableInputMSend(false);
                    }
                    else MessageBox.Show("Xoá dữ liệu thất bại");
                }
                else
                    MessageBox.Show("Bạn chưa chọn dữ liệu muốn xoá. Vui lòng chọn dữ liệu");
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        private void CreateToFormula(string item)
        {
            try
            {
                int txtSelectStart = txtFormula.SelectionStart;
                string[] phepTinh = new string[] { "+", "-", "*", "/" };
                if (string.IsNullOrEmpty(txtFormula.Text))
                {
                    if (!phepTinh.Contains(item))
                        //txtFormula.Text += item;
                        txtFormula.Text = txtFormula.Text.Insert(txtFormula.SelectionStart, item);
                }
                else
                {                    
                    string formula = txtFormula.Text;
                    string lastChar = formula.Substring(formula.Length - 1, 1);
                    string str = formula.Substring(0, formula.Length - 1);                   
                    if (phepTinh.Contains(item))
                    {
                        if (phepTinh.Contains(lastChar))
                            //txtFormula.Text = str + item;
                            txtFormula.Text = txtFormula.Text.Insert(txtSelectStart, str + item);
                        else
                            //txtFormula.Text += item;
                            txtFormula.Text = txtFormula.Text.Insert(txtSelectStart, item);
                    }
                    else
                    {
                        switch (lastChar)
                        {
                            case "]":
                                break;
                            default:
                                //txtFormula.Text += item;
                                txtFormula.Text = txtFormula.Text.Insert(txtSelectStart, item);
                                break;
                        }
                    }
                }
                txtFormula.Focus();
                txtFormula.SelectionStart = txtSelectStart + item.Length;
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        private void butSanLuongKeHoach_Click(object sender, EventArgs e)
        {
            CreateToFormula(((Button)sender).Tag.ToString());
        }

        private void butLuyKeTH_Click(object sender, EventArgs e)
        {
            CreateToFormula(((Button)sender).Tag.ToString());
        }

        private void butLuyKeBTPThoatChuyen_Click(object sender, EventArgs e)
        {
            CreateToFormula(((Button)sender).Tag.ToString());
        }

        private void butDinhMucNgay_Click(object sender, EventArgs e)
        {
            CreateToFormula(((Button)sender).Tag.ToString());
        }

        private void butBTPLoi_Click(object sender, EventArgs e)
        {
            CreateToFormula(((Button)sender).Tag.ToString());
        }

        private void butThucHienNgay_Click(object sender, EventArgs e)
        {
            CreateToFormula(((Button)sender).Tag.ToString());
        }

        private void butBTPTrenChuyen_Click(object sender, EventArgs e)
        {
            CreateToFormula(((Button)sender).Tag.ToString());
        }

        private void butSanLuongLoi_Click(object sender, EventArgs e)
        {
            CreateToFormula(((Button)sender).Tag.ToString());
        }

        private void butBTPThoatChuyenNgay_Click(object sender, EventArgs e)
        {
            CreateToFormula(((Button)sender).Tag.ToString());
        }

        private void FrmCauHinhDocSo_Load(object sender, EventArgs e)
        {
            try
            {
                LoadDataToGridView();
                //LoadCbbSoundInt();
                EnableControlSend(true, false, false, false, false);
                ClearInput();
                EnableInputMSend(false);
                var listError = BLLError.GetAll(); // errorDAO.GetListError();
                cbbError1.DataSource = listError;
                cbbError1.DisplayMember = "ErrorName";
                cbbError2.DataSource = listError;
                cbbError2.DisplayMember = "ErrorName";
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
                soundIntConfigDAO.LoadDataForGridView(dgListSound);
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        private void dgListSound_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int row = e.RowIndex;
                txtName.Text = dgListSound.Rows[row].Cells["FileName"].Value.ToString();
                txtDescription.Text = dgListSound.Rows[row].Cells["Description"].Value.ToString();
                txtFormula.Text = dgListSound.Rows[row].Cells["Formula"].Value.ToString();
                int.TryParse(dgListSound.Rows[row].Cells["Id"].Value.ToString(), out idConfig);
                bool isActive = false;
                bool.TryParse(dgListSound.Rows[row].Cells["IsActive"].Value.ToString(), out isActive);
                bool isProductivity = false;
                bool.TryParse(dgListSound.Rows[row].Cells["IsProductivity"].Value.ToString(), out isProductivity);
                chkIsActive.Checked = isActive;
                chkIsActive.Enabled = true;
                chkIsXetNangSuat.Checked = isProductivity;
                chkIsXetNangSuat.Enabled = true;
                txtCode.Text = dgListSound.Rows[row].Cells["Code"].Value.ToString();
                EnableControlSend(false, true, false, true, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void butclear_Click(object sender, EventArgs e)
        {
            try 
	        {
                if (!string.IsNullOrEmpty(txtFormula.Text))
                {
                    txtFormula.Text = txtFormula.Text.Substring(0, txtFormula.Text.Length - 1);
                }
	        }
	        catch (Exception ex)
	        {
                MessageBox.Show("Lỗi: " + ex.Message);
	        }
            
        }

        private void butChoose_Click(object sender, EventArgs e)
        {
            try
            {
                ModelSelect modelSelect = (ModelSelect)cbbSoundInt.SelectedItem;
                txtFormula.Text += "(" + modelSelect.Code.Trim() + ")";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void ClearInput()
        { 
            txtCode.Text = string.Empty;
            txtName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtFormula.Text = string.Empty;
            chkIsActive.Checked = true;
            chkIsActive.Enabled = false;
            chkIsXetNangSuat.Checked = false;            
            idConfig = 0;
            
        }

        private void butAddMailSend_Click(object sender, EventArgs e)
        {
            ClearInput();
            EnableControlSend(false, false, true, false, true);
            EnableInputMSend(true);
        }

        private void EnableControlSend(bool isButAdd, bool isButUpdate, bool isButSave, bool isButDelete, bool isButCancel)
        {
            butAddMailSend.Enabled = isButAdd;
            butUpdateMailSend.Enabled = isButUpdate;
            butSaveMailSend.Enabled = isButSave;
            butDeleteMailSend.Enabled = isButDelete;
            butHuyMailSend.Enabled = isButCancel;
        }

        private void butUpdateMailSend_Click(object sender, EventArgs e)
        {
            EnableControlSend(false, false, true, false, true);
            EnableInputMSend(true);
        }

        private void EnableInputMSend(bool enable)
        {
            try
            {
                txtCode.Enabled = enable;
                txtName.Enabled = enable;
                txtDescription.Enabled = enable;
                txtFormula.Enabled = enable;
                chkIsActive.Enabled = enable;
                chkIsXetNangSuat.Enabled = enable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void butNSHours_Click(object sender, EventArgs e)
        {
            int intMinuter =0;
            int.TryParse(txtNSHours.Text, out intMinuter);
            string str = "[NangSuatCachGioHienTai|" + intMinuter + "|Phut]";
            CreateToFormula(str);
        }

        private void butCountErrorHours_Click(object sender, EventArgs e)
        {
            int intMinuter = 0;
            int.TryParse(txtErrorMinuter.Text, out intMinuter);
            string str = "[SoLuongLoiCachGioHienTai|" + intMinuter + "|Phut]";
            CreateToFormula(str);
        }

        private void butCountErrorById_Click(object sender, EventArgs e)
        {
            try
            {
                var error = (Error)cbbError1.SelectedItem;
                if(error!=null)
                {
                    string str = "[SoLuongLoi|" +error.ErrorName+"_"+ error.Id + "|]";
                    CreateToFormula(str);
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }            
        }

        private void butCountErrorHoursById_Click(object sender, EventArgs e)
        {
            try
            {
                 int intMinuter = 0;
                int.TryParse(txtErrorMinuterById.Text, out intMinuter);
                var error = (Error)cbbError1.SelectedItem;
                if (error != null)
                {
                    string str = "[SoLuongLoiCachThoiGianHienTai|" + error.ErrorName + "_" + error.Id + "_" + intMinuter + "|Phut]";
                    CreateToFormula(str);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }       
        }

        private void butTCHours_Click(object sender, EventArgs e)
        {
            int intMinuter = 0;
            int.TryParse(txtTCHours.Text, out intMinuter);
            string str = "[ThoatChuyenCachGioHienTai|" + intMinuter + "|Phut]";
            CreateToFormula(str);
        }

    }
}
