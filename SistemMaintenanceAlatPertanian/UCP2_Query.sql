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


--sp delete
CREATE PROCEDURE sp_DeleteAlat
    @id_alat INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Alat WHERE id_alat = @id_alat;
END;
GO



--sp search
CREATE PROCEDURE sp_SearchAlat
    @Keyword VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    -- Menggunakan View untuk search
    SELECT * FROM vwAlatPublic
    WHERE nama_alat LIKE '%' + @Keyword + '%' OR kondisi_fisik LIKE '%' + @Keyword + '%';
END;
GO



--sp tabel teknisi
CREATE PROCEDURE sp_InsertTeknisi
    @nama_teknisi VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Teknisi (nama_teknisi) 
    VALUES (@nama_teknisi);
END;
GO

--sp update teknisi
CREATE PROCEDURE sp_UpdateTeknisi
    @id_teknisi INT,
    @nama_teknisi VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Teknisi 
    SET nama_teknisi = @nama_teknisi 
    WHERE id_teknisi = @id_teknisi;
END;
GO



--sp delete teknisi
CREATE PROCEDURE sp_DeleteTeknisi
    @id_teknisi INT
AS
BEGIN
    SET NOCOUNT ON;
    
    DELETE FROM Teknisi 
    WHERE id_teknisi = @id_teknisi;
END;
GO

--sp search teknisi
CREATE PROCEDURE sp_SearchTeknisi
    @Keyword VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM vwTeknisiPublic 
    WHERE nama_teknisi LIKE '%' + @Keyword + '%';
END;
GO



--sp insert maintenance
CREATE PROCEDURE sp_InsertMaintenance
    @id_alat INT,
    @id_teknisi INT,
    @tgl_service DATE,
    @jenis_perbaikan VARCHAR(255),
    @keterangan TEXT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Maintenance (id_alat, id_teknisi, tgl_service, jenis_perbaikan, keterangan)
    VALUES (@id_alat, @id_teknisi, @tgl_service, @jenis_perbaikan, @keterangan);
END;
GO

--