-----------------------------------------------
-- TẠO DATABASE
-----------------------------------------------
USE master;
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = N'thoitrangnu')
BEGIN
    CREATE DATABASE thoitrangnu;
END;
GO

USE thoitrangnu;
GO

-----------------------------------------------
-- 1. BẢNG NGƯỜI DÙNG
-----------------------------------------------
IF OBJECT_ID('NguoiDung', 'U') IS NULL
CREATE TABLE NguoiDung (
    maNguoiDung INT IDENTITY(1,1) PRIMARY KEY,
    tenDangNhap NVARCHAR(50) UNIQUE NOT NULL,
    matKhau NVARCHAR(255) NOT NULL,
    hoTen NVARCHAR(100),
    email NVARCHAR(100),
    sdt NVARCHAR(20),
    vaiTro NVARCHAR(20) DEFAULT 'khachhang' CHECK (vaiTro IN ('admin','khachhang')),
    ngayTao DATETIME DEFAULT GETDATE()
);
GO

-----------------------------------------------
-- 2. BẢNG DANH MỤC SẢN PHẨM
-----------------------------------------------
IF OBJECT_ID('DanhMuc', 'U') IS NULL
CREATE TABLE DanhMuc (
    id INT IDENTITY PRIMARY KEY,
    tenDanhMuc NVARCHAR(150) NOT NULL,
    slug NVARCHAR(200) UNIQUE NOT NULL,
    createdBy INT,
    updatedBy INT,
    FOREIGN KEY (createdBy) REFERENCES NguoiDung(maNguoiDung),
    FOREIGN KEY (updatedBy) REFERENCES NguoiDung(maNguoiDung)
);
GO

-----------------------------------------------
-- 3. BẢNG MÀU SẮC & KÍCH CỠ
-----------------------------------------------
IF OBJECT_ID('MauSac', 'U') IS NULL
CREATE TABLE MauSac (
    maMau INT IDENTITY(1,1) PRIMARY KEY,
    tenMau NVARCHAR(50) NOT NULL,
    maHex NVARCHAR(10)
);

IF OBJECT_ID('KichCo', 'U') IS NULL
CREATE TABLE KichCo (
    maKichCo INT IDENTITY(1,1) PRIMARY KEY,
    tenKichCo NVARCHAR(10) NOT NULL
);
GO

-----------------------------------------------
-- 4. BẢNG SẢN PHẨM
-----------------------------------------------
IF OBJECT_ID('SanPham', 'U') IS NULL
CREATE TABLE SanPham (
    id INT IDENTITY PRIMARY KEY,
    danhMucId INT NOT NULL,
    tenSanPham NVARCHAR(200) NOT NULL,
    slug NVARCHAR(250) NOT NULL UNIQUE,
    thuongHieu NVARCHAR(100),
    moTaNgan NVARCHAR(MAX),
    moTaChiTiet NVARCHAR(MAX),
    giaGoc DECIMAL(12,2) NOT NULL,
    phanTramGiam INT DEFAULT 0,
    giaSauGiam AS (
        CASE 
            WHEN phanTramGiam > 0 THEN giaGoc - (giaGoc * phanTramGiam / 100)
            ELSE giaGoc
        END
    ),
    hinhAnhChinh NVARCHAR(255),
    metaTitle NVARCHAR(200),
    metaDescription NVARCHAR(300),
    metaKeyword NVARCHAR(300),
    noiBat BIT DEFAULT 0,
    isActive BIT DEFAULT 1,
    createdAt DATETIME DEFAULT GETDATE(),
    updatedAt DATETIME,
    createdBy INT,
    updatedBy INT,
    FOREIGN KEY (danhMucId) REFERENCES DanhMuc(id),
    FOREIGN KEY (createdBy) REFERENCES NguoiDung(maNguoiDung),
    FOREIGN KEY (updatedBy) REFERENCES NguoiDung(maNguoiDung)
);
GO

-----------------------------------------------
-- 5. CÁC BẢNG KHÁC (Biến thể, Giỏ hàng, Đơn hàng...)
-----------------------------------------------
IF OBJECT_ID('BienTheSanPham', 'U') IS NULL
CREATE TABLE BienTheSanPham (
    id INT IDENTITY PRIMARY KEY,
    sanPhamId INT NOT NULL,
    mauId INT,
    kichCoId INT,
    soLuongTon INT DEFAULT 0,
    hinhAnh NVARCHAR(255),
    FOREIGN KEY (sanPhamId) REFERENCES SanPham(id),
    FOREIGN KEY (mauId) REFERENCES MauSac(maMau),
    FOREIGN KEY (kichCoId) REFERENCES KichCo(maKichCo)
);

