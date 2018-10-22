using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.DAO;
using PMS.Data;
using PMS.Business;
using PMS.Business.Models;

namespace DuAn03_HaiDang
{

    public partial class FrmMessageBox : Form
    {
        Chuyen_SanPham chuyen_sanpham = new Chuyen_SanPham();
        Chuyen_SanPhamDAO chuyen_sanphamDAO = new Chuyen_SanPhamDAO(); 
        private int stt;
        public int STT
        {
            get { return stt; }
            set { stt = value; }
        }
        private int machuyen;
        public int MaChuyen
        {
            get { return machuyen; }
            set { machuyen = value; }
        }

        private string tenchuyen;
        public string TenChuyen
        {
            get { return tenchuyen; }
            set { tenchuyen = value; }
        }

        private int masanpham;
        public int MaSanPham
        {
            get { return masanpham; }
            set { masanpham = value; }
        }

        private string tensanpham;
        public string TenSanPham
        {
            get { return tensanpham; }
            set { tensanpham = value; }
        }

        private int sanluongkehoach;
        public int SanLuongKeHoach
        {
            get { return sanluongkehoach; }
            set { sanluongkehoach = value; }
        }

        private double nangxuatsanxuat;
        public double NangXuatSanXuat
        {
            get { return nangxuatsanxuat; }
            set { nangxuatsanxuat = value; }
        }

        private int thang;
        public int Thang
        {
            get { return thang; }
            set { thang = value; }
        }

        private int nam;
        public int Nam
        {
            get { return nam; }
            set { nam = value; }
        }

        private int sttThucHien;
        public int STTThucHien
        {
            get { return sttThucHien; }
            set { sttThucHien = value; }
        }

