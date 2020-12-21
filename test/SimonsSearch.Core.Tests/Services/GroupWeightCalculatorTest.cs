using System.Collections.Generic;
using FluentAssertions;
using SimonsSearch.Core.Models;
using SimonsSearch.Core.Services;
using SimonsSearch.Testing;
using Xunit;

namespace SimonsSearch.Core.Tests.Services
{
    public class GroupWeightCalculatorTest : BaseTest
    {
        [Fact]
        public void ShouldDoNothingWhenNoGroupIsProvided()
        {
            var service = new GroupWeightCalculator();
            service.CalculateWeights(new List<GroupDto>(), string.Empty, new List<string>());
        }

        [Fact]
        public void ShouldReturnNoWeightWhenThereIsNoMatch()
        {
            const int expectedWeight = 0;
            const int expectedTransitoryWeight = 0;
            const string searchTerms = "search";
            var separateTerms = new List<string>
            {
                "search"
            };

            var groups = new List<GroupDto>
            {
                new GroupDto
                {
                    Name = "No match",
                    Description = "Whatsoever"
                }
            };

            var service = new GroupWeightCalculator();
            service.CalculateWeights(groups, searchTerms, separateTerms);

            groups[0].Weight.Should().Be(expectedWeight);
            groups[0].TransitoryWeight.Should().Be(expectedTransitoryWeight);
        }

        [Fact]
        public void ShouldBeAbleToCalculateFullMatch()
        {
            const int expectedWeight = 90;
            const int expectedTransitoryWeight = 80;
            const string searchTerm = "Search";
            var separateTerms = new List<string>
            {
                "Search"
            };

            var groups = new List<GroupDto>
            {
                new GroupDto
                {
                    Name = "Search"
                }
            };

            var service = new GroupWeightCalculator();

            service.CalculateWeights(groups, searchTerm, separateTerms);

            groups[0].Weight.Should().Be(expectedWeight);
            groups[0].TransitoryWeight.Should().Be(expectedTransitoryWeight);
        }

        [Fact]
        public void ShouldBeAbleToCalculateTransitoryWeight()
        {
            const int expectedWeight = 9;
            const int expectedTransitoryWeight = 8;
            const string searchTerms = "group one";
            var separateTerms = new List<string>
            {
                "group",
                "one"
            };

            var groups = new List<GroupDto>
            {
                new GroupDto
                {
                    Name = "Group two"
                }
            };

            var service = new GroupWeightCalculator();
            service.CalculateWeights(groups, searchTerms, separateTerms);

            groups[0].Weight.Should().Be(expectedWeight);
            groups[0].TransitoryWeight.Should().Be(expectedTransitoryWeight);
        }

        [Fact]
        public void ShouldBeAbleToCalculateWeightOfMultipleGroups()
        {
            const int expectedWeightOne = 5;
            const int expectedTransitoryWeightOne = 0;
            const int expectedWeightTwo = 90;
            const int expectedTransitoryWeightTwo = 80;
            const string searchTerm = "Group two";
            var separateTerms = new List<string>
            {
                "Group",
                "two"
            };

            var groups = new List<GroupDto>
            {
                new GroupDto
                {
                    Name = "One",
                    Description = "Group One"
                },
                new GroupDto
                {
                    Name = "Group Two",
                    Description = "Second one"
                }
            };

            var service = new GroupWeightCalculator();
            service.CalculateWeights(groups, searchTerm, separateTerms);

            groups[0].Weight.Should().Be(expectedWeightOne);
            groups[0].TransitoryWeight.Should().Be(expectedTransitoryWeightOne);

            groups[1].Weight.Should().Be(expectedWeightTwo);
            groups[1].TransitoryWeight.Should().Be(expectedTransitoryWeightTwo);
        }
    }
}