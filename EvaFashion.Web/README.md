# Hướng dẫn Cài đặt & Chạy Website EVA Fashion

Đây là tài liệu hướng dẫn chi tiết để bạn có thể cài đặt và chạy website EVA Fashion trên một máy tính Windows mới.

## 1. Yêu cầu Hệ thống (Prerequisites)

Trước khi bắt đầu, máy tính cần cài sẵn các phần mềm sau:

1.  **.NET 8.0 SDK** (hoặc mới hơn):
    *   Tải về tại: [https://dotnet.microsoft.com/download/dotnet](https://dotnet.microsoft.com/download/dotnet)
    *   Kiểm tra bằng cách mở CMD/Terminal và gõ: `dotnet --version`
2.  **SQL Server**:
    *   Có thể dùng **SQL Server 2019/2022 Express** hoặc **LocalDB**.
    *   SQL Server Management Studio (SSMS) để quản lý (tùy chọn).
3.  **Git** (để tải source code - tùy chọn nếu bạn copy file thủ công).

## 2. Cấu hình Cơ sở dữ liệu

Website được cấu hình để tự động tạo Database nếu chưa tồn tại. Tuy nhiên, bạn cần đảm bảo **Connection String** trỏ đúng đến SQL Server của máy bạn.

1.  Mở file `appsettings.json` trong thư mục `EvaFashion.Web`.
2.  Tìm đoạn `"ConnectionStrings"`:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=...;Database=thoitrangnu;..."
    }
    ```
3.  Sửa `Server=...` thành tên máy chủ SQL của bạn.
    *   Ví dụ dùng LocalDB: `Server=(localdb)\\mssqllocaldb`
    *   Ví dụ dùng SQL Express: `Server=.\\SQLEXPRESS` hoặc `Server=TEN_MAY_TINH`

## 3. Cách chạy Website

1.  Mở **Command Prompt (CMD)** hoặc **PowerShell**.
2.  Di chuyển đến thư mục chứa source code (nơi có file `EvaFashion.csproj`):
    ```powershell
    cd duong/dan/toi/EvaFashion.Web
    ```
3.  Chạy lệnh cài đặt các thư viện cần thiết:
    ```powershell
    dotnet restore
    ```
4.  Chạy website:
    ```powershell
    dotnet run
    ```
5.  Sau khi thấy thông báo như `Now listening on: http://localhost:5xxx`, hãy mở trình duyệt và truy cập địa chỉ đó.

## 4. Tài khoản Quản trị & Dữ liệu Mẫu

Trong lần chạy đầu tiên, hệ thống sẽ **tự động**:
*   Tạo Database `thoitrangnu`.
*   Tạo các bảng dữ liệu.
*   Thêm Danh mục và Sản phẩm mẫu.
*   Tạo tài khoản Admin mặc định.

> **Cách 2 (Thủ công):** Nếu bạn muốn tự chạy SQL, hãy mở file `database.sql` trong SQL Server Management Studio (SSMS) và nhấn Execute. File này chứa toàn bộ cấu trúc bảng và dữ liệu mẫu đầy đủ (bao gồm 16 sản phẩm và tài khoản Admin).

**Thông tin đăng nhập Admin:**
*   Link đăng nhập: `/Account/Login` (hoặc click nút Đăng nhập trên menu)
*   User: `admin`
*   Pass: `123456`

## 5. Cấu trúc Thư mục Chính

*   `Controllers`: Xử lý logic nghiệp vụ.
*   `Models`: Chứa các Entities (CSDL).
*   `Views`: Giao diện người dùng (HTML/Razor).
*   `wwwroot`: Chứa file tĩnh (CSS, JS, Hình ảnh).
    *   `images/products`: Ảnh sản phẩm.
    *   `images/banners`: Ảnh banner trang chủ.
*   `Areas/Admin`: Khu vực quản trị.
*   `Data/DbInitializer.cs`: Code khởi tạo dữ liệu mẫu.

## 6. Ghi chú

*   Ảnh sản phẩm và banner nằm trong thư mục `wwwroot/images`. Khi copy sang máy khác, hãy đảm bảo copy đầy đủ thư mục `wwwroot`.
*   Nếu muốn reset dữ liệu, bạn chỉ cần xóa Database `thoitrangnu` trong SQL Server, sau đó chạy lại website, nó sẽ tự tạo lại mới tinh.

---
*Chúc bạn thành công!*
