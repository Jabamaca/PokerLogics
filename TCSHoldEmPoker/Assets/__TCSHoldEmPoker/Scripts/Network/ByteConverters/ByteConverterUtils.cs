using System;
using System.Collections.Generic;
using TCSHoldEmPoker.Data;
using TCSHoldEmPoker.Models;
using TCSHoldEmPoker.Models.Define;
using TCSHoldEmPoker.Network.Activity;
using TCSHoldEmPoker.Network.Activity.PokerGameInputs;
using TCSHoldEmPoker.Network.Activity.PokerGameEvents;
using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Data {
    public static class ByteConverterUtils {

        #region Basic Data Byte Size

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

        #endregion

        #region Structured Data Byte Size

        public static int SizeOf (PokerHand pokerHand) {
            int byteSize = 0;
            byteSize += SizeOf (pokerHand.HandRank);                                    // Hand Rank
            byteSize += 
                SizeOf (PokerCard.BLANK) * HoldEmPokerDefines.POKER_HAND_SIZE;          // 5 Card Order

            return byteSize;
        }

        public static int SizeOf (PlayerStateData playerStateData) {
            int byteSize = 0;
            byteSize += sizeof (Int32);                                                 // Player ID
            byteSize += sizeof (Int32);                                                 // Chips in Hand

            return byteSize;
        }

        public static int SizeOf (SeatStateData seatStateData) {
            int byteSize = 0;
            byteSize += SizeOf (seatStateData.seatedPlayerStateData);                   // Seated Player State Data
            byteSize += sizeof (byte);                                                  // BoolSet #1 (didCheck, isPlaying)
            byteSize += sizeof (Int32);                                                 // Current Wager

            return byteSize;
        }

        public static int SizeOf (PrizePotStateData prizePotStateData) {
            int byteSize = 0;
            byteSize += sizeof (Int32);                                                 // Prize Amount
            byteSize += sizeof (Int16);                                                 // Qualified Players Count
            foreach (var playerID in prizePotStateData.qualifiedPlayerIDs)
                byteSize += sizeof (Int32);                                             // Qualified Player IDs [ENUMERATE]

            return byteSize;
        }

        public static int SizeOf (TableStateData tableStateData) {
            int byteSize = 0;
            byteSize += sizeof (Int32);                                                 // Minimum Wager
            byteSize += sizeof (Int32);                                                 // Current Table Stake
            byteSize += SizeOf (tableStateData.mainPrizeStateData);                     // Main Prize Pot
            byteSize += sizeof (Int16);                                                 // Side Prize Pot Count
            foreach (var sidePrizePot in tableStateData.sidePrizeStateDataList)
                byteSize += SizeOf (sidePrizePot);                                      // Side Prize Pots [ENUMERATE]
            byteSize += sizeof (Int16);                                                 // Table Seat Count
            foreach (var seatStateData in tableStateData.seatStateDataOrder)
                byteSize += SizeOf (seatStateData);                                     // Table Seats [ENUMERATE]
            byteSize += SizeOf (tableStateData.currentGamePhase);                       // Current Game Phase
            byteSize += sizeof (Int16);                                                 // Current Turn Seat Index
            byteSize += SizeOf (PokerCard.BLANK) * 
                HoldEmPokerDefines.POKER_COMMUNITY_CARD_COUNT;                          // Community Card Order

            return byteSize;
        }

        #endregion

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

        #region Poker Game Input Byte Size

        private static int BaseSizeOf (PokerGameInput gameInput) {
            int byteSize = 0;
            byteSize += SIZEOF_NETWORK_ACTIVITY_START;                                  // START Network Activity Stream
            byteSize += SizeOf (NetworkActivityID.SAMPLE);                              // Network Activity ID
            byteSize += sizeof (Int32);                                                 // Game ID
            byteSize += sizeof (Int32);                                                 // Player ID
            /* [[VARIOUS SIZE TOTAL OF UNIQUE DATA]] */                                 // *** UNIQUE DATA (If Any) ***
            byteSize += SIZEOF_NETWORK_ACTIVITY_END;                                    // END Network Activity Stream

            return byteSize;
        }

        #region Connectivity

        public static int SizeOf (PlayerJoinRequestGameInput input) {
            int byteSize = 0;
            byteSize += BaseSizeOf (input);                                             // Data Signature and Common Data
            byteSize += sizeof (Int32);                                                 // Buy-In Chips

            return byteSize;
        }

        public static int SizeOf (PlayerLeaveRequestGameInput input) {
            int byteSize = 0;
            byteSize += BaseSizeOf (input);                                             // Data Signature and Common Data

            return byteSize;
        }

        #endregion

        #region Player Action

        public const int SIZEOF_POKER_GAME_INPUT_PLAYER_ACTION_BET_CHECK =
            SIZEOF_POKER_GAME_INPUT_COMMON_DATA;                                    // Input Data Signature and Common Data
        public const int SIZEOF_POKER_GAME_INPUT_PLAYER_ACTION_BET_CALL =
            SIZEOF_POKER_GAME_INPUT_COMMON_DATA;                                    // Input Data Signature and Common Data
        public const int SIZEOF_POKER_GAME_INPUT_PLAYER_ACTION_BET_RAISE =
            SIZEOF_POKER_GAME_INPUT_COMMON_DATA +                                   // Input Data Signature and Common Data
            sizeof (Int32);                                                         // New Stake
        public const int SIZEOF_POKER_GAME_INPUT_PLAYER_ACTION_BET_FOLD =
            SIZEOF_POKER_GAME_INPUT_COMMON_DATA;                                    // Input Data Signature and Common Data

        #endregion

        #endregion

        #region Game State Update Byte Size

        public static int SizeOf (PokerGameStateUpdate gameStateUpdate) {
            int byteSize = 0;
            byteSize += SIZEOF_NETWORK_ACTIVITY_START;                                  // START Network Activity Stream
            byteSize += SizeOf (NetworkActivityID.SAMPLE);                              // Network Activity ID
            byteSize += sizeof (Int32);                                                 // Game ID
            byteSize += SizeOf (gameStateUpdate.updatedTableStateData);                 // Updated Table State Data
            byteSize += SIZEOF_NETWORK_ACTIVITY_END;                                    // END Network Activity Stream

            return byteSize;
        }

        #endregion

        #region Utility Methods

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

        #endregion

    }
}