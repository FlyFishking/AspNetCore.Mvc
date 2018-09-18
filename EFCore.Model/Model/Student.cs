using System;
using System.Collections.Generic;
using Microsoft.EFCore.Infrustructure;

namespace EFCore.Model.Model
{
    public class Student : EntityBaseWithDynamic
    {
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}