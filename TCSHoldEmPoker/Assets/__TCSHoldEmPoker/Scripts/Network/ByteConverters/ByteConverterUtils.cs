using System;
using TCSHoldEmPoker.Models.Define;

namespace TCSHoldEmPoker.Network.Data {
    internal static class ByteConverterUtils {

        // Sizes of Basic Data
        internal const int SIZEOF_CARD_DATA = sizeof (byte);
        internal const int SIZEOF_HAND_RANK = sizeof (byte);

        // Sizes of Structured Data
        internal const int SIZEOF_HAND_DATA =
            SIZEOF_HAND_RANK +                                          // Hand Rank
            (HoldEmPokerDefines.POKER_HAND_SIZE * SIZEOF_CARD_DATA);    // The 5 Cards
        internal const int SIZEOF_PLAYER_STATE_DATA =
            sizeof (Int32) +                                            // Player ID
            sizeof (Int32);                                             // Chips in Hand
        internal const int SIZEOF_SEAT_STATE_DATA =
            SIZEOF_PLAYER_STATE_DATA +
            sizeof (byte) +
            sizeof (Int32);

        internal static byte ConvertBoolArrayToByte (bool[] source) {
            if (source.Length != 8)
                return 0;

            byte result = 0x00;
            int index = 0;
            foreach (bool b in source) {
                if (b)
                    result |= (byte)(0x01 << index);
                index++;
            }

            return result;
        }

        internal static bool[] ConvertByteToBoolArray (byte b) {
            bool[] result = new bool[8];

            for (int i = 0; i < 8; i++)
                result[i] = (b & (0x01 << i)) != 0;

            return result;
        }
    }
}