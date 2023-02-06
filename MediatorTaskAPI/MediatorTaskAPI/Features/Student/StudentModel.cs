using MediatorTaskAPI.Features.House;
using System.ComponentModel.DataAnnotations;

namespace MediatorTaskAPI.Features.Student
{
    public class StudentModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]      
        public int Class { get; set; }
        [Required]
        public int HouseId { get; set; }
    }
}
