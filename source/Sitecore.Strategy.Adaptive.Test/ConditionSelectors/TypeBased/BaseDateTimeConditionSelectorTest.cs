using FluentAssertions;
using Sitecore.Strategy.Adaptive.ConditionSelectors.TypeBased;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sitecore.Strategy.Adaptive.Test.ConditionSelectors.TypeBased
{
    public class BaseDateTimeConditionSelectorTest
    {
        public static IEnumerable<object[]> GetRelativeData =>
            new List<object[]>
            {
                new object[] { null, DateTime.MinValue },
                new object[] { "D#20180120 10:30", DateTime.MinValue },
                new object[] { "$now-3d", DateTime.Now.AddDays(-3) },
                new object[] { "$now-3h", DateTime.Now.AddHours(-3) },
                new object[] { "$now-3m", DateTime.Now.AddMonths(-3) },
                new object[] { "$now-3y", DateTime.Now.AddYears(-3) },
                new object[] { "$now+3d", DateTime.Now.AddDays(3) },
                new object[] { "$now+3y", DateTime.Now.AddYears(3) },
            };

        [Theory]
        [MemberData(nameof(GetRelativeData))]
        public void RelativeDateTimeAsDateTimeWithComputedDate(string relativeDate, DateTime expected)
        {
            //Assign
            var dateTimeConditionSelector = new DateTimeConditionSelector();

            //Act
            var returnedDate = dateTimeConditionSelector.RelativeDateTimeAsDateTime(relativeDate);

            //Assert      

            //Add atime range of two minutes to complete the test
            if (expected == DateTime.MinValue)
                returnedDate.Should().Be(DateTime.MinValue);
            else
            {
                expected.Should().BeBefore(returnedDate);
                expected.AddMinutes(2).Should().BeAfter(returnedDate);
            }
        }

        [Fact]
        public void RelativeDateTimeAsDateTimeWithFixedDate()
        {
            //Assign
            string relativeDate = "20180120 10:30";
            DateTime expected = new DateTime(2018, 01, 20, 10, 30, 0);            
            var dateTimeConditionSelector = new DateTimeConditionSelector();

            //Act
            var returnedDate = dateTimeConditionSelector.RelativeDateTimeAsDateTime(relativeDate);

            //Assert      
            expected.Should().Be(returnedDate);
        }


    }
}
