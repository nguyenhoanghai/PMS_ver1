using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business
{
    public static class BLLAccount
    {
        public static ResponseBase FindAccount(string userName, string password)
        {
            var result = new ResponseBase(); 
            try
            {
                using (var db = new PMSEntities())
                { 
                    var user = db.TaiKhoans.FirstOrDefault(x => x.UserName.Trim().ToUpper().Equals(userName.Trim().ToUpper()) && x.Password.Trim().ToUpper().Equals(password.Trim().ToUpper()));
                    if (user != null)
                    {
                        result.IsSuccess = true;
                        result.Data = user;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Messages.Add(new Message() { Title = "Đăng Nhập", msg = "Tên Đăng Nhập hoặc Mật Khẩu không đúng." });
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Messages.Add(new Message(){ Title = "Lỗi Đăng Nhập", msg="Lỗi: trong quá trình kiểm tra đăng nhập. " + ex.Message });
              }
            return result;

        }
    }
}
