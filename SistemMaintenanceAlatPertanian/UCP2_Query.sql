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

--sp update maintenance
CREATE PROCEDURE sp_UpdateMaintenance
    @id_maintenance INT,
    @id_alat INT,
    @id_teknisi INT,
    @tgl_service DATE,
    @jenis_perbaikan VARCHAR(255),
    @keterangan TEXT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Maintenance 
    SET id_alat = @id_alat, 
        id_teknisi = @id_teknisi, 
        tgl_service = @tgl_service, 
        jenis_perbaikan = @jenis_perbaikan, 
        keterangan = @keterangan 
    WHERE id_maintenance = @id_maintenance;
END;
GO

--sp delete maintenance

CREATE PROCEDURE sp_DeleteMaintenance
    @id_maintenance INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Maintenance 
    WHERE id_maintenance = @id_maintenance;
END;
GO

--sp search maintenance
CREATE PROCEDURE sp_SearchMaintenance
    @Keyword VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM vwDetailMaintenance 
    WHERE jenis_perbaikan LIKE '%' + @Keyword + '%' 
       OR keterangan LIKE '%' + @Keyword + '%'
       OR nama_alat LIKE '%' + @Keyword + '%'
       OR nama_teknisi LIKE '%' + @Keyword + '%';
END;
GO






--perbaikan store procedure

--sp fix
ALTER PROCEDURE sp_InsertAlat
    @nama_alat VARCHAR(100),
    @kondisi_fisik VARCHAR(50)
AS
BEGIN
    BEGIN TRY
        -- Validasi 1: Nama alat minimal 3 karakter
        IF LEN(LTRIM(RTRIM(@nama_alat))) < 3
        BEGIN
            THROW 51000, 'Nama alat terlalu pendek (minimal 3 huruf).', 1;
        END

        -- Validasi 2: Kondisi fisik tidak boleh kosong
        IF LTRIM(RTRIM(@kondisi_fisik)) = ''
        BEGIN
            THROW 51001, 'Kondisi fisik wajib diisi.', 1;
        END

        -- Jika lolos validasi, lakukan INSERT
        INSERT INTO Alat (nama_alat, kondisi_fisik) 
        VALUES (@nama_alat, @kondisi_fisik);

    END TRY
    BEGIN CATCH
        -- WAJIB ADA: Melempar pesan error ke aplikasi C# (Visual Studio)
        THROW; 
    END CATCH
END;
GO

--sp update
ALTER PROCEDURE sp_UpdateAlat
    @id_alat INT,
    @nama_alat VARCHAR(100),
    @kondisi_fisik VARCHAR(50)
AS
BEGIN
    BEGIN TRY
        -- Validasi 1: Cek apakah ID Alat yang mau diupdate ADA di database
        IF NOT EXISTS (SELECT 1 FROM Alat WHERE id_alat = @id_alat)
        BEGIN
            THROW 51002, 'Error: ID Alat tidak ditemukan. Update dibatalkan.', 1;
        END

        -- Jika aman, lakukan UPDATE
        UPDATE Alat 
        SET nama_alat = @nama_alat, kondisi_fisik = @kondisi_fisik 
        WHERE id_alat = @id_alat;

        PRINT 'Sukses: Data alat berhasil diperbarui.';
    END TRY
    BEGIN CATCH
        PRINT 'Gagal memperbarui data alat.';
        PRINT ERROR_MESSAGE();
    END CATCH
END;
GO


--delete
ALTER PROCEDURE sp_DeleteAlat
    @id_alat INT
