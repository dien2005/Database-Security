-- Chạy bằng: SYS AS SYSDBA
-- Mục đích: Tạo user, cấp quyền, setup bảo mật cho AUDIT LOG


-- BƯỚC 1: TẠO USER HR_ADMIN
BEGIN
    FOR u IN (SELECT username FROM dba_users WHERE username = 'HR_ADMIN') LOOP
        EXECUTE IMMEDIATE 'DROP USER hr_admin CASCADE';
    END LOOP;
END;
/

CREATE USER hr_admin IDENTIFIED BY hr_admin
    DEFAULT TABLESPACE users
    QUOTA UNLIMITED ON users;


-- BƯỚC 2: CẤP QUYỀN CƠ BẢN CHO HR_ADMIN

GRANT CREATE SESSION          TO hr_admin;
GRANT CREATE TABLE            TO hr_admin;
GRANT CREATE VIEW             TO hr_admin;
GRANT CREATE PROCEDURE        TO hr_admin;
GRANT CREATE SEQUENCE         TO hr_admin;
GRANT CREATE TRIGGER          TO hr_admin;
--GRANT CREATE BLOCKCHAIN TABLE TO hr_admin;
GRANT EXECUTE ON DBMS_OUTPUT  TO hr_admin;



-- BƯỚC 3: TẠO USER BGD (Ban Giám Đốc) — chỉ được SELECT audit log
BEGIN
    FOR u IN (SELECT username FROM dba_users WHERE username = 'BGD_USER') LOOP
        EXECUTE IMMEDIATE 'DROP USER bgd_user CASCADE';
    END LOOP;
END;
/

CREATE USER bgd_user IDENTIFIED BY bgd_user
    DEFAULT TABLESPACE users
    QUOTA 0 ON users;  -- không cho tạo object

GRANT CREATE SESSION TO bgd_user;


-- BƯỚC 4: REVOKE QUYỀN DƯ THỪA CỦA HR_ADMIN TRÊN AUDIT LOG
-- Chạy SAU KHI file 01 đã tạo xong bảng SYSTEM_AUDIT_LOG

-- HR_ADMIN là owner → mặc định có ALL PRIVILEGES trên bảng của mình
-- Không thể REVOKE owner's privilege trực tiếp trong Oracle
-- Giải pháp: tạo bảng SYSTEM_AUDIT_LOG bằng SYS, sau đó GRANT INSERT cho HR_ADMIN

-- Tạo bảng audit log dưới schema SYS (hoặc schema riêng AUDIT_OWNER)
-- để HR_ADMIN không phải là owner → có thể kiểm soát quyền chặt hơn

-- Tạo audit schema riêng
BEGIN
    FOR u IN (SELECT username FROM dba_users WHERE username = 'AUDIT_OWNER') LOOP
        EXECUTE IMMEDIATE 'DROP USER audit_owner CASCADE';
    END LOOP;
END;
/

CREATE USER audit_owner IDENTIFIED BY audit_owner
    DEFAULT TABLESPACE users
    QUOTA UNLIMITED ON users;

GRANT CREATE SESSION          TO audit_owner;
GRANT CREATE TABLE            TO audit_owner;
--GRANT CREATE BLOCKCHAIN TABLE TO audit_owner;



-- BƯỚC 5: TẠO system_audit_log (BLOCKCHAIN TABLE) bằng SYS

-- Kết nối lại bằng AUDIT_OWNER để tạo bảng,
-- → Giải pháp thực tế: SYS tạo trong schema AUDIT_OWNER bằng cách
--   chạy CREATE TABLE với prefix schema

CREATE BLOCKCHAIN TABLE audit_owner.system_audit_log (
    log_id          NUMBER          GENERATED ALWAYS AS IDENTITY,
    event_time      TIMESTAMP       DEFAULT SYSTIMESTAMP      NOT NULL,
    db_user         VARCHAR2(128)   DEFAULT SYS_CONTEXT('USERENV','SESSION_USER'),
    os_user         VARCHAR2(128)   DEFAULT SYS_CONTEXT('USERENV','OS_USER'),
    ip_address      VARCHAR2(45)    DEFAULT SYS_CONTEXT('USERENV','IP_ADDRESS'),
    machine_name    VARCHAR2(128)   DEFAULT SYS_CONTEXT('USERENV','HOST'), -- MACHINE -> HOST
    action_type     VARCHAR2(50)    NOT NULL,
    object_schema   VARCHAR2(128),
    object_name     VARCHAR2(128),
    sql_text        CLOB,
    old_value       CLOB,
    new_value       CLOB,
    session_id      VARCHAR2(64)    DEFAULT SYS_CONTEXT('USERENV','SESSIONID'),
    result          VARCHAR2(20)    DEFAULT 'SUCCESS',

    CONSTRAINT pk_audit_log
        PRIMARY KEY (log_id),
    CONSTRAINT chk_audit_result
        CHECK (result IN ('SUCCESS','FAILURE','BLOCKED'))
)
-- blockchain table
NO DROP UNTIL 0 DAYS IDLE -- 15 minutes => 0 days ( vi oracle khong ho tro don vi minutes)
NO DELETE LOCKED
HASHING USING "SHA2_512" VERSION "v1";
COMMENT ON TABLE  audit_owner.system_audit_log IS 'Blockchain Table - chỉ SYS/AUDIT_OWNER toàn quyền, HR_ADMIN chỉ INSERT, BGD_USER chỉ SELECT';



-- BƯỚC 6: PHÂN QUYỀN CHÍNH XÁC TRÊN AUDIT LOG


-- HR_ADMIN: chỉ INSERT (để trigger/procedure ghi log)
GRANT INSERT ON audit_owner.system_audit_log TO hr_admin;
-- Không GRANT SELECT, UPDATE, DELETE cho HR_ADMIN

-- BGD_USER: chỉ SELECT
GRANT SELECT ON audit_owner.system_audit_log TO bgd_user;

-- Tạo synonym để HR_ADMIN gọi tên ngắn gọn
CREATE OR REPLACE SYNONYM hr_admin.system_audit_log
    FOR audit_owner.system_audit_log;

-- Tạo synonym để BGD_USER xem log
CREATE OR REPLACE SYNONYM bgd_user.system_audit_log
    FOR audit_owner.system_audit_log;



-- BƯỚC 7: VERIFY PHÂN QUYỀN

SELECT
    grantee,
    privilege,
    table_name,
    grantable
FROM dba_tab_privs
WHERE table_name = 'SYSTEM_AUDIT_LOG'
AND owner = 'AUDIT_OWNER'
ORDER BY grantee, privilege;

COMMIT;

