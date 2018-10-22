using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuAn03_HaiDang
{
    public class FormBase : XtraForm
    {
        public FormBase()
        {
            //CheckDateActiveWithDateNow();
        }

        public void CheckDateActiveWithDateNow()
        {
            //try
            //{
            //    if (ModelStatic.dateCheckActive != DateTime.Now.Date)
            //    {
            //        MessageBox.Show("Lỗi: Phát sinh lỗi trong quá trình chỉnh sửa giờ hệ thống. Vui lòng khởi động lại chương trình.");
            //        Application.Exit();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Lỗi: " + ex.Message);
            //}
        }
    }
}
