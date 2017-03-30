#region Copyright
//
// Copyright(C) Eric Singh., 2017.
//
#endregion

#region Using Declarations
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace Bio.Library
{
    /// <summary>
    /// Static class used to retrieve random prime numbers
    /// </summary>
    public static class PrimeNumber
    {
        #region Fields

        /// <summary>
        /// List of prime numbers
        /// </summary>
        private readonly static List<int> randomPrimeNumbers = new List<int>();

        /// <summary>
        /// Generates a random number
        /// </summary>
        private readonly static Random randomNumber = new Random(Guid.NewGuid().GetHashCode());

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. Fills array with prime numbers once.
        /// Increased performance by only having to generate the prime numbers once
        /// </summary>
        static PrimeNumber()
        {
            // Fill list with prime numbers. There are 1229 prime numbers in 2 to 10000
            // The number of prime numbers could be configurable if we wanted
            // The requirements didn't specify how many random prime numbers should be available
            randomPrimeNumbers.AddRange(Enumerable.Range(2, 10000).Where(i => IsPrimeNumber(i)));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get a random prime number from the list.
        /// </summary>
        /// <returns>The random prime number</returns>
        public static int GetRandomPrimeNumber()
        {
            // Get random prime number by randomnly selecting one from the list
            return randomPrimeNumbers[randomNumber.Next(randomPrimeNumbers.Count)];
        }

        /// <summary>
        /// Checks if the number is a prime number
        /// </summary>
        /// <param name="num">The number to check</param>
        /// <returns>True if the number is prime</returns>
        public static bool IsPrimeNumber(long num)
        {
            for (int i = 2; i <= num / 2; i++)
                if ((num % i) == 0)
                    return false;

            return true;
        } 

        #endregion
    }
}
