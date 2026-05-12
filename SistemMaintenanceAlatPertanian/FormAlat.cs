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
    public partial class FormAlat : Form
    {
        // Variabel Koneksi 
        private readonly SqlConnection conn;
        
        private readonly string connectionString = @"Data Source=LAPTOP-D3717QUD\USERHAFFI; Initial Catalog=DBMaintenanceAlat; Integrated Security=True;";

        private string idAlatTerpilih = ""; 

        // Constructor 
        public FormAlat()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }

   
        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
       
        private void ClearForm()
        {
            txtNamaAlat.Clear();
            txtKondisi.Clear();
            idAlatTerpilih = "";
            txtNamaAlat.Focus();
        }

        // Method Menampilkan Data dengan SqlDataReader
        private void TampilData()
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }

                dgvAlat.Rows.Clear();
                dgvAlat.Columns.Clear();

                // Menambahkan kolom secara manual 
                dgvAlat.Columns.Add("id_alat", "ID Alat");
                dgvAlat.Columns.Add("nama_alat", "Nama Alat");
                dgvAlat.Columns.Add("kondisi_fisik", "Kondisi Fisik");

                string query = "SELECT * FROM Alat";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    dgvAlat.Rows.Add(
                        reader["id_alat"].ToString(),
                        reader["nama_alat"].ToString(),
                        reader["kondisi_fisik"].ToString()
                    );
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menampilkan data: " + ex.Message);
            }
        }

        // Event Form_Load
        private void FormAlat_Load(object sender, EventArgs e)
        {
            // Pengaturan DataGridView 
            dgvAlat.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAlat.MultiSelect = false;
            dgvAlat.ReadOnly = true;
            dgvAlat.AllowUserToAddRows = false;
            dgvAlat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Panggil method tampil data
            TampilData();
        }

      
        private void btnSimpan_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }

                if (txtNamaAlat.Text == "")
                {
                    MessageBox.Show("Nama Alat harus diisi");
                    txtNamaAlat.Focus();
                    return;
                }
                if (txtKondisi.Text == "")
                {
                    MessageBox.Show("Kondisi Alat harus diisi");
                    txtKondisi.Focus();
                    return;
                }

                string query = "INSERT INTO Alat (nama_alat, kondisi_fisik) VALUES (@Nama, @Kondisi)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nama", txtNamaAlat.Text);
                cmd.Parameters.AddWithValue("@Kondisi", txtKondisi.Text);

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Data alat berhasil ditambahkan");
                    ClearForm();
                    TampilData();
                }
                else
                {
                    MessageBox.Show("Data gagal ditambahkan");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        // Event Cell Click 
        private void dgvAlat_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvAlat.Rows[e.RowIndex];
                idAlatTerpilih = row.Cells["id_alat"].Value.ToString();
                txtNamaAlat.Text = row.Cells["nama_alat"].Value.ToString();
                txtKondisi.Text = row.Cells["kondisi_fisik"].Value.ToString();
            }
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }

                string query = @"UPDATE Alat SET nama_alat = @Nama, kondisi_fisik = @Kondisi WHERE id_alat = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nama", txtNamaAlat.Text);
                cmd.Parameters.AddWithValue("@Kondisi", txtKondisi.Text);
                cmd.Parameters.AddWithValue("@ID", idAlatTerpilih);

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Data berhasil diupdate");
                    ClearForm();
                    TampilData();
                }
                else
                {
                    MessageBox.Show("Data tidak ditemukan / belum diklik dari tabel");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }

                DialogResult resultConfirm = MessageBox.Show(
                    "Yakin ingin menghapus data?",
                    "Konfirmasi",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultConfirm == DialogResult.Yes)
                {
                    string query = "DELETE FROM Alat WHERE id_alat = @ID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", idAlatTerpilih);

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Data berhasil dihapus");
                        ClearForm();
                        TampilData();
                    }
                    else
                    {
                        MessageBox.Show("Data tidak ditemukan / belum diklik");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }
    }
}