using Lms.Core.Entities;
using Lms.Core.Repositories;
using Lms.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Repositories
{
    public class GameRepository : IGameRepository
    {
       
            private LmsApiContext db;

            public GameRepository(LmsApiContext db)
            {
                this.db = db;
            }

            public async Task<IEnumerable<Game>> GetAllAsync()
            {
            return await db.Game.ToListAsync();
            }

            public async Task<IEnumerable<Game>> GetAsync(string name)
            {
                if (string.IsNullOrEmpty(name)) 
                { 
                throw new ArgumentNullException($"{nameof(name)} can't be null or whitespace.", nameof(name)); 
                }

                return await db.Game.Where(g => g.Title == name).ToListAsync();
            }

            public async Task<Game> GetAsync(int id)
            {
              ArgumentNullException.ThrowIfNull(id, nameof(id));

              return await db.Game.FirstOrDefaultAsync(g => g.Id == id);
        }


        public async Task<bool> AnyAsync(int id)
            {
                return await (db.Game.AnyAsync(g => g.Id == id));
            }

            public void Add(Game game)
            {
                if (game is null)
                {
                    throw new ArgumentNullException(nameof(game));
                }

                db.Add(game);
            }

            public void Update(Game game)
            {
                if (game is null)
                {
                    throw new ArgumentNullException(nameof(game));
                }

                db.Entry(game).State = EntityState.Modified;
       
            }

            public void Remove(Game game)
            {
                if (game is null)
                {
                    throw new ArgumentNullException(nameof(game));
                }

                db.Game.Remove(game);
            }
        }





    }

