using System.Collections.Generic;
using FluentAssertions;
using Moq;
using SimonsSearch.Core.Interfaces;
using SimonsSearch.Core.Models;
using SimonsSearch.Core.Repositories;
using SimonsSearch.Core.Services;
using SimonsSearch.Data.Entities;
using SimonsSearch.Testing;
using Xunit;

namespace SimonsSearch.Core.Tests.Services
{
    public class GroupSearchServiceTest : BaseTest
    {
        private readonly Mock<IGroupRepository> _groupRepositoryMock;
        private readonly Mock<IGroupWeightCalculator> _groupWeightCalculatorMock;

        public GroupSearchServiceTest()
        {
            _groupRepositoryMock = MockRepository.Create<IGroupRepository>();
            _groupWeightCalculatorMock = MockRepository.Create<IGroupWeightCalculator>();
        }

        [Fact]
        public void ShouldReturnEmptyListWhenSearchTermsAreEmpty()
        {
            var service = new GroupSearchService(_groupRepositoryMock.Object, _groupWeightCalculatorMock.Object);

            var result = service.Search(string.Empty, new List<string>());

            result.Should().BeEmpty();
        }

        [Fact]
        public void ShouldReturnEmptyListWhenNoGroupIsFound()
        {
            const string searchTerms = "Search";
            var separateTerms = new List<string>
            {
                "Search"
            };

            _groupRepositoryMock.Setup(x => x.GetGroupsMatchingTerms(separateTerms))
                .Returns(new List<Group>());

            var service = new GroupSearchService(_groupRepositoryMock.Object, _groupWeightCalculatorMock.Object);

            var result = service.Search(searchTerms, separateTerms);

            result.Should().BeEmpty();
        }

        [Fact]
        public void ShouldBeAbleToGetMatchingGroups()
        {
            const string searchTerms = "Search";
            var separateTerms = new List<string>
            {
                "Search"
            };

            _groupRepositoryMock.Setup(x => x.GetGroupsMatchingTerms(separateTerms))
                .Returns(new List<Group>
                {
                    new Group
                    {
                        Name = "Search"
                    }
                });

            _groupWeightCalculatorMock.Setup(x => x.CalculateWeights(
                It.Is<IReadOnlyList<GroupDto>>(v => v.Count == 1 && v[0].Name == "Search"),
                searchTerms,
                separateTerms));

            var service = new GroupSearchService(_groupRepositoryMock.Object, _groupWeightCalculatorMock.Object);

            var result = service.Search(searchTerms, separateTerms);

            result.Should().HaveCount(1);
            result[0].Name.Should().Be("Search");
        }
    }
}