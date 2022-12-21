using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nhom11.Models
{
    public class Account
    {
        public int Id { get; set; }

        [DisplayName("Tên đăng nhập")]
        [Required(ErrorMessage = "{0} không được bỏ trống")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "{0} từ 6-20 kí tự")]
        public string Username { get; set; }

        [DisplayName("Mật khẩu")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "{0} từ 6-20 kí tự")]
        public string Password { get; set; }

      
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        [DisplayName("Email")]
        [EmailAddress(ErrorMessage = "{0} không hợp lệ")]
        public string Email { get; set; }

        [DisplayName("SĐT")]
        [RegularExpression("0\\d{9}", ErrorMessage = "SĐT không hợp lệ")]
        public string Phone { get; set; }

        [DisplayName("Địa chỉ")]
        public string Address { get; set; }

        [DisplayName("Họ tên")]
        public string FullName { get; set; }

        [DisplayName("Là admin")]
        [DefaultValue(false)]
        public bool IsAdmin { get; set; } = false;

        [DisplayName("Còn hoạt động")]
        [DefaultValue(true)]
        public bool Status { get; set; } = true;
        [NotMapped]
        public string LoginErrorMessage { get; set; } = null;

        // Collection reference property cho khóa ngoại từ Invoice
        public List<Invoice> Invoices { get; set; }

        // Collection reference property cho khóa ngoại từ Cart
        public List<Cart> Carts { get; set; }
    }
}
