using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_Management_System.Models.ENTITIES
{
    [Table("StudentDetails")]
    public class StudentDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentDetailsId { get; set; }

        // Foreign key to Students table
        [Required]
        public int StudentId { get; set; } // NOT nullable now

        [ForeignKey("StudentId")]
        public Students? Student { get; set; } // Navigation property optional

        // Personal Information
        [StringLength(50)]
        public string? RegistrationNumber { get; set; } // Optional, readonly in view
        public string? Email { get; set; }
        [Required, StringLength(50)]
        public string FirstName { get; set; } = null!;

        [StringLength(50)]
        public string? MiddleName { get; set; } // Optional

        [Required, StringLength(50)]
        public string LastName { get; set; } = null!;

        [Required, StringLength(100)]
        public string FathersName { get; set; } = null!;

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }


        [StringLength(20)]
        public string? NIC { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }

        [StringLength(5)]
        public string? BloodGroup { get; set; }

        [StringLength(50)]
        public string? Religion { get; set; }

        public bool HasDomicile { get; set; }

        // Father's Information
        [StringLength(20)]
        public string? FathersCNIC { get; set; }

        [StringLength(50)]
        public string? FathersOccupation { get; set; }

        [StringLength(100)]
        public string? FathersOrganization { get; set; }

        [StringLength(20)]
        public string? OrganizationPhone1 { get; set; }

        [EmailAddress]
        public string? FathersEmail { get; set; }

        [Column(TypeName = "bytea")]
        public byte[]? ProfilePicture { get; set; }
    }
}