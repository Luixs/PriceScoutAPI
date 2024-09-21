using FakeItEasy;
using PriceScoutAPI.Helpers;
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

        public SearchTests()
        {
            // --- Dependencies
            _amazonHelper = A.Fake<AmazonHelper>();
        }

        [Fact]
        public void AmazonHelper_FindPrices_ReturnsTaskModel()
        {

        }
    }
}
