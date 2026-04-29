using System.ComponentModel.DataAnnotations;

namespace Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich.Models
{
    [MetadataType(typeof(ContactMetadata))]
    public partial class Contact
    {
    }

    public class ContactMetadata
    {
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [StringLength(200, MinimumLength = 3,
            ErrorMessage = "Họ tên phải từ 3 đến 200 ký tự")]
        [RegularExpression(@"^[^\d]+$",
            ErrorMessage = "Họ tên không được chứa số")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [StringLength(255)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập nội dung liên hệ")]
        [StringLength(2000, MinimumLength = 10,
            ErrorMessage = "Nội dung phải ít nhất 10 ký tự")]
        public string Message { get; set; }
    }
}
