using System.Collections.Generic;
using FluentAssertions;
using Moq;
using SimonsSearch.Common.Enums;
using SimonsSearch.Core.Interfaces;
using SimonsSearch.Core.Models;
using SimonsSearch.Core.Repositories;
using SimonsSearch.Core.Services;
using SimonsSearch.Data.Entities;
using SimonsSearch.Testing;
using Xunit;

namespace SimonsSearch.Core.Tests.Services
{
    public class LockSearchServiceTest : BaseTest
    {
        private readonly Mock<ILockRepository> _lockRepositoryMock;
        private readonly Mock<ILockWeightCalculator> _lockWeightCalculatorMock;

        public LockSearchServiceTest()
        {
            _lockRepositoryMock = MockRepository.Create<ILockRepository>();
            _lockWeightCalculatorMock = MockRepository.Create<ILockWeightCalculator>();
        }

        [Fact]
        public void ShouldReturnEmptyListWhenThereAreNoSearchTerms()
        {
            var service = new LockSearchService(_lockRepositoryMock.Object, _lockWeightCalculatorMock.Object);

            var result = service.Search(string.Empty, new List<string>(), new List<BuildingDto>());

            result.Should().BeEmpty();
        }

        [Fact]
        public void ShouldReturnEmptyListWhenNoLockIsFound()
        {
            const string searchTerms = "Lock";
            var separateTerms = new List<string>
            {
                "Lock"
            };

            _lockRepositoryMock.Setup(x => x.GetLocksMatchingTerms(separateTerms, It.IsAny<IReadOnlyList<LockType>>()))
                .Returns(new List<Lock>());

            var service = new LockSearchService(_lockRepositoryMock.Object, _lockWeightCalculatorMock.Object);

            var result = service.Search(searchTerms, separateTerms, new List<BuildingDto>());
            result.Should().BeEmpty();
        }

        [Fact]
        public void ShouldBeAbleToGetLocks()
        {
            const string expectedLockName = "Lock";
            const string searchTerms = "Lock";
            var separateTerms = new List<string>
            {
                "Lock"
            };

            _lockRepositoryMock.Setup(x => x.GetLocksMatchingTerms(separateTerms, It.IsAny<IReadOnlyList<LockType>>()))
                .Returns(new List<Lock>
                {
                    new Lock
                    {
                        Name = "Lock"
                    }
                });

            _lockWeightCalculatorMock.Setup(x =>
                x.CalculateWeights(It.Is<IReadOnlyList<LockDto>>(v => v.Count == 1 && v[0].Name == expectedLockName),
                    It.IsAny<IReadOnlyList<BuildingDto>>(),
                    searchTerms,
                    separateTerms));

            var service = new LockSearchService(_lockRepositoryMock.Object, _lockWeightCalculatorMock.Object);

            var result = service.Search(searchTerms, separateTerms, new List<BuildingDto>());
            result.Should().HaveCount(1);
            result[0].Name.Should().Be(expectedLockName);
        }

        [Fact]
        public void ShouldBeAbleToParseEnumTypesFromSearchTerms()
        {
            const string searchTerms = "Cylinder";
            var separateTerms = new List<string>
            {
                "Cylinder"
            };

            _lockRepositoryMock.Setup(x => x.GetLocksMatchingTerms(
                    separateTerms,
                    It.Is<IReadOnlyList<LockType>>(v => v.Count == 1 && v[0] == LockType.Cylinder)))
                .Returns(new List<Lock>());

            var service = new LockSearchService(_lockRepositoryMock.Object, _lockWeightCalculatorMock.Object);

            var result = service.Search(searchTerms, separateTerms, new List<BuildingDto>());
            result.Should().BeEmpty();
        }

        [Fact]
        public void ShouldBeAbleToPassBuildingsToCalculator()
        {
            const string expectedLockName = "Lock";
            const string expectedBuildingName = "building";
            const string searchTerms = "Lock";
            var separateTerms = new List<string>
            {
                "Lock"
            };

            var testBuildings = new List<BuildingDto>
            {
                new BuildingDto
                {
                    Name = expectedBuildingName
                }
            };

            _lockRepositoryMock.Setup(x => x.GetLocksMatchingTerms(separateTerms, It.IsAny<IReadOnlyList<LockType>>()))
                .Returns(new List<Lock>
                {
                    new Lock
                    {
                        Name = "Lock"
                    }
                });

            _lockWeightCalculatorMock.Setup(x =>
                x.CalculateWeights(It.Is<IReadOnlyList<LockDto>>(v => v.Count == 1 && v[0].Name == expectedLockName),
                    It.Is<IReadOnlyList<BuildingDto>>(v => v.Count == 1 && v[0].Name == expectedBuildingName),
                    searchTerms,
                    separateTerms));

            var service = new LockSearchService(_lockRepositoryMock.Object, _lockWeightCalculatorMock.Object);

            var result = service.Search(searchTerms, separateTerms, testBuildings);
            result.Should().HaveCount(1);
            result[0].Name.Should().Be(expectedLockName);
        }
    }
}