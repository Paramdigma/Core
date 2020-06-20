using System;

namespace Paramdigma.Core.Collections
{
    /// <summary>
    ///     Represents an range between two numbers.
    /// </summary>
    public struct Interval
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Interval" /> struct from a start and end value.
        /// </summary>
        /// <param name="start">Starting value of the interval.</param>
        /// <param name="end">Ending value of the interval.</param>
        public Interval(double start, double end)
        {
            if (double.IsNaN(start) || double.IsInfinity(start))
                throw new ArithmeticException("Start value is invalid");
            if (double.IsNaN(end) || double.IsInfinity(end))
                throw new ArithmeticException("End value is invalid");
            this.Start = start;
            this.End = end;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Interval" /> struct from another interval.
        /// </summary>
        /// <param name="interval">Interval to duplicate.</param>
        /// <returns>A new interval instance with the same values.</returns>
        public Interval(Interval interval)
            : this(interval.Start, interval.End) { }

        /// <summary>
        ///     Gets a new unit interval.
        /// </summary>
        /// <returns>New interval from 0.0 to 1.0.</returns>
        public static Interval Unit => new Interval(0, 1);

        /// <summary>
        ///     Gets or sets the starting value of the interval.
        /// </summary>
        /// <value>Value will always be lower unless interval is inverted.</value>
        public double Start
        {
            get;
            set;
        }

        /// <summary>
        ///     Gets or sets the ending value of the interval.
        /// </summary>
        /// <value>Value will always be the biggest unless interval is inverted.</value>
        public double End
        {
            get;
            set;
        }

        /// <summary>
        ///     Gets the space between the start and end of the interval.
        /// </summary>
        public double Length => this.End - this.Start;

        /// <summary>
        ///     Gets a value indicating whether an interval has it's direciton inverted (Start > End).
        /// </summary>
        public bool HasInvertedDirection => this.Length < 0;

        /// <summary>
        ///     Crop a number so that it's contained on the given interval.
        /// </summary>
        /// <param name="number">Number to crop.</param>
        /// <param name="interval">Interval to crop number with.</param>
        /// <returns>Cropped number value.</returns>
        public static double CropNumber(double number, Interval interval)
        {
            if (number <= interval.Start)
                return interval.Start;
            if (number >= interval.End)
                return interval.End;
            return number;
        }

        /// <summary>
        ///     Remap a number from one interval to another.
        /// </summary>
        /// <param name="number">Number to remap.</param>
        /// <param name="fromInterval">Origin interval.</param>
        /// <param name="toInterval">Destination interval.</param>
        /// <returns>Remapped number.</returns>
        public static double RemapNumber(double number, Interval fromInterval, Interval toInterval)
        {
            var cropped = fromInterval.Contains(number) ? number : fromInterval.Crop(number);
            var proportion = (cropped - fromInterval.Start) / Math.Abs(fromInterval.Length);

            return toInterval.Start + (toInterval.Length * proportion);
        }

        /// <summary>
        ///     Crop a number so that it is contained inside this interval.
        /// </summary>
        /// <param name="number">Number to crop.</param>
        /// <returns>Cropped number.</returns>
        public double Crop(double number) => CropNumber(number, this);

        /// <summary>
        ///     Remap a number from this interval to a given one.
        /// </summary>
        /// <param name="number">Number to remap.</param>
        /// <param name="toInterval">Interval to remap number to.</param>
        /// <returns>Remapped number inside given interval.</returns>
        public double Remap(double number, Interval toInterval) => RemapNumber(number, this, toInterval);

        /// <summary>
        ///     Remap a number from this interval to a unit interval.
        /// </summary>
        /// <param name="number">Number to remap.</param>
        /// <returns>Value remaped from 0 to 1.</returns>
        public double RemapToUnit(double number) => this.Remap(number, Unit);

        /// <summary>
        ///     Remap a number from a unit interval to this interval.
        /// </summary>
        /// <param name="number">Number to remap.</param>
        /// <returns>Remapped number.</returns>
        public double RemapFromUnit(double number) => Unit.Remap(number, this);

        /// <summary>
        ///     Check if a number is contained inside this interval.
        /// </summary>
        /// <param name="number">Number to check containment.</param>
        /// <returns>True if number is contained.</returns>
        public bool Contains(double number)
        {
            var min = this.HasInvertedDirection ? this.End : this.Start;
            var max = this.HasInvertedDirection ? this.Start : this.End;
            return min <= number && number <= max;
        }

        /// <summary>
        ///     Swap the Start and End values of this interval.
        /// </summary>
        public void FlipDirection()
        {
            var temp = this.Start;
            this.Start = this.End;
            this.End = temp;
        }
    }
}