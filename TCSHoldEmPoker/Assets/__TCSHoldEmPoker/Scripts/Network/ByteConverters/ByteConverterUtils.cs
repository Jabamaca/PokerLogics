using System;
using System.Collections.Generic;
using TCSHoldEmPoker.Models;
using TCSHoldEmPoker.Models.Define;
using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Data {
    public static class ByteConverterUtils {

        // Sizes of Basic Data

        private const int SIZEOF_CARD_DATA = sizeof (byte);
        public static int SizeOf (PokerCard pokerCard) {
            return SIZEOF_CARD_DATA;
        }

        private const int SIZEOF_HAND_RANK = sizeof (byte);
        public static int SizeOf (PokerHandRankEnum handRank) {
            return SIZEOF_HAND_RANK;
        }

        private const int SIZEOF_GAME_PHASE = sizeof (byte);
        public static int SizeOf (PokerGamePhaseEnum gamePhase) {
            return SIZEOF_GAME_PHASE;
        }

        private const int SIZEOF_NETWORK_ACTIVITY_ID = sizeof (Int16);
        public static int SizeOf (NetworkActivityID networkActivityID) {
            return SIZEOF_NETWORK_ACTIVITY_ID;
        }

        public const UInt16 NETWORK_ACTIVITY_START = 0xF99F;
        public const int SIZEOF_NETWORK_ACTIVITY_START = sizeof (UInt16);

        public const UInt16 NETWORK_ACTIVITY_END = 0xF66F;
        public const int SIZEOF_NETWORK_ACTIVITY_END = sizeof (UInt16);

        // Sizes of Structured Data
        public static int SizeOf (PokerHand pokerHand) {
            int byteSize = 0;
            byteSize += SizeOf (pokerHand.HandRank);
            foreach (var card in pokerHand.CardOrder)
                byteSize += SizeOf (card);

            return byteSize;
        }
        public const int SIZEOF_PLAYER_STATE_DATA =
            sizeof (Int32) +                                                        // Player ID
            sizeof (Int32);                                                         // Chips in Hand
        public const int SIZEOF_SEAT_STATE_DATA =
            SIZEOF_PLAYER_STATE_DATA +                                              // Seated Player State Data
            sizeof (byte) +                                                         // BoolSet #1 (didCheck, isPlaying)
            sizeof (Int32);                                                         // Current Wager
        public const int SIZEOF_TABLE_STATE_DATA_BASE =
            sizeof (Int32) +                                                        // Minimum Wager
            sizeof (Int32) +                                                        // Current Table Stake
            SIZEOF_PRIZE_POT_STATE_DATA_BASE +                                      // Main Prize Pot
            /* (Qualified Player Count) * (Player ID) */                            // Main Prize Pot Qualified Players Data (Flexible number)
            sizeof (Int16) +                                                        // Side Prize Pot Count
            /* (Side Prize Pot Count) * [[VARIED PRIZE POT SIZE]]  */               // Side Prize Pots Data (Flexible number)
            (SIZEOF_SEAT_STATE_DATA * HoldEmPokerDefines.POKER_TABLE_CAPACITY) +    // Seat State Data Order
            SIZEOF_GAME_PHASE +                                                     // Current Game Phase
            sizeof (Int16) +                                                        // Current Turn Seat Index
            (SIZEOF_CARD_DATA * HoldEmPokerDefines.POKER_COMMUNITY_CARD_COUNT);     // Community Card Order
        public const int SIZEOF_PRIZE_POT_STATE_DATA_BASE =
            sizeof (Int32) +                                                        // Prize Amount
            sizeof (Int16);                                                         // Qualified Players Count
            /* (Player Count) * (Player ID) */                                      // Qualified Players Data (Flexible number)
        public const int SIZEOF_PRIZE_POT_STATE_DATA_PLAYER = 
            sizeof (Int32);                                                         // Player ID

        // Sizes of Game Events
        // COMMON DATA
        public const int SIZEOF_POKER_GAME_EVENT_COMMON_DATA =
            SIZEOF_NETWORK_ACTIVITY_START +                                         // START Network Activity Stream
            SIZEOF_NETWORK_ACTIVITY_ID +                                            // Network Activity ID
            sizeof (Int32) +                                                        // Game ID
            /* [[VARIOUS SIZE TOTAL OF UNIQUE DATA]] */                             // *** UNIQUE DATA (If Any) ***
            SIZEOF_NETWORK_ACTIVITY_END;                                            // END Network Activity Stream
        // Connectivity
        public const int SIZEOF_POKER_GAME_EVENT_PLAYER_JOIN =
            SIZEOF_POKER_GAME_EVENT_COMMON_DATA +                                   // Event Data Signature and Common Data
            sizeof (Int32) +                                                        // Player ID
            sizeof (Int32);                                                         // Buy-In Chips
        public const int SIZEOF_POKER_GAME_EVENT_PLAYER_LEAVE =
            SIZEOF_POKER_GAME_EVENT_COMMON_DATA +                                   // Event Data Signature and Common Data
            sizeof (Int32);                                                         // Player ID
        // Game Progression
        public const int SIZEOF_POKER_GAME_EVENT_ANTE_START =
            SIZEOF_POKER_GAME_EVENT_COMMON_DATA;                                    // Event Data Signature and Common Data
        public const int SIZEOF_POKER_GAME_EVENT_ANTE_PHASE_CHANGE =
            SIZEOF_POKER_GAME_EVENT_COMMON_DATA +                                   // Event Data Signature and Common Data
            SIZEOF_GAME_PHASE;                                                      // Ante Game Phase
        public const int SIZEOF_POKER_GAME_EVENT_ANTE_TURN_CHANGE =
            SIZEOF_POKER_GAME_EVENT_COMMON_DATA +                                   // Event Data Signature and Common Data
            sizeof (Int16);                                                         // New Seat Index
        public const int SIZEOF_POKER_GAME_EVENT_ANTE_END =
            SIZEOF_POKER_GAME_EVENT_COMMON_DATA;                                    // Event Data Signature and Common Data
        // Card Dealing
        public const int SIZEOF_POKER_GAME_EVENT_PLAYER_CARD_DEAL =
            SIZEOF_POKER_GAME_EVENT_COMMON_DATA +                                   // Event Data Signature and Common Data
            sizeof (Int32) +                                                        // Player ID
            (SIZEOF_CARD_DATA * HoldEmPokerDefines.POKER_PLAYER_DEAL_COUNT);        // Cards Data
        public const int SIZEOF_POKER_GAME_EVENT_COMMUNITY_CARD_DEAL =
            SIZEOF_POKER_GAME_EVENT_COMMON_DATA +                                   // Event Data Signature and Common Data
            SIZEOF_CARD_DATA +                                                      // Card Data
            sizeof (Int16);                                                         // Card Position (Flop 1-2-3, Turn, River)
        // Player Action
        public const int SIZEOF_POKER_GAME_EVENT_BET_BLIND =
            SIZEOF_POKER_GAME_EVENT_COMMON_DATA +                                   // Event Data Signature and Common Data
            sizeof (Int32) +                                                        // Player ID
            sizeof (Int32);                                                         // Chips Spent
        public const int SIZEOF_POKER_GAME_EVENT_BET_CALL =
            SIZEOF_POKER_GAME_EVENT_COMMON_DATA +                                   // Event Data Signature and Common Data
            sizeof (Int32) +                                                        // Player ID
            sizeof (Int32);                                                         // Chips Spent
        public const int SIZEOF_POKER_GAME_EVENT_BET_CHECK =
            SIZEOF_POKER_GAME_EVENT_COMMON_DATA +                                   // Event Data Signature and Common Data
            sizeof (Int32);                                                         // Player ID
        public const int SIZEOF_POKER_GAME_EVENT_BET_RAISE =
            SIZEOF_POKER_GAME_EVENT_COMMON_DATA +                                   // Event Data Signature and Common Data
            sizeof (Int32) +                                                        // Player ID
            sizeof (Int32);                                                         // Chips Spent
        public const int SIZEOF_POKER_GAME_EVENT_BET_FOLD =
            SIZEOF_POKER_GAME_EVENT_COMMON_DATA +                                   // Event Data Signature and Common Data
            sizeof (Int32);                                                         // Player ID
        // Win Condition
        public const int SIZEOF_POKER_GAME_EVENT_TABLE_UPDATE_MAIN_POT =
            SIZEOF_POKER_GAME_EVENT_COMMON_DATA +                                   // Event Data Signature and Common Data
            sizeof (Int32);                                                         // Updated Prize Total
        public const int SIZEOF_POKER_GAME_EVENT_TABLE_CREATE_SIDE_POT =
            SIZEOF_POKER_GAME_EVENT_COMMON_DATA +                                   // Event Data Signature and Common Data
            sizeof (Int32);                                                         // Side Pot Prize Total
        public const int SIZEOF_POKER_GAME_EVENT_ALL_PLAYER_CARDS_REVEAL_BASE =
            SIZEOF_POKER_GAME_EVENT_COMMON_DATA +                                   // Event Data Signature and Common Data
            sizeof (Int16);                                                         // Participating Players Count
            /* (Player Count) * (Player ID + Cards Data) */                         // Participating Players Data (Flexible number)
        public const int SIZEOF_POKER_GAME_EVENT_ALL_PLAYER_CARDS_REVEAL_PLAYER =
            sizeof (Int32) +                                                        // Player ID
            (SIZEOF_CARD_DATA * HoldEmPokerDefines.POKER_PLAYER_DEAL_COUNT);        // Cards Data
        public const int SIZEOF_POKER_GAME_EVENT_PLAYER_WIN =
            SIZEOF_POKER_GAME_EVENT_COMMON_DATA +                                   // Event Data Signature and Common Data
            sizeof (Int32) +                                                        // Player ID
            sizeof (Int32);                                                         // Chips Won

        // Sizes of Game Input
        // COMMON DATA
        public const int SIZEOF_POKER_GAME_INPUT_COMMON_DATA =
            SIZEOF_NETWORK_ACTIVITY_START +                                         // START Network Activity Stream
            SIZEOF_NETWORK_ACTIVITY_ID +                                            // Network Activity ID
            sizeof (Int32) +                                                        // Game ID
            sizeof (Int32) +                                                        // Player ID
            /* [[VARIOUS SIZE TOTAL OF UNIQUE DATA]] */                             // *** UNIQUE DATA (If Any) ***
            SIZEOF_NETWORK_ACTIVITY_END;                                            // END Network Activity Stream
        // Connectivity
        public const int SIZEOF_POKER_GAME_INPUT_PLAYER_REQUEST_JOIN =
            SIZEOF_POKER_GAME_INPUT_COMMON_DATA +                                   // Input Data Signature and Common Data
            sizeof (Int32);                                                         // Buy-In Chips
        public const int SIZEOF_POKER_GAME_INPUT_PLAYER_REQUEST_LEAVE =
            SIZEOF_POKER_GAME_INPUT_COMMON_DATA;                                    // Input Data Signature and Common Data
        public const int SIZEOF_POKER_GAME_INPUT_PLAYER_ACTION_BET_CHECK =
            SIZEOF_POKER_GAME_INPUT_COMMON_DATA;                                    // Input Data Signature and Common Data
        public const int SIZEOF_POKER_GAME_INPUT_PLAYER_ACTION_BET_CALL =
            SIZEOF_POKER_GAME_INPUT_COMMON_DATA;                                    // Input Data Signature and Common Data
        public const int SIZEOF_POKER_GAME_INPUT_PLAYER_ACTION_BET_RAISE =
            SIZEOF_POKER_GAME_INPUT_COMMON_DATA +                                   // Input Data Signature and Common Data
            sizeof (Int32);                                                         // New Stake
        public const int SIZEOF_POKER_GAME_INPUT_PLAYER_ACTION_BET_FOLD =
            SIZEOF_POKER_GAME_INPUT_COMMON_DATA;                                    // Input Data Signature and Common Data

        // Size of Game State Update
        public const int SIZEOF_POKER_GAME_STATE_UPDATE_BASE =
            SIZEOF_NETWORK_ACTIVITY_START +                                         // START Network Activity Stream
            SIZEOF_NETWORK_ACTIVITY_ID +                                            // Network Activity ID
            sizeof (Int32) +                                                        // Game ID
            SIZEOF_TABLE_STATE_DATA_BASE +                                          // Updated Table State Data
            SIZEOF_NETWORK_ACTIVITY_END;                                            // END Network Activity Stream

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

        internal static void AddBytesToArray (IEnumerable<byte> bytesToAdd, byte[] bytesLocation, ref int currentDataIndex) {
            foreach (byte byteToAdd in bytesToAdd) {
                bytesLocation[currentDataIndex] = byteToAdd;
                currentDataIndex++;
            }
        }

        internal static void AddByteToArray (byte byteToAdd, byte[] bytesLocation, ref int currentDataIndex) {
            bytesLocation[currentDataIndex] = byteToAdd;
            currentDataIndex++;
        }

        internal static void BytesToBoolArray (byte[] bytes, int byteCount, ref int currentDataIndex, out bool[] boolArray) {
            if (byteCount <= 0) {
                boolArray = new bool[0];
                return;
            }

            boolArray = new bool[BIT_COUNT_OF_BYTE * byteCount];
            int boolIndex = 0;

            for (int i = 0; i < byteCount; i++) {
                byte boolByte = bytes[currentDataIndex];
                foreach (bool b in ByteConverterUtils.BoolArrayFromByte (boolByte)) {
                    boolArray[boolIndex] = b;
                    boolIndex++;
                }
                currentDataIndex++;
            }
        }

        internal static void BytesToInt32 (byte[] bytes, ref int currentDataIndex, out Int32 outValue) {
            outValue = BitConverter.ToInt32 (bytes, startIndex: currentDataIndex);
            currentDataIndex += sizeof (Int32);
        }

        internal static void BytesToInt16 (byte[] bytes, ref int currentDataIndex, out Int16 outValue) {
            outValue = BitConverter.ToInt16 (bytes, startIndex: currentDataIndex);
            currentDataIndex += sizeof (Int16);
        }
    }
}