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
    public partial class FormTeknisi : Form
    {
      
        private readonly SqlConnection conn;
        private readonly string connectionString = @"Data Source=LAPTOP-D3717QUD\USERHAFFI; Initial Catalog=DBMaintenanceAlat; Integrated Security=True;";

        private string idTeknisiTerpilih = ""; 

        public FormTeknisi()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }

    
        private void ClearForm()
        {
            txtNamaTeknisi.Clear();
            idTeknisiTerpilih = "";
            txtNamaTeknisi.Focus();
        }

        private void TampilData()
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed) conn.Open();

                dgvTeknisi.Rows.Clear();
                dgvTeknisi.Columns.Clear();

              
                dgvTeknisi.Columns.Add("id_teknisi", "ID Teknisi");
                dgvTeknisi.Columns.Add("nama_teknisi", "Nama Teknisi");

                string query = "SELECT * FROM Teknisi";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    dgvTeknisi.Rows.Add(
                        reader["id_teknisi"].ToString(),
                        reader["nama_teknisi"].ToString()
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

        private void FormTeknisi_Load(object sender, EventArgs e)
        {
           
            dgvTeknisi.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTeknisi.MultiSelect = false;
            dgvTeknisi.ReadOnly = true;
            dgvTeknisi.AllowUserToAddRows = false;
            dgvTeknisi.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            TampilData();
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNamaTeknisi.Text == "")
                {
                    MessageBox.Show("Nama Teknisi harus diisi");
                    txtNamaTeknisi.Focus();
                    return;
                }

                if (conn.State == System.Data.ConnectionState.Closed) conn.Open();

                string query = "INSERT INTO Teknisi (nama_teknisi) VALUES (@Nama)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nama", txtNamaTeknisi.Text);

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Data teknisi berhasil ditambahkan");
                    ClearForm();
                    TampilData();
                }
            }
            catch (Exception ex) { MessageBox.Show("Terjadi kesalahan: " + ex.Message); }
            finally { if (conn.State == System.Data.ConnectionState.Open) conn.Close(); }
        }

       
        private void dgvTeknisi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvTeknisi.Rows[e.RowIndex];
                idTeknisiTerpilih = row.Cells["id_teknisi"].Value.ToString();
                txtNamaTeknisi.Text = row.Cells["nama_teknisi"].Value.ToString();
            }
        }

       
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (idTeknisiTerpilih == "") { MessageBox.Show("Pilih data teknisi dari tabel dulu!"); return; }

                if (conn.State == System.Data.ConnectionState.Closed) conn.Open();

                string query = "UPDATE Teknisi SET nama_teknisi = @Nama WHERE id_teknisi = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nama", txtNamaTeknisi.Text);
                cmd.Parameters.AddWithValue("@ID", idTeknisiTerpilih);

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Data berhasil diupdate");
                    ClearForm();
                    TampilData();
                }
            }
            catch (Exception ex) { MessageBox.Show("Terjadi kesalahan: " + ex.Message); }
            finally { if (conn.State == System.Data.ConnectionState.Open) conn.Close(); }
        }

        
        private void btnHapus_Click(object sender, EventArgs e)
        {
            try
            {
                if (idTeknisiTerpilih == "") { MessageBox.Show("Pilih data teknisi dari tabel dulu!"); return; }

                DialogResult confirm = MessageBox.Show("Yakin ingin menghapus data?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    if (conn.State == System.Data.ConnectionState.Closed) conn.Open();

                    string query = "DELETE FROM Teknisi WHERE id_teknisi = @ID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", idTeknisiTerpilih);

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
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

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}