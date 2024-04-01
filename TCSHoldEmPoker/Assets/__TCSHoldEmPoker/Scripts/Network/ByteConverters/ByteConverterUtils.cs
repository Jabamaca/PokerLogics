using System;
using TCSHoldEmPoker.Models.Define;
using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Data {
    internal static class ByteConverterUtils {

        // Sizes of Basic Data
        internal const int SIZEOF_CARD_DATA = sizeof (byte);
        internal const int SIZEOF_HAND_RANK = sizeof (byte);
        internal const int SIZEOF_GAME_PHASE = sizeof (byte);
        internal const int SIZEOF_NETWORK_ACTIVITY_ID = sizeof (Int16);
        internal const int SIZEOF_NETWORK_ACTIVITY_START = sizeof (UInt16);
        internal const int SIZEOF_NETWORK_ACTIVITY_END = sizeof (UInt16);

        internal const UInt16 NETWORK_ACTIVITY_START = 0xF99F;
        internal const UInt16 NETWORK_ACTIVITY_END = 0xF66F;

        // Sizes of Structured Data
        internal const int SIZEOF_HAND_DATA =
            SIZEOF_HAND_RANK +                                                      // Hand Rank
            (HoldEmPokerDefines.POKER_HAND_SIZE * SIZEOF_CARD_DATA);                // The 5 Cards
        internal const int SIZEOF_PLAYER_STATE_DATA =
            sizeof (Int32) +                                                        // Player ID
            sizeof (Int32);                                                         // Chips in Hand
        internal const int SIZEOF_SEAT_STATE_DATA =
            SIZEOF_PLAYER_STATE_DATA +                                              // Seated Player State Data
            sizeof (byte) +                                                         // BoolSet #1 (didCheck, isPlaying)
            sizeof (Int32);                                                         // Current Wager
        internal const int SIZEOF_TABLE_STATE_DATA =
            sizeof (Int32) +                                                        // Minimum Wager
            sizeof (Int32) +                                                        // Current Table Stake
            sizeof (Int32) +                                                        // Cash Pot
            (SIZEOF_SEAT_STATE_DATA * HoldEmPokerDefines.POKER_TABLE_CAPACITY) +    // Seat State Data Order
            SIZEOF_GAME_PHASE +                                                     // Current Game Phase
            sizeof (Int16) +                                                        // Current Turn Seat Index
            (SIZEOF_CARD_DATA * HoldEmPokerDefines.POKER_COMMUNITY_CARD_COUNT);           // Community Card Order

        // Sizes of Game Events
        // Connectivity
        internal const int SIZEOF_POKER_GAME_EVENT_PLAYER_JOIN =
            SIZEOF_NETWORK_ACTIVITY_START +                                         // START Network Activity Stream
            SIZEOF_NETWORK_ACTIVITY_ID +                                            // Network Activity ID
            sizeof (Int32) +                                                        // Game ID
            sizeof (Int32) +                                                        // Player ID
            sizeof (Int32) +                                                        // Buy-In Chips
            SIZEOF_NETWORK_ACTIVITY_END;                                            // END Network Activity Stream
        internal const int SIZEOF_POKER_GAME_EVENT_PLAYER_LEAVE =
            SIZEOF_NETWORK_ACTIVITY_START +                                         // START Network Activity Stream
            SIZEOF_NETWORK_ACTIVITY_ID +                                            // Network Activity ID
            sizeof (Int32) +                                                        // Game ID
            sizeof (Int32) +                                                        // Player ID
            SIZEOF_NETWORK_ACTIVITY_END;                                            // END Network Activity Stream
        // Game Progression
        // Card Dealing
        internal const int SIZEOF_POKER_GAME_EVENT_PLAYER_CARD_DEAL =
            SIZEOF_NETWORK_ACTIVITY_START +                                         // START Network Activity Stream
            SIZEOF_NETWORK_ACTIVITY_ID +                                            // Network Activity ID
            sizeof (Int32) +                                                        // Game ID
            sizeof (Int32) +                                                        // Player ID
            (SIZEOF_CARD_DATA * HoldEmPokerDefines.POKER_PLAYER_DEAL_COUNT) +       // Cards Data
            SIZEOF_NETWORK_ACTIVITY_END;                                            // END Network Activity Stream
        internal const int SIZEOF_POKER_GAME_EVENT_COMMUNITY_CARD_REVEAL =
            SIZEOF_NETWORK_ACTIVITY_START +                                         // START Network Activity Stream
            SIZEOF_NETWORK_ACTIVITY_ID +                                            // Network Activity ID
            sizeof (Int32) +                                                        // Game ID
            SIZEOF_CARD_DATA +                                                      // Card Data
            sizeof (Int16) +                                                        // Card Position (Flop 1-2-3, Turn, River)
            SIZEOF_NETWORK_ACTIVITY_END;                                            // END Network Activity Stream
        // Player Action
        // Win Condention

        internal const int BIT_COUNT_OF_BYTE = 8;
        internal const int BIT_COUNT_OF_INT16 = 16;

        internal static byte BoolArrayToByte (bool[] source) {
            if (source.Length != BIT_COUNT_OF_BYTE)
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

        internal static bool[] BoolArrayFromByte (byte b) {
            bool[] result = new bool[BIT_COUNT_OF_BYTE];

            for (int i = 0; i < BIT_COUNT_OF_BYTE; i++)
                result[i] = (b & (0x01 << i)) != 0;

            return result;
        }
    }
}