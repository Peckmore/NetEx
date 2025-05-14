#if !NET6_0_OR_GREATER
using System;

namespace NetEx.IO.Tests
{
    internal static class CompatibilityShims
    {
        /// <summary>
        /// Generates a random 64-bit integer within the specified range.
        /// </summary>
        /// <param name="random">The <see cref="Random"/> instance.</param>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned.</param>
        /// <returns>A 64-bit signed integer greater than or equal to <paramref name="minValue"/> and less than <paramref name="maxValue"/>.</returns>
        /// <remarks>A compatibility-shim as this method doesn't exist < .Net 6.0.</remarks>
        public static long NextInt64(this Random random, long minValue, long maxValue)
        {
            if (random == null)
            {
                throw new ArgumentNullException(nameof(random));
            }

            if (minValue >= maxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(minValue), "minValue must be less than maxValue.");
            }

            // Generate a random ulong and scale it to the desired range
            ulong range = (ulong)(maxValue - minValue);
            ulong randomValue = ((ulong)random.Next(int.MinValue, int.MaxValue) << 32) | (uint)random.Next(int.MinValue, int.MaxValue);
            return (long)(randomValue % range) + minValue;
        }
    }
}
#endif