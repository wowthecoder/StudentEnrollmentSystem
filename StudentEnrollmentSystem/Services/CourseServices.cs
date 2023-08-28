using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentSystem.Models;
using StudentEnrollmentSystem.Enums;
using StudentEnrollmentSystem.Interfaces;

namespace StudentEnrollmentSystem.Services
{
    public class CourseServices : ICourseServices
    {
        private IUnitOfWork _unitOfWork;

        public CourseServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Course>> GetAllCourses()
        {
            var query = await _unitOfWork.CourseRepo.GetMany((x => x.IsDeleted == false),
                x => x.Enrollments);
            return query;
        }

        public async Task<Course> GetCourse(long id)
        {
            var course = await _unitOfWork.CourseRepo.GetById(id);

            if (course == null || course.IsDeleted)
            {
                throw new NotFoundException("Either course does not exist, or course is deleted.");
            }

            return course;
        }

        public async Task UpdateCourse(long id, Course course)
        {
            if (id != course.Id)
            {
                throw new BadRequestException("Id does not match!");
            }

            var OriginalCourse = await _unitOfWork.CourseRepo.GetById(id);
            if (OriginalCourse == null)
            {
                throw new NotFoundException("No such course!");
            }
            OriginalCourse.Name = course.Name;
            OriginalCourse.NumOfStudents = course.NumOfStudents;
            OriginalCourse.LecturerName = course.LecturerName;
            OriginalCourse.UpdatedDate = DateTime.UtcNow;
            await _unitOfWork.CourseRepo.Update(OriginalCourse);

            await _unitOfWork.Commit();
        }

        public async Task<Course> CreateCourse(Course course)
        {
            course.CreatedDate = DateTime.Now;
            course.UpdatedDate = DateTime.Now;
            course.IsDeleted = false;
            course.Enrollments = new List<Enrollment>();
            await _unitOfWork.CourseRepo.Add(course);
            await _unitOfWork.Commit();

            return course;
        }

        public async Task SoftDeleteCourse(long id)
        {
            var course = await _unitOfWork.CourseRepo.GetById(id);
            if (course == null)
            {
                throw new NotFoundException("No such course!");
            }
            course.IsDeleted = true;
            await _unitOfWork.CourseRepo.Update(course);
            await _unitOfWork.Commit();
        }

        public async Task HardDeleteCourse(long id)
        {
            var course = await _unitOfWork.CourseRepo.GetById(id);
            if (course == null)
            {
                throw new NotFoundException("No such course!");
            }

            await _unitOfWork.CourseRepo.Delete(course);
            await _unitOfWork.Commit();
        }
    }
}
