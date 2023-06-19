using Microsoft.EntityFrameworkCore;
using StudentEnrollmentSystem.Interfaces;
using StudentEnrollmentSystem.Models;
using System.Diagnostics.Eventing.Reader;

namespace StudentEnrollmentSystem.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StudentEnrollContext _context;
        private GenericRepository<Student> _studentRepo;
        private GenericRepository<Course> _courseRepo;
        private GenericRepository<Enrollment> _enrollmentRepo;

        public UnitOfWork(StudentEnrollContext context) 
        {
            _context = context;
        }

        public GenericRepository<Student> StudentRepo
        {
            get 
            {
                if (_studentRepo == null)
                {
                    _studentRepo = new GenericRepository<Student>(_context);
                }
                return _studentRepo; 
            }    
        }

        public GenericRepository<Course> CourseRepo
        {
            get
            {
                if (_courseRepo == null)
                {
                    _courseRepo = new GenericRepository<Course>(_context);
                }
                return _courseRepo;
            }
        }

        public GenericRepository<Enrollment> EnrollmentRepo
        {
            get
            {
                if (_enrollmentRepo == null)
                {
                    _enrollmentRepo = new GenericRepository<Enrollment>(_context);
                }
                return _enrollmentRepo;
            }
        }

        public async Task Commit()
        {
           await _context.SaveChangesAsync();
        }

        public async Task Rollback()
        {
            await _context.Database.CurrentTransaction.RollbackAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing) 
            {
                _context.Dispose();
            }
        }
    }
}
