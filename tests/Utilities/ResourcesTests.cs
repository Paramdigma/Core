using System.Globalization;
using Xunit;
using Xunit.Abstractions;

namespace Paramdigma.Core.Utilities
{
    public class ResourcesTests
    {
        public ResourcesTests( ITestOutputHelper testOutputHelper )
        {
            this.testOutputHelper = testOutputHelper;
        }

        private readonly ITestOutputHelper testOutputHelper;

        [Fact]
        public void ResetSettingsValues_FromResource()
        {
            testOutputHelper.WriteLine(Settings.Tolerance.ToString( CultureInfo.CurrentCulture ));
            testOutputHelper.WriteLine(Settings.MaxDecimals.ToString());
            testOutputHelper.WriteLine(Settings.GetDefaultTesselationLevel().ToString());
            Settings.SetTolerance(0.1);
            testOutputHelper.WriteLine(Settings.Tolerance.ToString( CultureInfo.CurrentCulture ));
            testOutputHelper.WriteLine(Settings.MaxDecimals.ToString());
            Settings.Reset();
            testOutputHelper.WriteLine(Settings.Tolerance.ToString( CultureInfo.CurrentCulture ));
            testOutputHelper.WriteLine(Settings.MaxDecimals.ToString());
        }
    }
}