AS
BEGIN
    BEGIN TRY
        -- Validasi 1: Cek apakah ID Alat ada
        IF NOT EXISTS (SELECT 1 FROM Alat WHERE id_alat = @id_alat)
        BEGIN
            THROW 51003, 'Error: Data Alat tidak ditemukan.', 1;
        END

        -- Validasi 2: Mencegah error Foreign Key jika alat sedang di-maintenance
        IF EXISTS (SELECT 1 FROM Maintenance WHERE id_alat = @id_alat)
        BEGIN
            THROW 51004, 'Error: Alat tidak bisa dihapus karena memiliki riwayat Maintenance.', 1;
        END

        -- Jika aman, lakukan DELETE
        DELETE FROM Alat WHERE id_alat = @id_alat;

        PRINT 'Sukses: Data alat berhasil dihapus.';
    END TRY
    BEGIN CATCH
        PRINT 'Gagal menghapus data alat.';
        PRINT ERROR_MESSAGE();
    END CATCH
END;
GO






--sp teknisi
ALTER PROCEDURE sp_InsertTeknisi
    @nama_teknisi VARCHAR(100)
AS
BEGIN
    BEGIN TRY
        -- Validasi: Nama teknisi tidak boleh kosong dan minimal 3 karakter
        IF LEN(LTRIM(RTRIM(@nama_teknisi))) < 3
        BEGIN
            THROW 51010, 'Nama teknisi terlalu pendek (minimal 3 huruf).', 1;
        END

        INSERT INTO Teknisi (nama_teknisi) 
        VALUES (@nama_teknisi);
        
    END TRY
    BEGIN CATCH
        -- Melempar error ke C#
        THROW; 
    END CATCH
END;
GO


--update sp teknisi
ALTER PROCEDURE sp_UpdateTeknisi
    @id_teknisi INT,
    @nama_teknisi VARCHAR(100)
AS
BEGIN
    BEGIN TRY
        -- Validasi 1: Pastikan ID Teknisi ada di database
        IF NOT EXISTS (SELECT 1 FROM Teknisi WHERE id_teknisi = @id_teknisi)
        BEGIN
            THROW 51011, 'Error: Data Teknisi tidak ditemukan.', 1;
        END

        -- Validasi 2: Nama teknisi minimal 3 karakter
        IF LEN(LTRIM(RTRIM(@nama_teknisi))) < 3
        BEGIN
            THROW 51012, 'Nama teknisi terlalu pendek (minimal 3 huruf).', 1;
        END

        UPDATE Teknisi 
        SET nama_teknisi = @nama_teknisi 
        WHERE id_teknisi = @id_teknisi;

    END TRY
    BEGIN CATCH
        THROW; 
    END CATCH
END;
GO



--delete sp teknisi
ALTER PROCEDURE sp_DeleteTeknisi
    @id_teknisi INT
AS
BEGIN
    BEGIN TRY
        -- Validasi 1:Memassstikan ID Teknisi ada
        IF NOT EXISTS (SELECT 1 FROM Teknisi WHERE id_teknisi = @id_teknisi)
        BEGIN
            THROW 51013, 'Error: Data Teknisi tidak ditemukan.', 1;
        END

        -- Validasi 2: Mencegah penghapusan jika teknisi memiliki riwayat di tabel Maintenance
        IF EXISTS (SELECT 1 FROM Maintenance WHERE id_teknisi = @id_teknisi)
        BEGIN
            THROW 51014, 'Teknisi tidak bisa dihapus karena masih memiliki riwayat perbaikan (Maintenance).', 1;
        END

        DELETE FROM Teknisi 
        WHERE id_teknisi = @id_teknisi;

    END TRY
    BEGIN CATCH
        THROW; 
    END CATCH
END;
GO




--sp maintenance
ALTER PROCEDURE sp_InsertMaintenance
    @id_alat INT,
    @id_teknisi INT,
    @tgl_service DATE,
    @jenis_perbaikan VARCHAR(255),
    @keterangan TEXT
