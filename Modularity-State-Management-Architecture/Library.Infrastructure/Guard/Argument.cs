using System;
using System.Collections;
using System.Diagnostics.Contracts;

namespace StateBuilder.Guard
{
    public static class Argument
    {
        /// <summary>
        /// Verifies that a given argument value is not <c>null</c>.
        /// </summary>
        /// <typeparam name="T">Type of the <paramref name="name" />.</typeparam>
        /// <param name="name">Argument name.</param>
        /// <param name="value">Argument value.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        [ContractArgumentValidator]
        public static void NotNull<T>(string name, T value)
            where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
            Contract.EndContractBlock();
        }

        /// <summary>
        /// Verifies that a given argument value is not <c>null</c>.
        /// </summary>
        /// <typeparam name="T">Type of the <paramref name="name" />.</typeparam>
        /// <param name="name">Argument name.</param>
        /// <param name="value">Argument value.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        [ContractArgumentValidator]
        public static void NotNull<T>(string name, T? value)
            where T : struct
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
            Contract.EndContractBlock();
        }

        /// <summary>
        /// Verifies that a given argument value is not <c>null</c> or empty.
        /// </summary>
        /// <param name="name">Argument name.</param>
        /// <param name="value">Argument value.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is empty.</exception>
        [ContractArgumentValidator]
        public static void NotNullOrEmpty(string name, string value)
        {
            Argument.NotNull(name, value);
            if (value.Length == 0)
            {
                throw new ArgumentException("Value can not be empty.", name);
            }
            Contract.EndContractBlock();
        }

        /// <summary>
        /// Verifies that a given argument value is not <c>null</c> or empty.
        /// </summary>
        /// <param name="name">Argument name.</param>
        /// <param name="value">Argument value.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is empty.</exception>
        [ContractArgumentValidator]
        public static void NotNullOrEmpty<T>(string name, T[] value)
        {
            Argument.NotNull(name, value);
            if (value.Length == 0)
            {
                throw new ArgumentException("Value can not be empty.", name);
            }
            Contract.EndContractBlock();
        }

        /// <summary>
        /// Verifies that a given argument value is not <c>null</c> or empty.
        /// </summary>
        /// <param name="name">Argument name.</param>
        /// <param name="value">Argument value.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is empty.</exception>
        [ContractArgumentValidator]
        public static void NotNullOrEmpty<TCollection>(string name, TCollection value)
            where TCollection : class, IEnumerable
        {
            Argument.NotNull(name, value);
            var enumerator = value.GetEnumerator();
            try
            {
                if (!enumerator.MoveNext())
                {
                    throw new ArgumentException("Value can not be empty.", name);
                }
            }
            finally
            {
                var disposable = enumerator as IDisposable;
                disposable?.Dispose();
            }
            Contract.EndContractBlock();
        }

        /// <summary>
        /// Casts a given argument into a given type if possible.
        /// </summary>
        /// <typeparam name="T">Type to cast <paramref name="value"/> into.</typeparam>
        /// <param name="name">Argument name.</param>
        /// <param name="value">Argument value.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> can not be cast into type <typeparamref name="T"/>.</exception>
        [ContractArgumentValidator]
        public static void CanCastTo<T>(string name, object value)
        {
            if (!(value is T))
            {
                throw new ArgumentException($"The value \"{value}\" isn't of type \"{typeof(T)}\".", name);
            }
            Contract.EndContractBlock();
        }

        /// <summary>
        /// Verifies that a given argument value is greater than or equal to zero.
        /// </summary>
        /// <param name="name">Argument name.</param>
        /// <param name="value">Argument value.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than zero.</exception>
        [ContractArgumentValidator]
        public static void PositiveOrZero(string name, int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(name, value, "Value must be positive or zero.");
            }
            Contract.EndContractBlock();
        }

        /// <summary>
        /// Verifies that a given argument value is greater than or equal to zero.
        /// </summary>
        /// <param name="name">Argument name.</param>
        /// <param name="value">Argument value.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than zero.</exception>
        [ContractArgumentValidator]
        public static void PositiveOrZero(string name, long value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(name, value, "Value must be positive or zero.");
            }
            Contract.EndContractBlock();
        }

        /// <summary>
        /// Verifies that a given argument value is greater than zero.
        /// </summary>
        /// <param name="name">Argument name.</param>
        /// <param name="value">Argument value.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than or equal to zero.</exception>
        [ContractArgumentValidator]
        public static void PositiveNonZero(string name, int value)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(name, value, "Value must be positive and not zero.");
            }
            Contract.EndContractBlock();
        }

        /// <summary>
        /// Verifies that a given argument value is greater than zero.
        /// </summary>
        /// <param name="name">Argument name.</param>
        /// <param name="value">Argument value.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than or equal to zero.</exception>
        [ContractArgumentValidator]
        public static void PositiveNonZero(string name, long value)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(name, value, "Value must be positive and not zero.");
            }
            Contract.EndContractBlock();
        }

        /// <summary>
        /// Verifies that a given argument value is zero.
        /// </summary>
        /// <param name="name">Argument name.</param>
        /// <param name="value">Argument value.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is not equal to zero.</exception>
        [ContractArgumentValidator]
        public static void Zero(string name, int value)
        {
            if (value != 0)
            {
                throw new ArgumentOutOfRangeException(name, value, "Value must be equal to zero.");
            }
            Contract.EndContractBlock();
        }

        /// <summary>
        /// Verifies that a given argument value is zero.
        /// </summary>
        /// <param name="name">Argument name.</param>
        /// <param name="value">Argument value.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is not equal to zero.</exception>
        [ContractArgumentValidator]
        public static void Zero(string name, long value)
        {
            if (value != 0)
            {
                throw new ArgumentOutOfRangeException(name, value, "Value must be equal to zero.");
            }
            Contract.EndContractBlock();
        }
    }
}
