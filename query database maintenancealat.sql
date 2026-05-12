
-- 1. Membuat database baru yang bersih
CREATE DATABASE DBMaintenanceAlat;
GO

--2. Mulai gunakan database yang baru dibuat
USE DBMaintenanceAlat;
GO

-- 3. Membuat Tabel Alat
CREATE TABLE Alat (
    id_alat INT IDENTITY(1,1) PRIMARY KEY,
    nama_alat VARCHAR(100) NOT NULL,
    kondisi_fisik VARCHAR(50) NOT NULL
);
GO

-- 4. Membuat Tabel Teknisi 
CREATE TABLE Teknisi (
    id_teknisi INT IDENTITY(1,1) PRIMARY KEY,
    nama_teknisi VARCHAR(100) NOT NULL
);
GO

-- 5. Membuat Tabel Maintenance 
CREATE TABLE Maintenance (
    id_maintenance INT IDENTITY(1,1) PRIMARY KEY,
    id_alat INT NOT NULL,
    id_teknisi INT NOT NULL,
    tgl_service DATE NOT NULL,
    jenis_perbaikan VARCHAR(255) NOT NULL,
    keterangan TEXT,
    
    CONSTRAINT FK_Maintenance_Alat FOREIGN KEY (id_alat) 
    REFERENCES Alat(id_alat) 
    ON DELETE CASCADE ON UPDATE CASCADE,
    
    CONSTRAINT FK_Maintenance_Teknisi FOREIGN KEY (id_teknisi) 
    REFERENCES Teknisi(id_teknisi) 
    ON DELETE CASCADE ON UPDATE CASCADE
);
GO

select * from Alat