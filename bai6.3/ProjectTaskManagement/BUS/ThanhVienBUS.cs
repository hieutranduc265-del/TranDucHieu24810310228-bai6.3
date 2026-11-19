using System.Collections.Generic;
using ProjectTaskManagement.DAL;
using ProjectTaskManagement.DTO;

namespace ProjectTaskManagement.BUS
{
    public class ThanhVienBUS
    {
        private readonly ThanhVienDAL dal = new ThanhVienDAL();

        public List<ThanhVienDTO> GetAll() => dal.GetAll();

        public (bool ok, string msg) Insert(ThanhVienDTO tv)
        {
            if (string.IsNullOrWhiteSpace(tv.HoTen))
                return (false, "Họ tên thành viên không được để trống.");

            return dal.Insert(tv) ? (true, "Thêm thành viên thành công.") : (false, "Thêm thành viên thất bại.");
        }

        public (bool ok, string msg) Delete(int maThanhVien)
        {
            if (maThanhVien <= 0)
                return (false, "Vui lòng chọn thành viên cần xóa.");

            return dal.Delete(maThanhVien) ? (true, "Xóa thành viên thành công.") : (false, "Xóa thành viên thất bại.");
        }
    }
}