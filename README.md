# Scanner Pocket PC Management System

Hệ thống quản lý máy quét Pocket PC bao gồm các thành phần:
- Công cụ chuyển đổi dữ liệu sang định dạng SDF cho máy quét đọc
- Phần mềm quét mã chạy trên thiết bị Pocket PC (WinForms)
- Web quản lý dữ liệu quét (ASP.NET)

## 🔄 Convert_Data (C# Console Application)

- Loại ứng dụng: **C# Console Application**
- Chức năng chính:
  - Nhận dữ liệu đầu vào (ví dụ: từ file, database, hoặc hệ thống khác)
  - Chuyển đổi dữ liệu sang định dạng **SDF**
  - Tạo file SDF để máy quét Pocket PC có thể đọc và sử dụng

📂 Thư mục: `Convert_Data/`

Ứng dụng này thường được chạy:
- Thủ công bởi người vận hành
- Và chạy theo web quản lý
---

## 🪟 Keyence_Device (C# WinForms Application)

- Loại ứng dụng: **C# WinForms**
- Chạy trực tiếp trên **máy quét Pocket PC / thiết bị Keyence**
- Chức năng chính:
  - Đọc dữ liệu từ file SDF
  - Quét mã (barcode / QR code)
  - Xử lý và hiển thị dữ liệu quét
  - Gửi kết quả quét về hệ thống server / web quản lý

📂 Thư mục: `Keyence_Device/`

Đây là chương trình **chạy trên thiết bị máy quét**, có giao diện người dùng để thao tác.

---

## 🌐 Manage_PocketPc (ASP.NET Web Application)

- Loại ứng dụng: **ASP.NET Web Application**
- Chạy trên server
- Chức năng chính:
  - Quản lý dữ liệu quét từ các máy Pocket PC
  - Hiển thị danh sách, lịch sử và báo cáo dữ liệu
  - Quản lý thiết bị, dữ liệu và người dùng
  - API để kết nối được viết trong Project_API (không có ở đây)

📂 Thư mục: `Manage_PocketPc/`

Người dùng truy cập thông qua trình duyệt web.

---
