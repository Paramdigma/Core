using System.Collections.Generic;
using Paramdigma.Core.Geometry;

namespace Paramdigma.Core.SpatialSearch
{
    /// <summary>
    ///     Class to compute 2 dimensional spatial searches by quad subdivision.
    /// </summary>
    public class QuadTree : ISpatialTree<Box2d, Point2d>
    {
        /// <summary>
        ///     Boundary of this QuadTree.
        /// </summary>
        public Box2d Boundary
        {
            get;
        }

        /// <summary>
        ///     Gets the list of points of this QuadTree.
        /// </summary>
        public List<Point2d> Points
        {
            get;
        }

        public bool ThresholdReached => this.Boundary.XDomain.Length < this.threshold
                                     || this.Boundary.YDomain.Length < this.threshold;

        private readonly double threshold;

        private QuadTree northEast;
        private QuadTree northWest;
        private QuadTree southEast;
        private QuadTree southWest;


        /// <summary>
        ///     Initializes a new instance of the <see cref="QuadTree" /> class.
        /// </summary>
        /// <param name="boundary">Boundary of this QuadTree.</param>
        /// <param name="threshold">Smallest allowed dimension.</param>
        public QuadTree(Box2d boundary, double threshold)
        {
            this.Boundary = boundary;
            this.Points = new List<Point2d>();
            this.threshold = threshold;
        }


        /// <summary>
        ///     Insert a point in the QuadTree.
        /// </summary>
        /// <param name="point">Point to insert.</param>
        /// <returns>True if point was inserted.</returns>
        public bool Insert(Point2d point)
        {
            if (!this.Boundary.ContainsPoint(point))
                return false;


            if (ThresholdReached)
            {
                this.Points.Add(point);
                return true;
            }

            if (this.northEast == null)
                this.Subdivide();

            return this.northEast.Insert(point)
                || this.northWest.Insert(point)
                || this.southWest.Insert(point)
                || this.southEast.Insert(point);
        }


        /// <summary>
        ///     Query the QuadTree for all points contained in this range.
        /// </summary>
        /// <param name="range">Range to look for.</param>
        /// <returns>Points contained in the range.</returns>
        public IEnumerable<Point2d> QueryRange(Box2d range)
        {
            var pointsInRange = new List<Point2d>();

            if (!this.Boundary.IntersectsBox(range))
                return pointsInRange;

            this.Points.ForEach(
                pt =>
                {
                    if (range.ContainsPoint(pt))
                        pointsInRange.Add(pt);
                });

            // If we reached threshold return
            if (this.Boundary.XDomain.Length < this.threshold
             || this.Boundary.YDomain.Length < this.threshold)
                return pointsInRange;

            if (this.southWest == null)
                return pointsInRange;

            pointsInRange.AddRange(this.southWest.QueryRange(range));
            pointsInRange.AddRange(this.southEast.QueryRange(range));
            pointsInRange.AddRange(this.northWest.QueryRange(range));
            pointsInRange.AddRange(this.northEast.QueryRange(range));

            return pointsInRange;
        }


        private void Subdivide()
        {
            this.southWest = new QuadTree(
                new Box2d(
                    this.Boundary.BottomLeft,
                    this.Boundary.Center),
                this.threshold);
            this.northWest = new QuadTree(
                new Box2d(
                    this.Boundary.MidLeft,
                    this.Boundary.MidTop),
                this.threshold);
            this.southEast = new QuadTree(
                new Box2d(
                    this.Boundary.MidBottom,
                    this.Boundary.MidRight),
                this.threshold);
            this.northEast = new QuadTree(
                new Box2d(
                    this.Boundary.Center,
                    this.Boundary.TopRight),
                this.threshold);
        }
    }
}