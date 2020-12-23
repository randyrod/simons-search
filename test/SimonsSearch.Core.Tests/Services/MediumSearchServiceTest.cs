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
    public class MediumSearchServiceTest : BaseTest
    {
        private readonly Mock<IMediumRepository> _mediumRepositoryMock;
        private readonly Mock<IMediumWeightCalculator> _mediumWeightCalculatorMock;

        public MediumSearchServiceTest()
        {
            _mediumRepositoryMock = MockRepository.Create<IMediumRepository>();
            _mediumWeightCalculatorMock = MockRepository.Create<IMediumWeightCalculator>();
        }

        [Fact]
        public void ShouldReturnEmptyListWhenTermsAreEmpty()
        {
            var service = new MediumSearchService(_mediumRepositoryMock.Object, _mediumWeightCalculatorMock.Object);
            var result = service.Search(string.Empty, new List<string>(), new List<GroupDto>());

            result.Should().BeEmpty();
        }

        [Fact]
        public void ShouldReturnEmptyListWhenNoMediumIsFound()
        {
            const string search = "search";
            var separateTerms = new List<string>
            {
                "search"
            };

            _mediumRepositoryMock.Setup(x => x.GetMediumsMatchingTerms(separateTerms, new List<MediumType>()))
                .Returns(new List<Medium>());

            var service = new MediumSearchService(_mediumRepositoryMock.Object, _mediumWeightCalculatorMock.Object);

            var result = service.Search(search, separateTerms, new List<GroupDto>());

            result.Should().BeEmpty();
        }

        [Fact]
        public void ShouldBeAbleToGetPossibleMediumTypesForQuery()
        {
            const string searchTerm = "Car";
            var separateTerms = new List<string>
            {
                "Car"
            };

            _mediumRepositoryMock.Setup(x => x.GetMediumsMatchingTerms(separateTerms,
                    It.Is<IReadOnlyList<MediumType>>(v => v.Count == 2 && v[0] == MediumType.Card && v[1] == MediumType.TransponderWithCardInlay)))
                .Returns(new List<Medium>());

            var service = new MediumSearchService(_mediumRepositoryMock.Object, _mediumWeightCalculatorMock.Object);

            var result = service.Search(searchTerm, separateTerms, new List<GroupDto>());
            result.Should().BeEmpty();
        }

        [Fact]
        public void ShouldBeAbleToPassListOfGroupsToWeightCalculator()
        {
            const string searchTerm = "Medium";
            var separateTerms = new List<string>
            {
                "Medium"
            };

            var groups = new List<GroupDto>
            {
                new GroupDto
                {
                    Name = "Group"
                }
            };

            _mediumRepositoryMock.Setup(x =>
                    x.GetMediumsMatchingTerms(separateTerms, It.IsAny<IReadOnlyList<MediumType>>()))
                .Returns(new List<Medium>
                {
                    new Medium
                    {
                        Owner = "Medium"
                    }
                });

            _mediumWeightCalculatorMock.Setup(x => x.CalculateWeights(
                It.Is<IReadOnlyList<MediumDto>>(v => v.Count == 1 && v[0].Owner == "Medium"),
                groups,
                searchTerm,
                separateTerms));

            var service = new MediumSearchService(_mediumRepositoryMock.Object, _mediumWeightCalculatorMock.Object);

            var result = service.Search(searchTerm, separateTerms, groups);

            result.Should().HaveCount(1);
            result[0].Owner.Should().Be("Medium");
        }
    }
}