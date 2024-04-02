using System;
using TCSHoldEmPoker.Models.Define;
using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Data {
    public static class ByteConverterUtils {

        // Sizes of Basic Data
        public const int SIZEOF_CARD_DATA = sizeof (byte);
        public const int SIZEOF_HAND_RANK = sizeof (byte);
        public const int SIZEOF_GAME_PHASE = sizeof (byte);
        public const int SIZEOF_NETWORK_ACTIVITY_ID = sizeof (Int16);
        public const int SIZEOF_NETWORK_ACTIVITY_START = sizeof (UInt16);
        public const int SIZEOF_NETWORK_ACTIVITY_END = sizeof (UInt16);

        public const UInt16 NETWORK_ACTIVITY_START = 0xF99F;
        public const UInt16 NETWORK_ACTIVITY_END = 0xF66F;

        // Sizes of Structured Data
        public const int SIZEOF_HAND_DATA =
            SIZEOF_HAND_RANK +                                                      // Hand Rank
            (HoldEmPokerDefines.POKER_HAND_SIZE * SIZEOF_CARD_DATA);                // The 5 Cards
        public const int SIZEOF_PLAYER_STATE_DATA =
            sizeof (Int32) +                                                        // Player ID
            sizeof (Int32);                                                         // Chips in Hand
        public const int SIZEOF_SEAT_STATE_DATA =
            SIZEOF_PLAYER_STATE_DATA +                                              // Seated Player State Data
            sizeof (byte) +                                                         // BoolSet #1 (didCheck, isPlaying)
            sizeof (Int32);                                                         // Current Wager
        public const int SIZEOF_TABLE_STATE_DATA =
            sizeof (Int32) +                                                        // Minimum Wager
            sizeof (Int32) +                                                        // Current Table Stake
            sizeof (Int32) +                                                        // Cash Pot
            (SIZEOF_SEAT_STATE_DATA * HoldEmPokerDefines.POKER_TABLE_CAPACITY) +    // Seat State Data Order
            SIZEOF_GAME_PHASE +                                                     // Current Game Phase
            sizeof (Int16) +                                                        // Current Turn Seat Index
            (SIZEOF_CARD_DATA * HoldEmPokerDefines.POKER_COMMUNITY_CARD_COUNT);     // Community Card Order

        // Sizes of Game Events
        // Connectivity
        public const int SIZEOF_POKER_GAME_EVENT_PLAYER_JOIN =
            SIZEOF_NETWORK_ACTIVITY_START +                                         // START Network Activity Stream
            SIZEOF_NETWORK_ACTIVITY_ID +                                            // Network Activity ID
            sizeof (Int32) +                                                        // Game ID
            sizeof (Int32) +                                                        // Player ID
            sizeof (Int32) +                                                        // Buy-In Chips
            SIZEOF_NETWORK_ACTIVITY_END;                                            // END Network Activity Stream
        public const int SIZEOF_POKER_GAME_EVENT_PLAYER_LEAVE =
            SIZEOF_NETWORK_ACTIVITY_START +                                         // START Network Activity Stream
            SIZEOF_NETWORK_ACTIVITY_ID +                                            // Network Activity ID
            sizeof (Int32) +                                                        // Game ID
            sizeof (Int32) +                                                        // Player ID
            SIZEOF_NETWORK_ACTIVITY_END;                                            // END Network Activity Stream
        // Game Progression
        public const int SIZEOF_POKER_GAME_EVENT_ANTE_START =
            SIZEOF_NETWORK_ACTIVITY_START +                                         // START Network Activity Stream
            SIZEOF_NETWORK_ACTIVITY_ID +                                            // Network Activity ID
            sizeof (Int32) +                                                        // Game ID
            SIZEOF_NETWORK_ACTIVITY_END;                                            // END Network Activity Stream
        public const int SIZEOF_POKER_GAME_EVENT_ANTE_PHASE_CHANGE =
            SIZEOF_NETWORK_ACTIVITY_START +                                         // START Network Activity Stream
            SIZEOF_NETWORK_ACTIVITY_ID +                                            // Network Activity ID
            sizeof (Int32) +                                                        // Game ID
            SIZEOF_GAME_PHASE +                                                     // Ante Game Phase
            SIZEOF_NETWORK_ACTIVITY_END;                                            // END Network Activity Stream
        public const int SIZEOF_POKER_GAME_EVENT_ANTE_TURN_CHANGE =
            SIZEOF_NETWORK_ACTIVITY_START +                                         // START Network Activity Stream
            SIZEOF_NETWORK_ACTIVITY_ID +                                            // Network Activity ID
            sizeof (Int32) +                                                        // Game ID
            sizeof (Int16) +                                                        // New Seat Index
            SIZEOF_NETWORK_ACTIVITY_END;                                            // END Network Activity Stream
        public const int SIZEOF_POKER_GAME_EVENT_ANTE_END =
            SIZEOF_NETWORK_ACTIVITY_START +                                         // START Network Activity Stream
            SIZEOF_NETWORK_ACTIVITY_ID +                                            // Network Activity ID
            sizeof (Int32) +                                                        // Game ID
            SIZEOF_NETWORK_ACTIVITY_END;                                            // END Network Activity Stream
        // Card Dealing
        public const int SIZEOF_POKER_GAME_EVENT_PLAYER_CARD_DEAL =
            SIZEOF_NETWORK_ACTIVITY_START +                                         // START Network Activity Stream
            SIZEOF_NETWORK_ACTIVITY_ID +                                            // Network Activity ID
            sizeof (Int32) +                                                        // Game ID
            sizeof (Int32) +                                                        // Player ID
            (SIZEOF_CARD_DATA * HoldEmPokerDefines.POKER_PLAYER_DEAL_COUNT) +       // Cards Data
            SIZEOF_NETWORK_ACTIVITY_END;                                            // END Network Activity Stream
        public const int SIZEOF_POKER_GAME_EVENT_COMMUNITY_CARD_DEAL =
            SIZEOF_NETWORK_ACTIVITY_START +                                         // START Network Activity Stream
            SIZEOF_NETWORK_ACTIVITY_ID +                                            // Network Activity ID
            sizeof (Int32) +                                                        // Game ID
            SIZEOF_CARD_DATA +                                                      // Card Data
            sizeof (Int16) +                                                        // Card Position (Flop 1-2-3, Turn, River)
            SIZEOF_NETWORK_ACTIVITY_END;                                            // END Network Activity Stream
        // Player Action
        // Win Condention

        public const int BIT_COUNT_OF_BYTE = 8;
        public const int BIT_COUNT_OF_INT16 = 16;

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