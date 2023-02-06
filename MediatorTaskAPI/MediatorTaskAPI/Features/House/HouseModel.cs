using MediatorTaskAPI.Features.Student;
using MediatorTaskAPI.Features.Student;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MediatorTaskAPI.Features.House
{
    public class HouseModel
    {
        [Key]
        [DisplayName("HouseId")]
        public int Id { get; set; }

        [Required]
        public string HouseName { get; set;}
    }
}
