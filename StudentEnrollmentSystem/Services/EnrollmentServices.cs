using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentSystem.Models;
using StudentEnrollmentSystem.Enums;
using StudentEnrollmentSystem.Interfaces;

namespace StudentEnrollmentSystem.Services
{
    public class EnrollmentServices : IEnrollmentServices
    {
        private IUnitOfWork _unitOfWork;

        public EnrollmentServices(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Enrollment>> GetAllEnrollments()
        {
            var query = await _unitOfWork.EnrollmentRepo.GetMany(
                (en => en.Student.IsDeleted == false && en.Course.IsDeleted == false));
            return query;
        }

        public async Task<Enrollment> GetEnrollment(long id)
        {
            var enrollment = await _unitOfWork.EnrollmentRepo.GetById(id);
            if (enrollment == null)
            {
                throw new NotFoundException("No such enrollment!");
            }
            var student = await _unitOfWork.StudentRepo.GetById(enrollment.StudentId);
            if (student == null || student.IsDeleted)
            {
                throw new ProblemException("Student profile is deleted, this enrollment is no longer valid.");
            }
            var course = await _unitOfWork.CourseRepo.GetById(enrollment.CourseId);
            if (course == null || course.IsDeleted)
            {
                throw new ProblemException("Course is deleted, this enrollment is no longer valid.");
            }

            return enrollment;
        }

        public async Task ApproveEnrollment(long id)
        {
            var enrollment = await _unitOfWork.EnrollmentRepo.GetById(id);
            if (enrollment == null)
            {
                throw new NotFoundException("No such enrollment!");
            }
            var student = await _unitOfWork.StudentRepo.GetById(enrollment.StudentId);
            if (student == null || student.IsDeleted)
            {
                throw new ProblemException("Student profile is deleted, this enrollment is no longer valid.");
            }
            var course = await _unitOfWork.CourseRepo.GetById(enrollment.CourseId);
            if (course == null || course.IsDeleted)
            {
                throw new ProblemException("Course is deleted, this enrollment is no longer valid.");
            }
            if (enrollment.Status == EnrollmentStatus.Withdrawn)
            {
                throw new ProblemException("Student has withdrawn enrollment, so this enrollment cannot be approved.");
            }
            enrollment.Status = EnrollmentStatus.Approved;
            await _unitOfWork.Commit();
        }

        public async Task RejectEnrollment(long id)
        {
            var enrollment = await _unitOfWork.EnrollmentRepo.GetById(id);
            if (enrollment == null)
            {
                throw new NotFoundException("No such enrollment!");
            }
            var student = await _unitOfWork.StudentRepo.GetById(enrollment.StudentId);
            if (student == null || student.IsDeleted)
            {
                throw new ProblemException("Student profile is deleted, this enrollment is no longer valid.");
            }
            var course = await _unitOfWork.CourseRepo.GetById(enrollment.CourseId);
            if (course == null || course.IsDeleted)
            {
                throw new ProblemException("Course is deleted, this enrollment is no longer valid.");
            }
            if (enrollment.Status == EnrollmentStatus.Withdrawn)
            {
                throw new ProblemException("Student has withdrawn enrollment, so this enrollment cannot be rejected.");
            }
            enrollment.Status = EnrollmentStatus.Rejected;
            await _unitOfWork.Commit();
        }

        public async Task HardDeleteEnrollment(long id)
        {
            var enrollment = await _unitOfWork.EnrollmentRepo.GetById(id);
            if (enrollment == null)
            {
                throw new NotFoundException("No such enrollment!");
            }

            await _unitOfWork.EnrollmentRepo.Delete(enrollment);
            await _unitOfWork.Commit();
        }
    }
}
