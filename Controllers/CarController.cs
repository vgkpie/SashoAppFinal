﻿using SashoApp.Data;
using SashoApp.Models.Cars;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SashoApp.Controllers
{
    public class CarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cars
        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            ViewData["MakeSortParm"] = string.IsNullOrEmpty(sortOrder) ? "make_desc" : "";
            ViewData["ModelSortParm"] = sortOrder == "model_asc" ? "model_desc" : "model_asc";
            ViewData["YearSortParm"] = sortOrder == "year_asc" ? "year_desc" : "year_asc";
            ViewData["PriceSortParm"] = sortOrder == "price_asc" ? "price_desc" : "price_asc";

            var cars = from c in _context.Cars
                       select c;

            if (!string.IsNullOrEmpty(searchString))
            {
                cars = cars.Where(c => c.Make.Contains(searchString) || c.Model.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "make_desc":
                    cars = cars.OrderByDescending(c => c.Make);
                    break;
                case "model_asc":
                    cars = cars.OrderBy(c => c.Model);
                    break;
                case "model_desc":
                    cars = cars.OrderByDescending(c => c.Model);
                    break;
                case "price_asc":
                    cars = cars.OrderBy(c => c.Price);
                    break;
                case "price_desc":
                    cars = cars.OrderByDescending(c => c.Price);
                    break;
                case "year_asc":
                    cars = cars.OrderBy(c => c.Year);
                    break;
                case "year_desc":
                    cars = cars.OrderByDescending(c => c.Year);
                    break;
                default:
                    cars = cars.OrderBy(c => c.Make);
                    break;
            }

            return View(await cars.ToListAsync());
        }

        // GET: Car/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // GET: Car/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View(new CreateCarViewModel());
        }

        // POST: Car/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateCarViewModel carCreateModel, IFormFile imageFile)
        {
            
            if(imageFile != null && imageFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(memoryStream);
                    carCreateModel.ImageFile = memoryStream.ToArray();
                }
            }

            Car car = new Car
            {

                Make = carCreateModel.Make,
                Model = carCreateModel.Model,
                Year = carCreateModel.Year,
                Color = carCreateModel.Color,
                FuelType = carCreateModel.FuelType,
                Transmission = carCreateModel.Transmission,
                Price = carCreateModel.Price,
                Description = carCreateModel.Description,
                EngineType = carCreateModel.EngineType,
                NumberOfDoors = carCreateModel.NumberOfDoors,
                HorsePower = carCreateModel.HorsePower,
                ImageData = carCreateModel.ImageFile
            };
            if (imageFile != null && imageFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(memoryStream);
                    car.ImageData = memoryStream.ToArray();
                }
            }

            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: Car/Edit/5
[Authorize(Roles = "Admin")]
public async Task<IActionResult> Edit(int id)
{
    

    var car = await _context.Cars.FindAsync(id);
    if (car == null)
    {
        return NotFound();
    }

    var carModel = new EditCarViewModel
    {
        Id = car.Id,
        Make = car.Make,
        Model = car.Model,
        Year = car.Year,
        Color = car.Color,
        FuelType = car.FuelType,
        Transmission = car.Transmission,
        Price = car.Price,
        Description = car.Description,
        EngineType = car.EngineType,
        NumberOfDoors = car.NumberOfDoors,
        HorsePower = car.HorsePower,
        ImageFile = car.ImageData
    };

    return View(carModel);
}

// POST: Car/Edit/5
[HttpPost]
[ValidateAntiForgeryToken]
[Authorize(Roles = "Admin")]
public async Task<IActionResult> Edit(int id, CreateCarViewModel carEditModel, IFormFile imageFile)
{
    
   

    {
        Car car =new Car
        {
        Id = carEditModel.Id,
        Make = carEditModel.Make,
        Model = carEditModel.Model,
        Year = carEditModel.Year,
        Color = carEditModel.Color,
        FuelType = carEditModel.FuelType,
        Transmission = carEditModel.Transmission,
        Price = carEditModel.Price,
        Description = carEditModel.Description,
        EngineType = carEditModel.EngineType,
        NumberOfDoors = carEditModel.NumberOfDoors,
        HorsePower = carEditModel.HorsePower,
        ImageData = carEditModel.ImageFile
        };
         if (imageFile != null)
    {
        using (var memoryStream = new MemoryStream())
        {
            await imageFile.CopyToAsync(memoryStream);
            car.ImageData = memoryStream.ToArray();
        }
    }
        _context.Update(car);
        await _context.SaveChangesAsync();
    }
     return RedirectToAction(nameof(Index));
}

// GET: Car/Delete/5
[Authorize(Roles = "Admin")]
public async Task<IActionResult> Delete(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var car = await _context.Cars
        .FirstOrDefaultAsync(m => m.Id == id);
    if (car == null)
    {
        return NotFound();
    }

    return View(car);
}

// POST: Car/Delete/5
[HttpPost, ActionName("Delete")]
[ValidateAntiForgeryToken]
[Authorize(Roles = "Admin")]
public async Task<IActionResult> DeleteConfirmed(int id)
{
    var car = await _context.Cars.FindAsync(id);
    if (car != null)
    {
        _context.Cars.Remove(car);
        await _context.SaveChangesAsync();
    }
    return RedirectToAction(nameof(Index));
}

    }
    
}     