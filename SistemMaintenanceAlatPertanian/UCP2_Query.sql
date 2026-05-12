-- View Tabel Alat
CREATE VIEW vwAlatPublic AS
SELECT id_alat, nama_alat, kondisi_fisik 
FROM Alat;
GO

-- View Tabel Teknisi
CREATE VIEW vwTeknisiPublic AS
SELECT id_teknisi, nama_teknisi 
FROM Teknisi;
GO

-- View Gabungan Maintenance
CREATE VIEW vwDetailMaintenance AS
SELECT 
    m.id_maintenance, a.nama_alat, t.nama_teknisi, m.tgl_service, m.jenis_perbaikan, m.keterangan
FROM Maintenance m
JOIN Alat a ON m.id_alat = a.id_alat
JOIN Teknisi t ON m.id_teknisi = t.id_teknisi;
GO