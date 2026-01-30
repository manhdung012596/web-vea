USE thoitrangeva;
GO

IF OBJECT_ID('DanhGia', 'U') IS NULL
BEGIN
    CREATE TABLE DanhGia (
        id INT IDENTITY PRIMARY KEY,
        sanPhamId INT,
        maNguoiDung INT,
        soSao INT CHECK (soSao BETWEEN 1 AND 5),
        noiDung NVARCHAR(MAX),
        ngayDanhGia DATETIME DEFAULT GETDATE(),
        FOREIGN KEY (sanPhamId) REFERENCES SanPham(id),
        FOREIGN KEY (maNguoiDung) REFERENCES NguoiDung(maNguoiDung)
    );
    PRINT 'Table DanhGia created successfully.';
END
ELSE
BEGIN
    PRINT 'Table DanhGia already exists.';
END
GO
