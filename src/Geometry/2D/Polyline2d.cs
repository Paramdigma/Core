using System;
using System.Collections.Generic;
using Paramdigma.Core.Collections;

namespace Paramdigma.Core.Geometry
{
    /// <summary>
    ///     Represents a 2-dimensional polyline.
    /// </summary>
    public class Polyline2d
    {
        private Interval domain;
        private bool isClosed;
        private List<Line2d> segments;
        private bool segmentsNeedUpdate;
        private List<Point2d> vertices;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Polyline2d" /> class.
        /// </summary>
        /// <param name="vertices">Vertices of the polyline.</param>
        /// <param name="closed">Determine if polyline should be closed or not.</param>
        public Polyline2d(List<Point2d> vertices, bool closed)
        {
            this.vertices = vertices;
            this.IsClosed = closed; // Call the property (not the field), to have if add the first point at the end if necessary.
            this.RebuildSegments();
        }

        /// <summary>
        ///     Gets or sets the polyline vertices.
        /// </summary>
        /// <value>List of vertices.</value>
        public List<Point2d> Vertices
        {
            get => this.vertices;

            set
            {
                this.vertices = value;
                this.segmentsNeedUpdate = true;
            }
        }

        /// <summary>
        ///     Gets the polyline segments.
        /// </summary>
        /// <value>List of segments.</value>
        public List<Line2d> Segments
        {
            get
            {
                if (this.segmentsNeedUpdate)
                    this.RebuildSegments();
                return this.segments;
            }
        }

        /// <summary>
        ///     Gets the domain of the polyline.
        /// </summary>
        public Interval Domain => this.domain;

        /// <summary>
        ///     Gets the bounding box of the polyline.
        /// </summary>
        /// <returns>2D bounding box.</returns>
        public BoundingBox2d BoundingBox => new BoundingBox2d(this);

        /// <summary>
        ///     Gets the length of the polyline.
        /// </summary>
        /// <value></value>
        public double Length
        {
            get
            {
                double length = 0;
                this.Segments.ForEach(segment => length += segment.Length);
                return length;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the polyline is closed.
        /// </summary>
        /// <value>True if closed.</value>
        public bool IsClosed
        {
            get => this.isClosed;
            set
            {
                if (this.isClosed == value)
                    return; // Do nothing

                if (this.isClosed)
                    this.vertices.RemoveAt(this.vertices.Count - 1);
                else
                    this.vertices.Add(this.vertices[0]);

                this.isClosed = value;
                this.RebuildSegments();
            }
        }

        /// <summary>
        ///     Computes the area of the polyline.
        /// </summary>
        /// <returns>Area as number.</returns>
        public double Area()
        {
            if (!this.isClosed)
                return 0;
            var v = this.vertices;
            var n = this.vertices.Count - 1;
            double area = 0;
            int i, j, k;

            if (n < 3)
                return 0; // a degenerate polygon

            for (i = 1, j = 2, k = 0; i < n; i++, j++, k++)
                area += v[i].X * (v[j].Y - v[k].Y);

            area += v[n].X * (v[1].Y - v[n - 1].Y); // wrap-around term
            return area / 2.0;
        }

        /// <summary>
        ///     Checks if the current polyline is CW or CCW.
        /// </summary>
        /// <returns>
        ///     TRUE if the polyline is CW.
        ///     FALSE if the polyline is CCW.
        /// </returns>
        /// <exception cref="Exception">Throws an exception if the polyline is not closed or is degenerate.</exception>
        public bool IsClockwise()
        {
            if (!this.isClosed)
                throw new Exception("Cannot compute orientation in an Open polyline");

            // first find rightmost lowest vertex of the polygon
            var rmin = 0;
            var xmin = this.vertices[0].X;
            var ymin = this.vertices[0].Y;

            for (var i = 1; i < this.vertices.Count; i++)
            {
                if (this.vertices[i].Y > ymin)
                    continue;
                if (this.vertices[i].Y == ymin)
                    // just as low
                    if (this.vertices[i].X < xmin)
                        // and to left
                        continue;

                rmin = i; // a new rightmost lowest vertex
                xmin = this.vertices[i].X;
                ymin = this.vertices[i].Y;
            }

            // test orientation at the rmin vertex
            // ccw <=> the edge leaving V[rmin] is left of the entering edge
            double result;
            if (rmin == 0)
                result = new Line2d(this.vertices[this.vertices.Count - 1], this.vertices[0]).IsLeft(this.vertices[1]);
            else
                result = new Line2d(this.vertices[rmin - 1], this.vertices[rmin]).IsLeft(this.vertices[rmin + 1]);

            if (result == 0)
                throw new Exception("Polyline is degenerate, cannot compute orientation.");
            return result < 0;
        }

        /// <summary>
        ///     Reparametrizes the current curve to a unit interval.
        /// </summary>
        public void Reparametrize()
        {
            var maxParameter = this.domain.End;
            var ratio = 1 / this.domain.End;

            double currentParam = 0;

            this.segments.ForEach(segment =>
            {
                var nextParam = currentParam + (segment.Domain.Length * ratio);
                segment.Domain = new Interval(currentParam, nextParam);
                currentParam = nextParam;
            });

            this.domain = Interval.Unit;
        }

        private void RebuildSegments()
        {
            this.segments = new List<Line2d>();
            double currentParam = 0;
            for (var i = 0; i < this.vertices.Count - 1; i++)
            {
                var vertA = this.vertices[i];
                var vertB = this.vertices[i + 1];
                var line = this.BuildSegment(ref currentParam, vertA, vertB);
                this.segments.Add(line);
            }

            this.domain = new Interval(0, currentParam);
        }

        private Line2d BuildSegment(ref double currentParam, Point2d vertA, Point2d vertB)
        {
            var line = new Line2d(vertA, vertB);
            var nextParam = currentParam + line.Length;
            line.Domain = new Interval(currentParam, nextParam);
            currentParam = nextParam;
            return line;
        }
    }
}