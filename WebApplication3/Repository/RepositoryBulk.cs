using Microsoft.EntityFrameworkCore;
using WebApplication3.Componet;
using WebApplication3.DBContext.Models;

namespace WebApplication3.Repository
{
    public partial interface IStudentRepository : IRepository<Student>
    {
    }

    public partial class StudentRepository : RepositoryBase<Student>, IStudentRepository
    {
        public StudentRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
    public partial interface IEnrollmentRepository : IRepository<Enrollment>
    {
    }

    public partial class EnrollmentRepository : RepositoryBase<Enrollment>, IEnrollmentRepository
    {
        public EnrollmentRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}