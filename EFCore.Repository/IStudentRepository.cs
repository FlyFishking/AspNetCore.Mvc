using System.Collections.Generic;
using System.Linq;
using EFCore.Model.Model;
using Microsoft.EFCore.Infrustructure;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Repository
{
    public partial interface IStudentRepository : IRepository<Student>
    {
        List<Student> GetAllStudent();
    }

    public partial class StudentRepository : RepositoryBase<Student>, IStudentRepository
    {
        public StudentRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public List<Student> GetAllStudent()
        {
            return GetAll().ToList();
        }
    }
}