using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus.DataSets;
using Lms.Core.Entities;
using Lms.Core.Repositories;
using Lms.Data.Data;
using Microsoft.EntityFrameworkCore;


namespace Lms.Data.Repositories
{
    public class TournamentRepository : ITournamentRepository
    {
        private LmsApiContext db;

        public TournamentRepository(LmsApiContext db)
        {
            this.db = db;
        }


        public async Task<IEnumerable<Tournament>> GetAllAsync()
        {
            return await db.Tournament.ToListAsync();
        }


        public async Task<Tournament?> GetAsync(int id)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));
           
            return await db.Tournament.FindAsync(id);
        }
        

        public async Task<bool> AnyAsync(int id)
        {
            return await (db.Tournament.AnyAsync(e => e.Id == id));
        }

        public void Add(Tournament tournament)
        {
            if (tournament is null)
            {
                throw new ArgumentNullException(nameof(tournament));
            }

            db.Tournament.Add(tournament);
            db.SaveChanges();
        }

        public void Update(Tournament tournament)
        {
            if (tournament is null)
            {
                throw new ArgumentNullException(nameof(tournament));
            }

            db.Entry(tournament).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Remove(Tournament tournament)
        {
            if (tournament is null)
            {
                throw new ArgumentNullException(nameof(tournament));
            }

            db.Tournament.Remove(tournament);
            db.SaveChanges();
        }
    }
}
