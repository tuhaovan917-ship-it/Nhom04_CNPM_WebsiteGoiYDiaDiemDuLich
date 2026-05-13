# ✈️ TravelSuggest - Website Gợi Ý Địa Điểm Du Lịch

**TravelSuggest** là một nền tảng hỗ trợ người dùng tìm kiếm, khám phá và lên kế hoạch du lịch dựa trên các tiêu chí cá nhân hóa. Dự án giúp giải quyết vấn đề thông tin du lịch rời rạc bằng cách cung cấp dữ liệu tập trung, tin cậy và các gợi ý thông minh.

---

## 📂 Cấu trúc thư mục (Folder Structure)

Dự án được xây dựng dựa trên kiến trúc **ASP.NET MVC 5**, đảm bảo sự tách biệt giữa logic xử lý, dữ liệu và giao diện.

```text
Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich/
├── App_Start/              # Cấu hình khởi tạo hệ thống (Route, Bundle, Filter)
├── Areas/
│   └── Admin/              # Phân hệ Quản trị dành cho Admin (Dashboard, CRUD)
│       ├── Controllers/    # Xử lý nghiệp vụ quản lý địa điểm, người dùng, đánh giá
│       └── Views/          # Giao diện dành riêng cho quản trị viên
├── Content/                # Tài nguyên tĩnh (CSS, Images, Web Fonts)
│   ├── Images/             # Chứa các hình ảnh tĩnh của hệ thống
│   └── bootstrap.css       # Thư viện UI Framework chính
├── Controllers/            # Điều hướng và xử lý logic cho người dùng (User)
│   ├── AccountController.cs   # Quản lý Đăng nhập, Đăng ký, Bảo mật
│   ├── TravelController.cs    # Logic So sánh, Gợi ý và Chi tiết địa điểm
│   ├── PlannerController.cs   # Xử lý lập kế hoạch và lịch trình du lịch
│   └── ...                    # Các Controller bổ trợ (Home, Profile, User, Extra)
├── Models/                 # Định nghĩa thực thể dữ liệu và ViewModels
│   ├── Model1.edmx         # Entity Framework (Kết nối trực tiếp SQL Server)
│   └── ViewModels/         # Các Model tùy chỉnh để tối ưu hiển thị giao diện
├── Scripts/                # Các tệp xử lý JavaScript và thư viện Client-side
├── Views/                  # Giao diện người dùng cuối (Razor View Engine)
│   ├── Shared/             # Các Layout dùng chung (_Layout.cshtml, Navigation)
│   ├── Travel/             # Giao diện trang So sánh và Gợi ý
│   ├── Planner/            # Giao diện Lập kế hoạch và Chia sẻ lịch trình
│   └── ...                 # Giao diện các trang Account, Profile, Home, Extra
├── screenshots/            # Hình ảnh minh họa phục vụ tài liệu hướng dẫn
└── Web.config              # Tệp cấu hình hệ thống và chuỗi kết nối Database
```

## 🚀 Tính Năng Chính

- **Quản lý người dùng:** Đăng ký, đăng nhập, bảo mật tài khoản và cá nhân hóa hồ sơ.
- **Tìm kiếm & Bộ lọc thông minh:** Tra cứu địa điểm theo tên, khu vực, mức giá, và loại hình du lịch.
- **Hệ thống gợi ý:** Đề xuất điểm đến dựa trên sở thích cá nhân và xu hướng.
- **Lập kế hoạch du lịch:** Tạo lịch trình chi tiết, quản lý danh sách yêu thích.
- **Tương tác cộng đồng:** Đánh giá (1-5 sao) và bình luận về các địa điểm.
- **Quản trị hệ thống (Admin):** Công cụ quản lý dữ liệu địa điểm, người dùng và nội dung.

---

## 🛠 Công Nghệ Sử Dụng

