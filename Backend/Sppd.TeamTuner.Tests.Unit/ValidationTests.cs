using System.Linq;

using Microsoft.EntityFrameworkCore.Internal;

using Moq;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Validation;

using Xunit;

namespace Sppd.TeamTuner.Tests.Unit
{
    public class ValidationTests
    {
        [Fact]
        public void CardValidationTest()
        {
            // Arrange
            var card = new Card();
            var validationContextMock = new Mock<IValidationContext>();

            // Act
            var validationErrors = card.Validate(validationContextMock.Object);

            // Assert
            Assert.True(EnumerableExtensions.Any(validationErrors));
        }

        [Fact]
        public void RarityValidationTest()
        {
            // Arrange
            var rarityNok = new Rarity();
            var rarityOk = new Rarity
                           {
                               Name = "RarityOk",
                               FriendlyLevel = 4
                           };
            var validationContextMock = new Mock<IValidationContext>();

            // Act
            var rarityNokValidationErrors = rarityNok.Validate(validationContextMock.Object).ToList();
            var rarityOkValidationErrors = rarityOk.Validate(validationContextMock.Object).ToList();

            // Assert
            Assert.True(EnumerableExtensions.Any(rarityNokValidationErrors));
            Assert.Contains(nameof(Rarity.FriendlyLevel), rarityNokValidationErrors.Select(e => e.PropertyName));

            Assert.False(EnumerableExtensions.Any(rarityOkValidationErrors));
        }
    }
}