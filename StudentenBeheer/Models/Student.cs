using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentenBeheer.Models
{
    [Authorize(Roles = "Admin")]
    public class Student
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Voornaam")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Achternaam")]
        public string Lastname { get; set; }
        [Required]
        [Display(Name = "Geboortedatum")]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }
        [ForeignKey("Gender")]
        public char GenderId { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? Deleted { get; set; } = DateTime.MaxValue;
    }

    public class StudentsIndexViewModel
    {
        public string? NameFilter { get; set; }
        public char GenderFilter { get; set; }
        public List<Student>? FilteredStudents { get; set; }
        public SelectList? ListGenders { get; set; }
    }
}