- **Backend:** ASP.NET MVC (C#)
- **Frontend:** HTML5, CSS3, Razor View Engine, Bootstrap, jQuery.
- **Database:** Microsoft SQL Server.
- **Tools:** Visual Studio 2022, GitHub, Figma.

---

## 📸 Giao Diện Ứng Dụng Tiêu Biểu (Screenshots)

### 🏠 1. Trải nghiệm Người dùng (User Interface)
<details>
  <summary><b>Trang chủ & Tìm kiếm (5 ảnh)</b></summary>
  <br>
  <p align="center">
    <img src="Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich/screenshots/home-1.png" width="32%"> <img src="Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich/screenshots/home-2.png" width="32%"> <img src="Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich/screenshots/home-3.png" width="32%">
    <br><br>
    <img src="Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich/screenshots/search.png" width="48%"> <img src="Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich/screenshots/search-result.png" width="48%">
     <br><i>Trang chủ và tìm kiếm</i>
  </p>
</details>

<details>
  <summary><b>Chi tiết & So sánh địa điểm (3 ảnh)</b></summary>
  <br>
  <p align="center">
    <img src="Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich/screenshots/detail-1.png" width="48%"> <img src="Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich/screenshots/detail-2.png" width="48%">
    <br><br>
    <img src="Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich/screenshots/compare.png" width="90%">
    <br><i>Hệ thống so sánh các tiêu chí du lịch thông minh và xem chi tiết địa điểm</i>
  </p>
</details>

<details>
  <summary><b>Lập kế hoạch & Trang cá nhân (4 ảnh)</b></summary>
  <br>
  <p align="center">
    <img src="Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich/screenshots/plan.png" width="90%">
    <br><br>
    <img src="Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich/screenshots/create-plan-1.png" width="32%"> <img src="Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich/screenshots/create-plan-2.png" width="32%"> <img src="Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich/screenshots/profile-place-saved.png" width="32%">
    <br><i>Quản lý lịch trình cá nhân và lưu trữ địa điểm yêu thích</i>
  </p>
</details>

### 🛠️ 2. Hệ thống Quản trị (Admin)
<details>
  <summary><b>Quản lý nghiệp vụ (1 ảnh)</b></summary>
  <br>
  <p align="center">
    <img src="Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich/screenshots/admin-place-management.png" width="90%">
    <br><i>Giao diện CRUD địa điểm dành cho Quản trị viên</i>
  </p>
</details>

---

## 💡 Điểm Nhấn Kỹ Thuật (Technical Highlights)

Trong dự án này, mình đã tập trung giải quyết các bài toán kỹ thuật sau:
1. **Thiết kế Database tối ưu:** Xây dựng mô hình quan hệ (RDBMS) chặt chẽ để quản lý đa dạng thực thể (User, Place, Plan, Review).
2. **Xử lý Logic Phức tạp:** Hiện thực hóa tính năng **So sánh địa điểm** và **Lập kế hoạch du lịch**, yêu cầu kỹ năng truy vấn dữ liệu nâng cao và xử lý mảng/đối tượng trong C#.
3. **Phân quyền người dùng (Authorization):** Tách biệt hoàn toàn luồng giao diện và quyền hạn giữa người dùng thông thường và Quản trị viên (Admin).
4. **Giao diện Responsive:** Sử dụng Bootstrap giúp website hiển thị tốt trên nhiều kích thước màn hình khác nhau.

---

## 💻 Cài Đặt (Installation)

### 1. Tiền đề (Prerequisites)

Để khởi chạy dự án, máy tính của bạn cần có:

- **Visual Studio 2022** (hoặc 2019) có cài đặt workload *ASP.NET and web development*
- **SQL Server Management Studio (SSMS)** hoặc SQL Server Express
- **.NET Framework 4.7.2** hoặc phiên bản tương đương

### 2. Các bước thực hiện

#### Bước 1: Tải mã nguồn về máy

```bash
git clone https://github.com/tuhaovan917-ship-it/Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich.git
cd Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich
```

#### Bước 2: Thiết lập Cơ sở dữ liệu

1. Mở **SQL Server Management Studio (SSMS)**
2. Tạo một Database mới với tên: `TravelSuggestDB`
3. Mở và thực thi (Execute) tệp script SQL (`SQLQueryTravelSuggest.sql`, `SQLQueryInsertData.sql`) để khởi tạo bảng và dữ liệu mẫu

#### Bước 3: Cấu hình Chuỗi kết nối (Connection String)

1. Mở file solution (`.sln`) bằng Visual Studio
2. Tìm đến file `Web.config` trong thư mục gốc
3. Cập nhật thuộc tính `connectionString` trong thẻ `<connectionStrings>` sao cho khớp với tên SQL Server trên máy của bạn

```xml
<add name="Model1"
     connectionString="metadata=res://*/Models.Model1.csdl|...;
     provider=System.Data.SqlClient;
     provider connection string=&quot;
     data source=TEN_MAY_CUA_BAN\SQLEXPRESS;
     initial catalog=TravelSuggestDB;
     integrated security=True;
     MultipleActiveResultSets=True;
     App=EntityFramework&quot;"
     providerName="System.Data.EntityClient" />
```

> **Lưu ý:** Thay `TEN_MAY_CUA_BAN\SQLEXPRESS` bằng tên SQL Server thực tế trên máy của bạn.

#### Bước 4: Khởi chạy dự án

1. Chuột phải vào **Solution** → chọn **Restore NuGet Packages**
2. Nhấn **F5** hoặc chọn **Start (IIS Express)** để chạy website

---

## 🔑 Tài Khoản Thử Nghiệm (Test Accounts)

| Vai trò | Email | Mật khẩu |
|---|---|---|
| **Quản trị viên (Admin)** | `admin@travelsuggest.com` | `admin123` |
| **Người dùng (User)** | `user@gmail.com` | `123456` |

---

## 💡 Mẹo Khi Phát Triển

- **NuGet Restore:** Nếu project báo lỗi thư viện khi vừa mở, hãy chọn **Build Solution** để Visual Studio tự động tải lại package còn thiếu.
- **Database First (.edmx):** Nếu thay đổi cấu trúc Database trong SQL Server, hãy mở file `.edmx` → chuột phải → chọn **Update Model from Database** để đồng bộ dữ liệu vào project.
