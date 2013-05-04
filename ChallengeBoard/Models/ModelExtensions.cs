﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ChallengeBoard.Models
{
    public static class ModelExtensions
    {
        public static bool IsOwner(this Board board, string name)
        {
            return (board.Owner.Profile.UserName.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public static bool CanEdit(this Competitor competitor, Board board, string name)
        {
            return (board.IsOwner(name) || competitor.Is(name));
        }

        public static bool Is(this Competitor competitor, string name)
        {
            return (competitor.Profile.UserName.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public static IEnumerable<Competitor> Active(this ICollection<Competitor> competitors)
        {
            return (competitors.Where(x => x.Status == CompetitorStatus.Active));
        }

        public static IEnumerable<Competitor> Retired(this ICollection<Competitor> competitors)
        {
            return (competitors.Where(x => x.Status == CompetitorStatus.Retired));
        }

        public static bool ContainsCompetitor(this IEnumerable<Competitor> competitors, string name)
        {
            return (competitors.Any(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)));
        }

        public static Competitor FindCompetitorByName(this IEnumerable<Competitor> competitors, string name)
        {
            return (competitors.SingleOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)));
        }

        public static Competitor FindCompetitorById(this IEnumerable<Competitor> competitors, int id)
        {
            return (competitors.SingleOrDefault(x => x.CompetitorId.Equals(id)));
        }

        public static Competitor FindCompetitorByProfileId(this IEnumerable<Competitor> competitors, int id)
        {
            return (competitors.SingleOrDefault(x => x.ProfileUserId.Equals(id)));
        }

        public static int CalculateUnverifiedRank(this Competitor competitor , IList<Match> matches)
        {
            return (competitor.Rating +
                    matches.Where(x => x.Loser.ProfileUserId == competitor.ProfileUserId && !x.IsResolved)
                           .Sum(x => x.LoserRatingDelta) +
                    matches.Where(x => x.Winner.ProfileUserId == competitor.ProfileUserId && !x.IsResolved)
                           .Sum(x => x.WinnerRatingDelta));
        }
    }
}