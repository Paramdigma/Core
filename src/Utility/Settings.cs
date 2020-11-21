using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace Paramdigma.Core
{
    /// <summary>
    ///     Multi-layered struct that holds the library settings.
    /// </summary>
    public static class Settings
    {
        private static int tesselationLevel = 10;

        /// <summary>
        ///     Gets the minimum value allowed when using this library.
        /// </summary>
        public static double Tolerance { get; private set; } = 0.0000001;

        /// <summary>
        ///     Gets how many decimals are allowed when using the library.
        /// </summary>
        public static int MaxDecimals
        {
            get
            {
                var t = Tolerance.ToString("N14");
                return t.Substring(t.IndexOf(".") + 1).IndexOf("1") + 1;
            }
        }


        /// <summary>
        ///     Gets the default tessellation level when converting nurbs to meshes.
        /// </summary>
        /// <returns>Integer representing the default tessellation level.</returns>
        public static int GetDefaultTesselationLevel() => tesselationLevel;


        /// <summary>
        ///     Sets the default tessellation level when converting nurbs to meshes.
        /// </summary>
        private static void SetDefaultTesselationLevel(int value) => tesselationLevel = value;


        /// <summary>
        ///     Modifies the tolerance and computes the maxDecimals value accordingly.
        /// </summary>
        /// <param name="tolerance">Desired tolerance.</param>
        public static void SetTolerance(double tolerance) => Tolerance = tolerance;


        /// <summary>
        ///     Reset the Settings to it's default values.
        /// </summary>
        public static void Reset()
        {
            var assembly = typeof(Settings).GetTypeInfo().Assembly;
            using (var stream =
                assembly.GetManifestResourceStream("Paramdigma.Core.Data.Settings.json"))
            {
                using (var reader = new StreamReader(
                    stream ?? throw new InvalidOperationException("Could not get settings.")))
                {
                    var result = reader.ReadToEnd();
                    var json = JsonConvert.DeserializeObject<EmbeddedSettings>(result);
                    SetTolerance(json.Tolerance);
                    SetDefaultTesselationLevel(json.DefaultTesselation);
                }
            }
        }
    }

    /// <summary>
    ///     This struct holds the settings from the embedded json file. It is only used to reset.
    /// </summary>
    internal struct EmbeddedSettings
    {
        public double Tolerance;
        public int DefaultTesselation;
    }
}