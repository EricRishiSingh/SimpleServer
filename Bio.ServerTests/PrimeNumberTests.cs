using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bio.Library.Tests
{
    /// <summary>
    /// Unit tests for the PrimeNumber class
    /// </summary>
    [TestClass()]
    public class PrimeNumberTests
    {
        /// <summary>
        /// Verify that the method properly validates a number to be prime or not
        /// </summary>
        [TestMethod()]
        public void IsPrimeNumberTest()
        {
            // Array of number that we know are prime
            var primeNumbers = new int[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47,
                53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127, 131, 137, 139,
                149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 199};

            foreach (var primeNumber in primeNumbers)
                Assert.IsTrue(PrimeNumber.IsPrimeNumber(primeNumber));

            // Array of non-prime numbers
            var nonPrimeNumbers = new int[] { 4, 8, 10, 12, 14, 16 };
            foreach (var nonPrimeNumber in nonPrimeNumbers)
                Assert.IsFalse(PrimeNumber.IsPrimeNumber(nonPrimeNumber));
        }

        /// <summary>
        /// Verify that the prime numbers are valid
        /// </summary>
        [TestMethod()]
        public void GetRandomPrimeNumberTest()
        {
            for (int i = 0; i <= 100; i++)
            {
                var result = PrimeNumber.GetRandomPrimeNumber();
                Assert.IsTrue(PrimeNumber.IsPrimeNumber(result));
            }
        }
    }
}