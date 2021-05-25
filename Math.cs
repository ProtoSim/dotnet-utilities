namespace ProtoSim.DotNetUtilities {
    /// <summary>
    /// Provides extension methods for doing mathematical functions
    /// </summary>
    public static class Math {
        /// <summary>
        /// Maps <c>byte</c> <paramref name="value"/> from one <c>byte</c> range (<paramref name="fromMin"/> - <paramref name="fromMax"/>) to another <c>byte</c> range (<paramref name="toMin"/> - <paramref name="toMax"/>)
        /// </summary>
        /// <param name="value">The <c>byte</c> value to map</param>
        /// <param name="fromMin">Low <c>byte</c> value for original range</param>
        /// <param name="fromMax">High <c>byte</c> value for original range</param>
        /// <param name="toMin">Low <c>byte</c> value for new range</param>
        /// <param name="toMax">High <c>byte</c> value for new range</param>
        /// <returns>A <c>decimal</c> respresenting the mapped value</returns>
        public static decimal Map(this byte value, byte fromMin, byte fromMax, byte toMin, byte toMax) {
            return ((decimal)value - (decimal)fromMin) * ((decimal)toMax - (decimal)toMin) / ((decimal)fromMax - (decimal)fromMin) + (decimal)toMin;
        }

        /// <summary>
        /// Maps <c>float</c> <paramref name="value"/> from one <c>float</c> range (<paramref name="fromMin"/> - <paramref name="fromMax"/>) to another <c>float</c> range (<paramref name="toMin"/> - <paramref name="toMax"/>)
        /// </summary>
        /// <param name="value">The <c>float</c> value to map</param>
        /// <param name="fromMin">Low <c>float</c> value for original range</param>
        /// <param name="fromMax">High <c>float</c> value for original range</param>
        /// <param name="toMin">Low <c>float</c> value for new range</param>
        /// <param name="toMax">High <c>float</c> value for new range</param>
        /// <returns>A <c>decimal</c> respresenting the mapped value</returns>
        public static decimal Map(this float value, float fromMin, float fromMax, float toMin, float toMax) {
            return ((decimal)value - (decimal)fromMin) * ((decimal)toMax - (decimal)toMin) / ((decimal)fromMax - (decimal)fromMin) + (decimal)toMin;
        }

        /// <summary>
        /// Maps <c>int</c> <paramref name="value"/> from one <c>int</c> range (<paramref name="fromMin"/> - <paramref name="fromMax"/>) to another <c>int</c> range (<paramref name="toMin"/> - <paramref name="toMax"/>)
        /// </summary>
        /// <param name="value">The <c>int</c> value to map</param>
        /// <param name="fromMin">Low <c>int</c> value for original range</param>
        /// <param name="fromMax">High <c>int</c> value for original range</param>
        /// <param name="toMin">Low <c>int</c> value for new range</param>
        /// <param name="toMax">High <c>int</c> value for new range</param>
        /// <returns>A <c>decimal</c> respresenting the mapped value</returns>
        public static decimal Map(this int value, int fromMin, int fromMax, int toMin, int toMax) {
            return ((decimal)value - (decimal)fromMin) * ((decimal)toMax - (decimal)toMin) / ((decimal)fromMax - (decimal)fromMin) + (decimal)toMin;
        }
    }
}