AS
BEGIN
    BEGIN TRY
        -- Validasi 1: Pastikan ID Alat benar-benar ada di tabel Alat
        IF NOT EXISTS (SELECT 1 FROM Alat WHERE id_alat = @id_alat)
        BEGIN
            THROW 51020, 'Error: Alat yang dipilih tidak ditemukan di database.', 1;
        END

        -- Validasi 2: Pastikan ID Teknisi benar-benar ada di tabel Teknisi
        IF NOT EXISTS (SELECT 1 FROM Teknisi WHERE id_teknisi = @id_teknisi)
        BEGIN
            THROW 51021, 'Error: Teknisi yang dipilih tidak ditemukan di database.', 1;
        END

        -- Validasi 3: Jenis Perbaikan tidak boleh kosong
        IF LEN(LTRIM(RTRIM(@jenis_perbaikan))) = 0
        BEGIN
            THROW 51022, 'Error: Jenis perbaikan wajib diisi.', 1;
        END

        -- Jika lolos validasi, lakukan INSERT
        INSERT INTO Maintenance (id_alat, id_teknisi, tgl_service, jenis_perbaikan, keterangan)
        VALUES (@id_alat, @id_teknisi, @tgl_service, @jenis_perbaikan, @keterangan);
        
    END TRY
    BEGIN CATCH
        -- Melempar error ke C# (Visual Studio)
        THROW; 
    END CATCH
END;
GO



--sp update maintenance
ALTER PROCEDURE sp_UpdateMaintenance
    @id_maintenance INT,
    @id_alat INT,
    @id_teknisi INT,
    @tgl_service DATE,
    @jenis_perbaikan VARCHAR(255),
    @keterangan TEXT
AS
BEGIN
    BEGIN TRY
        -- Validasi 1: Pastikan data Maintenance yang mau diubah itu ada
        IF NOT EXISTS (SELECT 1 FROM Maintenance WHERE id_maintenance = @id_maintenance)
        BEGIN
            THROW 51023, 'Error: Data riwayat perbaikan tidak ditemukan.', 1;
        END

        -- Validasi 2: Pastikan ID Alat valid
        IF NOT EXISTS (SELECT 1 FROM Alat WHERE id_alat = @id_alat)
        BEGIN
            THROW 51024, 'Error: Alat yang dipilih tidak ditemukan di database.', 1;
        END

        -- Validasi 3: Pastikan ID Teknisi valid
        IF NOT EXISTS (SELECT 1 FROM Teknisi WHERE id_teknisi = @id_teknisi)
        BEGIN
            THROW 51025, 'Error: Teknisi yang dipilih tidak ditemukan di database.', 1;
        END

        -- Validasi 4: Jenis Perbaikan tidak boleh kosong
        IF LEN(LTRIM(RTRIM(@jenis_perbaikan))) = 0
        BEGIN
            THROW 51026, 'Error: Jenis perbaikan wajib diisi.', 1;
        END

        -- Jika lolos validasi, lakukan UPDATE
        UPDATE Maintenance 
        SET id_alat = @id_alat, 
            id_teknisi = @id_teknisi, 
            tgl_service = @tgl_service, 
            jenis_perbaikan = @jenis_perbaikan, 
            keterangan = @keterangan 
        WHERE id_maintenance = @id_maintenance;

    END TRY
    BEGIN CATCH
        THROW; 
    END CATCH
END;
GO



--sp delete maintenance
ALTER PROCEDURE sp_DeleteMaintenance
    @id_maintenance INT
AS
BEGIN
    BEGIN TRY
        -- Validasi 1: Pastikan ID Maintenance ada sebelum dihapus
        IF NOT EXISTS (SELECT 1 FROM Maintenance WHERE id_maintenance = @id_maintenance)
        BEGIN
            THROW 51027, 'Error: Data riwayat perbaikan tidak ditemukan.', 1;
        END

        -- Jika aman, lakukan DELETE
        DELETE FROM Maintenance 
        WHERE id_maintenance = @id_maintenance;

    END TRY
    BEGIN CATCH
        THROW; 
    END CATCH
END;
GO