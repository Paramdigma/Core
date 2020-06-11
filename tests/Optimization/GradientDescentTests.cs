using System.Collections.Generic;
using Paramdigma.Core.Geometry;
using Paramdigma.Core.Optimization;
using Xunit;

namespace Paramdigma.Core.Tests.Optimization
{
    public class GradientDescentTests
    {
        [Fact]
        public void GradientDescent_Line()
        {
            var line = new Line(Point3d.WorldOrigin, new Point3d(1, 1, 0));
            var gd = new GradientDescent(GradientDescentOptions.Default) {Options = {MaxIterations = 1}};
            var input = 1;
            
            gd.Options.MaxIterations = 100;
            gd.Options.LearningRate = 100;
            
            gd.Minimize(
                values => line.PointAt(values[0]).Y,
                new List<double> {input}
            );
            var err = gd.Result.Error <= gd.Options.ErrorThreshold;
            var value = gd.Result.Values[0] <= 0.01;
            var gLength = gd.Result.GradientLength <= gd.Options.Limit;
            
            Assert.True(err || value || gLength);
        }
    }
}