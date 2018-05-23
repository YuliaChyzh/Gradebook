using System.ComponentModel.DataAnnotations;


namespace DAL.Entities
{
    public class Subject
    {
        //[ScaffoldColumn(false)]
        public int Id { get; set; }

        [MaxLength(256)]
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Назва предмету")]
        public string Name { get; set; }

        [Display(Name = "Середній бал по предмету")]
        //[Range(typeof(decimal), "0.00", "99.99")]
        public double SubjectAvg { get; set; }             //int/decimal ???
    }
}
