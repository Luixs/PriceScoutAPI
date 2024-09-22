using FakeItEasy;
using FluentAssertions;
using PriceScoutAPI.Helpers;
using PriceScoutAPI.Interfaces;
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
        private IBestOptionHelper _iBestOptionHelper;
        private AmazonHelper _amazonHelper;
        private SearchModel _searchModel;
        private ProductModel _productModel;

        public SearchTests()
        {
            // --- Dependencies
            _iBestOptionHelper = A.Fake<IBestOptionHelper>();
            _amazonHelper = A.Fake<AmazonHelper>();
            _searchModel = new SearchModel();
            _productModel = new ProductModel();
        }

        [Fact]
        public void IBestOptionHelper_ChooseBestOption_ReturnsProductModel()
        {
            // --- Arrange | What do i need to bring in here?
            var fakeListProducts = A.Fake<List<ProductModel>>();
            A.CallTo(()=> _iBestOptionHelper.ChooseBestOption(fakeListProducts)).Returns(_productModel);

            // --- Act
            var result = _iBestOptionHelper.ChooseBestOption(fakeListProducts);

            // --- Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ProductModel>();
        }

        //[Fact]
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
