using StudentEnrollmentSystem.Enums;
using System.ComponentModel.DataAnnotations;

namespace StudentEnrollmentSystem.Models
{
    public class Enrollment
    {
        public long Id { get; set; }
        //foreign keys
        public long StudentId { get; set; }
        public long CourseId { get; set; }
        
        public Student Student { get; set; }
        public Course Course { get; set; }
        public EnrollmentStatus Status { get; set; }
    }
}
