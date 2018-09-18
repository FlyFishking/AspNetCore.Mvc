using Microsoft.EFCore.Infrustructure;
using System;
using System.Linq;
using WebApplication3.DBContext.Models;

namespace WebApplication3.Service
{
    public interface IStudentService
    {
        IQueryable<Student> GetQuery();
        Student Get(int id);
        void Insert(Student stdu);
        void Update(Student stdu);
        void Delete(int id);
    }
    public class StudentService : IStudentService
    {
        private readonly IRepository<Student> repoStudent;
        public StudentService(IRepository<Student> student, IRepository<Enrollment> enrollment)
        {
            repoStudent = student;
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
