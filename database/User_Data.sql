-- ============================================================
-- Tạo Oracle Account cho nhân viên
-- Chạy bằng: SYS AS SYSDBA
-- Chạy SAU khi 01_HR_ADMIN_DDL_INSERT.sql đã có data
-- ============================================================

-- Lê Hoàng Minh - Giám đốc Nhân sự
CREATE USER minhlh IDENTIFIED BY minhlh
    DEFAULT TABLESPACE users
    QUOTA 0 ON users;
GRANT CREATE SESSION TO minhlh;

-- Nguyễn Thị Mai - Trưởng phòng Kế toán
CREATE USER maintt IDENTIFIED BY maintt
    DEFAULT TABLESPACE users
    QUOTA 0 ON users;
GRANT CREATE SESSION TO maintt;

-- Trần Văn Hải - Chuyên viên Kế toán
CREATE USER haitv IDENTIFIED BY haitv
    DEFAULT TABLESPACE users
    QUOTA 0 ON users;
GRANT CREATE SESSION TO haitv;

-- Phạm Quốc Việt - Lập trình viên
CREATE USER vietpq IDENTIFIED BY vietpq
    DEFAULT TABLESPACE users
    QUOTA 0 ON users;
GRANT CREATE SESSION TO vietpq;
