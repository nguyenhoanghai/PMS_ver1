using DuAn03_HaiDang.POJO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DuAn03_HaiDang.DATAACCESS;
using PMS.Business.Models;
using PMS.Business;
using PMS.Business.Enum;
using System.Threading;

namespace DuAn03_HaiDang.Helper
{
    public class HelperControl
    {
        public static void SetConfigForLable(LabelConfig labelConfig, Label label)
        {
            label.ForeColor = GetColor(labelConfig.Color);


            if (labelConfig.Bold)
            {
                if (!labelConfig.Italic)
                {
                    label.Font = new System.Drawing.Font(labelConfig.Font.Trim(), labelConfig.Size, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
                else
                {
                    label.Font = new System.Drawing.Font(labelConfig.Font.Trim(), labelConfig.Size, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
            }
            else
            {
                if (!labelConfig.Italic)
                {
                    label.Font = new System.Drawing.Font(labelConfig.Font.Trim(), labelConfig.Size, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
                else
                {
                    label.Font = new System.Drawing.Font(labelConfig.Font.Trim(), labelConfig.Size, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
            }
        }

        public static Color GetColor(string value)
        {
            Color result = Color.Black;
            try
            {
                var arrArgb = Regex.Split(value, ",");
                if (arrArgb != null)
                {
                    if (arrArgb.Length > 1)
                    {
                        result = Color.FromArgb(int.Parse(arrArgb[0]), int.Parse(arrArgb[1]), int.Parse(arrArgb[2]));
                    }
                    else
                    {
                        result = Color.FromName(value);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi định dạng màu không đúng : " + ex.Message, "Lỗi cấu hình", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            return result;
        }

        public static void ClearFormActiveLCD(string formName)
        {
            try
            {
                if (AccountSuccess.ListFormLCD != null && AccountSuccess.ListFormLCD.Count > 0)
                {
                    AccountSuccess.ListFormLCD = AccountSuccess.ListFormLCD.Where(c => !c.Name.Equals(formName.Trim())).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string ConvertVN(string chucodau)
        {
            const string FindText = "áàảãạâấầẩẫậăắằẳẵặđéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶĐÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴ";
            const string ReplText = "aaaaaaaaaaaaaaaaadeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAADEEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYY";
            int index = -1;
            char[] arrChar = FindText.ToCharArray();
            while ((index = chucodau.IndexOfAny(arrChar)) != -1)
            {
                int index2 = FindText.IndexOf(chucodau[index]);
                chucodau = chucodau.Replace(chucodau[index], ReplText[index2]);
            }
            return chucodau;
        }

        public static TimeSpan TimeIsWorkAllDayOfLine(List<LineWorkingShiftModel> shifts)
        {
            TimeSpan timeWork = new TimeSpan();
            timeWork = TimeSpan.Parse("00:00:00");
            if (shifts != null && shifts.Count > 0)
            {
                foreach (var item in shifts.OrderBy(x => x.ShiftOrder))
                {
                    timeWork += item.End - item.Start;
                }
            }
            return timeWork;
        }

        public static TimeSpan TimeIsWorkAllDayOfLine(List<LineWorkingShiftModel> shifts, int nsType)
        {
            TimeSpan timeWork = new TimeSpan();
            timeWork = TimeSpan.Parse("00:00:00");
            try
            {
                if (nsType == 0)
                {
                    foreach (var item in shifts.OrderBy(x => x.ShiftOrder))
                    {
                        timeWork += item.End - item.Start;
                    }
                }
                else
                {
                    var timeNow = DateTime.Now.TimeOfDay;
                    foreach (var item in shifts.OrderBy(x => x.ShiftOrder))
                    {
                        if (timeNow < item.End && timeNow > item.Start)
                        {
                            timeWork += timeNow - item.Start;
                            break;
                        }
                        else if (timeNow > item.Start && timeNow >= item.End)
                        {
                            timeWork += item.End - item.Start;
                        }
                    }
                }

            }
            catch (Exception) { }
            return timeWork;
        }

        /// <summary>
        /// khoi tao lai thong tin keypad cho chuyen
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="IsResetAllToZero">= true neu reset tat ca ve 0 else neu lay thong tin nang suat hien tai</param>
        /// <param name="frmMainNew"></param>
        /// <returns></returns>
        public static bool ResetKeypad(int lineId, bool IsResetAllToZero, FrmMainNew frmMainNew)
        {
            try
            {
                var listMapIdSanPhamNgay = new List<PMS.Data.MapIdSanPhamNgay>();
                string ngay = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;

                var listStrProductConfig = new List<string>();
                var errors = BLLError.GetAll();
                string strListChuyen = lineId + ",";
                var assigns = BLLProductivity.GetProductivitiesInDay(ngay, lineId);
                if (assigns != null && assigns.Count > 0)
                {
                    var listStrSend = new List<string>();
                    var listStrSetSanLuong = new List<string>();
                    var sanluongs = new List<string>();
                    var sttCSP = new List<int>();
                    int stt = 0;
                    foreach (var item in assigns)
                    {
                        var mapIdSanPhamNgay = new PMS.Data.MapIdSanPhamNgay();
                        mapIdSanPhamNgay.MaChuyen = item.LineId;
                        mapIdSanPhamNgay.STTChuyenSanPham = item.STTCHuyen_SanPham;
                        mapIdSanPhamNgay.MaSanPham = item.productId;
                        stt += 1;
                        mapIdSanPhamNgay.STT = stt;
                        mapIdSanPhamNgay.Ngay = ngay;
                        listMapIdSanPhamNgay.Add(mapIdSanPhamNgay);
                        string strSend = mapIdSanPhamNgay.STT.ToString() + ",";
                        string tenSanPham = item.ProductName;
                        if (!string.IsNullOrEmpty(tenSanPham))
                        {
                            if (tenSanPham.Length > 10)
                                strSend += tenSanPham.Substring(0, 10);
                            else
                                strSend += tenSanPham;
                        }
                        listStrSend.Add(strSend);
                        listStrSetSanLuong.Add(mapIdSanPhamNgay.STT.ToString());
                        sttCSP.Add(item.STTCHuyen_SanPham);
                    }

                    //
                    var keypads = BLLKeyPad.GetKeyPadInfoByLineId(lineId);
                    if (keypads != null && keypads.Count > 0)
                    {
                        if (listStrSend.Count > 0 || listStrSetSanLuong.Count > 0)
                        {
                            foreach (var kp_Obj in keypads)
                            {
                                if (kp_Obj.UseTypeId == (int)eUseKeyPadType.OneKeyPadOneObject)
                                {
                                    frmMainNew.listDataSendKeyPad.Add(kp_Obj.EquipmentId.ToString() + "," + (int)eCommandSend.ClearData + ",,");
                                    Thread.Sleep(500);
                                    foreach (var strSend in listStrSend)
                                    {
                                        listStrProductConfig.Add(kp_Obj.EquipmentId.ToString() + "," + (int)eCommandSend.ProductConfig + "," + strSend);
                                    }
                                    if (IsResetAllToZero) // reset keypad + reset so lieu ve 0
                                    {
                                        foreach (var strSend in listStrSetSanLuong)
                                        {
                                            listStrProductConfig.Add(kp_Obj.EquipmentId.ToString() + "," + (int)eCommandSend.ChangeBTPQuantities + "," + strSend + ",0,1,");
                                            if (errors != null && errors.Count > 0)
                                                foreach (var error in errors)
                                                {
                                                    listStrProductConfig.Add(kp_Obj.EquipmentId.ToString() + "," + (int)eCommandSend.ChangeProductError + "," + strSend + ",0," + error.Code);
                                                }

                                            listStrProductConfig.Add(kp_Obj.EquipmentId.ToString() + "," + (int)eCommandSend.ChangeProductQuantity + "," + strSend + ",0," + (int)eProductOutputType.KCS);
                                            listStrProductConfig.Add(kp_Obj.EquipmentId.ToString() + "," + (int)eCommandSend.ChangeProductQuantity + "," + strSend + ",0," + (int)eProductOutputType.TC); 
                                             
                                        }
                                    }
                                    else   // chi reset keypad van giu so lieu cũ
                                    {
                                        for (int i = 0; i < listStrSetSanLuong.Count; i++)
                                        {
                                            var nxInDay = BLLProductivity.GetProductivitiesInDay(DateTime.Now, sttCSP[i]);
                                            if (nxInDay != null)
                                            {
                                                var quantities = nxInDay.BTPTang - nxInDay.BTPGiam;
                                                listStrProductConfig.Add(kp_Obj.EquipmentId.ToString() + "," + (int)eCommandSend.ChangeBTPQuantities + "," + listStrSetSanLuong[i] + "," + (quantities < 0 ? 0 : quantities) + ",1");
                                                if (errors != null && errors.Count > 0)
                                                    foreach (var error in errors)
                                                    {
                                                        var err = nxInDay.Errors.FirstOrDefault(x => x.ErrorId == error.Id);
                                                        listStrProductConfig.Add(kp_Obj.EquipmentId.ToString() + "," + (int)eCommandSend.ChangeProductError + "," + listStrSetSanLuong[i] + "," + (err != null ? err.SoLuongTang - err.SoLuongGiam : 0) + "," + error.Code);
                                                    }

                                                switch (kp_Obj.TypeOfKeypad)
                                                {
                                                    case (int)eTypeOfKeypad.All:
                                                        quantities = (nxInDay.ThucHienNgay - nxInDay.ThucHienNgayGiam);
                                                        listStrProductConfig.Add(kp_Obj.EquipmentId.ToString() + "," + (int)eCommandSend.ChangeProductQuantity + "," + listStrSetSanLuong[i] + "," + (quantities < 0 ? 0 : quantities) + "," + (int)eProductOutputType.KCS); 

                                                        quantities = (nxInDay.BTPThoatChuyenNgay - nxInDay.BTPThoatChuyenNgayGiam);
                                                        listStrProductConfig.Add(kp_Obj.EquipmentId.ToString() + "," + (int)eCommandSend.ChangeProductQuantity + "," + listStrSetSanLuong[i] + "," + (quantities < 0 ? 0 : quantities) + "," + (int)eProductOutputType.TC); 
                                                        break;
                                                    case (int)eTypeOfKeypad.KCS:
                                                        quantities = (nxInDay.ThucHienNgay - nxInDay.ThucHienNgayGiam);
                                                        listStrProductConfig.Add(kp_Obj.EquipmentId.ToString() + "," + (int)eCommandSend.ChangeProductQuantity + "," + listStrSetSanLuong[i] + "," + (quantities < 0 ? 0 : quantities) + "," + (int)eProductOutputType.KCS); 
                                                        break;
                                                    case (int)eTypeOfKeypad.TC:
                                                        quantities = (nxInDay.BTPThoatChuyenNgay - nxInDay.BTPThoatChuyenNgayGiam);
                                                        listStrProductConfig.Add(kp_Obj.EquipmentId.ToString() + "," + (int)eCommandSend.ChangeProductQuantity + "," + listStrSetSanLuong[i] + "," + (quantities < 0 ? 0 : quantities) + "," + (int)eProductOutputType.TC); 
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(strListChuyen) && listMapIdSanPhamNgay.Count > 0)
                {
                    var resultAdd = BLLMapCommoIdForDay.AddMapIdSanPhamNgay(strListChuyen.Substring(0, strListChuyen.Length - 1).Split(',').Select(x => Convert.ToInt32(x)).ToList(), listMapIdSanPhamNgay); ;
                    if (resultAdd)
                    {
                        if (listStrProductConfig.Count > 0)
                            frmMainNew.listDataSendKeyPad.AddRange(listStrProductConfig);

                        if (frmMainNew.listMapIdSanPhamNgay != null)
                        {
                            var olds = frmMainNew.listMapIdSanPhamNgay.Where(x => x.MaChuyen == lineId).ToList();
                            if (olds.Count > 0)
                            {
                                foreach (var item in olds)
                                {
                                    frmMainNew.listMapIdSanPhamNgay.Remove(item);
                                }
                            }
                        }
                        else if (frmMainNew.listMapIdSanPhamNgay == null)
                            frmMainNew.listMapIdSanPhamNgay = new List<PMS.Data.MapIdSanPhamNgay>();
                        frmMainNew.listMapIdSanPhamNgay.AddRange(listMapIdSanPhamNgay);

                        // Reset lai thong tin ngay ve 0
                        if (IsResetAllToZero)
                            BLLProductivity.ResetDayInforByDate(DateTime.Now, assigns.Select(x => x.STTCHuyen_SanPham).ToList(), frmMainNew.getBTPInLineByType);
                        return true;
                    }
                }
            }
            catch (Exception)
            { }
            return false;
        }

        /// <summary>
        /// Dong bo lai thong tin keypad cua tat cac cac cum sau khi su ly xong
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="sttChuyenSanPham"></param>
        /// <param name="errorId"></param>
        /// <param name="typeOfProduction">loai hinh nang suat KCS - TC - BTP -ERROR </param>
        /// <param name="todayStr"></param>
        /// <param name="frmMainNew"></param>
        public static void ResetKeypad(int lineId, int sttChuyenSanPham, int errorId, int typeOfProduction, string todayStr, FrmMainNew frmMainNew)
        {
            try
            {
                var assigns = BLLProductivity.GetProductivitiesInDay(todayStr, lineId);
                if (assigns != null && assigns.Count > 0)
                {
                    for (int i = 0; i < assigns.Count; i++)
                    {
                        if (assigns[i].STTCHuyen_SanPham == sttChuyenSanPham)
                        {
                            var keypads = BLLKeyPad.GetKeyPadInfoByLineId(lineId);
                            if (keypads != null && keypads.Count > 0)
                            {
                                var nxInDay = BLLProductivity.GetProductivitiesInDay(DateTime.Now, sttChuyenSanPham);
                                int quantities = 0;
                                if (nxInDay != null)
                                {
                                    foreach (var keyPadInfo in keypads)
                                    {
                                        if (keyPadInfo.UseTypeId == (int)eUseKeyPadType.OneKeyPadOneObject)
                                        {
                                           // frmMainNew.listDataSendKeyPad.Add(keyPadInfo.EquipmentId.ToString() + "," + (int)eCommandSend.ClearData + ",,");
                                            switch (typeOfProduction)
                                            {
                                                case (int)eProductOutputType.KCS:
                                                    if (keyPadInfo.TypeOfKeypad == (int)eTypeOfKeypad.All || keyPadInfo.TypeOfKeypad == (int)eTypeOfKeypad.KCS)
                                                    {
                                                        quantities = (nxInDay.ThucHienNgay - nxInDay.ThucHienNgayGiam);
                                                        frmMainNew.listDataSendKeyPad.Add(keyPadInfo.EquipmentId.ToString() + "," + (int)eCommandSend.ChangeProductQuantity + "," + (i + 1) + "," + (quantities < 0 ? 0 : quantities) + "," + (int)eProductOutputType.KCS);
                                                    }
                                                    break;
                                                case (int)eProductOutputType.TC:
                                                    if (keyPadInfo.TypeOfKeypad == (int)eTypeOfKeypad.All || keyPadInfo.TypeOfKeypad == (int)eTypeOfKeypad.TC)
                                                    {
                                                        quantities = (nxInDay.BTPThoatChuyenNgay - nxInDay.BTPThoatChuyenNgayGiam);
                                                        frmMainNew.listDataSendKeyPad.Add(keyPadInfo.EquipmentId.ToString() + "," + (int)eCommandSend.ChangeProductQuantity + "," + (i + 1) + "," + (quantities < 0 ? 0 : quantities) + "," + (int)eProductOutputType.TC);
                                                    }
                                                    break;
                                                case (int)eProductOutputType.BTP:
                                                    quantities = nxInDay.BTPTang - nxInDay.BTPGiam;
                                                    frmMainNew.listDataSendKeyPad.Add(keyPadInfo.EquipmentId.ToString() + "," + (int)eCommandSend.ChangeBTPQuantities + "," + (i + 1) + "," + (quantities < 0 ? 0 : quantities) + ",1");
                                                    break;
                                                case (int)eProductOutputType.Error:
                                                    var errorObj = BLLError.GetById(errorId);
                                                    var err = nxInDay.Errors.FirstOrDefault(x => x.ErrorId == errorId);
                                                    frmMainNew.listDataSendKeyPad.Add(keyPadInfo.EquipmentId.ToString() + "," + (int)eCommandSend.ChangeProductError + "," + (i + 1) + "," + (err != null ? err.SoLuongTang - err.SoLuongGiam : 0) + "," + errorObj.Code);
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
