using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_Management_System.Models.ENTITIES
{
    [Table("Department")]
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartmentId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = null!;

        // Navigation properties
        public ICollection<Students> Students { get; set; } = new List<Students>();
        public ICollection<Faculty> Faculty { get; set; } = new List<Faculty>();
        public ICollection<Courses> Courses { get; set; } = new List<Courses>();
    }
}
