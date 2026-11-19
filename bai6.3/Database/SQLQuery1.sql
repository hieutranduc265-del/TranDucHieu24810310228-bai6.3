USE master;
GO

IF DB_ID('ProjectTaskDB') IS NOT NULL
BEGIN
    ALTER DATABASE ProjectTaskDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE ProjectTaskDB;
END
GO

CREATE DATABASE ProjectTaskDB;
GO

USE ProjectTaskDB;
GO

-- 1. Bảng DuAn (Dự án)
CREATE TABLE DuAn (
    MaDuAn INT IDENTITY(1,1) PRIMARY KEY,
    TenDuAn NVARCHAR(150) NOT NULL,
    NgayBatDau DATETIME NULL,
    NgayKetThuc DATETIME NULL,
    TrangThai NVARCHAR(50) NULL -- Ví dụ: "Đang chạy", "Hoàn thành", "Tạm dừng"
);
GO

-- 2. Bảng ThanhVien (Thành viên nhóm)
CREATE TABLE ThanhVien (
    MaThanhVien INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    Email VARCHAR(100) UNIQUE NULL,
    ChuyenMon NVARCHAR(100) NULL
);
GO

-- 3. Bảng CongViec (Công việc / Task)
CREATE TABLE CongViec (
    MaTask INT IDENTITY(1,1) PRIMARY KEY,
    MaDuAn INT NOT NULL,
    TenTask NVARCHAR(200) NOT NULL,
    MoTa NVARCHAR(MAX) NULL,
    HanNop DATETIME NULL,
    DoUuTien NVARCHAR(20) NULL, -- Ví dụ: "Cao", "Trung bình", "Thấp"
    TrangThai NVARCHAR(50) NULL, -- Ví dụ: "To Do", "In Progress", "Review", "Done"
    CONSTRAINT FK_CongViec_DuAn FOREIGN KEY (MaDuAn) REFERENCES DuAn(MaDuAn) ON DELETE CASCADE
);
GO

-- 4. Bảng PhanCong (Phân công công việc)
CREATE TABLE PhanCong (
    MaTask INT NOT NULL,
    MaThanhVien INT NOT NULL,
    NgayGiao DATETIME NOT NULL DEFAULT GETDATE(),
    PRIMARY KEY (MaTask, MaThanhVien),
    CONSTRAINT FK_PhanCong_CongViec FOREIGN KEY (MaTask) REFERENCES CongViec(MaTask) ON DELETE CASCADE,
    CONSTRAINT FK_PhanCong_ThanhVien FOREIGN KEY (MaThanhVien) REFERENCES ThanhVien(MaThanhVien) ON DELETE CASCADE
);
GO

-- =============================================
-- CHÈN DỮ LIỆU MẪU
-- =============================================

INSERT INTO DuAn (TenDuAn, NgayBatDau, NgayKetThuc, TrangThai) VALUES
(N'Hệ thống Quản lý Kho Thông minh', '2026-09-01', '2026-12-15', N'Đang chạy'),
(N'Ứng dụng Đặt lịch Khám bệnh', '2026-10-01', '2026-12-30', N'Đang chạy'),
(N'Website Thương mại Điện tử', '2026-06-01', '2026-08-30', N'Hoàn thành');
GO

INSERT INTO ThanhVien (HoTen, Email, ChuyenMon) VALUES
(N'Nguyễn Minh Tuấn', 'tuan.nm@example.com', N'Backend C# / SQL'),
(N'Trần Thu Hà', 'ha.tt@example.com', N'Frontend / UI-UX'),
(N'Lê Hoàng Nam', 'nam.lh@example.com', N'Mobile / Tester'),
(N'Phạm Gia Bảo', 'bao.pg@example.com', N'DevOps / System');
GO

INSERT INTO CongViec (MaDuAn, TenTask, MoTa, HanNop, DoUuTien, TrangThai) VALUES
-- Dự án 1
(1, N'Thiết kế cơ sở dữ liệu kho', N'Vẽ ERD và tạo bảng SQL Server', '2026-09-10', N'Cao', N'Done'),
(1, N'Xây dựng API nhập xuất kho', N'Viết API xử lý tồn kho theo lô', '2026-11-05', N'Cao', N'In Progress'), -- Quá hạn, chưa Done
(1, N'Làm giao diện quét mã vạch', N'Thiết kế form quét mã sản phẩm', '2026-11-25', N'Trung bình', N'To Do'),
(1, N'Kiểm thử hiệu năng truy vấn', N'Test tốc độ báo cáo tồn kho', '2026-11-01', N'Thấp', N'Review'), -- Quá hạn, chưa Done

-- Dự án 2
(2, N'Phân tích luồng đặt lịch', N'Lên danh sách chức năng và actor', '2026-10-10', N'Cao', N'Done'),
(2, N'Tích hợp thông báo nhắc lịch', N'Gửi thông báo trước giờ khám 24h', '2026-11-10', N'Trung bình', N'In Progress'), -- Quá hạn, chưa Done
(2, N'Thiết kế màn hình hồ sơ bệnh nhân', N'Hiển thị lịch sử khám và đơn thuốc', '2026-12-05', N'Cao', N'To Do'),

-- Dự án 3
(3, N'Xây dựng giỏ hàng và thanh toán', N'Xử lý tính tổng tiền và mã giảm giá', '2026-08-15', N'Cao', N'Done'),
(3, N'Tối ưu SEO trang sản phẩm', N'Cấu hình meta tag và tốc độ tải trang', '2026-08-25', N'Thấp', N'Done');
GO

INSERT INTO PhanCong (MaTask, MaThanhVien, NgayGiao) VALUES
(1, 1, '2026-09-02'),
(2, 1, '2026-09-15'),
(2, 4, '2026-09-16'),
(3, 2, '2026-10-01'),
(4, 3, '2026-10-05'),
(5, 2, '2026-10-02'),
(6, 1, '2026-10-15'),
(6, 3, '2026-10-16'),
(7, 2, '2026-10-20'),
(8, 1, '2026-06-10'),
(9, 4, '2026-07-01');
GO