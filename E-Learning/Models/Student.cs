using System;
using System.Collections.Generic;

namespace E_Learning.Models
{
    public partial class Student
    {
        public int Id { get; set; }

        public string Mssv { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public DateTime? EnrollmentDate { get; set; }

        public int Status { get; set; }

        /// <summary>
        /// Khi mà học sinh vô hồ sơ thì sẽ điền vô
        /// </summary>
        public string? Email { get; set; }

        public Guid ImageId { get; set; }

        public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

        // Đổi từ Aspnetuser → User (đúng với entity bạn dùng trong Identity)
        public virtual Aspnetuser User { get; set; } = null!;

        // ✅ Thêm property để Razor view có thể hiển thị định dạng ngày đẹp
        public string FormattedEnrollmentDate => EnrollmentDate?.ToString("dd/MM/yyyy") ?? "Chưa nhập";
    }
}
