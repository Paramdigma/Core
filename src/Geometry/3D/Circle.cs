using System;
using Paramdigma.Core.Geometry.Interfaces;

namespace Paramdigma.Core.Geometry
{
    public class Circle : ICurve
    {
        public Plane Plane;
        public double Radius;

        public Circle(Plane plane, double radius)
        {
            this.Plane = plane;
            this.Radius = radius;
        }

        public Point3d PointAt(double t)
        {
            var radians = t * 2 * Math.PI;
            var x = this.Radius * Math.Cos(radians);
            var y = this.Radius * Math.Sin(radians);
            return Plane.PointAt(x, y, 0);
        }

        public Vector3d TangentAt(double t) => NormalAt(t).Cross(this.Plane.ZAxis);

        public Vector3d NormalAt(double t) => (this.Plane.Origin - PointAt(t)).Unit();

        public Vector3d BinormalAt(double t) => TangentAt(t).Cross(NormalAt(t));


        public Plane FrameAt(double t) => new Plane(PointAt(t), NormalAt(t), BinormalAt(t), TangentAt(t));
        
    }
}