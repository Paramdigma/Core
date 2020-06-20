using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Paramdigma.Core.Exceptions;

namespace Paramdigma.Core.Geometry
{
    /// <summary>
    /// Represents a polyline of 3-dimensional points.
    /// </summary>
    public class Polyline : BaseCurve, IEnumerable<Point3d>
    {
        // TODO: Frame, Normal, Binormal and Tangent calculation is still slightly sketchy. It must be checked.
        private readonly List<Point3d> knots;
        private List<Line> segments;
        private bool segmentsNeedUpdate;

        /// <summary>
        /// Initializes a new instance of the <see cref="Polyline"/> class.
        /// </summary>
        public Polyline()
        {
            knots = new List<Point3d>();
            segments = new List<Line>();
            segmentsNeedUpdate = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Polyline"/> class from a list of points.
        /// </summary>
        /// <param name="knots">List of points.</param>
        public Polyline(List<Point3d> knots)
        {
            this.knots = knots;
            segments = new List<Line>();
            this.RebuildSegments();
        }

        /// <summary>
        /// Gets the segment lines of the polyline.
        /// </summary>
        /// <value><see cref="Line"/>.</value>
        public List<Line> Segments
        {
            get
            {
                if (segmentsNeedUpdate)
                    this.RebuildSegments();
                return segments;
            }
        }

        /// <summary>
        /// Gets the knot at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        public Point3d this[int index] => this.knots[index];

        /// <summary>
        /// Gets the list of knots for this polyline.
        /// </summary>
        public List<Point3d> Knots => this.knots;

        /// <summary>
        /// Gets a value indicating whether the polyline is closed (first point == last point).
        /// </summary>
        public bool IsClosed => knots[0] == knots[this.knots.Count- 1];

        /// <summary>
        /// Gets a value indicating whether the polyline is unset.
        /// </summary>
        public bool IsUnset => knots.Count == 0;

        /// <inheritdoc/>
        public IEnumerator<Point3d> GetEnumerator() => ((IEnumerable<Point3d>)knots).GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<Point3d>)knots).GetEnumerator();

        /// <summary>
        /// Add a new knot vertex at the end of the polyline.
        /// </summary>
        /// <param name="knot">Point to add.</param>
        public void AddKnot(Point3d knot)
        {
            knots.Add(knot); // Add knot to list
            segmentsNeedUpdate = true;
        }

        /// <summary>
        /// Add a new knot vertex at the specified index.
        /// </summary>
        /// <param name="knot">Point to add.</param>
        /// <param name="index">Location to add at.</param>
        public void InsertKnot(Point3d knot, int index)
        {
            knots.Insert(index, knot); // Add knot to list
            segmentsNeedUpdate = true;
        }

        /// <summary>
        /// Delete a specific knot if it exists in the polyline.
        /// If the point exists multiple times, it will remove the first occurrence.
        /// </summary>
        /// <param name="knot">Point to delete.</param>
        public void RemoveKnot(Point3d knot)
        {
            if (!this.knots.Contains(knot))
                throw new Exception("Point is not a knot in the polyline");
            this.knots.Remove(knot);
            this.segmentsNeedUpdate = true;
        }

        /// <summary>
        /// Delete a knot at a specific index.
        /// </summary>
        /// <param name="index">Index to delete knot at.</param>
        public void RemoveKnotAt(int index)
        {
            if (IsUnset)
                throw new UnsetGeometryException("Cannot erase knot from an Unset polyline");
            if (index < 0 || index > knots.Count - 1)
                throw new IndexOutOfRangeException("Knot index must be within the Knot list count");
            knots.RemoveAt(index);
            this.segmentsNeedUpdate = true;
        }

        private void RebuildSegments()
        {
            segments = new List<Line>(knots.Count - 1);
            double t = 0;
            for (int i = 1; i < knots.Count; i++)
            {
                Line l = new Line(knots[i - 1], knots[i]);
                var t0 = t;
                t += l.Length;
                var t1 = t;
                l.Domain = new Collections.Interval(t0, t1);
                segments.Add(l);
            }
        }

        /// <inheritdoc/>
        public override Vector3d BinormalAt(double t) => (from segment in this.segments
                                                          where segment.Domain.Contains(t)
                                                          select segment.BinormalAt(t)).FirstOrDefault();

        /// <inheritdoc/>
        public override Vector3d NormalAt(double t) => (from segment in this.segments
                                                        where segment.Domain.Contains(t)
                                                        select segment.NormalAt(t)).FirstOrDefault();

        /// <inheritdoc/>
        public override Point3d PointAt(double t) => (from segment in this.segments
                                                      where segment.Domain.Contains(t)
                                                      select segment.PointAt(t)).FirstOrDefault();

        /// <inheritdoc/>
        public override Vector3d TangentAt(double t) => (from segment in this.segments
                                                         where segment.Domain.Contains(t)
                                                         select segment.TangentAt(t)).FirstOrDefault();

        /// <inheritdoc/>
        public override Plane FrameAt(double t) => (from segment in this.segments
                                                    where segment.Domain.Contains(t)
                                                    select segment.FrameAt(t)).FirstOrDefault();

        /// <inheritdoc/>
        protected override double ComputeLength()
        {
            double length = 0;
            segments.ForEach(segment => length += segment.Length);
            return length;
        }

        /// <summary>
        /// Checks the validity of the polyline. Currently only checks if some segments are collapsed (length == 0).
        /// </summary>
        /// <returns>True if polyline has no collapsed segments.</returns>
        public override bool CheckValidity()
        {
            if (IsUnset)
                return false;
            bool valid = true;
            foreach (var segment in this.segments)
            {
                if (segment.Length > Settings.Tolerance)
                    continue;
                valid = false;
                break;
            }

            return valid;
        }
    }
}