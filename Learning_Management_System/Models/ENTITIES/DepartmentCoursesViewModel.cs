using System.Collections.Generic;
using Learning_Management_System.Models.ENTITIES;

namespace Learning_Management_System.Models.ViewModels
{
    public class DepartmentCoursesViewModel
    {
        public Department Department { get; set; } = null!;
        public List<CourseInfo> Courses { get; set; } = new List<CourseInfo>();
    }

    public class CourseInfo
    {
        public Courses Course { get; set; } = null!;
        public int StudentsCount { get; set; }
    }
}
