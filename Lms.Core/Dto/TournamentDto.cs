using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Dto
{
#nullable disable

    /// <summary>
    /// A tournament with title, start date, end time and games
    /// </summary>
    public class TournamentDto
    {
        /// <summary>
        /// Title of the tournament
        /// </summary>
        public string Title { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }

        public DateTime EndDate => StartDate.AddMonths(3);

        public ICollection<GameDto> Games { get; set; }


      
    }
}
