﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameEngine.Models
{
    public class Player
    {
        public int Id { get; set; }
        public int BoardId { get; set; }
        public List<Token> Tokens { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }
    }
}