IF OBJECT_ID('GioHang', 'U') IS NULL
CREATE TABLE GioHang (
    maGioHang INT IDENTITY(1,1) PRIMARY KEY,
    sessionId NVARCHAR(255),
    maNguoiDung INT,
    tongTien DECIMAL(12,2) DEFAULT 0,
    trangThai NVARCHAR(20) DEFAULT 'DangSuDung',
    ghiChu NVARCHAR(MAX),
    ngayTao DATETIME DEFAULT GETDATE(),
    ngayCapNhat DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (maNguoiDung) REFERENCES NguoiDung(maNguoiDung)
);

IF OBJECT_ID('GioHangChiTiet', 'U') IS NULL
CREATE TABLE GioHangChiTiet (
    id INT IDENTITY PRIMARY KEY,
    gioHangId INT NOT NULL,
    bienTheId INT NOT NULL,
    soLuong INT NOT NULL,
    donGia DECIMAL(12,2) NOT NULL,
    thanhTien DECIMAL(12,2) NOT NULL,
    ngayCapNhat DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (gioHangId) REFERENCES GioHang(maGioHang),
    FOREIGN KEY (bienTheId) REFERENCES BienTheSanPham(id)
);

IF OBJECT_ID('DonHang', 'U') IS NULL
CREATE TABLE DonHang (
    maDonHang INT IDENTITY(1,1) PRIMARY KEY,
    maDonHangCode NVARCHAR(20) UNIQUE,
    hoTen NVARCHAR(100),
    soDienThoai NVARCHAR(20),
    diaChi NVARCHAR(255),
    tongTien DECIMAL(12,2),
    phiVanChuyen DECIMAL(12,2) DEFAULT 0,
    phuongThucThanhToan NVARCHAR(20),
    trangThaiThanhToan NVARCHAR(20) DEFAULT 'ChuaThanhToan',
    trangThai NVARCHAR(20) DEFAULT 'ChoXacNhan',
    ghiChu NVARCHAR(MAX),
    ngayDat DATETIME DEFAULT GETDATE(),
    ngayCapNhat DATETIME DEFAULT GETDATE(),
    maNguoiDung INT,
    FOREIGN KEY (maNguoiDung) REFERENCES NguoiDung(maNguoiDung)
);

IF OBJECT_ID('ChiTietDonHang', 'U') IS NULL
CREATE TABLE ChiTietDonHang (
    maChiTiet INT IDENTITY(1,1) PRIMARY KEY,
    maDonHang INT,
    maBienThe INT,
    soLuong INT,
    donGia DECIMAL(12,2),
    thanhTien DECIMAL(12,2),
    ghiChu NVARCHAR(MAX),
    FOREIGN KEY (maDonHang) REFERENCES DonHang(maDonHang),
    FOREIGN KEY (maBienThe) REFERENCES BienTheSanPham(id)
);

IF OBJECT_ID('BaiViet', 'U') IS NULL
CREATE TABLE BaiViet (
    id INT IDENTITY PRIMARY KEY,
    tieuDe NVARCHAR(200),
    slug NVARCHAR(250) UNIQUE,
    hinhAnh NVARCHAR(255),
    moTa NVARCHAR(MAX),
    noiDung NVARCHAR(MAX),
    metaTitle NVARCHAR(200),
    metaDescription NVARCHAR(300),
    metaKeyword NVARCHAR(300),
    isPublished BIT DEFAULT 1,
    noiBat BIT DEFAULT 0,
    createdAt DATETIME DEFAULT GETDATE(),
    createdBy INT,
    FOREIGN KEY (createdBy) REFERENCES NguoiDung(maNguoiDung)
);

IF OBJECT_ID('TrangNoiDung', 'U') IS NULL
CREATE TABLE TrangNoiDung (
    maTrang INT IDENTITY(1,1) PRIMARY KEY,
    tieuDe NVARCHAR(200),
    slug NVARCHAR(200),
    loaiTrang NVARCHAR(50),
    moTaNgan NVARCHAR(MAX),
    noiDung NVARCHAR(MAX),
    thuTu INT DEFAULT 0,
    hienThi BIT DEFAULT 1,
    noiBat BIT DEFAULT 0,
    ngayCapNhat DATETIME DEFAULT GETDATE(),
    maNguoiTao INT,
    maNguoiCapNhat INT,
    FOREIGN KEY (maNguoiTao) REFERENCES NguoiDung(maNguoiDung),
    FOREIGN KEY (maNguoiCapNhat) REFERENCES NguoiDung(maNguoiDung)
);

