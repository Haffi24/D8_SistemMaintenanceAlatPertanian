using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace SistemMaintenanceAlatPertanian
{
    public class DAL
    {
        
        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["SistemMaintenanceAlatPertanian.Properties.Settings.DBMaintenanceAlatConnectionString"].ConnectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(GetConnectionString());
        }

        
        public DataTable GetAllTeknisi()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = GetConnection())
            {
                
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Teknisi", conn))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }

        public bool InsertTeknisi(string namaTeknisi, byte[] fotoTeknisi)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertTeknisi", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nama_teknisi", namaTeknisi);

                    if (fotoTeknisi != null)
                        cmd.Parameters.AddWithValue("@foto_teknisi", fotoTeknisi);
                    else
                        cmd.Parameters.AddWithValue("@foto_teknisi", DBNull.Value);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdateTeknisi(int idTeknisi, string namaTeknisi, byte[] fotoTeknisi)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_UpdateTeknisi", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_teknisi", idTeknisi);
                    cmd.Parameters.AddWithValue("@nama_teknisi", namaTeknisi);

                    if (fotoTeknisi != null)
                        cmd.Parameters.AddWithValue("@foto_teknisi", fotoTeknisi);
                    else
                        cmd.Parameters.AddWithValue("@foto_teknisi", DBNull.Value);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteTeknisi(int idTeknisi)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_DeleteTeknisi", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_teknisi", idTeknisi);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public DataTable SearchTeknisi(string keyword)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_SearchTeknisi", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    cmd.Parameters.AddWithValue("@Keyword", keyword);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }

        
        public DataTable GetAllAlat()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Alat", conn))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }

        public bool InsertAlat(string namaAlat, string kondisiFisik)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertAlat", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nama_alat", namaAlat);
                    cmd.Parameters.AddWithValue("@kondisi_fisik", kondisiFisik);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdateAlat(int idAlat, string namaAlat, string kondisiFisik)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_UpdateAlat", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_alat", idAlat);
                    cmd.Parameters.AddWithValue("@nama_alat", namaAlat);
                    cmd.Parameters.AddWithValue("@kondisi_fisik", kondisiFisik);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteAlat(int idAlat)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_DeleteAlat", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_alat", idAlat);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }


        public DataTable GetAllMaintenance()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = GetConnection())
            {
                string query = "SELECT m.id_maintenance, m.id_alat, a.nama_alat, m.id_teknisi, t.nama_teknisi, m.tgl_service, m.jenis_perbaikan, m.keterangan " +
                               "FROM Maintenance m JOIN Alat a ON m.id_alat = a.id_alat JOIN Teknisi t ON m.id_teknisi = t.id_teknisi";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }

        public bool InsertMaintenance(int idAlat, int idTeknisi, DateTime tglService, string jenisPerbaikan, string keterangan)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertMaintenance", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_alat", idAlat);
                    cmd.Parameters.AddWithValue("@id_teknisi", idTeknisi);
                    cmd.Parameters.AddWithValue("@tgl_service", tglService);
                    cmd.Parameters.AddWithValue("@jenis_perbaikan", jenisPerbaikan);
                    cmd.Parameters.AddWithValue("@keterangan", keterangan);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdateMaintenance(int idMaintenance, int idAlat, int idTeknisi, DateTime tglService, string jenisPerbaikan, string keterangan)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_UpdateMaintenance", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_maintenance", idMaintenance);
                    cmd.Parameters.AddWithValue("@id_alat", idAlat);
                    cmd.Parameters.AddWithValue("@id_teknisi", idTeknisi);
                    cmd.Parameters.AddWithValue("@tgl_service", tglService);
                    cmd.Parameters.AddWithValue("@jenis_perbaikan", jenisPerbaikan);
                    cmd.Parameters.AddWithValue("@keterangan", keterangan);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteMaintenance(int idMaintenance)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_DeleteMaintenance", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_maintenance", idMaintenance);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public DataTable SearchMaintenance(string keyword)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_SearchMaintenance", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Keyword", keyword);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }

       
        public DataTable GetLaporanMaintenance()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_LaporanMaintenance", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }

        public DataTable SearchAlat(string keyword)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_SearchAlat", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Keyword", keyword);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }



        public DataTable GetGrafikKondisiAlat()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_GrafikKondisiAlat", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }
    }
}