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
using Azure;
using Microsoft.AspNetCore.JsonPatch;

namespace Lms.Api.Controllers
{
    //[Route("api/[controller]")]
    [Route("[controller]")]
    [ApiController]
    public class TournamentsController : ControllerBase
    {
       // private readonly LmsApiContext _context;
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public TournamentsController(IUnitOfWork uow, IMapper mapper)
        {
            // _context = context;
            this.uow = uow;
            this.mapper = mapper;
        }



        //// GET: api/Tournaments
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Tournament>>> GetTournament()
        //{
        //  if (_context.Tournament == null)
        //  {
        //      return NotFound();
        //  }
        //    return await _context.Tournament.ToListAsync();
        //}

        //// GET: api/Tournaments
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournament()
        //{           
        //    var tournaments = await uow.TournamentRepository.GetAllAsync();
            
        //    if(tournaments == null) return NotFound();

        //    var tournamentDto = mapper.Map<IEnumerable<TournamentDto>>(tournaments);

        //    return Ok(tournamentDto);
        //}


        // GET: api/Tournaments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournament(bool includeGames = false)
        {
            var tournaments = await uow.TournamentRepository.GetAllAsync(includeGames);

            if (tournaments == null) return NotFound();

            var tournamentDto = mapper.Map<IEnumerable<TournamentDto>>(tournaments);

            return Ok(tournamentDto);
        }

        //// GET: api/Tournaments/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Tournament>> GetTournament(int id)
        //{
        //  if (_context.Tournament == null)
        //  {
        //      return NotFound();
        //  }
        //    var tournament = await _context.Tournament.FindAsync(id);

        //    if (tournament == null)
        //    {
        //        return NotFound();
        //    }

        //    return tournament;
        //}


        // GET: api/Tournaments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDto>> GetTournament(int id)
        {          
            
            var tournament = await uow.TournamentRepository.GetAsync(id);   

            if (tournament == null) return NotFound();

            var tournamentDto = mapper.Map<TournamentDto>(tournament);

            return Ok(tournamentDto);            
        }


        //// PUT: api/Tournaments/5       
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutTournament(int id, Tournament tournament)
        //{
        //    if (id != tournament.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(tournament).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!TournamentExists(id))
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


        // PUT: api/Tournaments/5       
        [HttpPut("{id}")]
        public async Task<ActionResult<TournamentDto>> PutTournament(int id, TournamentDto tournamentDto)
        {
            var tournament = await uow.TournamentRepository.GetAsync(id);

            if (tournament == null) return NotFound();

            mapper.Map(tournamentDto, tournament);
            try
            {
                 uow.TournamentRepository.Update(tournament); 
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await uow.TournamentRepository.AnyAsync(id) == false)
                {
                    return NotFound();
                }

                else
                {
                    throw;
                }
            }
            return Ok(mapper.Map<TournamentDto>(tournament));
           }



        [HttpPatch("{tournamentId}")]
        public async Task<ActionResult<TournamentDto>> PatchTournament(int tournamentId,
            JsonPatchDocument<TournamentDto> patchDocument)
        {
            var tournament = await uow.TournamentRepository.GetAsync(tournamentId);
            if (tournament == null) return NotFound();

            var dto = mapper.Map<TournamentDto>(tournament);

            patchDocument.ApplyTo(dto, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            mapper.Map(dto, tournament);
            await uow.CompleteAsync();

            return Ok(mapper.Map<TournamentDto>(tournament));        
        }




        //// POST: api/Tournaments      
        //[HttpPost]
        //public async Task<ActionResult<Tournament>> PostTournament(Tournament tournament)
        //{
        //  if (_context.Tournament == null)
        //  {
        //      return Problem("Entity set 'LmsApiContext.Tournament'  is null.");
        //  }
        //    _context.Tournament.Add(tournament);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetTournament", new { id = tournament.Id }, tournament);
        //}

        // POST: api/Tournaments    
        [HttpPost]
        public async Task<ActionResult<TournamentDto>> PostTournament(CreateTournamentDto dto)
        {
            var tournament = mapper.Map<Tournament>(dto);
            uow.TournamentRepository.Add(tournament);
            await uow.CompleteAsync();
            
            return CreatedAtAction(nameof(GetTournament), new {title = tournament.Title}, mapper.Map<TournamentDto>(dto));         
        }


        //// DELETE: api/Tournaments/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteTournament(int id)
        //{
        //    if (_context.Tournament == null)
        //    {
        //        return NotFound();
        //    }
        //    var tournament = await _context.Tournament.FindAsync(id);
        //    if (tournament == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Tournament.Remove(tournament);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        // DELETE: api/Tournaments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournament(int id)
        {
            
            var tournament = await uow.TournamentRepository.GetAsync(id);
            if (tournament == null)
            {
                return NotFound();
            }

            uow.TournamentRepository.Remove(tournament);
            

            return NoContent();
        }

        //private bool TournamentExists(int id)
        //{
        //    return (_context.Tournament?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