IF OBJECT_ID('LienHe', 'U') IS NULL
CREATE TABLE LienHe (
    maLienHe INT IDENTITY(1,1) PRIMARY KEY,
    hoTen NVARCHAR(100),
    email NVARCHAR(100),
    soDienThoai NVARCHAR(20),
    tieuDe NVARCHAR(200),
    loaiLienHe NVARCHAR(50),
    noiDung NVARCHAR(MAX) NOT NULL,
    daDoc BIT DEFAULT 0,
    trangThai NVARCHAR(20) DEFAULT 'ChuaXuLy',
    ngayGui DATETIME DEFAULT GETDATE(),
    maNguoiDung INT,
    FOREIGN KEY (maNguoiDung) REFERENCES NguoiDung(maNguoiDung)
);

IF OBJECT_ID('DanhGia', 'U') IS NULL
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

IF OBJECT_ID('LichSuDonHang', 'U') IS NULL
CREATE TABLE LichSuDonHang (
    id INT IDENTITY PRIMARY KEY,
    maDonHang INT,
    trangThai NVARCHAR(20),
    ghiChu NVARCHAR(255),
    ngayCapNhat DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (maDonHang) REFERENCES DonHang(maDonHang)
);

IF OBJECT_ID('Voucher', 'U') IS NULL
CREATE TABLE Voucher (
    id INT IDENTITY PRIMARY KEY,
    maCode NVARCHAR(50) UNIQUE,
    giamGia DECIMAL(12,2),
    loai NVARCHAR(20) CHECK (loai IN ('Tien','PhanTram')),
    ngayBatDau DATETIME,
    ngayKetThuc DATETIME,
    soLuong INT,
    trangThai BIT DEFAULT 1
);
GO

-----------------------------------------------
-- 6. DỮ LIỆU MẪU (SEED DATA)
-----------------------------------------------

-- 6.1. Tài khoản Admin (Pass: 123456)
IF NOT EXISTS (SELECT * FROM NguoiDung WHERE tenDangNhap = 'admin')
INSERT INTO NguoiDung (tenDangNhap, matKhau, hoTen, email, vaiTro, ngayTao)
VALUES ('admin', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'Quản Trị Viên', 'admin@evafashion.vn', 'admin', GETDATE());

DECLARE @adminId INT = (SELECT maNguoiDung FROM NguoiDung WHERE tenDangNhap = 'admin');

-- 6.2. Danh Mục
IF NOT EXISTS (SELECT * FROM DanhMuc WHERE slug = 'vay-dam')
    INSERT INTO DanhMuc (tenDanhMuc, slug, createdBy, updatedBy) VALUES (N'Váy Đầm', 'vay-dam', @adminId, @adminId);

IF NOT EXISTS (SELECT * FROM DanhMuc WHERE slug = 'ao-thoi-trang')
    INSERT INTO DanhMuc (tenDanhMuc, slug, createdBy, updatedBy) VALUES (N'Áo Thời Trang', 'ao-thoi-trang', @adminId, @adminId);

IF NOT EXISTS (SELECT * FROM DanhMuc WHERE slug = 'chan-vay')
    INSERT INTO DanhMuc (tenDanhMuc, slug, createdBy, updatedBy) VALUES (N'Chân Váy', 'chan-vay', @adminId, @adminId);

IF NOT EXISTS (SELECT * FROM DanhMuc WHERE slug = 'quan-nu')
    INSERT INTO DanhMuc (tenDanhMuc, slug, createdBy, updatedBy) VALUES (N'Quần Nữ', 'quan-nu', @adminId, @adminId);

IF NOT EXISTS (SELECT * FROM DanhMuc WHERE slug = 'phu-kien')
    INSERT INTO DanhMuc (tenDanhMuc, slug, createdBy, updatedBy) VALUES (N'Phụ Kiện', 'phu-kien', @adminId, @adminId);

DECLARE @catVay INT = (SELECT id FROM DanhMuc WHERE slug = 'vay-dam');
DECLARE @catAo INT = (SELECT id FROM DanhMuc WHERE slug = 'ao-thoi-trang');
DECLARE @catChanVay INT = (SELECT id FROM DanhMuc WHERE slug = 'chan-vay');
DECLARE @catQuan INT = (SELECT id FROM DanhMuc WHERE slug = 'quan-nu');
DECLARE @catPhuKien INT = (SELECT id FROM DanhMuc WHERE slug = 'phu-kien');

