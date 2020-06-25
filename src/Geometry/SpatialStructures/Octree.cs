using System.Collections.Generic;
using Paramdigma.Core.Geometry;

namespace Paramdigma.Core.SpatialSearch
{
    /// <summary>
    ///     Class for computing Octree spatial searches.
    /// </summary>
    public class Octree : ISpatialTree<Box, Point3d>
    {
        // TODO: This class is empty!
        public Box Boundary
        {
            get;
        }

        public List<Point3d> Points
        {
            get;
        }

        public Octree BottomNorthEast;
        public Octree BottomNorthWest;
        public Octree BottomSouthEast;
        public Octree BottomSouthWest;
        public Octree TopNorthEast;
        public Octree TopNorthWest;
        public Octree TopSouthEast;
        public Octree TopSouthWest;

        private readonly double threshold;

        public Octree(Box boundary, double threshold)
        {
            this.Boundary = boundary;
            this.threshold = threshold;
        }

        public bool ThresholdReached => this.Boundary.DomainX.Length < this.threshold
                                     || this.Boundary.DomainY.Length < this.threshold
                                     || this.Boundary.DomainZ.Length < this.threshold;

        public IEnumerable<Octree> Quadrants => new List<Octree>
        {
            this.BottomNorthEast,
            this.BottomNorthWest,
            this.BottomSouthWest,
            this.BottomSouthEast,
            this.TopNorthEast,
            this.TopNorthWest,
            this.TopSouthWest,
            this.TopNorthEast,
        };

        public bool Insert(Point3d point)
        {
            if (!this.Boundary.Contains(point))
                return false;

            if (this.Boundary.DomainX.Length < this.threshold || this.Boundary.DomainY.Length < this.threshold || this.Boundary.DomainZ.Length < this.threshold)
            {
                this.Points.Add(point);
                return true;
            }

            if (this.BottomNorthEast == null)
                this.Subdivide();

            return this.BottomNorthEast.Insert(point)
                || this.BottomNorthWest.Insert(point)
                || this.BottomSouthEast.Insert(point)
                || this.BottomSouthWest.Insert(point)
                || this.TopNorthEast.Insert(point)
                || this.TopNorthWest.Insert(point)
                || this.TopSouthEast.Insert(point)
                || this.TopSouthWest.Insert(point);
        }

        public IEnumerable<Point3d> QueryRange(Box range)
        {
            var pointsInRange = new List<Point3d>();

            if (!this.Boundary.Intersects(range))
                return pointsInRange;

            this.Points.ForEach(pt =>
            {
                if (range.Contains(pt))
                    pointsInRange.Add(pt);
            });

            // If we reached threshold return
            if (this.ThresholdReached)
                return pointsInRange;

            if (this.BottomSouthWest == null)
                return pointsInRange;

            foreach (var quadrant in this.Quadrants)
                pointsInRange.AddRange(quadrant.QueryRange(range));

            return pointsInRange;
        }

        private void Subdivide()
        {
            this.BottomNorthEast = new Octree(new Box(this.Boundary.Center, this.Boundary.BottomNorthEast), this.threshold);
            this.BottomNorthWest = new Octree(new Box(this.Boundary.Center, this.Boundary.BottomNorthWest), this.threshold);
            this.BottomSouthEast = new Octree(new Box(this.Boundary.Center, this.Boundary.BottomSouthEast), this.threshold);
            this.BottomSouthWest = new Octree(new Box(this.Boundary.Center, this.Boundary.BottomSouthWest), this.threshold);

            this.TopNorthEast = new Octree(new Box(this.Boundary.Center, this.Boundary.TopNorthEast), this.threshold);
            this.TopNorthWest = new Octree(new Box(this.Boundary.Center, this.Boundary.TopNorthWest), this.threshold);
            this.TopSouthEast = new Octree(new Box(this.Boundary.Center, this.Boundary.TopSouthEast), this.threshold);
            this.TopSouthWest = new Octree(new Box(this.Boundary.Center, this.Boundary.TopSouthWest), this.threshold);
        }
    }
}