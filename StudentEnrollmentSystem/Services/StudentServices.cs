using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentSystem.Enums;
using StudentEnrollmentSystem.Interfaces;
using StudentEnrollmentSystem.Models;
using StudentEnrollmentSystem.Repositories;

namespace StudentEnrollmentSystem.Services
{
    public class StudentServices : IStudentServices
    {
        //private readonly StudentEnrollContext _context;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public StudentServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StudentDTO>> GetAllStudents()
        {
            var query = await _unitOfWork.StudentRepo.GetMany((x => x.IsDeleted == false),
                x => x.Enrollments);

            var output = query.ToList().Select(x => ItemToDTO(x));
            return output;
            /*var ans = output.Where(x => x.IsDeleted == false)
                .Include(x => x.Enrollments)
                .ThenInclude(x => x.Course)
                .Select(x => ItemToDTO(x))
                .ToListAsync();*/
        }
        
        public async Task<StudentDTO> GetStudent(long id)
        {
            var student = await _unitOfWork.StudentRepo.GetById(id);

            if (student == null || student.IsDeleted)
            {
                throw new NotFoundException("No such student!");
            }

            var withEnrollments = await _unitOfWork.StudentRepo.GetOneWithCondition(
                (x => x.IsDeleted == false && x.Id == id),
                x => x.Enrollments);

            /*var withEnrollments = await _context.Students
                .Where(st => st.Id == id)
                .Include(x => x.Enrollments)
                .ThenInclude(x => x.Course)
                .FirstOrDefaultAsync();*/

            return ItemToDTO(withEnrollments);
        }

        public async Task<IEnumerable<Enrollment>> GetSelfEnrollments(long studentId)
        {
            var student = await _unitOfWork.StudentRepo.GetById(studentId);

            if (student == null || student.IsDeleted)
            {
                throw new NotFoundException("No such student!");
            }

            var enrollments = await _unitOfWork.EnrollmentRepo.GetMany(
                (en => en.StudentId == studentId), en => en.Course);

            /*var enrollments = await _context.Enrollments
                .Where(en => en.StudentId == studentId)
                .Include(en => en.Course)
                .ToListAsync();*/

            return enrollments;
        }

        public async Task UpdateStudent(long id, StudentDTO studentDTO)
        {
            if (id != studentDTO.Id)
            {
                throw new BadRequestException("Id does not match!");
            }

            var OriginalStudent = await _unitOfWork.StudentRepo.GetById(id);
            if (OriginalStudent == null || OriginalStudent.IsDeleted)
            {
                throw new NotFoundException("No such student!");
            }
            if (!Enum.IsDefined(typeof(Genders), studentDTO.Gender))
            {
                throw new BadRequestException("No such gender!");
            }
            OriginalStudent.Name = studentDTO.Name;
            OriginalStudent.Age = studentDTO.Age;
            OriginalStudent.Gender = studentDTO.Gender;
            OriginalStudent.UpdatedDate = DateTime.Now;
            //_context.Entry(OriginalStudent).State = EntityState.Modified;
            await _unitOfWork.StudentRepo.Update(OriginalStudent);

            await _unitOfWork.Commit();
        }

        public async Task<StudentDTO> CreateStudentProfile(StudentDTO studentDTO)
        {
            if (!Enum.IsDefined(typeof(Genders), studentDTO.Gender))
            {
                throw new BadRequestException("No such gender!");
            }
            var student = new Student
            {
                Name = studentDTO.Name,
                Age = studentDTO.Age,
                Gender = studentDTO.Gender,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                IsDeleted = false,
                Enrollments = new List<Enrollment>()
            };
            await _unitOfWork.StudentRepo.Add(student);
            await _unitOfWork.Commit();

            return ItemToDTO(student);
        }

        public async Task<Enrollment> EnrollCourse(long studentId, long courseId)
        {
            var student = await _unitOfWork.StudentRepo.GetById(studentId);
            var course = await _unitOfWork.CourseRepo.GetById(courseId);
            if (student == null || student.IsDeleted || course == null || course.IsDeleted)
            {
                throw new NotFoundException("Student or Course does not exist!");
            }
            //Check if this enrollment already exists
            var enrollment = await _unitOfWork.EnrollmentRepo.GetOneWithCondition(
                (en => (en.StudentId == studentId && en.CourseId == courseId)));
            /*var enrollment = await _context.Enrollments.
                Where(en => (en.StudentId == studentId && en.CourseId == courseId))
                .FirstOrDefaultAsync();*/
            if (enrollment != null) 
            {
                throw new ProblemException("A student cannot enroll to the same course more than once!");
            }

            enrollment = new Enrollment
            {
                Student = student,
                Course = course,
                Status = EnrollmentStatus.Pending_Approval
            };
            student.Enrollments.Add(enrollment);
            await _unitOfWork.StudentRepo.Update(student);
            await _unitOfWork.Commit();
            return await _unitOfWork.EnrollmentRepo.GetOneWithCondition(
                (en => (en.StudentId == studentId && en.CourseId == courseId))); //return enrollment;
        }

        public async Task<Enrollment> WithdrawCourse(long studentId, long courseId)
        {
            var student = await _unitOfWork.StudentRepo.GetById(studentId);
            var course = await _unitOfWork.CourseRepo.GetById(courseId);
            if (student == null || course == null)
            {
                throw new NotFoundException("Student or Course does not exist!");
            }
            var enrollment = await _unitOfWork.EnrollmentRepo.GetOneWithCondition(
                (en => (en.StudentId == studentId && en.CourseId == courseId)));
            if (enrollment == null)
            {
                throw new NotFoundException("This course enrollment does not exist!");
            }
            enrollment.Status = EnrollmentStatus.Withdrawn;
            await _unitOfWork.EnrollmentRepo.Update(enrollment);
            await _unitOfWork.Commit();
            return await _unitOfWork.EnrollmentRepo.GetOneWithCondition(
                (en => (en.StudentId == studentId && en.CourseId == courseId)));
        }

        public async Task SoftDeleteProfile(long id)
        {
            var student = await _unitOfWork.StudentRepo.GetById(id);
            if (student == null)
            {
                throw new NotFoundException();
            }

            student.IsDeleted = true;
            await _unitOfWork.StudentRepo.Update(student);
            await _unitOfWork.Commit();

        }

        public async Task HardDeleteProfile(long id)
        {
            var student = await _unitOfWork.StudentRepo.GetById(id);
            if (student == null)
            {
                throw new NotFoundException();
            }

            await _unitOfWork.StudentRepo.Delete(student);
            await _unitOfWork.Commit();
        }       

        public StudentDTO ItemToDTO(Student student)
        {
            var output = _mapper.Map<StudentDTO>(student);
            return output;
        }
    }
}
