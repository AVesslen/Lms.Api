using Bogus;
using Lms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Data
{
    public class SeedData
    {
        private static LmsApiContext db;        
        public static async Task InitAsync(LmsApiContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            db = context;

            if (await db.Tournament.AnyAsync()) return;

            var tournaments = GenerateTournaments(5);
            await db.AddRangeAsync(tournaments);
            await db.SaveChangesAsync();
        }

        private static IEnumerable<Tournament> GenerateTournaments(int numberOfTournaments)
        {
            var faker = new Faker("sv");
            var tournaments = new List<Tournament>();

             string [] titles = {"Fotbollscupen", "Juniorcupen", "Allsvenskan", "Sverigespelen"};
             

            for (int i = 0; i < numberOfTournaments; i++)
            {
                var title = titles[faker.Random.Int(0,titles.Length-1)];
                var startDate = faker.Date.Soon();
                var games = GenerateGames(faker.Random.Int(5, 20));

                var tournament = new Tournament
                {
                    Title = title,
                    StartDate = startDate,
                    Games = games
                };
                tournaments.Add(tournament);
            }
            return tournaments;           
        }


        private static ICollection<Game> GenerateGames(int numberOfGames)
        {
            var faker = new Faker("sv");
            var games = new List<Game>();

            for (int i = 0; i < numberOfGames; i++)
            {
                var title = $"{ faker.Address.City()} - { faker.Address.City()}";
                var time = faker.Date.Soon();
                var game = new Game
                {
                    Title= title,
                    Time= time
                };
                games.Add(game);
            }
            return games;
        }

    }
}
