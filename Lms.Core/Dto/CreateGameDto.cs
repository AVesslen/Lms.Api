using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Dto
{
    public class CreateGameDto
    {
        [Required(ErrorMessage = "Games need titles")]
        [MaxLength(50)]
        public string Title { get; set; } = string.Empty;
        public DateTime Time { get; set; }

        public int TournamentId { get; set; }
    }
}
