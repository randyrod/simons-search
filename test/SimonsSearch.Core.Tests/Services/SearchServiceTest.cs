using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using SimonsSearch.Common.Enums;
using SimonsSearch.Core.Interfaces;
using SimonsSearch.Core.Models;
using SimonsSearch.Core.Services;
using SimonsSearch.Testing;
using Xunit;

namespace SimonsSearch.Core.Tests.Services
{
    public class SearchServiceTest : BaseTest
    {
        private readonly Mock<IBuildingSearchService> _buildingSearchEngineMock;
        private readonly Mock<ILockSearchService> _lockSearchServiceMock;
        private readonly Mock<IGroupSearchService> _groupSearchServiceMock;
        private readonly Mock<IMediumSearchService> _mediumSearchServiceMock;

        public SearchServiceTest()
        {
            _buildingSearchEngineMock = MockRepository.Create<IBuildingSearchService>();
            _lockSearchServiceMock = MockRepository.Create<ILockSearchService>();
            _groupSearchServiceMock = MockRepository.Create<IGroupSearchService>();
            _mediumSearchServiceMock = MockRepository.Create<IMediumSearchService>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ShouldReturnEmptyListWhenSearchTermIsNullOrEmpty(string searchTerm)
        {
            var service = new SearchService(
                _buildingSearchEngineMock.Object,
                _lockSearchServiceMock.Object,
                _groupSearchServiceMock.Object,
                _mediumSearchServiceMock.Object);

            var result = service.Search(searchTerm);

            result.Should().BeEmpty();
        }

        [Fact]
        public void ShouldBeAbleToGetAllItemsInOrderByWeight()
        {
            const string searchTerm = "search term";

            var buildingGuid = Guid.NewGuid();
            var lockGuid = Guid.NewGuid();
            var groupGuid = Guid.NewGuid();
            var mediumGuid = Guid.NewGuid();

            var building = new List<BuildingDto>
            {
                new BuildingDto
                {
                    Id = buildingGuid,
                    Weight = 3
                }
            };

            var locks = new List<LockDto>
            {
                new LockDto
                {
                    Id = lockGuid,
                    Weight = 1
                }
            };

            var groups = new List<GroupDto>
            {
                new GroupDto
                {
                    Id = groupGuid,
                    Weight = 2
                }
            };

            var mediums = new List<MediumDto>
            {
                new MediumDto
                {
                    Id = mediumGuid,
                    Weight = 4
                }
            };

            _buildingSearchEngineMock.Setup(x => x.Search(
                    It.Is<string>(v => v == searchTerm.ToUpperInvariant()),
                    It.Is<IReadOnlyList<string>>(v => v.Count == 2 && v[0] == "SEARCH" && v[1] == "TERM")))
                .Returns(building);

            _lockSearchServiceMock.Setup(x => x.Search(
                    It.Is<string>(v => v == searchTerm.ToUpperInvariant()),
                    It.Is<IReadOnlyList<string>>(v => v.Count == 2 && v[0] == "SEARCH" && v[1] == "TERM"),
                    building))
                .Returns(locks);

            _groupSearchServiceMock.Setup(x => x.Search(
                    It.Is<string>(v => v == searchTerm.ToUpperInvariant()),
                    It.Is<IReadOnlyList<string>>(v => v.Count == 2 && v[0] == "SEARCH" && v[1] == "TERM")))
                .Returns(groups);

            _mediumSearchServiceMock.Setup(x => x.Search(
                    It.Is<string>(v => v == searchTerm.ToUpperInvariant()),
                    It.Is<IReadOnlyList<string>>(v => v.Count == 2 && v[0] == "SEARCH" && v[1] == "TERM"),
                    groups))
                .Returns(mediums);

            var service = new SearchService(
                _buildingSearchEngineMock.Object,
                _lockSearchServiceMock.Object,
                _groupSearchServiceMock.Object,
                _mediumSearchServiceMock.Object);

            var result = service.Search(searchTerm);

            result.Should().HaveCount(4);

            result[0].Id.Should().Be(mediumGuid);
            result[0].Weight.Should().Be(4);
            result[0].Type.Should().Be(ResultType.Medium);

            result[1].Id.Should().Be(buildingGuid);
            result[1].Weight.Should().Be(3);
            result[1].Type.Should().Be(ResultType.Building);

            result[2].Id.Should().Be(groupGuid);
            result[2].Weight.Should().Be(2);
            result[2].Type.Should().Be(ResultType.Group);

            result[3].Id.Should().Be(lockGuid);
            result[3].Weight.Should().Be(1);
            result[3].Type.Should().Be(ResultType.Lock);
        }
    }
}