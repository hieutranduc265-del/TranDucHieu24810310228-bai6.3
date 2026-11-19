using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ProjectTaskManagement.DTO;

namespace ProjectTaskManagement.DAL
{
    public class PhanCongDAL
    {
        public List<PhanCongDTO> GetAll()
        {
            var list = new List<PhanCongDTO>();
            string sql = @"SELECT p.MaTask, c.TenTask, p.MaThanhVien, t.HoTen, p.NgayGiao
                           FROM PhanCong p
                           INNER JOIN CongViec c ON p.MaTask = c.MaTask
                           INNER JOIN ThanhVien t ON p.MaThanhVien = t.MaThanhVien
                           ORDER BY p.NgayGiao DESC";

            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new PhanCongDTO
                        {
                            MaTask = reader.GetInt32(0),
                            TenTask = reader.GetString(1),
                            MaThanhVien = reader.GetInt32(2),
                            HoTen = reader.GetString(3),
                            NgayGiao = reader.GetDateTime(4)
                        });
                    }
                }
            }
            return list;
        }

        public bool AssignTask(int maTask, int maThanhVien)
        {
            string sql = @"IF NOT EXISTS (SELECT 1 FROM PhanCong WHERE MaTask = @MaTask AND MaThanhVien = @MaThanhVien)
                           INSERT INTO PhanCong (MaTask, MaThanhVien, NgayGiao) VALUES (@MaTask, @MaThanhVien, GETDATE())";

            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@MaTask", SqlDbType.Int).Value = maTask;
                cmd.Parameters.Add("@MaThanhVien", SqlDbType.Int).Value = maThanhVien;

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public int GetTotalAssignedTasksByProject(int maDuAn)
        {
            string sql = @"SELECT COUNT(*)
                           FROM PhanCong p
                           INNER JOIN CongViec c ON p.MaTask = c.MaTask
                           WHERE c.MaDuAn = @MaDuAn";

            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@MaDuAn", SqlDbType.Int).Value = maDuAn;
                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
            }
        }

        public DataTable GetCompletionStatsByProject(int maDuAn)
        {
            string sql = @"SELECT t.HoTen AS [Thành viên],
                                  COUNT(c.MaTask) AS [Tổng task],
                                  SUM(CASE WHEN c.TrangThai = 'Done' THEN 1 ELSE 0 END) AS [Đã xong],
                                  CAST(
                                      CASE WHEN COUNT(c.MaTask) = 0 THEN 0.0
                                           ELSE SUM(CASE WHEN c.TrangThai = 'Done' THEN 1.0 ELSE 0.0 END) * 100.0 / COUNT(c.MaTask)
                                      END AS DECIMAL(5,1)
                                  ) AS [Tỷ lệ %]
                           FROM ThanhVien t
                           INNER JOIN PhanCong p ON t.MaThanhVien = p.MaThanhVien
                           INNER JOIN CongViec c ON p.MaTask = c.MaTask
                           WHERE c.MaDuAn = @MaDuAn
                           GROUP BY t.MaThanhVien, t.HoTen
                           ORDER BY [Tỷ lệ %] DESC";

            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@MaDuAn", SqlDbType.Int).Value = maDuAn;
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }
    }
}