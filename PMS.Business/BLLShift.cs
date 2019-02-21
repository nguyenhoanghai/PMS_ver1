using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business
{
    public class BLLShift
    {
        public static List<WorkingTimeModel> GetWorkingTimeOfLine(int lineId)
        {
            try
            {
                var db = new PMSEntities();
                List<WorkingTimeModel> listWorkHours = null;
                var listShiftOfLine = BLLShift.GetShiftsOfLine(lineId);
                if (listShiftOfLine != null && listShiftOfLine.Count > 0)
                {
                    listWorkHours = new List<WorkingTimeModel>();
                    int intHours = 1;
                    TimeSpan timeEnd = new TimeSpan(0, 0, 0);
                    TimeSpan timeStart = new TimeSpan(0, 0, 0);
                    bool isWaitingTimeEnd = false;
                    double dHoursShiftOld = 0;
                    for (int i = 0; i < listShiftOfLine.Count; i++)
                    {
                        var shift = listShiftOfLine[i];
                        while (true)
                        {
                            if (!isWaitingTimeEnd)
                            {
                                if (timeStart == new TimeSpan(0, 0, 0))
                                    timeStart = shift.Start;
                                else
                                    timeStart = timeEnd;
                            }
                            else
                            {
                                if (dHoursShiftOld == 0)
                                    timeStart = shift.Start;
                            }


                            if (timeStart > shift.End)
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
                                        timeEnd = shift.Start.Add(new TimeSpan(0, minuter, 0));

                                    }
                                    else
                                        timeEnd = timeStart.Add(new TimeSpan(1, 0, 0));
                                    isWaitingTimeEnd = false;
                                }
                                if (timeEnd <= shift.End)
                                {
                                    listWorkHours.Add(new WorkingTimeModel() { IntHours = intHours, TimeStart = timeStart, TimeEnd = timeEnd, Name = "Giờ " + intHours + " (" + timeStart + "-" + timeEnd + ")" });
                                    intHours++;
                                }
                                else
                                {
                                    isWaitingTimeEnd = true;
                                    dHoursShiftOld = shift.End.TotalHours - timeStart.TotalHours;
                                    if (dHoursShiftOld != 0 && i == listShiftOfLine.Count - 1)
                                    {
                                        listWorkHours.Add(new WorkingTimeModel() { IntHours = intHours, TimeStart = timeStart, TimeEnd = shift.End, Name = "Giờ " + intHours + " (" + timeStart + "-" + shift.End + ")" });
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

        public static List<LineModel> GetWorkingTime()
        {
            try
            {
                var db = new PMSEntities();
                var shifts = db.Shifts.OrderBy(c => c.TimeStart).ToList();
                if (shifts != null && shifts.Count > 0)
                {
                    var chuyenIds = shifts.Select(x => x.MaChuyen).Distinct();
                    var Lines = db.Chuyens.Where(x => !x.IsDeleted && chuyenIds.Contains(x.MaChuyen)).Select(x => new LineModel() { MaChuyen = x.MaChuyen, TenChuyen = x.TenChuyen }).OrderBy(x => x.MaChuyen).ToList();
                    foreach (var line in Lines)
                    {
                        line.WorkingTimes = ProcessShift(shifts.Where(x => x.MaChuyen == line.MaChuyen).ToList());
                    }
                    return Lines;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static List<WorkingTimeModel> ProcessShift(List<Shift> shifts)
        {
            try
            {
                var listWorkHours = new List<WorkingTimeModel>();
                int intHours = 1;
                TimeSpan timeEnd = new TimeSpan(0, 0, 0);
                TimeSpan timeStart = new TimeSpan(0, 0, 0);
                bool isWaitingTimeEnd = false;
                double dHoursShiftOld = 0;
                for (int i = 0; i < shifts.Count; i++)
                {
                    var shift = shifts[i];
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
                                listWorkHours.Add(new WorkingTimeModel() { IntHours = intHours, TimeStart = timeStart, TimeEnd = timeEnd, Name = "Giờ " + intHours + " (" + timeStart + "-" + timeEnd + ")" });
                                intHours++;
                            }
                            else
                            {
                                isWaitingTimeEnd = true;
                                dHoursShiftOld = shift.TimeEnd.TotalHours - timeStart.TotalHours;
                                if (dHoursShiftOld != 0 && i == shifts.Count - 1)
                                {
                                    listWorkHours.Add(new WorkingTimeModel() { IntHours = intHours, TimeStart = timeStart, TimeEnd = shift.TimeEnd, Name = "Giờ " + intHours + " (" + timeStart + "-" + shift.TimeEnd + ")" });
                                    intHours++;
                                }
                                break;

                            }
                        }
                        if (intHours > 30)
                            break;
                    }
                }
                return listWorkHours;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<LineWorkingShiftModel> GetShiftsOfLine(int lineId)
        {
            try
            {
                var db = new PMSEntities();
                return db.P_LineWorkingShift.Where(x => x.LineId == lineId && !x.IsDeleted).Select(x => new LineWorkingShiftModel()
                {
                    Id = x.Id,
                    ShiftId = x.ShiftId,
                    ShiftOrder = x.ShiftOrder,
                    LineId = x.LineId,
                    Start = x.P_WorkingShift.TimeStart,
                    End = x.P_WorkingShift.TimeEnd
                }).OrderBy(x => x.ShiftOrder).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static TimeSpan GetTotalWorkingHourOfLine(int lineId)
        {
            try
            {
                TimeSpan timeWork = new TimeSpan();
                timeWork = TimeSpan.Parse("00:00:00");
                var listShift = GetShiftsOfLine(lineId);
                if (listShift != null && listShift.Count > 0)
                {
                    for (int j = 0; j < listShift.Count; j++)
                    {
                        timeWork += listShift[j].End - listShift[j].Start;
                    }
                }
                return timeWork;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static TimeSpan GetTotalWorkingHourOfLine(List<LineWorkingShiftModel> shifts)
        {
            try
            {
                TimeSpan timeWork = new TimeSpan();
                timeWork = TimeSpan.Parse("00:00:00");
                if (shifts != null && shifts.Count > 0)
                {
                    for (int j = 0; j < shifts.Count; j++)
                    {
                        timeWork += shifts[j].End - shifts[j].Start;
                    }
                }
                return timeWork;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<P_WorkingShift> GetShift()
        {
            try
            {
                var db = new PMSEntities();
                return db.P_WorkingShift.Where(x => !x.IsDeleted).ToList();
            }
            catch (Exception)
            {
            }
            return new List<P_WorkingShift>();
        }

        public static ResponseBase InsertOrUpdateShift(P_WorkingShift obj)
        {
            var result = new ResponseBase();
            try
            {
                var db = new PMSEntities();
                var check = false;
                if (obj.Id == 0)
                {
                    db.P_WorkingShift.Add(obj);
                    check = true;
                }
                else
                {
                    var shift = db.P_WorkingShift.FirstOrDefault(x => !x.IsDeleted && x.Id == obj.Id);
                    if (shift != null)
                    {
                        shift.Name = obj.Name;
                        shift.TimeStart = obj.TimeStart;
                        shift.TimeEnd = obj.TimeEnd;
                        check = true;
                    }
                }
                if (check)
                {
                    db.SaveChanges();
                    result.IsSuccess = true;
                    result.Messages.Add(new Message() { msg = "Lưu thông tin thành công.", Title = "Thông Báo" });
                }
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Messages.Add(new Message() { msg = "Lưu thông tin bị lỗi.", Title = "Lỗi" });
            }
            return result;
        }

        public static ResponseBase DeleteShift(int Id)
        {
            var result = new ResponseBase();
            try
            {
                var db = new PMSEntities();
                var shift = db.P_WorkingShift.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (shift != null)
                {
                    shift.IsDeleted = true;
                    db.SaveChanges();
                    result.IsSuccess = true;
                    result.Messages.Add(new Message() { msg = "Lưu thông tin thành công.", Title = "Thông Báo" });
                }
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Messages.Add(new Message() { msg = "Lưu thông tin bị lỗi.", Title = "Lỗi" });
            }
            return result;
        }

        public static P_WorkingShift FindShift(int shiftId)
        {
            var db = new PMSEntities();
            return db.P_WorkingShift.FirstOrDefault(x => !x.IsDeleted && x.Id == shiftId);
        }

        public static List<WorkingTimeModel> GetListWorkHoursOfLineByLineId(int lineId)
        {
            try
            {
                List<WorkingTimeModel> listWorkHours = null;
                var listShiftOfLine = BLLShift.GetShiftsOfLine(lineId);
                if (listShiftOfLine != null && listShiftOfLine.Count > 0)
                {
                    listWorkHours = new List<WorkingTimeModel>();
                    int intHours = 1;
                    TimeSpan timeEnd = new TimeSpan(0, 0, 0);
                    TimeSpan timeStart = new TimeSpan(0, 0, 0);
                    bool isWaitingTimeEnd = false;
                    double dHoursShiftOld = 0;
                    for (int i = 0; i < listShiftOfLine.Count; i++)
                    {
                        var shift = listShiftOfLine[i];
                        while (true)
                        {
                            if (!isWaitingTimeEnd)
                            {
                                if (timeStart == new TimeSpan(0, 0, 0))
                                    timeStart = shift.Start;
                                else
                                    timeStart = timeEnd;
                            }
                            else
                            {
                                if (dHoursShiftOld == 0)
                                    timeStart = shift.Start;
                            }
                            if (timeStart > shift.End)
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
                                        timeEnd = shift.Start.Add(new TimeSpan(0, minuter, 0));
                                    }
                                    else
                                        timeEnd = timeStart.Add(new TimeSpan(1, 0, 0));
                                    isWaitingTimeEnd = false;
                                }
                                if (timeEnd <= shift.End)
                                {
                                    listWorkHours.Add(new WorkingTimeModel()
                                    {
                                        IntHours = intHours,
                                        TimeStart = timeStart,
                                        TimeEnd = timeEnd,
                                        Name = "Giờ " + intHours + " (" + timeStart.ToString(@"hh\:mm") + "-" + timeEnd.ToString(@"hh\:mm") + ")",
                                        IsShow = (DateTime.Now.TimeOfDay > timeStart && DateTime.Now.TimeOfDay < timeEnd) ? true : false
                                    });
                                    intHours++;
                                }
                                else
                                {
                                    isWaitingTimeEnd = true;
                                    dHoursShiftOld = shift.End.TotalHours - timeStart.TotalHours;
                                    if (dHoursShiftOld != 0 && i == listShiftOfLine.Count - 1)
                                    {
                                        listWorkHours.Add(new WorkingTimeModel()
                                        {
                                            IntHours = intHours,
                                            TimeStart = timeStart,
                                            TimeEnd = shift.End,
                                            Name = "Giờ " + intHours + " (" + timeStart.ToString(@"hh\:mm") + "-" + shift.End.ToString(@"hh\:mm") + ")",
                                            IsShow = (DateTime.Now.TimeOfDay > timeStart && DateTime.Now.TimeOfDay < timeEnd) ? true : false

                                        });
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

        public static TimeSpan FindTimeStart(int MaChuyen)
        {
            try
            {
                var db = new PMSEntities();
                var time = db.P_LineWorkingShift.Where(x => !x.IsDeleted && !x.P_WorkingShift.IsDeleted && x.LineId == MaChuyen).Min(x => x.P_WorkingShift.TimeStart);
                if (time != null)
                    return TimeSpan.Parse(time.ToString());
                else
                    return TimeSpan.Parse("00:00:00");
            }
            catch (Exception)
            {
                return TimeSpan.Parse("00:00:00");
            }
        }

        public static List<WorkingTimeModel> GetListWorkHoursOfLineByLineId(int lineId, int solan)
        {
            try
            {
                List<WorkingTimeModel> listWorkHours = null;
                var listShiftOfLine = GetShiftsOfLine(lineId);
                if (listShiftOfLine != null && listShiftOfLine.Count > 0)
                {
                    TimeSpan timeWork = new TimeSpan();
                    timeWork = TimeSpan.Parse("00:00:00");
                    int secondsEachTimes = 0;
                    foreach (var s in listShiftOfLine)
                    {
                        timeWork += s.End - s.Start;
                    }
                    secondsEachTimes = (int)timeWork.TotalSeconds / solan;

                    //
                    listWorkHours = new List<WorkingTimeModel>();
                    int intHours = 1;
                    TimeSpan timeEnd = new TimeSpan(0, 0, 0);
                    TimeSpan timeStart = new TimeSpan(0, 0, 0);
                    bool isWaitingTimeEnd = false;
                    int secondsOfShift = 0, sodu = 0;
                    foreach (var item in listShiftOfLine)
                    {
                        secondsOfShift = (int)(item.End - item.Start).TotalSeconds;
                        if (listWorkHours.Count > 0 || (isWaitingTimeEnd && sodu > 0))
                        {
                            var last = listWorkHours.LastOrDefault();
                            if (last != null || (isWaitingTimeEnd && sodu > 0))
                            {
                                var gio_nghi = (item.Start - listShiftOfLine[0].End).TotalSeconds;
                                if (isWaitingTimeEnd)
                                {
                                    DateTime dt = DateTime.ParseExact(last != null ? last.TimeEnd.ToString() : timeStart.ToString(), "HH:mm:ss", null);
                                    timeEnd = dt.AddSeconds(gio_nghi + secondsEachTimes).TimeOfDay;
                                    listWorkHours.Add(new WorkingTimeModel() { IntHours = intHours, TimeStart = timeStart, TimeEnd = timeEnd, Name = "Giờ " + intHours + " (" + timeStart + "-" + timeEnd + ")" });
                                    intHours++;
                                    isWaitingTimeEnd = false;

                                    secondsOfShift -= (secondsEachTimes - sodu);
                                    timeStart = timeEnd;
                                    sodu = 0;

                                    if (secondsOfShift > secondsEachTimes)
                                    {
                                        for (int i = secondsEachTimes; i < secondsOfShift; i += secondsEachTimes)
                                        {
                                            dt = DateTime.ParseExact(timeStart.ToString(), "HH:mm:ss", null);
                                            timeEnd = dt.AddSeconds(secondsEachTimes).TimeOfDay;
                                            listWorkHours.Add(new WorkingTimeModel() { IntHours = intHours, TimeStart = timeStart, TimeEnd = timeEnd, Name = "Giờ " + intHours + " (" + timeStart + "-" + timeEnd + ")" });
                                            intHours++;
                                            isWaitingTimeEnd = false;
                                            timeStart = timeEnd;

                                            if ((i + secondsEachTimes) >= secondsOfShift)
                                            {
                                                sodu = secondsOfShift - i;
                                                isWaitingTimeEnd = true;
                                                break;
                                            }
                                        }
                                    }
                                    else if (secondsEachTimes == secondsOfShift)
                                    {
                                        dt = DateTime.ParseExact(timeStart.ToString(), "HH:mm:ss", null);
                                        timeEnd = dt.AddSeconds(secondsEachTimes).TimeOfDay;
                                        listWorkHours.Add(new WorkingTimeModel() { IntHours = intHours, TimeStart = timeStart, TimeEnd = timeEnd, Name = "Giờ " + intHours + " (" + timeStart + "-" + timeEnd + ")" });
                                        intHours++;
                                        isWaitingTimeEnd = false;
                                        timeStart = timeEnd;
                                    }
                                    else
                                    {
                                        if (secondsOfShift != 0)
                                        {
                                            sodu = secondsOfShift;
                                            isWaitingTimeEnd = true;
                                        }
                                    }
                                }
                                else
                                {
                                    for (int i = secondsEachTimes; i < secondsOfShift; i += secondsEachTimes)
                                    {
                                        if (timeStart == new TimeSpan(0, 0, 0))
                                            timeStart = item.Start;

                                        var dt = DateTime.ParseExact(timeStart.ToString(), "HH:mm:ss", null);
                                        timeEnd = dt.AddSeconds(i - secondsEachTimes).TimeOfDay;
                                        listWorkHours.Add(new WorkingTimeModel() { IntHours = intHours, TimeStart = timeStart, TimeEnd = timeEnd, Name = "Giờ " + intHours + " (" + timeStart + "-" + timeEnd + ")" });
                                        intHours++;
                                        isWaitingTimeEnd = false;
                                        timeStart = dt.AddSeconds(i - secondsEachTimes).TimeOfDay;

                                        if ((secondsOfShift % secondsEachTimes) == (secondsOfShift - i))
                                        {
                                            sodu = secondsOfShift - i;
                                            isWaitingTimeEnd = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (timeStart == new TimeSpan(0, 0, 0))
                                timeStart = item.Start;
                            if (secondsEachTimes > secondsOfShift)
                            {
                                sodu = secondsOfShift;
                                isWaitingTimeEnd = true;
                            }
                            else if (secondsOfShift == secondsEachTimes)
                            {
                                DateTime dt = DateTime.ParseExact(timeStart.ToString(), "HH:mm:ss", null);
                                timeEnd = dt.AddSeconds(secondsEachTimes).TimeOfDay;
                                listWorkHours.Add(new WorkingTimeModel() { IntHours = intHours, TimeStart = timeStart, TimeEnd = timeEnd, Name = "Giờ " + intHours + " (" + timeStart + "-" + timeEnd + ")" });
                                intHours++;
                                isWaitingTimeEnd = false;
                                timeStart = timeEnd;
                            }
                            else
                            {
                                for (int i = secondsEachTimes; i < secondsOfShift; i += secondsEachTimes)
                                {

                                    DateTime dt = DateTime.ParseExact(timeStart.ToString(), "HH:mm:ss", null);
                                    timeEnd = dt.AddSeconds(secondsEachTimes).TimeOfDay;
                                    listWorkHours.Add(new WorkingTimeModel() { IntHours = intHours, TimeStart = timeStart, TimeEnd = timeEnd, Name = "Giờ " + intHours + " (" + timeStart + "-" + timeEnd + ")" });
                                    intHours++;
                                    isWaitingTimeEnd = false;
                                    timeStart = timeEnd;

                                    if ((i + secondsEachTimes) > secondsOfShift)
                                    {
                                        sodu = secondsOfShift - i;
                                        isWaitingTimeEnd = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (isWaitingTimeEnd)
                    {
                        var dt = DateTime.ParseExact(timeStart.ToString(), "HH:mm:ss", null);
                        timeEnd = dt.AddSeconds(sodu).TimeOfDay;
                        listWorkHours.Add(new WorkingTimeModel() { IntHours = intHours, TimeStart = timeStart, TimeEnd = timeEnd, Name = "Giờ " + intHours + " (" + timeStart + "-" + timeEnd + ")" });
                    }
                }
                return listWorkHours;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<WorkingTimeModel> GetListWorkHoursOfLineByLineId(List<LineWorkingShiftModel> shifts, int solan)
        {
            try
            {
                List<WorkingTimeModel> listWorkHours = null;
                if (shifts != null && shifts.Count > 0)
                {
                    TimeSpan timeWork = new TimeSpan();
                    timeWork = TimeSpan.Parse("00:00:00");
                    int secondsEachTimes = 0;
                    foreach (var s in shifts)
                    {
                        timeWork += s.End - s.Start;
                    }
                    secondsEachTimes = (int)timeWork.TotalSeconds / solan;

                    //
                    listWorkHours = new List<WorkingTimeModel>();
                    int intHours = 1;
                    TimeSpan timeEnd = new TimeSpan(0, 0, 0);
                    TimeSpan timeStart = new TimeSpan(0, 0, 0);
                    bool isWaitingTimeEnd = false;
                    int secondsOfShift = 0, sodu = 0;
                    foreach (var item in shifts)
                    {
                        secondsOfShift = (int)(item.End - item.Start).TotalSeconds;
                        if (listWorkHours.Count > 0 || (isWaitingTimeEnd && sodu > 0))
                        {
                            var last = listWorkHours.LastOrDefault();
                            if (last != null || (isWaitingTimeEnd && sodu > 0))
                            {
                                var gio_nghi = (item.Start - shifts[0].End).TotalSeconds;
                                if (isWaitingTimeEnd)
                                {
                                    DateTime dt = DateTime.ParseExact(last != null ? last.TimeEnd.ToString() : timeStart.ToString(), "HH:mm:ss", null);
                                    timeEnd = dt.AddSeconds(gio_nghi + secondsEachTimes).TimeOfDay;
                                    listWorkHours.Add(new WorkingTimeModel() { IntHours = intHours, TimeStart = timeStart, TimeEnd = timeEnd, Name = "Giờ " + intHours + " (" + timeStart + "-" + timeEnd + ")" });
                                    intHours++;
                                    isWaitingTimeEnd = false;

                                    secondsOfShift -= (secondsEachTimes - sodu);
                                    timeStart = timeEnd;
                                    sodu = 0;

                                    if (secondsOfShift > secondsEachTimes)
                                    {
                                        for (int i = secondsEachTimes; i < secondsOfShift; i += secondsEachTimes)
                                        {
                                            dt = DateTime.ParseExact(timeStart.ToString(), "HH:mm:ss", null);
                                            timeEnd = dt.AddSeconds(secondsEachTimes).TimeOfDay;
                                            listWorkHours.Add(new WorkingTimeModel() { IntHours = intHours, TimeStart = timeStart, TimeEnd = timeEnd, Name = "Giờ " + intHours + " (" + timeStart + "-" + timeEnd + ")" });
                                            intHours++;
                                            isWaitingTimeEnd = false;
                                            timeStart = timeEnd;

                                            if ((i + secondsEachTimes) >= secondsOfShift)
                                            {
                                                sodu = secondsOfShift - i;
                                                isWaitingTimeEnd = true;
                                                break;
                                            }
                                        }
                                    }
                                    else if (secondsEachTimes == secondsOfShift)
                                    {
                                        dt = DateTime.ParseExact(timeStart.ToString(), "HH:mm:ss", null);
                                        timeEnd = dt.AddSeconds(secondsEachTimes).TimeOfDay;
                                        listWorkHours.Add(new WorkingTimeModel() { IntHours = intHours, TimeStart = timeStart, TimeEnd = timeEnd, Name = "Giờ " + intHours + " (" + timeStart + "-" + timeEnd + ")" });
                                        intHours++;
                                        isWaitingTimeEnd = false;
                                        timeStart = timeEnd;
                                    }
                                    else
                                    {
                                        if (secondsOfShift != 0)
                                        {
                                            sodu = secondsOfShift;
                                            isWaitingTimeEnd = true;
                                        }
                                    }
                                }
                                else
                                {
                                    for (int i = secondsEachTimes; i < secondsOfShift; i += secondsEachTimes)
                                    {
                                        if (timeStart == new TimeSpan(0, 0, 0))
                                            timeStart = item.Start;

                                        var dt = DateTime.ParseExact(timeStart.ToString(), "HH:mm:ss", null);
                                        timeEnd = dt.AddSeconds(i - secondsEachTimes).TimeOfDay;
                                        listWorkHours.Add(new WorkingTimeModel() { IntHours = intHours, TimeStart = timeStart, TimeEnd = timeEnd, Name = "Giờ " + intHours + " (" + timeStart + "-" + timeEnd + ")" });
                                        intHours++;
                                        isWaitingTimeEnd = false;
                                        timeStart = dt.AddSeconds(i - secondsEachTimes).TimeOfDay;

                                        if ((secondsOfShift % secondsEachTimes) == (secondsOfShift - i))
                                        {
                                            sodu = secondsOfShift - i;
                                            isWaitingTimeEnd = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (timeStart == new TimeSpan(0, 0, 0))
                                timeStart = item.Start;
                            if (secondsEachTimes > secondsOfShift)
                            {
                                sodu = secondsOfShift;
                                isWaitingTimeEnd = true;
                            }
                            else if (secondsOfShift == secondsEachTimes)
                            {
                                DateTime dt = DateTime.ParseExact(timeStart.ToString(), "HH:mm:ss", null);
                                timeEnd = dt.AddSeconds(secondsEachTimes).TimeOfDay;
                                listWorkHours.Add(new WorkingTimeModel() { IntHours = intHours, TimeStart = timeStart, TimeEnd = timeEnd, Name = "Giờ " + intHours + " (" + timeStart + "-" + timeEnd + ")" });
                                intHours++;
                                isWaitingTimeEnd = false;
                                timeStart = timeEnd;
                            }
                            else
                            {
                                for (int i = secondsEachTimes; i < secondsOfShift; i += secondsEachTimes)
                                {

                                    DateTime dt = DateTime.ParseExact(timeStart.ToString(), "HH:mm:ss", null);
                                    timeEnd = dt.AddSeconds(secondsEachTimes).TimeOfDay;
                                    listWorkHours.Add(new WorkingTimeModel() { IntHours = intHours, TimeStart = timeStart, TimeEnd = timeEnd, Name = "Giờ " + intHours + " (" + timeStart + "-" + timeEnd + ")" });
                                    intHours++;
                                    isWaitingTimeEnd = false;
                                    timeStart = timeEnd;

                                    if ((i + secondsEachTimes) > secondsOfShift)
                                    {
                                        sodu = secondsOfShift - i;
                                        isWaitingTimeEnd = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (isWaitingTimeEnd)
                    {
                        var dt = DateTime.ParseExact(timeStart.ToString(), "HH:mm:ss", null);
                        timeEnd = dt.AddSeconds(sodu).TimeOfDay;
                        listWorkHours.Add(new WorkingTimeModel() { IntHours = intHours, TimeStart = timeStart, TimeEnd = timeEnd, Name = "Giờ " + intHours + " (" + timeStart + "-" + timeEnd + ")" });
                    }
                }
                return listWorkHours;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static TimeSpan TGLamViecToiHienTai(int LineId)
        {
            var timeNow = DateTime.Now.TimeOfDay;
            var lineShifts = BLLShift.GetShiftsOfLine(LineId);
            TimeSpan timeWork = TimeSpan.Parse("00:00:00");
            for (int j = 0; j < lineShifts.Count; j++)
            {
                if (timeNow > lineShifts[j].Start)
                {
                    if (timeNow < lineShifts[j].End)
                    {
                        timeWork += (timeNow - lineShifts[j].Start);
                    }
                    else
                    {
                        timeWork += (lineShifts[j].End - lineShifts[j].Start);
                    }
                }
            }
            return timeWork;
        }

    }
}
