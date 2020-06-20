using System;
using System.Collections.Generic;
using Paramdigma.Core.Collections;

namespace Paramdigma.Core.Geometry
{
    /// <summary>
    /// Represents a 2-dimensional polyline.
    /// </summary>
    public class Polyline2d
    {
        private List<Point2d> vertices;
        private List<Line2d> segments;
        private Interval domain;
        private bool isClosed;
        private bool segmentsNeedUpdate;

        /// <summary>
        /// Initializes a new instance of the <see cref="Polyline2d"/> class.
        /// </summary>
        /// <param name="vertices">Vertices of the polyline.</param>
        /// <param name="closed">Determine if polyline should be closed or not.</param>
        public Polyline2d(List<Point2d> vertices, bool closed)
        {
            this.vertices = vertices;
            this.IsClosed = closed; // Call the property (not the field), to have if add the first point at the end if necessary.
            RebuildSegments();
        }

        /// <summary>
        /// Gets or sets the polyline vertices.
        /// </summary>
        /// <value>List of vertices.</value>
        public List<Point2d> Vertices
        {
            get
            {
                return vertices;
            }

            set
            {
                vertices = value;
                segmentsNeedUpdate = true;
            }
        }

        /// <summary>
        /// Gets the polyline segments.
        /// </summary>
        /// <value>List of segments.</value>
        public List<Line2d> Segments
        {
            get
            {
                if (segmentsNeedUpdate)
                    RebuildSegments();
                return segments;
            }
        }

        /// <summary>
        /// Gets the domain of the polyline.
        /// </summary>
        public Interval Domain => domain;

        /// <summary>
        /// Gets the bounding box of the polyline.
        /// </summary>
        /// <returns>2D bounding box.</returns>
        public BoundingBox2d BoundingBox => new BoundingBox2d(this);

        /// <summary>
        /// Gets the length of the polyline.
        /// </summary>
        /// <value></value>
        public double Length
        {
            get
            {
                double length = 0;
                Segments.ForEach(segment => length += segment.Length);
                return length;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the polyline is closed.
        /// </summary>
        /// <value>True if closed.</value>
        public bool IsClosed
        {
            get => isClosed;
            set
            {
                if (isClosed == value)
                    return; // Do nothing

                if (isClosed)
                {
                    vertices.RemoveAt(vertices.Count - 1);
                }
                else
                {
                    vertices.Add(vertices[0]);
                }

                isClosed = value;
                RebuildSegments();
            }
        }

        /// <summary>
        /// Computes the area of the polyline.
        /// </summary>
        /// <returns>Area as number.</returns>
        public double Area()
        {
            if (!isClosed)
                return 0;
            List<Point2d> v = vertices;
            int n = vertices.Count - 1;
            double area = 0;
            int i, j, k;

            if (n < 3)
                return 0;  // a degenerate polygon

            for (i = 1, j = 2, k = 0; i < n; i++, j++, k++)
            {
                area += v[i].X * (v[j].Y - v[k].Y);
            }

            area += v[n].X * (v[1].Y - v[n - 1].Y);  // wrap-around term
            return area / 2.0;
        }

        /// <summary>
        /// Checks if the current polyline is CW or CCW.
        /// </summary>
        /// <returns>
        ///
        /// TRUE if the polyline is CW.
        /// FALSE if the polyline is CCW.
        /// </returns>
        /// <exception cref="Exception">Throws an exception if the polyline is not closed or is degenerate.</exception>
        public bool IsClockwise()
        {
            if (!isClosed)
                throw new Exception("Cannot compute orientation in an Open polyline");

            // first find rightmost lowest vertex of the polygon
            int rmin = 0;
            double xmin = vertices[0].X;
            double ymin = vertices[0].Y;

            for (int i = 1; i < vertices.Count; i++)
            {
                if (vertices[i].Y > ymin)
                    continue;
                if (vertices[i].Y == ymin)
                {
                    // just as low
                    if (vertices[i].X < xmin)
                    {// and to left
                        continue;
                    }
                }

                rmin = i;      // a new rightmost lowest vertex
                xmin = vertices[i].X;
                ymin = vertices[i].Y;
            }

            // test orientation at the rmin vertex
            // ccw <=> the edge leaving V[rmin] is left of the entering edge
            double result;
            if (rmin == 0)
                result = new Line2d(vertices[vertices.Count - 1], vertices[0]).IsLeft(vertices[1]);
            else
                result = new Line2d(vertices[rmin - 1], vertices[rmin]).IsLeft(vertices[rmin + 1]);

            if (result == 0)
                throw new Exception("Polyline is degenerate, cannot compute orientation.");
            return result < 0;
        }

        /// <summary>
        /// Reparametrizes the current curve to a unit interval.
        /// </summary>
        public void Reparametrize()
        {
            double maxParameter = domain.End;
            double ratio = 1 / domain.End;

            double currentParam = 0;

            segments.ForEach(segment =>
            {
                double nextParam = currentParam + (segment.Domain.Length * ratio);
                segment.Domain = new Interval(currentParam, nextParam);
                currentParam = nextParam;
            });

            this.domain = Interval.Unit;
        }

        private void RebuildSegments()
        {
            segments = new List<Line2d>();
            double currentParam = 0;
            for (int i = 0; i < vertices.Count - 1; i++)
            {
                Point2d vertA = vertices[i];
                Point2d vertB = vertices[i + 1];
                Line2d line = BuildSegment(ref currentParam, vertA, vertB);
                segments.Add(line);
            }

            domain = new Interval(0, currentParam);
        }

        private Line2d BuildSegment(ref double currentParam, Point2d vertA, Point2d vertB)
        {
            Line2d line = new Line2d(vertA, vertB);
            double nextParam = currentParam + line.Length;
            line.Domain = new Interval(currentParam, nextParam);
            currentParam = nextParam;
            return line;
        }
    }
}