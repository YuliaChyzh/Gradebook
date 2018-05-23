using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Group
    {
        //[ScaffoldColumn(false)]
        public int Id { get; set; }

        [MaxLength(256)]
        [Required]
        [Display(Name = "Група")]
        public string Name { get; set; }
    }
}
