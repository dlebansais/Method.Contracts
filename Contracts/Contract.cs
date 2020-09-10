namespace Contracts
{
    using System;

    /// <summary>
    /// A set of tools to enforce contracts in methods.
    /// </summary>
    public static class Contract
    {
        /// <summary>
        /// Checks that <paramref name="obj"/> is not null, and provide an alias that is guaranteed to be non-null.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="obj"/> and <paramref name="result"/>.</typeparam>
        /// <param name="obj">The object instance to check.</param>
        /// <param name="result">The non-null alias upon return.</param>
        public static void RequireNotNull<T>(object? obj, out T result)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            result = (T)obj;
        }

        /// <summary>
        /// Provide a value for variables that should be uninitialized.
        /// </summary>
        /// <typeparam name="T">The value type.</typeparam>
        /// <returns>A value that can be used.</returns>
        public static T Unused<T>()
            where T : class
        {
            T Result = null !;
            return Result;
        }
    }
}
