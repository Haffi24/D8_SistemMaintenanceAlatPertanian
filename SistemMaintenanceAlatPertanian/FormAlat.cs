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

        private readonly SqlConnection conn;

        private readonly string connectionString = @"Data Source=LAPTOP-D3717QUD\USERHAFFI; Initial Catalog=DBMaintenanceAlat; Integrated Security=True;";

        private string idAlatTerpilih = "";


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
            
            cbKondisi.SelectedIndex = -1;
            idAlatTerpilih = "";
            txtNamaAlat.Focus();
        }


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
            finally { if (conn.State == System.Data.ConnectionState.Open) conn.Close(); }
        }


        private void FormAlat_Load(object sender, EventArgs e)
        {

            dgvAlat.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAlat.MultiSelect = false;
            dgvAlat.ReadOnly = true;
            dgvAlat.AllowUserToAddRows = false;
            dgvAlat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            
            cbKondisi.Items.Clear();
            cbKondisi.Items.Add("Bagus");
            cbKondisi.Items.Add("Perlu Perawatan");
            cbKondisi.DropDownStyle = ComboBoxStyle.DropDownList; 

            TampilData();
        }


        private void btnSimpan_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNamaAlat.Text))
                {
                    MessageBox.Show("Nama Alat harus diisi", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNamaAlat.Focus();
                    return;
                }

                
                if (cbKondisi.SelectedIndex == -1)
                {
                    MessageBox.Show("Silakan pilih Kondisi Alat terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbKondisi.Focus();
                    return;
                }

                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }

                string query = "INSERT INTO Alat (nama_alat, kondisi_fisik) VALUES (@Nama, @Kondisi)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nama", txtNamaAlat.Text);

                
                cmd.Parameters.AddWithValue("@Kondisi", cbKondisi.SelectedItem.ToString());

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
            finally { if (conn.State == System.Data.ConnectionState.Open) conn.Close(); }
        }


        private void dgvAlat_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvAlat.Rows[e.RowIndex];
                idAlatTerpilih = row.Cells["id_alat"].Value.ToString();
                txtNamaAlat.Text = row.Cells["nama_alat"].Value.ToString();

                
                cbKondisi.Text = row.Cells["kondisi_fisik"].Value.ToString();
            }
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (idAlatTerpilih == "")
                {
                    MessageBox.Show("Pilih data dari tabel dulu!");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtNamaAlat.Text))
                {
                    MessageBox.Show("Nama Alat harus diisi", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNamaAlat.Focus();
                    return;
                }

                
                if (cbKondisi.SelectedIndex == -1)
                {
                    MessageBox.Show("Silakan pilih Kondisi Alat terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbKondisi.Focus();
                    return;
                }

                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }

                string query = @"UPDATE Alat SET nama_alat = @Nama, kondisi_fisik = @Kondisi WHERE id_alat = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nama", txtNamaAlat.Text);

                
                cmd.Parameters.AddWithValue("@Kondisi", cbKondisi.SelectedItem.ToString());
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
            finally { if (conn.State == System.Data.ConnectionState.Open) conn.Close(); }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            try
            {
                if (idAlatTerpilih == "")
                {
                    MessageBox.Show("Pilih data dari tabel dulu!");
                    return;
                }

                DialogResult resultConfirm = MessageBox.Show(
                    "Yakin ingin menghapus data?",
                    "Konfirmasi",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultConfirm == DialogResult.Yes)
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

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
            finally { if (conn.State == System.Data.ConnectionState.Open) conn.Close(); }
        }

        private void txtCari_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed) conn.Open();

                dgvAlat.Rows.Clear(); 

                string query = "SELECT * FROM Alat WHERE nama_alat LIKE @Cari";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Cari", "%" + txtCari.Text + "%");

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
                MessageBox.Show("Gagal mencari data alat: " + ex.Message);
            }
            finally { if (conn.State == System.Data.ConnectionState.Open) conn.Close(); }
        }
    }
}