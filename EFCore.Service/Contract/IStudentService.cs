using System.Collections.Generic;
using EFCore.Model.Model;
using System.Linq;

namespace EFCore.Service.Contract
{
    public interface IStudentService
    {
        List<Student> GetAllStudents();
        IQueryable<Student> GetQuery();
        Student Get(int id);
        void Insert(Student stdu);
        void Update(Student stdu);
        void Delete(int id);
    }
}