-- 6.3. Sản Phẩm (16 Sản phẩm mẫu)
-- SP 1-4
IF NOT EXISTS (SELECT * FROM SanPham WHERE slug = 'dam-hoa-mua-xuan')
    INSERT INTO SanPham (danhMucId, tenSanPham, slug, thuongHieu, moTaNgan, giaGoc, phanTramGiam, hinhAnhChinh, noiBat, isActive, createdBy)
    VALUES (@catVay, N'Đầm Hoa Nhí Mùa Xuân', 'dam-hoa-mua-xuan', 'Eva Design', N'Thiết kế nhẹ nhàng.', 550000, 10, '/images/products/p1.png', 1, 1, @adminId);

IF NOT EXISTS (SELECT * FROM SanPham WHERE slug = 'ao-blazer-cong-so')
    INSERT INTO SanPham (danhMucId, tenSanPham, slug, thuongHieu, moTaNgan, giaGoc, phanTramGiam, hinhAnhChinh, noiBat, isActive, createdBy)
    VALUES (@catAo, N'Áo Blazer Công Sở', 'ao-blazer-cong-so', 'Eva Office', N'Sang trọng, lịch sự.', 850000, 0, '/images/products/p2.png', 1, 1, @adminId);

IF NOT EXISTS (SELECT * FROM SanPham WHERE slug = 'chan-vay-xep-ly')
    INSERT INTO SanPham (danhMucId, tenSanPham, slug, thuongHieu, moTaNgan, giaGoc, phanTramGiam, hinhAnhChinh, noiBat, isActive, createdBy)
    VALUES (@catChanVay, N'Chân Váy Xếp Ly Hồng', 'chan-vay-xep-ly', 'Eva Young', N'Trẻ trung, nữ tính.', 420000, 5, '/images/products/p3.png', 1, 1, @adminId);

IF NOT EXISTS (SELECT * FROM SanPham WHERE slug = 'dam-da-hoi-cao-cap')
    INSERT INTO SanPham (danhMucId, tenSanPham, slug, thuongHieu, moTaNgan, giaGoc, phanTramGiam, hinhAnhChinh, noiBat, isActive, createdBy)
    VALUES (@catVay, N'Đầm Dạ Hội Satin', 'dam-da-hoi-cao-cap', 'Eva Luxury', N'Quyến rũ, quý phái.', 1200000, 0, '/images/products/p4.png', 1, 1, @adminId);

-- SP 5-10
IF NOT EXISTS (SELECT * FROM SanPham WHERE slug = 'ao-so-mi-trang')
    INSERT INTO SanPham (danhMucId, tenSanPham, slug, thuongHieu, moTaNgan, giaGoc, phanTramGiam, hinhAnhChinh, noiBat, isActive, createdBy)
    VALUES (@catAo, N'Áo Sơ Mi Trắng Cổ Điển', 'ao-so-mi-trang', 'Eva Basic', N'Chất liệu Cotton lụa.', 350000, 0, '/images/products/p5.png', 0, 1, @adminId);

IF NOT EXISTS (SELECT * FROM SanPham WHERE slug = 'quan-jeans-cap-cao')
    INSERT INTO SanPham (danhMucId, tenSanPham, slug, thuongHieu, moTaNgan, giaGoc, phanTramGiam, hinhAnhChinh, noiBat, isActive, createdBy)
    VALUES (@catQuan, N'Quần Jeans Nữ Cạp Cao', 'quan-jeans-cap-cao', 'Eva Denim', N'Tôn dáng, thời thượng.', 450000, 0, '/images/products/p6.png', 1, 1, @adminId);

IF NOT EXISTS (SELECT * FROM SanPham WHERE slug = 'tui-xach-da')
    INSERT INTO SanPham (danhMucId, tenSanPham, slug, thuongHieu, moTaNgan, giaGoc, phanTramGiam, hinhAnhChinh, noiBat, isActive, createdBy)
    VALUES (@catPhuKien, N'Túi Xách Da Cao Cấp', 'tui-xach-da', 'Eva Bags', N'Da thật 100%.', 1500000, 0, '/images/products/p7.png', 1, 1, @adminId);

IF NOT EXISTS (SELECT * FROM SanPham WHERE slug = 'giay-cao-got-den')
    INSERT INTO SanPham (danhMucId, tenSanPham, slug, thuongHieu, moTaNgan, giaGoc, phanTramGiam, hinhAnhChinh, noiBat, isActive, createdBy)
    VALUES (@catPhuKien, N'Giày Cao Gót Đen', 'giay-cao-got-den', 'Eva Shoes', N'Gót nhọn sành điệu.', 650000, 10, '/images/products/p8.png', 0, 1, @adminId);

