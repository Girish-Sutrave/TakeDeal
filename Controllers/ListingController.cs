using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TakeDeal.DTOs;
using TakeDeal.Models;

namespace TakeDeal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ListingController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ListingController(AppDbContext context)
        {
            this._context = context;
        }

        [HttpGet("GetAllListing")]
        public async Task<IActionResult> GetAllListing()
        {
            var listings = await _context.Listings
            .Select(l => new
            {
                l.Id,
                l.Title,
                l.Price,
                l.City,
                l.Description,
                CategoryName = l.Category.Name,
                UserName = l.User.Name
            })
            .ToListAsync();
            return Ok(listings);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(ListingDto listingDto)
        {
            var userId = User.FindFirst("id")?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            var listing = new Listing
            {
                Title = listingDto.Title,
                Description = listingDto.Description,
                Price = listingDto.Price,
                City = listingDto.City,
                State = listingDto.State,

                CategoryId = listingDto.CategoryId,
                UserId = int.Parse(userId)
            };
            _context.Listings.Add(listing);
            await  _context.SaveChangesAsync();
            return Ok();
        }
    }
}
