using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemMaintenanceAlatPertanian
{
    public partial class FormMaintenance : Form
    {
        // (Integrated Security=True)
        private readonly SqlConnection conn;
        private readonly string connectionString = @"Data Source=LAPTOP-D3717QUD\USERHAFFI; Initial Catalog=DBMaintenanceAlat; Integrated Security=True;";

        private string idMaintenanceTerpilih = ""; 

        public FormMaintenance()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }

       
        private void ClearForm()
        {
            cbAlat.SelectedIndex = -1;
            cbTeknisi.SelectedIndex = -1;
            dtpTanggal.Value = DateTime.Now;
            txtJenisPerbaikan.Clear();
            txtKeterangan.Clear();
            idMaintenanceTerpilih = "";
            cbAlat.Focus();
        }

   
        private void LoadAlat()
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed) conn.Open();
                string query = "SELECT id_alat, nama_alat FROM Alat";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                System.Data.DataTable dt = new System.Data.DataTable();
                dt.Load(reader);
                cbAlat.DataSource = dt;
                cbAlat.DisplayMember = "nama_alat";
                cbAlat.ValueMember = "id_alat";
                cbAlat.SelectedIndex = -1;
                reader.Close();
            }
            catch (Exception ex) { MessageBox.Show("Gagal Load Alat: " + ex.Message); }
            finally { if (conn.State == System.Data.ConnectionState.Open) conn.Close(); }
        }

    
        private void LoadTeknisi()
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed) conn.Open();
                string query = "SELECT id_teknisi, nama_teknisi FROM Teknisi";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                System.Data.DataTable dt = new System.Data.DataTable();
                dt.Load(reader);
                cbTeknisi.DataSource = dt;
                cbTeknisi.DisplayMember = "nama_teknisi";
                cbTeknisi.ValueMember = "id_teknisi";
                cbTeknisi.SelectedIndex = -1;
                reader.Close();
            }
            catch (Exception ex) { MessageBox.Show("Gagal Load Teknisi: " + ex.Message); }
            finally { if (conn.State == System.Data.ConnectionState.Open) conn.Close(); }
        }

    
        private void TampilData()
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed) conn.Open();

                dgvMaintenance.Rows.Clear();
                dgvMaintenance.Columns.Clear();

               
                dgvMaintenance.Columns.Add("id_m", "ID");
                dgvMaintenance.Columns.Add("alat", "Nama Alat");
                dgvMaintenance.Columns.Add("teknisi", "Teknisi");
                dgvMaintenance.Columns.Add("tgl", "Tanggal");
                dgvMaintenance.Columns.Add("jenis", "Jenis Perbaikan");
                dgvMaintenance.Columns.Add("ket", "Keterangan");

                string query = @"SELECT m.id_maintenance, a.nama_alat, t.nama_teknisi, m.tgl_service, m.jenis_perbaikan, m.keterangan 
                                 FROM Maintenance m 
                                 JOIN Alat a ON m.id_alat = a.id_alat 
                                 JOIN Teknisi t ON m.id_teknisi = t.id_teknisi";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    dgvMaintenance.Rows.Add(
                        reader["id_maintenance"].ToString(),
                        reader["nama_alat"].ToString(),
                        reader["nama_teknisi"].ToString(),
                        Convert.ToDateTime(reader["tgl_service"]).ToShortDateString(),
                        reader["jenis_perbaikan"].ToString(),
                        reader["keterangan"].ToString()
                    );
                }
                reader.Close();
            }
            catch (Exception ex) { MessageBox.Show("Gagal menampilkan data: " + ex.Message); }
            finally { if (conn.State == System.Data.ConnectionState.Open) conn.Close(); }
        }

        private void FormMaintenance_Load(object sender, EventArgs e)
        {
            
            dgvMaintenance.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMaintenance.MultiSelect = false;
            dgvMaintenance.ReadOnly = true;
            dgvMaintenance.AllowUserToAddRows = false;
            dgvMaintenance.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


            dtpTanggal.MinDate = DateTime.Today;
            dtpTanggal.MaxDate = DateTime.Today;


            LoadAlat();
            LoadTeknisi();
            TampilData();
        }

      
        private void btnSimpan_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbAlat.SelectedValue == null || cbTeknisi.SelectedValue == null || txtJenisPerbaikan.Text == "")
                {
                    MessageBox.Show("Alat, Teknisi, dan Jenis Perbaikan wajib diisi!");
                    return;
                }

                if (conn.State == System.Data.ConnectionState.Closed) conn.Open();

                string query = "INSERT INTO Maintenance (id_alat, id_teknisi, tgl_service, jenis_perbaikan, keterangan) VALUES (@idA, @idT, @tgl, @jenis, @ket)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idA", cbAlat.SelectedValue);
                cmd.Parameters.AddWithValue("@idT", cbTeknisi.SelectedValue);
                cmd.Parameters.AddWithValue("@tgl", dtpTanggal.Value.Date);
                cmd.Parameters.AddWithValue("@jenis", txtJenisPerbaikan.Text);
                cmd.Parameters.AddWithValue("@ket", txtKeterangan.Text);

                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Data maintenance berhasil ditambahkan");
                    ClearForm();
                    TampilData();
                }
            }
            catch (Exception ex) { MessageBox.Show("Terjadi kesalahan: " + ex.Message); }
            finally { if (conn.State == System.Data.ConnectionState.Open) conn.Close(); }
        }

        private void dgvMaintenance_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvMaintenance.Rows[e.RowIndex];
                idMaintenanceTerpilih = row.Cells["id_m"].Value.ToString();
                cbAlat.Text = row.Cells["alat"].Value.ToString();
                cbTeknisi.Text = row.Cells["teknisi"].Value.ToString();
                dtpTanggal.Value = Convert.ToDateTime(row.Cells["tgl"].Value);
                txtJenisPerbaikan.Text = row.Cells["jenis"].Value.ToString();
                txtKeterangan.Text = row.Cells["ket"].Value.ToString();
            }
        }

   
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (idMaintenanceTerpilih == "") { MessageBox.Show("Pilih data dari tabel dulu!"); return; }

                if (conn.State == System.Data.ConnectionState.Closed) conn.Open();

                string query = "UPDATE Maintenance SET id_alat=@idA, id_teknisi=@idT, tgl_service=@tgl, jenis_perbaikan=@jenis, keterangan=@ket WHERE id_maintenance=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idA", cbAlat.SelectedValue);
                cmd.Parameters.AddWithValue("@idT", cbTeknisi.SelectedValue);
                cmd.Parameters.AddWithValue("@tgl", dtpTanggal.Value.Date);
                cmd.Parameters.AddWithValue("@jenis", txtJenisPerbaikan.Text);
                cmd.Parameters.AddWithValue("@ket", txtKeterangan.Text);
                cmd.Parameters.AddWithValue("@id", idMaintenanceTerpilih);

                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Data berhasil diperbarui");
                    ClearForm();
                    TampilData();
                }
            }
            catch (Exception ex) { MessageBox.Show("Terjadi kesalahan: " + ex.Message); }
            finally { if (conn.State == System.Data.ConnectionState.Open) conn.Close(); }
        }

        // Event Tombol Hapus sesuai Modul
        private void btnHapus_Click(object sender, EventArgs e)
        {
            try
            {
                if (idMaintenanceTerpilih == "") { MessageBox.Show("Pilih data dari tabel dulu!"); return; }

                DialogResult result = MessageBox.Show("Yakin ingin menghapus data?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (conn.State == System.Data.ConnectionState.Closed) conn.Open();

                    string query = "DELETE FROM Maintenance WHERE id_maintenance=@id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", idMaintenanceTerpilih);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Data berhasil dihapus");
                        ClearForm();
                        TampilData();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Terjadi kesalahan: " + ex.Message); }
            finally { if (conn.State == System.Data.ConnectionState.Open) conn.Close(); }
        }
    }
}
