using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.POJO;
using QuanLyNangSuat.Model;

namespace DuAn03_HaiDang.DAO
{
    class ShiftDAO
    {
        public DataTable DSOBJ(string MaChuyen)
        {
            DataTable dt = new DataTable();
            string sql = "select IdShift, Name, TimeStart, TimeEnd from Shift where MaChuyen ='" + MaChuyen + "'";
            try
            {
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                return dt;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: không thể lấy được danh sách các ca làm việc của chuyền từ CSDL.", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return dt;
            }
        }

        public int ThemOBJ(Shift obj)
        {
            int kq = 0;
            try
            {
                string sql = "insert into Shift (MaChuyen, IdShift, Name, TimeStart, TimeEnd) values(N'" + obj.MaChuyen + "',N'" + obj.IdShift + "',N'" + obj.Name + "',N'" + obj.TimeStart + "',N'" + obj.TimeEnd + "')";
                kq = dbclass.TruyVan_XuLy(sql);
                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể thêm ca mới vào chuyền", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }

        public int SuaThongTinOBJ(Shift obj)
        {
            int kq = 0;
            try
            {
                string sql = "update Shift set TimeStart = N'" + obj.TimeStart + "', TimeEnd =N'" + obj.TimeEnd + "' where MaChuyen ='" + obj.MaChuyen + "' and IdShift = '"+obj.IdShift+"'";
                kq = dbclass.TruyVan_XuLy(sql);

                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể thay đổi thông tin ca làm việc dưới CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }

        public int XoaOBJ(string MaChuyen, string IdShift)
        {
            int kq = 0;
            try
            {
                string sql = "delete from Shift where MaChuyen ='" + MaChuyen + "' and IdShift ='"+IdShift+"'";
                kq = dbclass.TruyVan_XuLy(sql);
                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể xoá mặt hàng dưới CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }

        public DataTable TimKiemOBJ(string content)
        {
            DataTable dt = new DataTable();
            string sql = "select * from SanPham where TenSanPham like N'" + content + "' or MaSanPham = '" + content + "'";
            try
            {
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                return dt;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: không thể thực hiện tìm kiếm mặt hàng trên CSDL.", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return dt;
            }
        }

        public int IdShiftFinal(string MaChuyen)
        {
            DataTable dt = new DataTable();
            string sql = "select TOP 1 IdShift from Shift where MaChuyen ='"+MaChuyen+"' Order By IdShift DESC";
            try
            {
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    int IdFinal = int.Parse(dt.Rows[0][0].ToString());
                    return IdFinal;
                }
                else
                    return 0;                
            }
            catch (Exception)
            {
               return 0;                
            }
        }

        public TimeSpan FindTimeStart(string MaChuyen)
        {
            DataTable dt = new DataTable();
            string sql = "select MIN(TimeStart) from Shift where MaChuyen ='" + MaChuyen + "'";
            try
            {
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt.Rows.Count > 0)
                {

                    return TimeSpan.Parse(dt.Rows[0][0].ToString()); 
                }
                else
                    return TimeSpan.Parse("00:00:00");

            }
            catch (Exception)
            {
                return TimeSpan.Parse("00:00:00");

            }
        }

        public DataTable GetAllShift()
        {
            DataTable dt = new DataTable();
            string sql = "select IdShift, Name, TimeStart, TimeEnd, MaChuyen from Shift";
            try
            {
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                return dt;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: không thể lấy được danh sách các ca làm việc từ CSDL.", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return dt;
            }
        }

        public List<Shift> GetShiftsOfLine(string MaChuyen) 
        {
            List<Shift> listShift = new List<Shift>();            
            string strSQL = "select IdShift, Name, TimeStart, TimeEnd from Shift where MaChuyen =" + MaChuyen + " order by IdShift";
            DataTable dtAllShift = dbclass.TruyVan_TraVe_DataTable(strSQL);
            if (dtAllShift!=null && dtAllShift.Rows.Count > 0)
            {
                for (int i = 0; i < dtAllShift.Rows.Count; i++)
                {
                    Shift shift = new Shift()
                    {
                        IdShift = int.Parse(dtAllShift.Rows[i]["IdShift"].ToString()),
                        Name = dtAllShift.Rows[i]["Name"].ToString(),
                        TimeStart = TimeSpan.Parse(dtAllShift.Rows[i]["TimeStart"].ToString()),
                        TimeEnd = TimeSpan.Parse(dtAllShift.Rows[i]["TimeEnd"].ToString())
                    };
                    listShift.Add(shift);                   
                }
            }
            return listShift;
        }

        public List<ModelWorkHours> GetListWorkHoursOfLineByLineId(string lineId)
        {
            try
            {
                List<ModelWorkHours> listWorkHours = null;
                var listShiftOfLine = GetShiftsOfLine(lineId).OrderBy(c => c.TimeStart).ToList();
                if (listShiftOfLine != null && listShiftOfLine.Count > 0)
                {
                    listWorkHours = new List<ModelWorkHours>();
                    int intHours = 1;
                    TimeSpan timeEnd = new TimeSpan(0, 0, 0);
                    TimeSpan timeStart = new TimeSpan(0, 0, 0);
                    bool isWaitingTimeEnd = false;
                    double dHoursShiftOld = 0;
                    for (int i = 0; i < listShiftOfLine.Count; i++ )
                       
                        {
                            var shift = listShiftOfLine[i];
                            while (true)
                            {
                                if (!isWaitingTimeEnd)
                                {
                                    if (timeStart == new TimeSpan(0, 0, 0))
                                        timeStart = shift.TimeStart;
                                    else
                                        timeStart = timeEnd;
                                }
                                else
                                {
                                    if (dHoursShiftOld == 0)
                                        timeStart = shift.TimeStart;
                                }
                                if (timeStart > shift.TimeEnd)
                                {
                                    break;
                                }
                                else
                                {
                                    if (!isWaitingTimeEnd)
                                        timeEnd = timeStart.Add(new TimeSpan(1, 0, 0));
                                    else
                                    {
                                        if (dHoursShiftOld > 0)
                                        {
                                            double hour = 1 - dHoursShiftOld;
                                            int minuter = (int)(hour * 60);
                                            timeEnd = shift.TimeStart.Add(new TimeSpan(0, minuter, 0));

                                        }
                                        else
                                            timeEnd = timeStart.Add(new TimeSpan(1, 0, 0));
                                        isWaitingTimeEnd = false;
                                    }
                                    if (timeEnd <= shift.TimeEnd)
                                    {
                                        listWorkHours.Add(new ModelWorkHours() { IntHours = intHours, TimeStart = timeStart, TimeEnd = timeEnd, Name = "Giờ " + intHours + " (" + timeStart + "-" + timeEnd + ")" });
                                        intHours++;
                                    }
                                    else
                                    {
                                        isWaitingTimeEnd = true;
                                        dHoursShiftOld = shift.TimeEnd.TotalHours - timeStart.TotalHours;
                                        if (dHoursShiftOld!=0 && i == listShiftOfLine.Count-1)
                                        {
                                            listWorkHours.Add(new ModelWorkHours() { IntHours = intHours, TimeStart = timeStart, TimeEnd = shift.TimeEnd, Name = "Giờ " + intHours + " (" + timeStart + "-" + shift.TimeEnd + ")" });
                                            intHours++;
                                        }
                                        break;

                                    }
                                }
                                if (intHours > 30)
                                    break;
                            }
                        }
                }
                return listWorkHours;
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        public TimeSpan TimeIsWorkAllDayOfLine(string MaChuyen)
        {
            TimeSpan timeWork = new TimeSpan();
            timeWork = TimeSpan.Parse("00:00:00");
            List<Shift> listShift = GetShiftsOfLine(MaChuyen);
            if (listShift != null && listShift.Count>0)
            {
                for (int j = 0; j < listShift.Count; j++)
                {
                    timeWork += listShift[j].TimeEnd - listShift[j].TimeStart;
                }
            }
            return timeWork;
        }

       
    }
}
