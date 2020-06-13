using Paramdigma.Core.Optimization;
using Xunit;

namespace Paramdigma.Core.Tests.Optimization
{
    public class GradientDescentOptionsTests
    {
        [Fact]
        public void CanCreate_Default()
        {
            var actual = GradientDescentOptions.Default;
            var expected = new GradientDescentOptions(
                0.001,
                10000,
                0.01,
                20,
                .01
            );
            Assert.Equal(expected,actual);
        }
        
        [Fact]
        public void CanCreate_DefaultSmall()
        {
            var actual = GradientDescentOptions.DefaultSmall;
            var expected = new GradientDescentOptions(
                0.0001, 
                10000, 
                0.02, 
                40, 
                .001
            );
            Assert.Equal(expected,actual);
        }
        
        [Fact]
        public void CanCreate_FromExisting()
        {
            var expected = GradientDescentOptions.DefaultSmall;
            var actual =  new GradientDescentOptions(expected);
            Assert.Equal(expected,actual);
        }
    }
}