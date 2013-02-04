﻿using System;
using System.Linq;
using NUnit.Framework;
using ChallengeBoard.Services;

namespace ChallengeBoardTests
{
    [TestFixture]
    public class GenerateMatchTests
    {
        [Test]
        public void ThrowsIfBoardDoesNotExist()
        {
            var repository = Repository.CreatePopulatedRepository();
            var service = new MatchService(repository);

            Assert.Throws<ServiceException>(() => service.GenerateMatch(100, "User1", "User3"));
        }

        [Test]
        public void ThrowsIfReporterNotFound()
        {
            var repository = Repository.CreatePopulatedRepository();
            var service = new MatchService(repository);

            Assert.Throws<ServiceException>(() => service.GenerateMatch(1, "User3", "User2"));
        }

        [Test]
        public void ThrowsIfOpponentNotFound()
        {
            var repository = Repository.CreatePopulatedRepository();
            var service = new MatchService(repository);
            
            Assert.Throws<ServiceException>(() => service.GenerateMatch(1, "User1", "User3"));
        }

        [Test]
        public void ThrowsIfPlayingYourself()
        {
            var repository = Repository.CreatePopulatedRepository();
            var service = new MatchService(repository);

            // Can't find board
            Assert.Throws<ServiceException>(() => service.GenerateMatch(1, "User1", "User1"));
        }

        [Test]
        public void ThrowsIfBoardHasEnded()
        {
            var repository = Repository.CreatePopulatedRepository();
            var service = new MatchService(repository);

            var board = repository.GetBoardByIdWithCompetitors(1);
            board.End = DateTime.Now.AddDays(-1);

            Assert.Throws<ServiceException>(() => service.GenerateMatch(1, "User1", "User2"));
        }

        [Test]
        public void GeneratesMatch()
        {
            var repository = Repository.CreatePopulatedRepository();
            var service = new MatchService(repository);

            var match = service.GenerateMatch(1, "User1", "User2");

            Assert.NotNull(match);
        }
    }

    [TestFixture]
    public class CreateMatchTests
    {
        [Test]
        public void CreatesMatch()
        {
            var repository = Repository.CreatePopulatedRepository();
            var service = new MatchService(repository);

            var matchCount = repository.Matches.Count();

            service.CreateMatch(1, "User1", "User2");

            Assert.That(repository.Matches.Count(), Is.EqualTo(matchCount + 1));
        }
    }

    [TestFixture]
    public class MatchVerificationTests
    {
        [Test]
        public void ThrowsIfMatchNotFound()
        {
            var repository = Repository.CreatePopulatedRepository();
            var service = new MatchService(repository);

            Assert.Throws<ServiceException>(() => service.VerifyMatch(100));
        }
        
        [Test]
        public void VerifiesMatch()
        {
            var repository = Repository.CreatePopulatedRepository();
            var service = new MatchService(repository);
            
            service.VerifyMatch(1);
            
            var match = repository.GetMatchById(1);

            Assert.That(match.Verified, Is.True);
            Assert.That(match.Resolved, Is.Not.Null);

            Assert.That(match.Loser.Wins, Is.EqualTo(0));
            Assert.That(match.Loser.Streak, Is.EqualTo(0));
            Assert.That(match.Loser.Loses, Is.EqualTo(1));
            Assert.That(match.Loser.Ties, Is.EqualTo(0));

            Assert.That(match.Winner.Wins, Is.EqualTo(1));
            Assert.That(match.Winner.Streak, Is.EqualTo(1));
            Assert.That(match.Winner.Loses, Is.EqualTo(0));
            Assert.That(match.Winner.Ties, Is.EqualTo(0));
        }

        [Test]
        public void VerifiesTiedMatch()
        {
            var repository = Repository.CreatePopulatedRepository();
            var service = new MatchService(repository);

            repository.GetMatchById(1).Tied = true;
            
            service.VerifyMatch(1);

            var match = repository.GetMatchById(1);

            Assert.That(match.Verified, Is.True);
            Assert.That(match.Resolved, Is.Not.Null);

            Assert.That(match.Loser.Wins, Is.EqualTo(0));
            Assert.That(match.Loser.Streak, Is.EqualTo(0));
            Assert.That(match.Loser.Loses, Is.EqualTo(0));
            Assert.That(match.Loser.Ties, Is.EqualTo(1));

            Assert.That(match.Winner.Wins, Is.EqualTo(0));
            Assert.That(match.Winner.Streak, Is.EqualTo(0));
            Assert.That(match.Winner.Loses, Is.EqualTo(0));
            Assert.That(match.Winner.Ties, Is.EqualTo(1));
        }
    }
}
