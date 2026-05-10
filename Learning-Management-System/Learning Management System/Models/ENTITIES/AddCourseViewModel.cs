using Learning_Management_System.Models.ENTITIES;
using System.Collections.Generic;

namespace Learning_Management_System.Models.ViewModels
{
    public class AddCourseViewModel
    {
        public Department Department { get; set; } = null!;
        public List<Courses> AvailableCourses { get; set; } = new List<Courses>();
        public int SelectedCourseId { get; set; }
    }
}
