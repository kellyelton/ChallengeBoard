﻿using System.ComponentModel.DataAnnotations;
using ChallengeBoard.Models;

namespace ChallengeBoard.ViewModels
{
    public class MatchViewModel
    {
        public Board Board { get; set; }

        [Required]
        [Display(Name = "Match Opponent")]
        public string Loser { get; set; }

        [Display(Name = "Match Tied?")]
        public bool Tie { get; set; }

        public MatchViewModel()
        {
        }
    }
}