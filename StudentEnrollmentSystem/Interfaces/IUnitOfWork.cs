using StudentEnrollmentSystem.Models;
using StudentEnrollmentSystem.Repositories;

namespace StudentEnrollmentSystem.Interfaces
{
    public interface IUnitOfWork
    {
        public GenericRepository<Student> StudentRepo { get; }
        public GenericRepository<Course> CourseRepo { get; }
        public GenericRepository<Enrollment> EnrollmentRepo { get; }
        public Task Commit();
        public Task Rollback();
    }
}
