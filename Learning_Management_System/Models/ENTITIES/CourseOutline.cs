using System.ComponentModel.DataAnnotations;

namespace Learning_Management_System.Models.ENTITIES
{
    public class CourseOutline
    {

        [Key]
            public int CourseOutlineId { get; set; }
        [Required]
            public int WeekNumber { get; set; }
        [Required]
        public string WeeklyPlan { get; set; }
        [Required]
        public string Topic { get; set; }
        [Required]
        public string Description { get; set; }

            // Only Course (because outline belongs to course, not a student)
            public int CourseId { get; set; }
         
        }

    }

