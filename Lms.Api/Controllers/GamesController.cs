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
using AutoMapper;
using Lms.Core.Dto;

namespace Lms.Api.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    [Route("[controller]")]
    public class GamesController : ControllerBase
    {
        //private readonly LmsApiContext _context;
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GamesController(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
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
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGame()
        {
            var games = await uow.GameRepository.GetAllAsync();
            
            if (games == null) return NotFound();

            var gameDto = mapper.Map<GameDto>(games);

            return Ok(gameDto);
           
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameDto>> GetGame(int id)
        {
          
            var game = await uow.GameRepository.GetAsync(id);

            if (game == null) return NotFound();

            var gameDto = mapper.Map<GameDto>(game);

            return Ok(gameDto);
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
        public async Task<ActionResult<GameDto>> PutGame(int id, GameDto gameDto)
        {

            var game = await uow.GameRepository.GetAsync(id);

            if (game == null) return NotFound();

            mapper.Map(gameDto,game);

            try
            {
              uow.GameRepository.Update(game);
              
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await uow.GameRepository.AnyAsync(id) == false)
                {
                    return NotFound();
                }

                else
                throw;
            }
            await uow.CompleteAsync();
            
          

            return Ok(mapper.Map<GameDto>(game));
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
        public async Task<ActionResult<GameDto>> PostGame(CreateGameDto dto)
        {
            var game = mapper.Map<Game>(dto);
            uow.GameRepository.Add(game);    
            await uow.CompleteAsync();

            return CreatedAtAction(nameof(GetGame), new { title = game.Title }, mapper.Map<GameDto>(dto));
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
