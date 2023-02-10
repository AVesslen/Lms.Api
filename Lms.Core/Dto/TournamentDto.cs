using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Dto
{
#nullable disable
    public class TournamentDto
    {
        public string Title { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }

        public DateTime EndDate => StartDate.AddMonths(3);

        public ICollection<GameDto> Games { get; set; }



    }
}