IF NOT EXISTS (SELECT * FROM SanPham WHERE slug = 'kinh-mat-thoi-trang')
    INSERT INTO SanPham (danhMucId, tenSanPham, slug, thuongHieu, moTaNgan, giaGoc, phanTramGiam, hinhAnhChinh, noiBat, isActive, createdBy)
    VALUES (@catPhuKien, N'Kính Mát Thời Trang', 'kinh-mat-thoi-trang', 'Eva Accessories', N'Chống tia UV.', 300000, 0, '/images/products/p9.png', 0, 1, @adminId);

IF NOT EXISTS (SELECT * FROM SanPham WHERE slug = 'ao-thun-soc-ke')
    INSERT INTO SanPham (danhMucId, tenSanPham, slug, thuongHieu, moTaNgan, giaGoc, phanTramGiam, hinhAnhChinh, noiBat, isActive, createdBy)
    VALUES (@catAo, N'Áo Thun Sọc Kẻ', 'ao-thun-soc-ke', 'Eva Casual', N'Thoải mái năng động.', 250000, 0, '/images/products/p10.png', 0, 1, @adminId);

-- SP 11-16
IF NOT EXISTS (SELECT * FROM SanPham WHERE slug = 'dam-maxi-vang')
    INSERT INTO SanPham (danhMucId, tenSanPham, slug, thuongHieu, moTaNgan, giaGoc, phanTramGiam, hinhAnhChinh, noiBat, isActive, createdBy)
    VALUES (@catVay, N'Đầm Maxi Vàng Mùa Hè', 'dam-maxi-vang', 'Eva Summer', N'Phong cách Bohemian.', 580000, 15, '/images/products/p11.png', 1, 1, @adminId);

IF NOT EXISTS (SELECT * FROM SanPham WHERE slug = 'ao-len-co-lo')
    INSERT INTO SanPham (danhMucId, tenSanPham, slug, thuongHieu, moTaNgan, giaGoc, phanTramGiam, hinhAnhChinh, noiBat, isActive, createdBy)
    VALUES (@catAo, N'Áo Len Cổ Lọ', 'ao-len-co-lo', 'Eva Winter', N'Ấm áp, mềm mại.', 400000, 0, '/images/products/p12.png', 0, 1, @adminId);

IF NOT EXISTS (SELECT * FROM SanPham WHERE slug = 'ao-khoac-jeans')
    INSERT INTO SanPham (danhMucId, tenSanPham, slug, thuongHieu, moTaNgan, giaGoc, phanTramGiam, hinhAnhChinh, noiBat, isActive, createdBy)
    VALUES (@catAo, N'Áo Khoác Jeans', 'ao-khoac-jeans', 'Eva Denim', N'Bụi bặm, cá tính.', 600000, 0, '/images/products/p13.png', 0, 1, @adminId);

IF NOT EXISTS (SELECT * FROM SanPham WHERE slug = 'quan-short-trang')
    INSERT INTO SanPham (danhMucId, tenSanPham, slug, thuongHieu, moTaNgan, giaGoc, phanTramGiam, hinhAnhChinh, noiBat, isActive, createdBy)
    VALUES (@catQuan, N'Quần Short Trắng', 'quan-short-trang', 'Eva Shorts', N'Năng động mùa hè.', 320000, 5, '/images/products/p14.png', 0, 1, @adminId);

IF NOT EXISTS (SELECT * FROM SanPham WHERE slug = 'mu-coi-vanh-rong')
    INSERT INTO SanPham (danhMucId, tenSanPham, slug, thuongHieu, moTaNgan, giaGoc, phanTramGiam, hinhAnhChinh, noiBat, isActive, createdBy)
    VALUES (@catPhuKien, N'Mũ Cói Vành Rộng', 'mu-coi-vanh-rong', 'Eva Accessories', N'Đi biển cực xinh.', 200000, 0, '/images/products/p15.png', 0, 1, @adminId);

IF NOT EXISTS (SELECT * FROM SanPham WHERE slug = 'khan-lua-cao-cap')
    INSERT INTO SanPham (danhMucId, tenSanPham, slug, thuongHieu, moTaNgan, giaGoc, phanTramGiam, hinhAnhChinh, noiBat, isActive, createdBy)
    VALUES (@catPhuKien, N'Khăn Lụa Cao Cấp', 'khan-lua-cao-cap', 'Eva Silk', N'Họa tiết sang trọng.', 450000, 0, '/images/products/p16.png', 1, 1, @adminId);
GO
