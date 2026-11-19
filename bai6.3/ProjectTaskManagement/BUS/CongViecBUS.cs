using System.Collections.Generic;
using ProjectTaskManagement.DAL;
using ProjectTaskManagement.DTO;

namespace ProjectTaskManagement.BUS
{
    public class CongViecBUS
    {
        private readonly CongViecDAL dal = new CongViecDAL();

        public List<CongViecDTO> GetTasks(int? maDuAn = null, string trangThai = null)
            => dal.GetTasks(maDuAn, trangThai);

        public (bool ok, string msg) Insert(CongViecDTO cv)
        {
            if (cv.MaDuAn <= 0)
                return (false, "Vui lòng chọn dự án cho công việc.");
            if (string.IsNullOrWhiteSpace(cv.TenTask))
                return (false, "Tên công việc không được để trống.");

            return dal.Insert(cv) ? (true, "Tạo task thành công.") : (false, "Tạo task thất bại.");
        }

        public (bool ok, string msg) UpdateTrangThai(int maTask, string trangThaiMoi)
        {
            if (maTask <= 0 || string.IsNullOrWhiteSpace(trangThaiMoi))
                return (false, "Thông tin cập nhật trạng thái chưa hợp lệ.");

            return dal.UpdateTrangThai(maTask, trangThaiMoi)
                ? (true, "Đã cập nhật trạng thái task.")
                : (false, "Cập nhật trạng thái thất bại.");
        }

        public List<CongViecDTO> GetOverdueTasks() => dal.GetOverdueTasks();
    }
}