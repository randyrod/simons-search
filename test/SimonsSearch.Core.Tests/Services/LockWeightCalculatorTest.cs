using System;
using System.Collections.Generic;
using FluentAssertions;
using SimonsSearch.Common.Enums;
using SimonsSearch.Core.Models;
using SimonsSearch.Core.Services;
using SimonsSearch.Testing;
using Xunit;

namespace SimonsSearch.Core.Tests.Services
{
    public class LockWeightCalculatorTest : BaseTest
    {
        [Fact]
        public void ShouldDoNothingWhenListOfLocksIsEmpty()
        {
            var service = new LockWeightCalculator();

            service.CalculateWeights(new List<LockDto>(), new List<BuildingDto>(), "", new List<string>());
        }

        [Fact]
        public void ShouldCalculateFullMatch()
        {
            const int expectedWeight = 60;
            const string searchTerm = "Super lock";
            var separateTerms = new List<string>
            {
                "Super",
                "lock"
            };

            var locks = new List<LockDto>
            {
                new LockDto
                {
                    Description = "super lock"
                }
            };

            var service = new LockWeightCalculator();

            service.CalculateWeights(locks, new List<BuildingDto>(), searchTerm, separateTerms);

            locks[0].Weight.Should().Be(expectedWeight);
        }

        [Fact]
        public void ShouldCalculateSinglePartialMatch()
        {
            const int expectedWeight = 6;
            const string searchTerm = "floor 5H";
            var separateTerms = new List<string>
            {
                "floor",
                "5H"
            };

            var locks = new List<LockDto>
            {
                new LockDto
                {
                    Name = "Super lock",
                    RoomNumber = "3.5H"
                }
            };

            var service = new LockWeightCalculator();

            service.CalculateWeights(locks, new List<BuildingDto>(), searchTerm, separateTerms);

            locks[0].Weight.Should().Be(expectedWeight);
        }

        [Fact]
        public void ShouldBeAbleToCalculateMultiplePartialMatches()
        {
            const int expectedWeight = 16;
            const string searchTerm = "lock floor 5H";
            var separateTerms = new List<string>
            {
                "lock",
                "floor",
                "5H"
            };

            var locks = new List<LockDto>
            {
                new LockDto
                {
                    Name = "Lock 5",
                    Description = "Main floor",
                    Floor = "5"
                }
            };

            var service = new LockWeightCalculator();

            service.CalculateWeights(locks, new List<BuildingDto>(), searchTerm, separateTerms);

            locks[0].Weight.Should().Be(expectedWeight);
        }

        [Fact]
        public void ShouldBeAbleToFullyMatchLockType()
        {
            const int expectedWeight = 30;
            const string searchTerm = "cylinder";
            var separateTerms = new List<string>
            {
                "cylinder"
            };

            var locks = new List<LockDto>
            {
                new LockDto
                {
                    Type = LockType.Cylinder
                }
            };

            var service = new LockWeightCalculator();

            service.CalculateWeights(locks, new List<BuildingDto>(), searchTerm, separateTerms);

            locks[0].Weight.Should().Be(expectedWeight);
        }

        [Fact]
        public void ShouldIncludeTransitoryWeightFromBuilding()
        {
            const int expectedWeight = 6;
            const int transitoryWeight = 6;
            var searchTerms = "Not match on purpose";
            var separateTerms = new List<string>
            {
                "Not",
                "match",
                "on",
                "purpose"
            };
            var guid = Guid.NewGuid();

            var locks = new List<LockDto>
            {
                new LockDto
                {
                    Name = "Lock",
                    BuildingId = guid
                }
            };

            var buildings = new List<BuildingDto>
            {
                new BuildingDto
                {
                    Id = guid,
                    TransitoryWeight = transitoryWeight
                }
            };

            var service = new LockWeightCalculator();

            service.CalculateWeights(locks, buildings, searchTerms, separateTerms);

            locks[0].Weight.Should().Be(expectedWeight);
        }

        [Fact]
        public void ShouldBeAbleToCalculatePartialAndFullMatches()
        {
            const int expectedWeight = 106;
            const string searchTerm = "Lock room";
            var separateTerms = new List<string>
            {
                "Lock",
                "room"
            };

            var locks = new List<LockDto>
            {
                new LockDto
                {
                    Name = "Lock room",
                    Description = "Second floor room",
                    Floor = "2.0G"
                }
            };

            var service = new LockWeightCalculator();

            service.CalculateWeights(locks, new List<BuildingDto>(), searchTerm, separateTerms);

            locks[0].Weight.Should().Be(expectedWeight);
        }

        [Fact]
        public void ShouldBeAbleToCalculateWeightForMultipleLocks()
        {
            const int lockOneExpectedWeight = 106;
            const int lockTwoExpectedWeight = 16;
            const string searchTerm = "Lock floor five";
            var separateTerms = new List<string>
            {
                "Lock",
                "floor",
                "five"
            };

            var locks = new List<LockDto>
            {
                new LockDto
                {
                    Name = "Lock floor five",
                    Description = "Cylinder lock",
                    Type = LockType.Cylinder,
                    Floor = "5"
                },
                new LockDto
                {
                    Name = "Lock main",
                    Description = "Main floor"
                }
            };

            var service = new LockWeightCalculator();

            service.CalculateWeights(locks, new List<BuildingDto>(), searchTerm, separateTerms);

            locks[0].Weight.Should().Be(lockOneExpectedWeight);
            locks[1].Weight.Should().Be(lockTwoExpectedWeight);
        }

        [Fact]
        public void ShouldDistributeBuildingTransitoryWeightToCorrectLock()
        {
            const int lockOneExpectedWeight = 106;
            const int lockTwoExpectedWeight = 26;
            const string searchTerm = "Lock floor five";
            var separateTerms = new List<string>
            {
                "Lock",
                "floor",
                "five"
            };
            var guid = Guid.NewGuid();

            var locks = new List<LockDto>
            {
                new LockDto
                {
                    Name = "Lock floor five",
                    Description = "Cylinder lock",
                    Type = LockType.Cylinder,
                    Floor = "5"
                },
                new LockDto
                {
                    Name = "Lock main",
                    Description = "Main floor",
                    BuildingId = guid
                }
            };

            var buildings = new List<BuildingDto>
            {
                new BuildingDto
                {
                    Id = guid,
                    TransitoryWeight = 10
                }
            };

            var service = new LockWeightCalculator();

            service.CalculateWeights(locks, buildings, searchTerm, separateTerms);

            locks[0].Weight.Should().Be(lockOneExpectedWeight);
            locks[1].Weight.Should().Be(lockTwoExpectedWeight);
        }
    }
}