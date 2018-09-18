using Microsoft.EFCore.Infrustructure;
using System;
using System.Collections.Generic;

namespace WebApplication3.DBContext.Models
{
    public class Student : EntityBaseWithDynamic
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}