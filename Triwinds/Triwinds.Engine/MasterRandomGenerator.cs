using System;
using System.Security.Cryptography;

namespace Triwinds.Engine
{
    public class MasterRandomGenerator
    {
        private static readonly RNGCryptoServiceProvider RngCsp = new RNGCryptoServiceProvider();

        // This method simulates a roll of the dice. The input parameter is the
        // number of sides of the dice.
        public static byte RollDice(byte numberOfSides)
        {
            if (numberOfSides <= 0)
                throw new ArgumentOutOfRangeException("numberOfSides");

            // Create a byte array to hold the random value.
            byte[] randomNumber = new byte[1];
            do
            {
                // Fill the array with a random value.
                RngCsp.GetBytes(randomNumber);
            }
            while (!IsFairRoll(randomNumber[0], numberOfSides));
            // Return the random number mod the number
            // of sides.  The possible values are zero-
            // based, so we add one.
            return (byte)((randomNumber[0] % numberOfSides) + 1);
        }

        // Rolls a set number of dice
        public static int RollDice(int numberOfDice, byte numberOfSides)
        {
            if (numberOfDice <= 0)
                throw new ArgumentOutOfRangeException("numberOfDice");

            if (numberOfSides <= 0)
                throw new ArgumentOutOfRangeException("numberOfSides");

            int total = 0;
            for (int i = 0; i < numberOfDice; i++)
            {
                total += RollDice(numberOfSides);
            }

            return total;
        }

        private static bool IsFairRoll(byte roll, byte numSides)
        {
            // There are MaxValue / numSides full sets of numbers that can come up
            // in a single byte.  For instance, if we have a 6 sided die, there are
            // 42 full sets of 1-6 that come up.  The 43rd set is incomplete.
            int fullSetsOfValues = Byte.MaxValue / numSides;

            // If the roll is within this range of fair values, then we let it continue.
            // In the 6 sided die case, a roll between 0 and 251 is allowed.  (We use
            // < rather than <= since the = portion allows through an extra 0 value).
            // 252 through 255 would provide an extra 0, 1, 2, 3 so they are not fair
            // to use.
            return roll < numSides * fullSetsOfValues;
        }
    }
}
