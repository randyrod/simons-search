using System.Collections.Generic;
using FluentAssertions;
using Moq;
using SimonsSearch.Core.Models;
using SimonsSearch.Core.Repositories;
using SimonsSearch.Core.Services;
using SimonsSearch.Data.Entities;
using SimonsSearch.Testing;
using Xunit;

namespace SimonsSearch.Core.Tests.Services
{
    public class BuildingSearchServiceTest : BaseTest
    {
        private readonly Mock<IBuildingRepository> _buildingRepositoryMock;
        private readonly Mock<IBuildingWeightCalculator> _buildingWeightCalculatorMock;

        public BuildingSearchServiceTest()
        {
            _buildingRepositoryMock = MockRepository.Create<IBuildingRepository>();
            _buildingWeightCalculatorMock = MockRepository.Create<IBuildingWeightCalculator>();
        }

        [Fact]
        public void ShouldReturnEmptyListWhenSearchTermsAreEmpty()
        {
            var service = new BuildingSearchService(
                _buildingRepositoryMock.Object,
                _buildingWeightCalculatorMock.Object);

            var result = service.Search(string.Empty, new List<string>());

            result.Should().BeEmpty();
        }

        [Fact]
        public void ShouldBeAbleToGetBuildings()
        {
            const string searchTerms = "Building One";
            const string buildingName = "Building One";
            var separateTerms = new List<string>
            {
                "Building",
                "One"
            };

            _buildingRepositoryMock.Setup(x => x.GetBuildingsMatchingTerms(separateTerms))
                .Returns(new List<Building>
                {
                    new Building
                    {
                        Name = buildingName
                    }
                });

            _buildingWeightCalculatorMock.Setup(x => x.CalculateWeights(
                It.Is<IReadOnlyList<BuildingDto>>(v =>
                    v.Count == 1 && v[0].Name == buildingName),
                searchTerms,
                separateTerms));

            var service = new BuildingSearchService(_buildingRepositoryMock.Object, _buildingWeightCalculatorMock.Object);

            var result = service.Search(searchTerms, separateTerms);

            result.Should().HaveCount(1);
            result[0].Name.Should().Be(buildingName);
        }

        [Fact]
        public void ShouldReturnEmptyListWhenNoBuildingsAreFound()
        {
            const string searchTerm = "Building";
            var separateTerms = new List<string>
            {
                "Building"
            };

            _buildingRepositoryMock.Setup(x => x.GetBuildingsMatchingTerms(separateTerms))
                .Returns(new List<Building>());

            var service = new BuildingSearchService(
                _buildingRepositoryMock.Object,
                _buildingWeightCalculatorMock.Object);

            var result = service.Search(searchTerm, separateTerms);

            result.Should().BeEmpty();
        }
    }
}