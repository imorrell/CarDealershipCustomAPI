using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarDealershipCapstone.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarDealershipCapstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CarDbController : ControllerBase
    {
        private readonly CarsDbContext _context;

        public CarDbController(CarsDbContext context)
        {
            _context = context;
        }

        //Get: api/Car
        [HttpGet]
        public async Task<ActionResult<List<Car>>> GetCars()
        {
            return await _context.Car.ToListAsync();
        }

        //Get: api/Car/1
        [HttpGet("{Id}")]
        public async Task<ActionResult<Car>> GetCarById(int id)
        {
            var car = await _context.Car.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return car;
        }

        // GET api/Car/search?make=audi
        [HttpGet("search")]
        public async Task<ActionResult<List<Car>>> GetCarByMake(string? make, string? model, string? year, string? color)
        {

            return Ok(await _context.Car.Where(x => x.Make.Contains(make) 
           || x.Model.Contains(model) || x.Year.Contains(year) || x.Color.Contains(color)).ToListAsync());
        }

        //POST: API/Company
        [HttpPost]

        public async Task<ActionResult<Car>> AddCar(Car car)
        {
            _context.Car.Add(car);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCarById), new { id = car.Id }, car);

        }

        //PUT: api/Car/{id}
        [HttpPut("{Id}")]
        public async Task<ActionResult> PutCar(int id, Car car)
        {
            if (id != car.Id)
            {
                return BadRequest();
            }

            _context.Entry(car).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return NoContent();
            //response 204 (No content) - requires the client to send an entirely updated entity and not just the chanes
            //to support partial updates, we would use HTTP Patch
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCar(int Id)
        {
            var car = await _context.Car.FindAsync(Id);
            if (car == null)
            {
                return NotFound();
            }

            _context.Car.Remove(car);

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}