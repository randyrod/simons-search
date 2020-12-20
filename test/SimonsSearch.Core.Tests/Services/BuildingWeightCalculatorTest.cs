using System.Collections.Generic;
using FluentAssertions;
using SimonsSearch.Core.Models;
using SimonsSearch.Core.Services;
using Xunit;

namespace SimonsSearch.Core.Tests.Services
{
    public class BuildingWeightCalculatorTest
    {
        [Fact]
        public void ShouldBeAbleToCalculateFullMatchWeights()
        {
            const int expectedWeight = 90;
            const int expectedTransitoryWeight = 80;
            const string searchTerm = "Building one";
            var separateTerms = new List<string>
            {
                "Building",
                "one"
            };

            var testData = new List<BuildingDto>
            {
                new BuildingDto
                {
                    Name = "Building One",
                    ShortCut = "BLD1"
                }
            };

            var service = new BuildingWeightCalculator();

            service.CalculateWeights(testData, searchTerm, separateTerms);

            testData[0].Weight.Should().Be(expectedWeight);
            testData[0].TransitoryWeight.Should().Be(expectedTransitoryWeight);
        }

        [Fact]
        public void ShouldBeAbleToCalculateSinglePartialMatchWeights()
        {
            const int expectedWeight = 7;
            const int expectedTransitoryWeight = 5;
            const string searchTerm = "bld1 lock";
            var separateTerms = new List<string>
            {
                "bld1",
                "lock"
            };

            var testData = new List<BuildingDto>
            {
                new BuildingDto
                {
                    Name = "Building one",
                    ShortCut = "BLD1",
                    Description = "The building number 1"
                }
            };

            var service = new BuildingWeightCalculator();

            service.CalculateWeights(testData, searchTerm, separateTerms);

            testData[0].Weight.Should().Be(expectedWeight);
            testData[0].TransitoryWeight.Should().Be(expectedTransitoryWeight);
        }

        [Fact]
        public void ShouldBeAbleToCalculateMultiplePartialMatchesWeights()
        {
            const int expectedWeight = 31;
            const int expectedTransitoryWeight = 13;
            const string searchTerm = "Building number one";
            var separateTerms = new List<string>
            {
                "Building",
                "number",
                "1"
            };

            var testData = new List<BuildingDto>
            {
                new BuildingDto
                {
                    Name = "Building one", // Partial match with word Building
                    ShortCut = "BLD1", // Partial match with 1
                    Description = "The building number 1" // Partial match with number and 1
                }
            };

            var service = new BuildingWeightCalculator();

            service.CalculateWeights(testData, searchTerm, separateTerms);

            testData[0].Weight.Should().Be(expectedWeight);
            testData[0].TransitoryWeight.Should().Be(expectedTransitoryWeight);
        }

        [Fact]
        public void ShouldBeAbleToCalculateWeightForMultipleBuildings()
        {
            const int buildingOneExpectedWeight = 31;
            const int buildingOneExpectedTransitoryWeight = 13;
            const int buildingTwoExpectedWeight = 19;
            const int buildingTwoExpectedTransitoryWeight = 8;
            const string searchTerm = "Building number one";
            var separateTerms = new List<string>
            {
                "Building",
                "number",
                "1"
            };

            var testData = new List<BuildingDto>
            {
                new BuildingDto
                {
                    Name = "Building one", // Partial match with word Building
                    ShortCut = "BLD1", // Partial match with 1
                    Description = "The building number 1" // Partial match with number and 1
                },
                new BuildingDto
                {
                    Name = "Building two", // Partial match with building
                    ShortCut = "BLD2", // No match
                    Description = "The building number 2" // Partial match with building
                }
            };

            var service = new BuildingWeightCalculator();

            service.CalculateWeights(testData, searchTerm, separateTerms);

            testData[0].Weight.Should().Be(buildingOneExpectedWeight);
            testData[0].TransitoryWeight.Should().Be(buildingOneExpectedTransitoryWeight);

            testData[1].Weight.Should().Be(buildingTwoExpectedWeight);
            testData[1].TransitoryWeight.Should().Be(buildingTwoExpectedTransitoryWeight);
        }

        [Fact]
        public void ShouldBeAbleToFindFullMatchAndPartialMatchInMultipleBuildings()
        {
            const int buildingOneExpectedWeight = 95;
            const int buildingOneExpectedTransitoryWeight = 80;
            const int buildingTwoExpectedWeight = 14;
            const int buildingTwoExpectedTransitoryWeight = 8;
            const string searchTerm = "Building one";
            var separateTerms = new List<string>
            {
                "Building",
                "one"
            };

            var testData = new List<BuildingDto>
            {
                new BuildingDto
                {
                    Name = "Building one", // Partial match with word Building
                    ShortCut = "BLD1", // Partial match with 1
                    Description = "The building number 1" // Partial match with number and 1
                },
                new BuildingDto
                {
                    Name = "Building two", // Partial match with building
                    ShortCut = "BLD2", // No match
                    Description = "The building number 2" // Partial match with building
                }
            };

            var service = new BuildingWeightCalculator();

            service.CalculateWeights(testData, searchTerm, separateTerms);

            testData[0].Weight.Should().Be(buildingOneExpectedWeight);
            testData[0].TransitoryWeight.Should().Be(buildingOneExpectedTransitoryWeight);

            testData[1].Weight.Should().Be(buildingTwoExpectedWeight);
            testData[1].TransitoryWeight.Should().Be(buildingTwoExpectedTransitoryWeight);
        }
    }
}