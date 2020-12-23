using FluentAssertions;
using SimonsSearch.Common.Extensions;
using SimonsSearch.Testing;
using Xunit;

namespace SimonsSearch.Common.Tests.Extensions
{
    public class StringExtensionsTest : BaseTest
    {
        [Theory]
        [InlineData("This contains text", "text")]
        [InlineData("THIS CONTAINS TEXT", "contains")]
        [InlineData("ThIs ConTaInS TeXt", "contains Text")]
        [InlineData("Contains", "CoNtAiNs")]
        public void ShouldContainText(string mainString, string comparisonString)
        {
            var result = mainString.ContainsInvariant(comparisonString);
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("this contains text", "No")]
        [InlineData("THIS CONTAINS TEXT", "Not in the the other")]
        [InlineData("", "Text")]
        [InlineData("Text", "")]
        public void ShouldNotContainText(string mainString, string comparisonString)
        {
            var result = mainString.ContainsInvariant(comparisonString);
            result.Should().BeFalse();
        }
    }
}