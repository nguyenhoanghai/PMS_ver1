using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuAn03_HaiDang.POJO;
using DuAn03_HaiDang.Model;
namespace DuAn03_HaiDang.DATAACCESS
{
    public static class AccountSuccess
    {
        public static string TenTK { get; set; }
        public static string TenChuTK { get; set; }       
        public static int ThanhPham { get; set; }
        public static int BTP { get; set; }
        public static int ThaoTac { get; set; }
        public static string IdFloor { get; set; }
        public static int IsAll { get; set; }
        public static string strListChuyenId { get; set; }
        public static List<string> listChuyenId { get; set; }
        public static bool isWriteLog { get; set; }
        public static string strError { get; set; }
        public static List<System.Windows.Forms.Form> ListFormLCD { get; set; }
        public static bool IsOwner { get; set; }
        public static bool IsCompleteAcc { get; set; }
    
    }
}
