using Microsoft.EFCore.Infrustructure;

namespace EFCore.Model.Model
{
    public enum Grade
    {
        A, B, C, D, F
    }

    public class Enrollment : EntityBase
    {
        public int EnrollmentID { get; set; }
        public int CourseID { get; set; }
        public int StudentID { get; set; }
        public Grade? Grade { get; set; }

        public Course Course { get; set; }
        public Student Student { get; set; }
    }
}
