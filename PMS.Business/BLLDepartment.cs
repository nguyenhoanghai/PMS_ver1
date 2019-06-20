using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PMS.Business
{
    public class BLLDepartment
    {
        static Object key = new object();
        private static volatile BLLDepartment _Instance;
        public static BLLDepartment Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLDepartment();

                return _Instance;
            }
        }

        private BLLDepartment() { }

        public   List<DepartmentModel> Gets()
        {
            using (var db = new PMSEntities())
            {
                try
                {
                    return db.P_Department.Where(x => !x.IsDeleted).OrderByDescending(x => x.Id).Select(x=> new DepartmentModel() {Id = x.Id , Name = x.Name, BaseLabours = x.BaseLabours }).ToList();
                }
                catch (Exception)
                {
                }
                return new List<DepartmentModel>();
            }

        }

        public   ResponseBase Delete(int Id)
        {
            var result = new ResponseBase();
            try
            {
                var db = new PMSEntities();
                var commo = db.P_Department.FirstOrDefault(x => x.Id == Id);
                if (commo != null)
                {
                    commo.IsDeleted = true; 
                    db.SaveChanges();
                    result.IsSuccess = true;
                    result.Messages.Add(new Message() { msg = "Xóa Bộ phận thành công.", Title = "Thông Báo" });
                }
                else
                {
                    result.IsSuccess = true;
                    result.Messages.Add(new Message() { msg = "Không tìm thấy thông tin Bộ phận . Xóa Bộ phận thất bại.", Title = "Lỗi CSDL" });
                }
            }
            catch (Exception)
            {
                result.IsSuccess = true;
                result.Messages.Add(new Message() { msg = "Không tìm thấy thông tin Bộ phận . Xóa Bộ phận thất bại.", Title = "Lỗi Exception" });
            }
            return result;
        }

        public   ResponseBase InsertOrUpdate(P_Department objModel)
        {
            var rs = new ResponseBase(); 
            using (var db = new PMSEntities())
            {
                try
                {
                    if (CheckName(objModel.Id, objModel.Name.Trim(),db) != null)
                    {
                        rs.IsSuccess = false;
                        rs.Messages.Add(new Message() { msg = "Tên Bộ phận đã tồn tại. Vui lòng chọn lại tên khác", Title = "Lỗi trùng tên" });
                    }
                    else
                    {
                        if (objModel.Id == 0)
                        {
                            db.P_Department.Add(objModel);
                            rs.IsSuccess = true;
                        }
                        else
                        {
                            var oldObj = db.P_Department.FirstOrDefault(x => !x.IsDeleted && x.Id == objModel.Id);
                            if (oldObj != null)
                            {
                                oldObj.Name = objModel.Name;
                                oldObj.BaseLabours = objModel.BaseLabours;
                                rs.IsSuccess = true;
                            }
                            else
                            {
                                rs.IsSuccess = false;
                                rs.Messages.Add(new Message() { msg = "Bộ phận đang thao tác không tồn tại hoặc đã bị xóa. Vui lòng chọn lại tên khác", Title = "Lỗi trùng tên" });
                            }
                        }
                        if (rs.IsSuccess)
                        {
                            db.SaveChanges();
                            rs.IsSuccess = true;
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            return rs;
        }

        private   P_Department CheckName(int Id, string name, PMSEntities db)
        { 
            return db.P_Department.FirstOrDefault(x => !x.IsDeleted && x.Id != Id && x.Name.Trim().Equals(name));
        }
    }
}
