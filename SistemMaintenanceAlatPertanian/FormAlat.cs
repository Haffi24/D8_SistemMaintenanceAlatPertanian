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
        private readonly string connectionString = @"Data Source=LAPTOP-D3717QUD\USERHAFFI; Initial Catalog=DBMaintenanceAlat; Integrated Security=True;";
        private string idAlatTerpilih = "";

        public FormAlat()
        {
            InitializeComponent();
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
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    
                    string query = "SELECT * FROM vwAlatPublic";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvAlat.DataSource = dt;

                    
                    if (dgvAlat.Columns.Count > 0)
                    {
                        dgvAlat.Columns["id_alat"].HeaderText = "ID Alat";
                        dgvAlat.Columns["nama_alat"].HeaderText = "Nama Alat";
                        dgvAlat.Columns["kondisi_fisik"].HeaderText = "Kondisi Fisik";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menampilkan data: " + ex.Message);
            }
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
            if (string.IsNullOrWhiteSpace(txtNamaAlat.Text) || cbKondisi.SelectedIndex == -1)
            {
                MessageBox.Show("Nama dan Kondisi Alat wajib diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    
                    using (SqlCommand cmd = new SqlCommand("sp_InsertAlat", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@nama_alat", txtNamaAlat.Text);
                        cmd.Parameters.AddWithValue("@kondisi_fisik", cbKondisi.SelectedItem.ToString());

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data alat berhasil ditambahkan");
                        ClearForm();
                        TampilData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idAlatTerpilih))
            {
                MessageBox.Show("Pilih data dari tabel terlebih dahulu!");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    
                    using (SqlCommand cmd = new SqlCommand("sp_UpdateAlat", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_alat", idAlatTerpilih);
                        cmd.Parameters.AddWithValue("@nama_alat", txtNamaAlat.Text);
                        cmd.Parameters.AddWithValue("@kondisi_fisik", cbKondisi.SelectedItem.ToString());

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data berhasil diperbarui");
                        ClearForm();
                        TampilData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idAlatTerpilih))
            {
                MessageBox.Show("Pilih data dari tabel terlebih dahulu!");
                return;
            }

            if (MessageBox.Show("Yakin ingin menghapus data ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        
                        using (SqlCommand cmd = new SqlCommand("sp_DeleteAlat", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@id_alat", idAlatTerpilih);

                            conn.Open();
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Data berhasil dihapus");
                            ClearForm();
                            TampilData();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan: " + ex.Message);
                }
            }
        }

        private void txtCari_TextChanged(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    
                    using (SqlCommand cmd = new SqlCommand("sp_SearchAlat", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Keyword", txtCari.Text);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgvAlat.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal mencari data: " + ex.Message);
            }
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
    }
}