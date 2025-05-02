using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SashoApp.Models.Cars
{
    public class CreateCarViewModel
    {
        [Key]

        public int Id { get; set; }

        [Required]
        public string Make { get; set; }

        [Required]
        [StringLength(50)]
        public string Model { get; set; }
        [Required]
        [Range(1900, 2100)]
        public int Year { get; set; }
        [Required]
        [StringLength(30)]
        public string Color { get; set; }

        [Required]
        [StringLength(20)]
        public string FuelType { get; set; }

        [Required]
        [StringLength(20)]
        public string Transmission { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [Required]
        [StringLength(1000)]
        public string Description { get; set; }
        [Required]
        [StringLength(50)]
        public string EngineType { get; set; }
        [Required]
        public int NumberOfDoors { get; set; }
        [Required]
        public int HorsePower { get; set; }

        public byte[] ImageFile { get; set; }
        // This property is used to hold the existing image data when editing a car


    }
}
