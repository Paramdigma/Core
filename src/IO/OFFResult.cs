#pragma warning disable 1591

namespace Paramdigma.Core.IO
{
    /// <summary>
    ///     Enum containing the result of the OFF conversion.
    /// </summary>
    public enum OffResult
    {
        Ok,
        IncorrectFormat,
        IncorrectVertex,
        IncorrectFace,
        NonMatchingVerticesSize,
        NonMatchingFacesSize,
        FileNotFound
    }
}