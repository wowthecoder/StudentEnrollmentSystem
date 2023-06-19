using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using StudentEnrollmentSystem.Enums;

namespace StudentEnrollmentSystem.Models
{
    public class Student
    {
        public long Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public Genders Gender { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set;}
        public bool IsDeleted { get; set; }

        public List<Enrollment> Enrollments { get; set; } = new();

    }

    public class StudentDTO
    {
        public long Id { get; set;}
        public string? Name { get; set; }
        public int Age { get; set; }
        public Genders Gender { get; set;}
        public List<Enrollment> Enrollments { get; set;} = new();
    }

}
