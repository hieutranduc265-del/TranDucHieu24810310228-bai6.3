using System;
using System.Collections.Generic;
using ProjectTaskManagement.DAL;
using ProjectTaskManagement.DTO;

namespace ProjectTaskManagement.BUS
{
    public class DuAnBUS
    {
        private readonly DuAnDAL dal = new DuAnDAL();

        public List<DuAnDTO> GetAll() => dal.GetAll();

        public (bool ok, string msg) Insert(DuAnDTO da)
        {
            if (string.IsNullOrWhiteSpace(da.TenDuAn))
                return (false, "Tên dự án không được để trống.");
            if (da.NgayBatDau.HasValue && da.NgayKetThuc.HasValue && da.NgayKetThuc < da.NgayBatDau)
                return (false, "Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu.");

            return dal.Insert(da) ? (true, "Thêm dự án thành công.") : (false, "Thêm dự án thất bại.");
        }

        public (bool ok, string msg) Update(DuAnDTO da)
        {
            if (da.MaDuAn <= 0)
                return (false, "Vui lòng chọn dự án cần sửa.");
            if (string.IsNullOrWhiteSpace(da.TenDuAn))
                return (false, "Tên dự án không được để trống.");

            return dal.Update(da) ? (true, "Cập nhật dự án thành công.") : (false, "Cập nhật thất bại.");
        }

        public (bool ok, string msg) Delete(int maDuAn)
        {
            if (maDuAn <= 0)
                return (false, "Vui lòng chọn dự án cần xóa.");

            return dal.Delete(maDuAn) ? (true, "Xóa dự án thành công.") : (false, "Xóa dự án thất bại.");
        }
    }
}