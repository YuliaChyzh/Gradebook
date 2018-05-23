using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Student
    {
        //[ScaffoldColumn(false)]
        public int Id { get; set; }

        [MaxLength(256)]
        [Required]
        public string Name { get; set; }

        public Group Group { get; set; }
        [Required]
        public int IdGroup { get; set; }

        [Display(Name = "Середній бал студента")]
        //[Range(typeof(decimal), "0.00", "99.99")]
        public double StudentAvg { get; set; }         //int/decimal ???
    }
}
