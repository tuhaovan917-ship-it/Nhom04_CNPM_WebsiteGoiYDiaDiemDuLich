using Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Net.Mail;

namespace Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich.Controllers
{
    public class AccountController : Controller
    {
        TravelSuggestEntities db = new TravelSuggestEntities();
        
        // Tạo SALT ngẫu nhiên
        private string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        // Hash password + salt
        private string HashPassword(string password, string salt)
        {
            using (SHA256 sha = SHA256.Create())
            {
                var combined = password + salt;
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(combined));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }


        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string Email, string Password)
        {
            var user = db.Users.FirstOrDefault(u => u.Email == Email);

            if (user == null)
            {
                ViewBag.Error = "Email hoặc mật khẩu không đúng!";
                return View();
            }

            // Hash lại password + salt
            string hashed = HashPassword(Password, user.Salt);

            if (hashed != user.PasswordHash)
            {
                ViewBag.Error = "Email hoặc mật khẩu không đúng!";
                return View();
            }

            // Lưu session
            Session["UserId"] = user.UserId;
            Session["UserName"] = user.FullName;
            Session["Email"] = user.Email;
            Session["Role"] = user.Role; // 🔥 RẤT QUAN TRỌNG

            // ===== PHÂN LUỒNG =====
            if (user.Role == "Admin")
            {
                return RedirectToAction(
                    "Index",
                    "Dashboard",
                    new { area = "Admin" }
                );
            }

            return RedirectToAction("Index", "Home");
        }


        // GET: Register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(string FullName, string Email, string Password, string ConfirmPassword)
        {
            if (Password != ConfirmPassword)
            {
                ViewBag.Error = "Mật khẩu xác nhận không trùng khớp!";
                return View();
            }

            if (db.Users.Any(u => u.Email == Email))
            {
                ViewBag.Error = "Email đã được sử dụng!";
                return View();
            }

            // Tạo SALT
            string salt = GenerateSalt();

            // Hash mật khẩu + salt
            string hashed = HashPassword(Password, salt);

            var user = new User
            {
                FullName = FullName,
                Email = Email,
                PasswordHash = hashed,
                Salt = salt, // 🔥 Lưu SALT vào DB
                CreatedAt = DateTime.Now,
                Role = "User"
            };

            db.Users.Add(user);
            db.SaveChanges();

            ViewBag.Success = "Đăng ký thành công! Bạn có thể đăng nhập.";

            return View();
        }


        // GET: Forgot Password
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string Email)
        {
            var user = db.Users.FirstOrDefault(u => u.Email == Email);

            if (user == null)
            {
                ViewBag.Error = "Email không tồn tại trong hệ thống!";
                return View();
            }

            // Tạo token reset
            string token = Guid.NewGuid().ToString();

            db.PasswordResets.Add(new PasswordReset
            {
                UserId = user.UserId,
                Token = token,
                ExpireAt = DateTime.Now.AddMinutes(30),
                IsUsed = false
            });

            db.SaveChanges();

            // Tạo link reset
            string resetLink = Url.Action(
                "ResetPassword",
                "Account",
                new { token = token },
                protocol: Request.Url.Scheme
            );

            // Gửi email
            SendResetPasswordEmail(user.Email, resetLink);

            ViewBag.Success = "Liên kết đặt lại mật khẩu đã được gửi tới email của bạn.";
            return View();
        }

        // GET: ResetPassword
        public ActionResult ResetPassword(string token)
        {
            var reset = db.PasswordResets
                .FirstOrDefault(x => x.Token == token && !x.IsUsed && x.ExpireAt > DateTime.Now);

            if (reset == null)
            {
                return HttpNotFound();
            }

            ViewBag.Token = token;
            return View();
        }

        // POST: ResetPassword
        [HttpPost]
        public ActionResult ResetPassword(string token, string NewPassword, string ConfirmPassword)
        {
            if (NewPassword != ConfirmPassword)
            {
                ViewBag.Error = "Mật khẩu xác nhận không khớp!";
                ViewBag.Token = token;
                return View();
            }

            var reset = db.PasswordResets
                .FirstOrDefault(x => x.Token == token && !x.IsUsed);

            if (reset == null) return HttpNotFound();

            var user = db.Users.Find(reset.UserId);

            // Tạo salt mới
            string newSalt = GenerateSalt();
            string newHashed = HashPassword(NewPassword, newSalt);

            user.PasswordHash = newHashed;
            user.Salt = newSalt;

            reset.IsUsed = true;
            db.SaveChanges();

            return RedirectToAction("Login");
        }

        // Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // ================= GỬI EMAIL RESET MẬT KHẨU (GMAIL THẬT) =================
        private void SendResetPasswordEmail(string toEmail, string resetLink)
        {
            try
            {
                MailMessage mail = new MailMessage();

                // 🔴 GMAIL CỦA BẠN (phải trùng với gmail đăng nhập SMTP)
                mail.From = new MailAddress("", "TravelSuggest");

                mail.To.Add(toEmail);
                mail.Subject = "Đặt lại mật khẩu - TravelSuggest";

                mail.Body = $@"
                    <p>Xin chào,</p>
                    <p>Bạn đã yêu cầu đặt lại mật khẩu cho tài khoản TravelSuggest.</p>
                    <p>Vui lòng nhấn vào link bên dưới để tạo mật khẩu mới:</p>
                    <p>
                        <a href='{resetLink}'>
                            Đặt lại mật khẩu
                        </a>
                    </p>
                    <p>Liên kết này có hiệu lực trong <b>30 phút</b>.</p>
                    <p>Nếu bạn không yêu cầu, hãy bỏ qua email này.</p>
                ";

                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,

                    // 🔴 TÀI KHOẢN + APP PASSWORD GMAIL
                    Credentials = new System.Net.NetworkCredential(
                        "",   // Gmail của bạn
                        ""      // App Password (16 ký tự)
                    )
                };

                smtp.Send(mail);
            }
            //catch (Exception ex)
            //{
            //    // Debug khi lỗi gửi mail
            //    throw new Exception("Lỗi gửi email reset mật khẩu: " + ex.Message);
            //}

            catch (Exception ex)
            {
                // Log ex ở đây
                throw new ApplicationException(
                    "Không thể gửi email reset mật khẩu. Vui lòng thử lại sau.",
                    ex
                );
            }

        }

    }
}
