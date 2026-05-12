using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SistemMaintenanceAlatPertanian
{
    public partial class FormMaintenance : Form
    {
        private readonly string connectionString = @"Data Source=LAPTOP-D3717QUD\USERHAFFI; Initial Catalog=DBMaintenanceAlat; Integrated Security=True;";
        private string idMaintenanceTerpilih = "";

        public FormMaintenance()
        {
            InitializeComponent();
        }

        private void ClearForm()
        {
            cbAlat.SelectedIndex = -1;
            cbTeknisi.SelectedIndex = -1;
            cbJenisPerbaikan.SelectedIndex = -1;
            dtpTanggal.Value = DateTime.Today;
            txtKeterangan.Clear();
            idMaintenanceTerpilih = "";
            cbAlat.Focus();
        }

        private void LoadAlat()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    
                    string query = "SELECT * FROM vwAlatPublic";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cbAlat.DataSource = dt;
                    cbAlat.DisplayMember = "nama_alat";
                    cbAlat.ValueMember = "id_alat";
                    cbAlat.SelectedIndex = -1;
                }
            }
            catch (Exception ex) { MessageBox.Show("Gagal Load Alat: " + ex.Message); }
        }

        private void LoadTeknisi()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    
                    string query = "SELECT * FROM vwTeknisiPublic";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cbTeknisi.DataSource = dt;
                    cbTeknisi.DisplayMember = "nama_teknisi";
                    cbTeknisi.ValueMember = "id_teknisi";
                    cbTeknisi.SelectedIndex = -1;
                }
            }
            catch (Exception ex) { MessageBox.Show("Gagal Load Teknisi: " + ex.Message); }
        }

        private void TampilData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    
                    string query = "SELECT * FROM vwDetailMaintenance";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvMaintenance.DataSource = dt;

                    
                    dgvMaintenance.Columns["id_maintenance"].HeaderText = "ID";
                    dgvMaintenance.Columns["nama_alat"].HeaderText = "Nama Alat";
                    dgvMaintenance.Columns["nama_teknisi"].HeaderText = "Teknisi";
                    dgvMaintenance.Columns["tgl_service"].HeaderText = "Tanggal";
                    dgvMaintenance.Columns["jenis_perbaikan"].HeaderText = "Jenis Perbaikan";
                    dgvMaintenance.Columns["keterangan"].HeaderText = "Keterangan";
                }
            }
            catch (Exception ex) { MessageBox.Show("Gagal menampilkan data: " + ex.Message); }
        }

        private void FormMaintenance_Load(object sender, EventArgs e)
        {
           
            dgvMaintenance.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMaintenance.MultiSelect = false;
            dgvMaintenance.ReadOnly = true;
            dgvMaintenance.AllowUserToAddRows = false;
            dgvMaintenance.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            cbJenisPerbaikan.Items.Clear();
            cbJenisPerbaikan.Items.Add("Perbaikan Ringan");
            cbJenisPerbaikan.Items.Add("Perbaikan Berat");

            cbJenisPerbaikan.DropDownStyle = ComboBoxStyle.DropDownList;
            cbAlat.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTeknisi.DropDownStyle = ComboBoxStyle.DropDownList;

            LoadAlat();
            LoadTeknisi();
            TampilData();
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (cbAlat.SelectedIndex == -1 || cbTeknisi.SelectedIndex == -1 || cbJenisPerbaikan.SelectedIndex == -1)
            {
                MessageBox.Show("Alat, Teknisi, dan Jenis Perbaikan wajib dipilih!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    
                    using (SqlCommand cmd = new SqlCommand("sp_InsertMaintenance", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@id_alat", cbAlat.SelectedValue);
                        cmd.Parameters.AddWithValue("@id_teknisi", cbTeknisi.SelectedValue);
                        cmd.Parameters.AddWithValue("@tgl_service", dtpTanggal.Value.Date);
                        cmd.Parameters.AddWithValue("@jenis_perbaikan", cbJenisPerbaikan.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@keterangan", txtKeterangan.Text);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data maintenance berhasil ditambahkan");
                        ClearForm();
                        TampilData();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Terjadi kesalahan: " + ex.Message); }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idMaintenanceTerpilih)) { MessageBox.Show("Pilih data dari tabel dulu!"); return; }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    
                    using (SqlCommand cmd = new SqlCommand("sp_UpdateMaintenance", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@id_maintenance", idMaintenanceTerpilih);
                        cmd.Parameters.AddWithValue("@id_alat", cbAlat.SelectedValue);
                        cmd.Parameters.AddWithValue("@id_teknisi", cbTeknisi.SelectedValue);
                        cmd.Parameters.AddWithValue("@tgl_service", dtpTanggal.Value.Date);
                        cmd.Parameters.AddWithValue("@jenis_perbaikan", cbJenisPerbaikan.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@keterangan", txtKeterangan.Text);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data berhasil diperbarui");
                        ClearForm();
                        TampilData();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Terjadi kesalahan: " + ex.Message); }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idMaintenanceTerpilih)) { MessageBox.Show("Pilih data dari tabel dulu!"); return; }

            if (MessageBox.Show("Yakin ingin menghapus data?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                      
                        using (SqlCommand cmd = new SqlCommand("sp_DeleteMaintenance", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@id_maintenance", idMaintenanceTerpilih);

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
                    
                    using (SqlCommand cmd = new SqlCommand("sp_SearchMaintenance", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Keyword", txtCari.Text);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgvMaintenance.DataSource = dt;
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Gagal mencari data: " + ex.Message); }
        }

        private void dgvMaintenance_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvMaintenance.Rows[e.RowIndex];
                idMaintenanceTerpilih = row.Cells["id_maintenance"].Value.ToString();
                cbAlat.Text = row.Cells["nama_alat"].Value.ToString();
                cbTeknisi.Text = row.Cells["nama_teknisi"].Value.ToString();
                dtpTanggal.Value = Convert.ToDateTime(row.Cells["tgl_service"].Value);
                cbJenisPerbaikan.Text = row.Cells["jenis_perbaikan"].Value.ToString();
                txtKeterangan.Text = row.Cells["keterangan"].Value.ToString();
            }
        }
    }
}