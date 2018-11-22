using DuAn03_HaiDang;
using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.Helper;
using PMS.Business;
using PMS.Business.Enum;
using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QuanLyNangSuat
{
    public partial class FrmResetKeypad : Form
    {
        FrmMainNew frmMainNew;
        public FrmResetKeypad(FrmMainNew _frmMainNew)
        {
            InitializeComponent();
            this.frmMainNew = _frmMainNew;
        }

        private void FrmResetKeypad_Load(object sender, EventArgs e)
        {
            try
            {
                var listChuyen = new List<LineModel>();
                listChuyen.Add(new LineModel() { MaChuyen = 0, TenChuyen = " - - Chọn Chuyền - - " });
                var sbc = BLLLine.GetLines(AccountSuccess.strListChuyenId.Split(',').Select(x => Convert.ToInt32(x)).ToList());
                listChuyen.AddRange(sbc);
                cboChuyen.DataSource = null;
                cboChuyen.DataSource = listChuyen;
                cboChuyen.DisplayMember = "TenChuyen";
                cboChuyen.SelectedIndex = 0;
            }
            catch (Exception)
            {
                MessageBox.Show("Không lấy được thông tin chuyền.", "Lỗi");
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                var line = (LineModel)cboChuyen.SelectedItem;
                if (line != null)
                    if (frmMainNew.KeypadQuantityProcessingType == 0)
                        HelperControl.ResetKeypad(line.MaChuyen, radioGroup1.SelectedIndex == 1 ? true : false, frmMainNew);
                    else
                        HelperControl.ResetKeypad_Moi(line.MaChuyen, radioGroup1.SelectedIndex == 1 ? true : false, frmMainNew);
                else
                    MessageBox.Show("Lỗi: Không thể khởi tạo thông tin KeyPad. Vì không có danh sách chuyền. Có thể bạn chưa chạy tiến trình tự động.");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}