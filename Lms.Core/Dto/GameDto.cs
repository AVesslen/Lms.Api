﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Dto
{
    public class GameDto
    {
        public string Title { get; set; } = string.Empty;
        public DateTime Time { get; set; }
        public int TournamentId { get; set; }

    }
}
