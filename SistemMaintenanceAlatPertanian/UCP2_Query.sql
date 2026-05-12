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