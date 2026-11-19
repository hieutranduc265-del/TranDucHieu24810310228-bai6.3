using System.Collections.Generic;
using System.Data;
using ProjectTaskManagement.DAL;
using ProjectTaskManagement.DTO;

namespace ProjectTaskManagement.BUS
{
    public class PhanCongBUS
    {
        private readonly PhanCongDAL dal = new PhanCongDAL();

        public List<PhanCongDTO> GetAll() => dal.GetAll();

        public (bool ok, string msg) AssignTask(int maTask, int maThanhVien)
        {
            if (maTask <= 0 || maThanhVien <= 0)
                return (false, "Vui lòng chọn đầy đủ Task và Thành viên.");

            return dal.AssignTask(maTask, maThanhVien)
                ? (true, "Phân công thành công.")
                : (false, "Thành viên đã được giao task này hoặc lỗi CSDL.");
        }

        public int GetTotalAssignedTasksByProject(int maDuAn) => dal.GetTotalAssignedTasksByProject(maDuAn);

        public DataTable GetCompletionStatsByProject(int maDuAn) => dal.GetCompletionStatsByProject(maDuAn);
    }
}