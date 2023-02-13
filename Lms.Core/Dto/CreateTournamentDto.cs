using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Dto
{
    /// <summary>
    /// A dto for creating a new Tournament
    /// </summary>
    public class CreateTournamentDto
    {
        [Required(ErrorMessage = "Tournaments need titles")]
        [MaxLength(40)]
        public string Title { get; set; }

        public DateTime StartDate { get; set; }

       
    }
}
