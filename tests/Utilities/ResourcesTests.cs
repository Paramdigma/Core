using System.Globalization;
using Xunit;
using Xunit.Abstractions;

namespace Paramdigma.Core.Utilities
{
    public class ResourcesTests
    {
        public ResourcesTests(ITestOutputHelper testOutputHelper) => this.testOutputHelper = testOutputHelper;

        private readonly ITestOutputHelper testOutputHelper;

        [Fact]
        public void ResetSettingsValues_FromResource()
        {
            this.testOutputHelper.WriteLine(Settings.Tolerance.ToString(CultureInfo.CurrentCulture));
            this.testOutputHelper.WriteLine(Settings.MaxDecimals.ToString());
            this.testOutputHelper.WriteLine(Settings.GetDefaultTesselationLevel().ToString());
            Settings.SetTolerance(0.1);
            this.testOutputHelper.WriteLine(Settings.Tolerance.ToString(CultureInfo.CurrentCulture));
            this.testOutputHelper.WriteLine(Settings.MaxDecimals.ToString());
            Settings.Reset();
            this.testOutputHelper.WriteLine(Settings.Tolerance.ToString(CultureInfo.CurrentCulture));
            this.testOutputHelper.WriteLine(Settings.MaxDecimals.ToString());
        }
    }
}