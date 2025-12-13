# SCMS Worker Service - Hướng dẫn cấu hình và sử dụng

## Tính năng đã nâng cấp

Worker service đã được nâng cấp với các tính năng mới:

1. ✅ **Xuất báo cáo Excel** - Tự động xuất dữ liệu club ra file Excel
2. ✅ **Gửi email tự động** - Gửi email kèm file Excel đính kèm
3. ✅ **Ghi log JSON** - Vẫn giữ chức năng ghi log JSON như cũ

## Cấu hình Email

### 1. Sử dụng Gmail

Để sử dụng Gmail, bạn cần:

1. Bật **2-Step Verification** cho tài khoản Gmail
2. Tạo **App Password**:
   - Vào: https://myaccount.google.com/apppasswords
   - Tạo password mới cho ứng dụng
   - Copy password này (16 ký tự)

3. Cập nhật file `appsettings.json`:

```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SenderEmail": "your-email@gmail.com",
    "SenderName": "SCMS Worker Service",
    "Username": "your-email@gmail.com",
    "Password": "your-16-char-app-password",
    "EnableSsl": true,
    "DefaultRecipients": [
      "recipient1@gmail.com",
      "recipient2@gmail.com"
    ]
  }
}
```

### 2. Sử dụng Outlook/Hotmail

```json
{
  "EmailSettings": {
    "SmtpServer": "smtp-mail.outlook.com",
    "SmtpPort": 587,
    "SenderEmail": "your-email@outlook.com",
    "SenderName": "SCMS Worker Service",
    "Username": "your-email@outlook.com",
    "Password": "your-password",
    "EnableSsl": true,
    "DefaultRecipients": [
      "recipient1@outlook.com"
    ]
  }
}
```

### 3. SMTP Server khác

Tùy chỉnh theo SMTP server của bạn:

```json
{
  "EmailSettings": {
    "SmtpServer": "your-smtp-server.com",
    "SmtpPort": 587,
    "SenderEmail": "sender@example.com",
    "SenderName": "Worker Service",
    "Username": "username",
    "Password": "password",
    "EnableSsl": true,
    "DefaultRecipients": [
      "admin@example.com"
    ]
  }
}
```

## Cài đặt và chạy Worker Service

### Khôi phục packages

```powershell
dotnet restore
```

### Build project

```powershell
dotnet build
```

### Chạy thử nghiệm

```powershell
dotnet run --project SCMS.WorkerService.TienPVK
```

### Cài đặt Windows Service

```powershell
# Tạo service
sc create "Clubs_WorkerService" binPath= "C:\Users\Admin\source\PRN222\prn222-3w-fa25\FA25_PRN222_3W1_ASM2_SE170488_TienPVK\SCMS.WorkerService.TienPVK\bin\Debug\net8.0\SCMS.WorkerService.TienPVK.exe"

# Khởi động service
sc start "Clubs_WorkerService"

# Dừng service
sc stop "Clubs_WorkerService"

# Xóa service
sc delete "Clubs_WorkerService"
```

## Cấu trúc thư mục output

Worker service sẽ tạo các thư mục sau trong Documents:

```
Documents/
└── Club Worker/
    ├── logs/
    │   └── worker_log_2024-12-13_10-30-00.txt
    └── excel_reports/
        └── clubs_report_2024-12-13_10-30-00.xlsx
```

## Lịch chạy

- Worker service chạy **mỗi 24 giờ** (chạy hàng ngày)
- Mỗi lần chạy sẽ:
  1. Lấy dữ liệu clubs từ database
  2. Ghi log JSON
  3. Xuất file Excel
  4. Gửi email kèm file Excel

## Kiểm tra logs

### Trong Event Viewer (Windows)

1. Mở **Event Viewer**
2. Vào: **Windows Logs → Application**
3. Tìm source: **.NET Runtime** hoặc tên service của bạn

### Trong file log

Kiểm tra thư mục `Documents/Club Worker/logs/`

## Troubleshooting

### Email không gửi được

1. Kiểm tra cấu hình SMTP trong `appsettings.json`
2. Đối với Gmail: Đảm bảo đã tạo App Password
3. Kiểm tra firewall/antivirus có chặn port 587 không
4. Xem logs trong Event Viewer để biết lỗi cụ thể

### Excel không tạo được

1. Đảm bảo có quyền ghi vào thư mục Documents
2. Kiểm tra logs để xem lỗi chi tiết

### Service không chạy

1. Kiểm tra đường dẫn binPath khi tạo service
2. Xem logs trong Event Viewer
3. Đảm bảo connection string đến database đúng

## Dependencies

Các package NuGet đã được thêm:

- **EPPlus** (7.5.2) - Xuất Excel
- **MailKit** (4.8.0) - Gửi email

## Lưu ý bảo mật

⚠️ **QUAN TRỌNG**: 

- Không commit file `appsettings.json` có chứa password thật lên Git
- Sử dụng User Secrets hoặc Environment Variables cho production
- App Password của Gmail nên được bảo mật cẩn thận
