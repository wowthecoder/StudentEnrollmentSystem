﻿using Microsoft.AspNetCore.Mvc;
using StudentEnrollmentSystem.Models;

namespace StudentEnrollmentSystem.Services
{
    public interface IStudentServices 
    {
        public Task<IEnumerable<StudentDTO>> GetAllStudents();
        public Task<StudentDTO> GetStudent(long id);
        public Task<IEnumerable<Enrollment>> GetSelfEnrollments(long id);
        public Task UpdateStudent(long id, StudentDTO studentDTO);
        public Task<StudentDTO> CreateStudentProfile(StudentDTO studentDTO);
        public Task<StudentDTO> EnrollCourse(long StudentId, long CourseId);
        public Task<StudentDTO> WithdrawCourse(long StudentId, long CourseId);
        public Task SoftDeleteProfile(long id);
        public Task HardDeleteProfile(long id);

    }

    public interface ICourseServices
    {
        public Task<IEnumerable<Course>> GetAllCourses();
        public Task<Course> GetCourse(long id);
        public Task UpdateCourse(long id, Course course);
        public Task<Course> CreateCourse(Course course);
        public Task SoftDeleteCourse(long id);
        public Task HardDeleteCourse(long id);
    }

    public interface IEnrollmentServices
    {
        public Task<IEnumerable<Enrollment>> GetAllEnrollments();
        public Task<Enrollment> GetEnrollment(long id);
        public Task ApproveEnrollment(long id);
        public Task RejectEnrollment(long id);
        public Task HardDeleteEnrollment(long id);
    }
}