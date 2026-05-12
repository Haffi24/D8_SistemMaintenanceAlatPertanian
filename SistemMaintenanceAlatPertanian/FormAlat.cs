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

        private BindingSource bindingSource = new BindingSource();
        private DataTable dtAlat = new DataTable();
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

        private void BindControls()
        {
            txtNamaAlat.DataBindings.Clear();
            cbKondisi.DataBindings.Clear();

            txtNamaAlat.DataBindings.Add("Text", bindingSource, "nama_alat");
            cbKondisi.DataBindings.Add("Text", bindingSource, "kondisi_fisik");
        }

        private void TampilData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM vwAlatPublic";
                    using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                    {
                        dtAlat = new DataTable();
                        da.Fill(dtAlat);

                        bindingSource.DataSource = dtAlat;

                        dgvAlat.DataSource = bindingSource;

                        BindControls();
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
            bindingNavigator1.BindingSource = bindingSource;

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
            DataRowView row = (DataRowView)bindingSource.Current;
            if (row == null)
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
                        cmd.Parameters.AddWithValue("@id_alat", row["id_alat"]);
                        cmd.Parameters.AddWithValue("@nama_alat", txtNamaAlat.Text);
                        cmd.Parameters.AddWithValue("@kondisi_fisik", cbKondisi.Text);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data berhasil diperbarui");
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
            DataRowView row = (DataRowView)bindingSource.Current;
            if (row == null)
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
                            cmd.Parameters.AddWithValue("@id_alat", row["id_alat"]);

                            conn.Open();
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Data berhasil dihapus");
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

                        bindingSource.DataSource = dt;
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
                DataRowView row = (DataRowView)bindingSource.Current;
                if (row != null)
                {
                    idAlatTerpilih = row["id_alat"].ToString();
                }
            }
        }

        private void btnTestInjection_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Alat SET nama_alat = 'DIRETAS' WHERE nama_alat = '" + txtNamaAlat.Text + "'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        int result = cmd.ExecuteNonQuery();
                        MessageBox.Show(result + " baris terupdate");
                    }
                }
                TampilData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonreset_click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Apakah Anda yakin ingin mengembalikan seluruh data ke kondisi awal (Backup)? Semua perubahan saat ini akan hilang.",
                "Konfirmasi Reset Data", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dialog == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        string query = @"
                    -- 1. Reset Tabel Maintenance
                    IF OBJECT_ID('dbo.Maintenance_Backup') IS NOT NULL
                    BEGIN
                        DELETE FROM dbo.Maintenance;
                        
                        SET IDENTITY_INSERT dbo.Maintenance ON;
                        INSERT INTO dbo.Maintenance (id_maintenance, id_alat, id_teknisi, tgl_service, jenis_perbaikan, keterangan) 
                        SELECT id_maintenance, id_alat, id_teknisi, tgl_service, jenis_perbaikan, keterangan FROM dbo.Maintenance_Backup;
                        SET IDENTITY_INSERT dbo.Maintenance OFF;
                    END

                    -- 2. Reset Tabel Alat
                    IF OBJECT_ID('dbo.Alat_Backup') IS NOT NULL
                    BEGIN
                        DELETE FROM dbo.Alat;
                        
                        SET IDENTITY_INSERT dbo.Alat ON;
                        INSERT INTO dbo.Alat (id_alat, nama_alat, kondisi_fisik) 
                        SELECT id_alat, nama_alat, kondisi_fisik FROM dbo.Alat_Backup;
                        SET IDENTITY_INSERT dbo.Alat OFF;
                    END

                    -- 3. Reset Tabel Teknisi
                    IF OBJECT_ID('dbo.Teknisi_Backup') IS NOT NULL
                    BEGIN
                        DELETE FROM dbo.Teknisi;
                        
                        SET IDENTITY_INSERT dbo.Teknisi ON;
                        INSERT INTO dbo.Teknisi (id_teknisi, nama_teknisi) 
                        SELECT id_teknisi, nama_teknisi FROM dbo.Teknisi_Backup;
                        SET IDENTITY_INSERT dbo.Teknisi OFF;
                    END";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Data berhasil direset ke kondisi backup.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    TampilData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan saat proses reset: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}