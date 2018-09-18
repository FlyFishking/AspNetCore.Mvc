using System;
using System.Collections.Generic;
using System.Linq;
using EFCore.Model.Model;
using EFCore.Repository;
using EFCore.Service.Contract;
using Microsoft.EFCore.Infrustructure;

namespace EFCore.Service.Implement
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository repoStudent;
        public IValuesService svc { get; set; }
        public StudentService(IStudentRepository student)
        {
            repoStudent = student;
        }

        public List<Student> GetAllStudents()
        {
            return repoStudent.GetAllStudent();
        }

        public IQueryable<Student> GetQuery()
        {
            return repoStudent.GetQuery();
        }

        public Student Get(int id)
        {
            return repoStudent.GetByKey(id);
        }

        public void Insert(Student stdu)
        {
            stdu = new Student()
            {
                LastName = "LLL",
                EnrollmentDate = DateTime.Now,
                FirstMidName = "mmm"
            };
            repoStudent.Insert(stdu);
        }

        public void Update(Student stdu)
        {
            repoStudent.Update(stdu);
        }

        public void Delete(int id)
        {
            repoStudent.Delete(t => t.ID == id);
        }
    }
}