        public FrmMessageBox()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            try
            {
                chuyen_sanpham.STT = stt;
                chuyen_sanpham.MaChuyen = machuyen;
                chuyen_sanpham.MaSanPham = masanpham;
              //  chuyen_sanpham.NangXuatSanXuat = nangxuatsanxuat;
                chuyen_sanpham.SanLuongKeHoach = sanluongkehoach;
                chuyen_sanpham.IsFinish = false;
                chuyen_sanpham.TimeAdd = DateTime.Now;
                chuyen_sanpham.Thang = thang;
                chuyen_sanpham.Nam = nam;
                chuyen_sanpham.STTThucHien = sttThucHien;
                if (comboBox1.SelectedIndex == 1)
                {
                    var kq = Save(chuyen_sanpham);
                    if (kq.IsSuccess)
                    {
                        MessageBox.Show("Thêm phân công thành công.");
                        this.Close();
                    }
                    else
                    {
                        if (kq.Messages.Count > 0)
                        {
                            string error = string.Empty;
                            foreach (var modelerror in kq.Messages)
                            {
                                error += modelerror.msg + "\n";
                            }
                            MessageBox.Show(error);
                        }
                        else
                            MessageBox.Show("Quá trình thêm phân công thất bại.\n Lỗi: " + dbclass.error);
                    }
                }
                else
                {
                    var oldCSP = BLLAssignmentForLine.Instance.FindByStt(chuyen_sanpham.STT);
                    //chuyen_sanpham.TimeAdd = DateTime.Now;
                    int LuyKeTH = oldCSP.LuyKeTH;
                    if (chuyen_sanpham.SanLuongKeHoach > LuyKeTH)
                    {
                        var kq = BLLAssignmentForLine.Instance.Update(chuyen_sanpham, true, 1);
                        if (kq.IsSuccess)
                        {
                            MessageBox.Show("Thay đổi thông tin phân công thành công.");
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Lỗi: không thể thay đổi thông tin phân công.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Hiện tại Mặt Hàng này đã sản xuất được " + LuyKeTH + " Mặt Hàng, con số này lớn hơn sản lượng kế hoạch của Mặt Hàng mà bạn nhập. Vui lòng nhập lại sản lượng kế hoạch cho Mặt Hàng...", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private ResponseBase Save(Chuyen_SanPham chuyen_sanpham)
        {
            var result = new ResponseBase();
            try
            {
                var dateNow = DateTime.Now;
                int morthNow = dateNow.Month;
                int yearNow = dateNow.Year;
                if (chuyen_sanpham.Nam >= yearNow)
                {
                    if (chuyen_sanpham.Nam == yearNow)
                    {
                        if (chuyen_sanpham.Thang >= morthNow)
                        {
                            var cspExist = BLLAssignmentForLine.Instance.Find(chuyen_sanpham.MaChuyen, chuyen_sanpham.MaSanPham);
                            if (cspExist == null)
                            {
                                result = BLLAssignmentForLine.Instance.Insert(chuyen_sanpham);
                            }
                            else
                            {
                                if (cspExist.LuyKeTH == 0)
                                {
                                    if (MessageBox.Show("Bạn đã phân công mặt hàng " + TenSanPham + " cho chuyền " + tenchuyen + " vào thời gian " + chuyen_sanpham.Thang + "/" + chuyen_sanpham.Nam + " rồi. Bạn có muốn cập nhập thông tin này ?", "Cập nhập dữ liệu", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                    {
                                        var reUpdate = BLLAssignmentForLine.Instance.Update(chuyen_sanpham, true, 1);
                                        if (!reUpdate.IsSuccess)
                                            MessageBox.Show("Cập nhập thông tin phân công mặt hàng " + TenSanPham + " cho chuyền " + TenChuyen + " vào thời gian " + chuyen_sanpham.Thang + "/" + chuyen_sanpham.Nam + "không thành công.");
                                        else
                                            result = reUpdate;
                                    }
                                }
                                else
                                    MessageBox.Show("Bạn đã phân công mặt hàng " + TenSanPham + " cho chuyền " + TenChuyen + " vào thời gian " + chuyen_sanpham.Thang + "/" + chuyen_sanpham.Nam + " rồi. Việc phân công có thông tin sản xuất nên không thể chỉnh sửa");
                            }
                        }
                        else
                        {
                            result.Messages.Add(new PMS.Business.Models.Message()
                            {
                                Title = "AddListPCC",
                                msg = "Lỗi: Tháng nhập phân công hàng cho chuyền không được nhỏ hơn tháng hiện tại."
                            });
                        }
                    }
                    else
                    {
                        var cspExist = BLLAssignmentForLine.Instance.Find(chuyen_sanpham.MaChuyen, chuyen_sanpham.MaSanPham);
                        if (cspExist == null)
                        {
                            result = BLLAssignmentForLine.Instance.Insert(chuyen_sanpham);
                        }
                        else
                        {
                            if (chuyen_sanpham.LuyKeTH == 0)
                            {
                                if (MessageBox.Show("Bạn đã phân công mặt hàng " + TenSanPham + " cho chuyền " + TenChuyen + " vào thời gian " + chuyen_sanpham.Thang + "/" + chuyen_sanpham.Nam + " rồi. Bạn có muốn cập nhập thông tin này ?", "Cập nhập dữ liệu", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    var reUpdate = BLLAssignmentForLine.Instance.Update(chuyen_sanpham, true, 1);
                                    if (!reUpdate.IsSuccess)
                                        MessageBox.Show("Cập nhập thông tin phân công mặt hàng " + TenSanPham + " cho chuyền " + TenChuyen + " vào thời gian " + chuyen_sanpham.Thang + "/" + chuyen_sanpham.Nam + "không thành công.");
                                    else
                                        result = reUpdate;
                                }
                            }
                            else
                                MessageBox.Show("Bạn đã phân công mặt hàng " + TenSanPham + " cho chuyền " + TenChuyen + " vào thời gian " + chuyen_sanpham.Thang + "/" + chuyen_sanpham.Nam + " rồi. Việc phân công có thông tin sản xuất nên không thể chỉnh sửa");
                        }
                    }
                }
                else
                {
                    result.Messages.Add(new PMS.Business.Models.Message()
                    {
                        Title = "AddListPCC",
                        msg = "Lỗi: Năm nhập phân công hàng cho chuyền không được nhỏ hơn năm hiện tại."
                    });
                }
            }
            catch (Exception ex)
            {
                result.Messages.Add(new PMS.Business.Models.Message()
                {
                    Title = "AddListPCC",
                    msg = "Lỗi: việc phân công Mặt Hàng " + TenSanPham + " cho chuyền " + TenChuyen + " thất bại."
                });
            }
            return result;
        }

    }
}
