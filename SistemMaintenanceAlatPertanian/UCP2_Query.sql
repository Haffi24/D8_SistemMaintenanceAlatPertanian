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


--Stored Procedure
CREATE PROCEDURE sp_InsertAlat
    @nama_alat VARCHAR(100),
    @kondisi_fisik VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    -- Validasi sederhana
    IF LTRIM(RTRIM(@nama_alat)) = ''
    BEGIN
        RAISERROR ('Nama alat tidak boleh kosong!', 16, 1);
        RETURN;
    END

    INSERT INTO Alat (nama_alat, kondisi_fisik) 
    VALUES (@nama_alat, @kondisi_fisik);
END;
GO


--sp update
CREATE PROCEDURE sp_UpdateAlat
    @id_alat INT,
    @nama_alat VARCHAR(100),
    @kondisi_fisik VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Alat SET nama_alat = @nama_alat, kondisi_fisik = @kondisi_fisik WHERE id_alat = @id_alat;
END;
GO