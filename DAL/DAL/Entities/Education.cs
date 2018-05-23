using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Education
    {
        //[ScaffoldColumn(false)]
        public int Id { get; set; }

        public Student Student { get; set; }
        [Required]
        public int IdStudent { get; set; }

        public Subject Subject { get; set; }
        [Required]
        public int IdSubject { get; set; }

        [Display(Name = "Оцінка")]
        [Range(0, 100, ErrorMessage = "Недопустиме значення")]
        public int SubjectResult { get; set; }
    }
}
