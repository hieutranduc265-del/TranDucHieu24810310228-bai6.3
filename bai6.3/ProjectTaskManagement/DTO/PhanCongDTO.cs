using System;

namespace ProjectTaskManagement.DTO
{
    public class PhanCongDTO
    {
        public int MaTask { get; set; }
        public string TenTask { get; set; }
        public int MaThanhVien { get; set; }
        public string HoTen { get; set; }
        public DateTime NgayGiao { get; set; }
    }
}