using FakeItEasy;
using FluentAssertions;
using PriceScoutAPI.Helpers;
using PriceScoutAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceScoutAPI.Tests
{
    public class SearchTests
    {
        private AmazonHelper _amazonHelper;
        private SearchModel _searchModel;

        public SearchTests()
        {
            // --- Dependencies
            _amazonHelper = A.Fake<AmazonHelper>();
            _searchModel = new SearchModel();
        }

        [Fact]
        public void AmazonHelper_FindPrices_ReturnsTaskModel()
        {
            // --- Arrange | What do i need to bring in?
            var amazonModel = A.Fake<AmazonModel>();
            A.CallTo(() => _amazonHelper.FindPrices(_searchModel)).Returns(amazonModel);

            // --- Act
            var result = _amazonHelper.FindPrices(_searchModel);

            // --- Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Task<AmazonModel>>();
        }
    }
}
