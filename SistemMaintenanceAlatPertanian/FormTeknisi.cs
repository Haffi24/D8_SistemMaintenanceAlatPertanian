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
        private readonly string connectionString = @"Data Source=LAPTOP-D3717QUD\USERHAFFI; Initial Catalog=DBMaintenanceAlat; Integrated Security=True;";
        private string idTeknisiTerpilih = "";

        public FormTeknisi()
        {
            InitializeComponent();
        }

       
        private void label1_Click(object sender, EventArgs e) { }

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
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    
                    string query = "SELECT * FROM vwTeknisiPublic";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvTeknisi.DataSource = dt;

                    
                    if (dgvTeknisi.Columns.Count > 0)
                    {
                        dgvTeknisi.Columns["id_teknisi"].HeaderText = "ID Teknisi";
                        dgvTeknisi.Columns["nama_teknisi"].HeaderText = "Nama Teknisi";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menampilkan data: " + ex.Message);
            }
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

        private void txtNamaTeknisi_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNamaTeknisi.Text))
            {
                MessageBox.Show("Nama Teknisi harus diisi", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    
                    using (SqlCommand cmd = new SqlCommand("sp_InsertTeknisi", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@nama_teknisi", txtNamaTeknisi.Text);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Data teknisi berhasil ditambahkan");
                        ClearForm();
                        TampilData();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Terjadi kesalahan: " + ex.Message); }
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
            if (string.IsNullOrEmpty(idTeknisiTerpilih))
            {
                MessageBox.Show("Pilih data teknisi dari tabel dulu!");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    
                    using (SqlCommand cmd = new SqlCommand("sp_UpdateTeknisi", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_teknisi", idTeknisiTerpilih);
                        cmd.Parameters.AddWithValue("@nama_teknisi", txtNamaTeknisi.Text);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Data berhasil diupdate");
                        ClearForm();
                        TampilData();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Terjadi kesalahan: " + ex.Message); }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idTeknisiTerpilih))
            {
                MessageBox.Show("Pilih data teknisi dari tabel dulu!");
                return;
            }

            if (MessageBox.Show("Yakin ingin menghapus data?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        
                        using (SqlCommand cmd = new SqlCommand("sp_DeleteTeknisi", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@id_teknisi", idTeknisiTerpilih);

                            conn.Open();
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Data berhasil dihapus");
                            ClearForm();
                            TampilData();
                        }
                    }
                }
                catch (Exception ex) { MessageBox.Show("Terjadi kesalahan: " + ex.Message); }
            }
        }

        private void txtCari_TextChanged(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    
                    using (SqlCommand cmd = new SqlCommand("sp_SearchTeknisi", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Keyword", txtCari.Text);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgvTeknisi.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal mencari data teknisi: " + ex.Message);
            }
        }
    }
}