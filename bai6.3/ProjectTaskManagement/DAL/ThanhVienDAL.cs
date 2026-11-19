using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ProjectTaskManagement.DTO;

namespace ProjectTaskManagement.DAL
{
    public class ThanhVienDAL
    {
        public List<ThanhVienDTO> GetAll()
        {
            var list = new List<ThanhVienDTO>();
            string sql = "SELECT MaThanhVien, HoTen, Email, ChuyenMon FROM ThanhVien ORDER BY MaThanhVien DESC";

            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new ThanhVienDTO
                        {
                            MaThanhVien = reader.GetInt32(0),
                            HoTen = reader.GetString(1),
                            Email = reader.IsDBNull(2) ? "" : reader.GetString(2),
                            ChuyenMon = reader.IsDBNull(3) ? "" : reader.GetString(3)
                        });
                    }
                }
            }
            return list;
        }

        public bool Insert(ThanhVienDTO tv)
        {
            string sql = "INSERT INTO ThanhVien (HoTen, Email, ChuyenMon) VALUES (@HoTen, @Email, @ChuyenMon)";

            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@HoTen", SqlDbType.NVarChar, 100).Value = tv.HoTen;
                cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value = string.IsNullOrWhiteSpace(tv.Email) ? (object)DBNull.Value : tv.Email;
                cmd.Parameters.Add("@ChuyenMon", SqlDbType.NVarChar, 100).Value = (object)tv.ChuyenMon ?? DBNull.Value;

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(int maThanhVien)
        {
            string sql = "DELETE FROM ThanhVien WHERE MaThanhVien = @MaThanhVien";

            using (SqlConnection conn = DBConnect.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@MaThanhVien", SqlDbType.Int).Value = maThanhVien;
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}