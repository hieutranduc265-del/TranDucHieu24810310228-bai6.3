using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ProjectTaskManagement.DTO;

namespace ProjectTaskManagement.DAL
{
    public class CongViecDAL
    {
        public List<CongViecDTO> GetTasks(int? maDuAn = null, string trangThai = null)
        {
            var list = new List<CongViecDTO>();
            string sql = @"SELECT c.MaTask, c.MaDuAn, d.TenDuAn, c.TenTask, c.MoTa, c.HanNop, c.DoUuTien, c.TrangThai
                           FROM CongViec c
                           INNER JOIN DuAn d ON c.MaDuAn = d.MaDuAn
                           WHERE (@MaDuAn IS NULL OR c.MaDuAn = @MaDuAn)
                             AND (@TrangThai IS NULL OR @TrangThai = '' OR @TrangThai = 'Tất cả' OR c.TrangThai = @TrangThai)
                           ORDER BY c.MaTask DESC";

            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@MaDuAn", SqlDbType.Int).Value = (object)maDuAn ?? DBNull.Value;
                cmd.Parameters.Add("@TrangThai", SqlDbType.NVarChar, 50).Value = (object)trangThai ?? DBNull.Value;

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new CongViecDTO
                        {
                            MaTask = reader.GetInt32(0),
                            MaDuAn = reader.GetInt32(1),
                            TenDuAn = reader.GetString(2),
                            TenTask = reader.GetString(3),
                            MoTa = reader.IsDBNull(4) ? "" : reader.GetString(4),
                            HanNop = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5),
                            DoUuTien = reader.IsDBNull(6) ? "" : reader.GetString(6),
                            TrangThai = reader.IsDBNull(7) ? "To Do" : reader.GetString(7)
                        });
                    }
                }
            }
            return list;
        }

        public bool Insert(CongViecDTO cv)
        {
            string sql = @"INSERT INTO CongViec (MaDuAn, TenTask, MoTa, HanNop, DoUuTien, TrangThai)
                           VALUES (@MaDuAn, @TenTask, @MoTa, @HanNop, @DoUuTien, @TrangThai)";

            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@MaDuAn", SqlDbType.Int).Value = cv.MaDuAn;
                cmd.Parameters.Add("@TenTask", SqlDbType.NVarChar, 200).Value = cv.TenTask;
                cmd.Parameters.Add("@MoTa", SqlDbType.NVarChar, -1).Value = (object)cv.MoTa ?? DBNull.Value;
                cmd.Parameters.Add("@HanNop", SqlDbType.DateTime).Value = (object)cv.HanNop ?? DBNull.Value;
                cmd.Parameters.Add("@DoUuTien", SqlDbType.NVarChar, 20).Value = (object)cv.DoUuTien ?? DBNull.Value;
                cmd.Parameters.Add("@TrangThai", SqlDbType.NVarChar, 50).Value = (object)cv.TrangThai ?? "To Do";

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool UpdateTrangThai(int maTask, string trangThaiMoi)
        {
            string sql = "UPDATE CongViec SET TrangThai = @TrangThai WHERE MaTask = @MaTask";

            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@MaTask", SqlDbType.Int).Value = maTask;
                cmd.Parameters.Add("@TrangThai", SqlDbType.NVarChar, 50).Value = trangThaiMoi;

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public List<CongViecDTO> GetOverdueTasks()
        {
            var list = new List<CongViecDTO>();
            string sql = @"SELECT c.MaTask, c.MaDuAn, d.TenDuAn, c.TenTask, c.MoTa, c.HanNop, c.DoUuTien, c.TrangThai
                           FROM CongViec c
                           INNER JOIN DuAn d ON c.MaDuAn = d.MaDuAn
                           WHERE c.HanNop < GETDATE() AND ISNULL(c.TrangThai, '') <> 'Done'
                           ORDER BY c.HanNop ASC";

            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new CongViecDTO
                        {
                            MaTask = reader.GetInt32(0),
                            MaDuAn = reader.GetInt32(1),
                            TenDuAn = reader.GetString(2),
                            TenTask = reader.GetString(3),
                            MoTa = reader.IsDBNull(4) ? "" : reader.GetString(4),
                            HanNop = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5),
                            DoUuTien = reader.IsDBNull(6) ? "" : reader.GetString(6),
                            TrangThai = reader.IsDBNull(7) ? "" : reader.GetString(7)
                        });
                    }
                }
            }
            return list;
        }
    }
}