using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lms.Data.Data;
using Lms.Core.Entities;
using Lms.Data.Repositories;
using Lms.Core.Repositories;

namespace Lms.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        //private readonly LmsApiContext _context;
        private readonly IUnitOfWork uow;

        public GamesController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        //// GET: api/Games
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Game>>> GetGame()
        //{
        //  if (_context.Game == null)
        //  {
        //      return NotFound();
        //  }
        //    return await _context.Game.ToListAsync();
        //}

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGame()
        {
            var games = await uow.GameRepository.GetAllAsync();
            return games.ToList();
           
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
          
            var game = await uow.GameRepository.GetAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            return game;
        }

        //// PUT: api/Games/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutGame(int id, Game game)
        //{
        //    if (id != game.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(game).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!GameExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}



        // PUT: api/Games/5        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, Game game)
        {
            if (id != game.Id)
            {
                return BadRequest();
            }                      

            try
            {
                uow.GameRepository.Update(game);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }



        //// POST: api/Games
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Game>> PostGame(Game game)
        //{
        //  if (_context.Game == null)
        //  {
        //      return Problem("Entity set 'LmsApiContext.Game'  is null.");
        //  }
        //    _context.Game.Add(game);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetGame", new { id = game.Id }, game);
        //}


        // POST: api/Games       
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
            
            uow.GameRepository.Add(game);            

            return CreatedAtAction("GetGame", new { id = game.Id }, game);
        }


        //// DELETE: api/Games/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteGame(int id)
        //{
        //    if (_context.Game == null)
        //    {
        //        return NotFound();
        //    }
        //    var game = await _context.Game.FindAsync(id);
        //    if (game == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Game.Remove(game);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}


        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
           
            var game = await uow.GameRepository.GetAsync(id);
            
            if (game == null)
            {
                return NotFound();
            }

            uow.GameRepository.Remove(game);           

            return NoContent();
        }

        //private bool GameExists(int id)
        //{
        //    return (_context.Game?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
