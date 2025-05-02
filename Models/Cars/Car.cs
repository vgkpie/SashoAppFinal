﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SashoApp.Models.Cars
{
    public class Car
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Make { get; set; } // Марка (например BMW)

        [Required]
        [StringLength(50)]
        public string Model { get; set; } // Модел (например 320d)

        [Range(1900, 2100)]
        public int Year { get; set; } // Година на производство

        [StringLength(30)]
        public string Color { get; set; } // Цвят

      

        [Required]
        [StringLength(20)]
        public string FuelType { get; set; } // Тип гориво

        [Required]
        [StringLength(20)]
        public string Transmission { get; set; } // Скоростна кутия

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } // Цена

        [StringLength(1000)]
        public string Description { get; set; } // Описание на колата

        public byte[] ImageData { get; set; } // Real image data stored as byte array



        [StringLength(50)]
        public string EngineType { get; set; } // Тип двигател

        public int NumberOfDoors { get; set; } // Брой врати

        public int HorsePower { get; set; } // Мощност на двигателя


    }
}
