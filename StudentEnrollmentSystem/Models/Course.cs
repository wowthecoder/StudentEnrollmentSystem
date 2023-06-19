using System.ComponentModel.DataAnnotations;

namespace StudentEnrollmentSystem.Models
{
    public class Course
    {
        public long Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public int NumOfStudents { get; set; }
        public string? LecturerName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public List<Enrollment> Enrollments { get; set; } = new();
    }
}
