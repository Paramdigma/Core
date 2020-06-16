using System.Collections.Generic;
using System.Linq;
using Paramdigma.Core.Geometry;

namespace Paramdigma.Core.SpatialSearch
{
    /// <summary>
    /// Class to compute Quadtree spatial searches.
    /// </summary>
    public class Quadtree
    {
        // TODO: This class is empty!

        public BoundingBox2d Boundary;
        public List<Point2d> Points;

        public Quadtree(BoundingBox2d boundary, double threshold)
        {
            this.Boundary = boundary;
            this.Points = new List<Point2d>();
        }


        private double threshold;
        private Quadtree northWest;
        private Quadtree northEast;
        private Quadtree southWest;
        private Quadtree southEast;

        public bool Insert(Point2d point)
        {
            if (!this.Boundary.ContainsPoint(point))
                return false;


            if (this.Boundary.XDomain.Length < this.threshold || this.Boundary.YDomain.Length < this.threshold)
            {
                this.Points.Add(point);
                return true;
            }

            this.Subdivide();

            if (this.northEast.Insert(point)
             || this.northWest.Insert(point)
             || this.southWest.Insert(point)
             || this.southEast.Insert(point))
                return true;

            return false;
        }

        public void Subdivide()
        {
            this.southWest = new Quadtree(
                new BoundingBox2d(this.Boundary.BottomLeft, this.Boundary.Center),
                this.threshold);
            this.northWest = new Quadtree(
                new BoundingBox2d(this.Boundary.MidLeft, this.Boundary.MidTop),
                this.threshold);
            this.southEast = new Quadtree(
                new BoundingBox2d(this.Boundary.MidBottom, this.Boundary.MidRight),
                this.threshold);
            this.northEast = new Quadtree(
                new BoundingBox2d(this.Boundary.Center, this.Boundary.TopRight),
                this.threshold);
        }

        public List<Point2d> QueryRange(BoundingBox2d range)
        {
            List<Point2d> pointsInRange = new List<Point2d>();

            if (!this.Boundary.IntersectsBox(range))
                return pointsInRange;

            this.Points.ForEach(pt =>
            {
                if (range.ContainsPoint(pt))
                    pointsInRange.Add(pt);
            });

            // If we reached threshold return
            if (this.Boundary.XDomain.Length < this.threshold || this.Boundary.YDomain.Length < this.threshold)
                return pointsInRange;

            pointsInRange.AddRange(this.southWest.QueryRange(range));
            pointsInRange.AddRange(this.southEast.QueryRange(range));
            pointsInRange.AddRange(this.northWest.QueryRange(range));
            pointsInRange.AddRange(this.northEast.QueryRange(range));

            return pointsInRange;
        }
    }
}