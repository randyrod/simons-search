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
    public class MediumWeightCalculatorTest : BaseTest
    {
        [Fact]
        public void ShouldDoNothingWhenListOfMediumsIsEmpty()
        {
            var service = new MediumWeightCalculator();

            service.CalculateWeights(new List<MediumDto>(), new List<GroupDto>(), string.Empty, new List<string>());
        }

        [Fact]
        public void ShouldCalculateFullMatch()
        {
            const int expectedWeight = 100;
            const string searchTerm = "Super owner";
            var separateTerms = new List<string>
            {
                "Super",
                "owner"
            };

            var mediums = new List<MediumDto>
            {
                new MediumDto
                {
                    Owner = "Super owner"
                }
            };

            var service = new MediumWeightCalculator();

            service.CalculateWeights(mediums, new List<GroupDto>(), searchTerm, separateTerms);

            mediums[0].Weight.Should().Be(expectedWeight);
        }

        [Fact]
        public void ShouldBeAbleToCalculateSinglePartialMatch()
        {
            const int expectedWeight = 6;
            const string searchTerms = "Lock description";
            var separateTerms = new List<string>
            {
                "Lock",
                "description"
            };

            var mediums = new List<MediumDto>
            {
                new MediumDto
                {
                    Owner = "Owner",
                    Description = "Medium description"
                }
            };

            var service = new MediumWeightCalculator();

            service.CalculateWeights(mediums, new List<GroupDto>(), searchTerms, separateTerms);

            mediums[0].Weight.Should().Be(expectedWeight);
        }

        [Fact]
        public void ShouldBeAbleToCalculateMultiplePartialWeights()
        {
            const int expectedWeight = 16;
            const string searchTerms = "Medium lock";
            var separateTerms = new List<string>
            {
                "Medium",
                "lock"
            };

            var mediums = new List<MediumDto>
            {
                new MediumDto
                {
                    Description = "Medium desc",
                    Owner = "Owner medium"
                }
            };

            var service = new MediumWeightCalculator();

            service.CalculateWeights(mediums, new List<GroupDto>(), searchTerms, separateTerms);

            mediums[0].Weight.Should().Be(expectedWeight);
        }

        [Fact]
        public void ShouldBeAbleToFullyMatchMediumType()
        {
            const int expectedWeight = 30;
            const string searchTerms = "Card";
            var separateTerms = new List<string>
            {
                "Card"
            };

            var mediums = new List<MediumDto>
            {
                new MediumDto
                {
                    Type = MediumType.Card
                }
            };

            var service = new MediumWeightCalculator();

            service.CalculateWeights(mediums, new List<GroupDto>(), searchTerms, separateTerms);

            mediums[0].Weight.Should().Be(expectedWeight);
        }

        [Fact]
        public void ShouldIncludeTransitoryWeightFromGroup()
        {
            const int expectedWeight = 6;
            const int transitoryWeight = 6;
            var guid = Guid.NewGuid();

            var searchTerms = "No Match";
            var separateTerms = new List<string>
            {
                "No",
                "Match"
            };

            var mediums = new List<MediumDto>
            {
                new MediumDto
                {
                    Owner = "Own",
                    GroupId = guid
                }
            };

            var groups = new List<GroupDto>
            {
                new GroupDto
                {
                    Id = guid,
                    TransitoryWeight = transitoryWeight
                }
            };

            var service = new MediumWeightCalculator();

            service.CalculateWeights(mediums, groups, searchTerms, separateTerms);

            mediums[0].Weight.Should().Be(expectedWeight);
        }

        [Fact]
        public void ShouldBeAbleToCalculatePartialAndFullMatches()
        {
            const int expectedWeight = 106;
            const string searchTerms = "Medium Owner";
            var separateTerms = new List<string>
            {
                "Medium",
                "Owner"
            };

            var mediums = new List<MediumDto>
            {
                new MediumDto
                {
                    Owner = "Medium Owner",
                    Description = "Is a Medium"
                }
            };

            var service = new MediumWeightCalculator();

            service.CalculateWeights(mediums, new List<GroupDto>(), searchTerms, separateTerms);

            mediums[0].Weight.Should().Be(expectedWeight);
        }

        [Fact]
        public void ShouldBeAbleToCalculateWeightForMultipleMediums()
        {
            const int mediumOneExpectedWeight = 130;
            const int mediumTwoExpectedWeight = 40;
            const string searchTerms = "Card medium";
            var separateTerms = new List<string>
            {
                "Card",
                "medium"
            };

            var mediums = new List<MediumDto>
            {
                new MediumDto
                {
                    Owner = "Card Medium",
                    Type = MediumType.Card
                },
                new MediumDto
                {
                    Owner = "Medium one",
                    Type = MediumType.Card
                }
            };

            var service = new MediumWeightCalculator();

            service.CalculateWeights(mediums, new List<GroupDto>(), searchTerms, separateTerms);

            mediums[0].Weight.Should().Be(mediumOneExpectedWeight);
            mediums[1].Weight.Should().Be(mediumTwoExpectedWeight);
        }

        [Fact]
        public void ShouldDistributeTransitoryWeightToCorrectMedium()
        {
            const int mediumOneExpectedWeight = 130;
            const int mediumTwoExpectedWeight = 50;
            const int transitoryWeight = 10;
            var guid = Guid.NewGuid();
            const string searchTerms = "Card medium";
            var separateTerms = new List<string>
            {
                "Card",
                "medium"
            };

            var mediums = new List<MediumDto>
            {
                new MediumDto
                {
                    Owner = "Card Medium",
                    Type = MediumType.Card
                },
                new MediumDto
                {
                    Owner = "Medium one",
                    Type = MediumType.Card,
                    GroupId = guid
                }
            };

            var groups = new List<GroupDto>
            {
                new GroupDto
                {
                    Id = guid,
                    TransitoryWeight = transitoryWeight
                }
            };
            var service = new MediumWeightCalculator();

            service.CalculateWeights(mediums, groups, searchTerms, separateTerms);

            mediums[0].Weight.Should().Be(mediumOneExpectedWeight);
            mediums[1].Weight.Should().Be(mediumTwoExpectedWeight);
        }
    }
}