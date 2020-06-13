namespace Paramdigma.Core.Tests
{
    public abstract class VectorEntityTests<T>
    {
        public abstract void CanAdd(T a, T b, T expected);

        public abstract void CanAdd_Itself(T a, T b, T expected);
        public abstract void CanSubstract_New(T a, T b, T expected);
        public abstract void CanSubstract_ToItself(T a, T b, T expected);

        public abstract void CanMultiply_New(T a, double scalar, T expected);
        public abstract void CanMultiply_Itself(T a, double scalar, T expected);

        public abstract void CanDivide_New(T a, double scalar, T expected);
        public abstract void CanDivide_Itself(T a, double scalar, T expected);

        public abstract void IsEqual_WithinTolerance(T a, T b);

        public abstract void IsEqual_HasEqualHashcodes(T a, T b);
    }
}