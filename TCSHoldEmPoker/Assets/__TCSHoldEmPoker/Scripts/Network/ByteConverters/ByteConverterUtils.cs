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

        #region Poker Game Event Byte Size

        private static int BaseSizeOf (PokerGameEvent evt) {
            int byteSize = 0;
            byteSize += SIZEOF_NETWORK_ACTIVITY_START;                                  // START Network Activity Stream
            byteSize += SizeOf (NetworkActivityID.SAMPLE);                              // Network Activity ID
            byteSize += sizeof (Int32);                                                 // Game ID
            /* [[VARIOUS SIZE TOTAL OF UNIQUE DATA]] */                                 // *** UNIQUE DATA (If Any) ***
            byteSize += SIZEOF_NETWORK_ACTIVITY_END;                                    // END Network Activity Stream

            return byteSize;
        }

        #region Connectivity

        public static int SizeOf (PlayerJoinGameEvent evt) {
            int byteSize = 0;
            byteSize += BaseSizeOf (evt);                                               // Data Signature and Common Data
            byteSize += sizeof (Int32);                                                 // Player ID
            byteSize += sizeof (Int32);                                                 // Buy-In Chips

            return byteSize;
        }

        public static int SizeOf (PlayerLeaveGameEvent evt) {
            int byteSize = 0;
            byteSize += BaseSizeOf (evt);                                               // Data Signature and Common Data
            byteSize += sizeof (Int32);                                                 // Player ID

            return byteSize;
        }

        public static int SizeOf (PlayerGetKickedGameEvent evt) {
            int byteSize = 0;
            byteSize += BaseSizeOf (evt);                                               // Data Signature and Common Data
            byteSize += sizeof (Int32);                                                 // Player ID

            return byteSize;
        }

        #endregion

        #region Game Progression

        public static int SizeOf (AnteStartGameEvent evt) {
            int byteSize = 0;
            byteSize += BaseSizeOf (evt);                                               // Data Signature and Common Data

            return byteSize;
        }

        public static int SizeOf (GamePhaseChangeGameEvent evt) {
            int byteSize = 0;
            byteSize += BaseSizeOf (evt);                                               // Data Signature and Common Data
            byteSize += SizeOf (PokerGamePhaseEnum.SAMPLE);                             // New Ante Game Phase

            return byteSize;
        }

        public static int SizeOf (ChangeTurnSeatIndexGameEvent evt) {
            int byteSize = 0;
            byteSize += BaseSizeOf (evt);                                               // Data Signature and Common Data
            byteSize += sizeof (Int16);                                                 // Turning Seat Index

            return byteSize;
        }

        public static int SizeOf (AnteEndGameEvent evt) {
            int byteSize = 0;
            byteSize += BaseSizeOf (evt);                                               // Data Signature and Common Data

            return byteSize;
        }

        #endregion

        #region Card Dealing

        public static int SizeOf (PlayerCardsDealGameEvent evt) {
            int byteSize = 0;
            byteSize += BaseSizeOf (evt);                                               // Data Signature and Common Data
            byteSize += sizeof (Int32);                                                 // Player ID
            byteSize += SizeOf (PokerCard.BLANK) * HoldEmPokerDefines.POKER_PLAYER_DEAL_COUNT;  // Cards Data

            return byteSize;
        }

        public static int SizeOf (CommunityCardDealGameEvent evt) {
            int byteSize = 0;
            byteSize += BaseSizeOf (evt);                                               // Data Signature and Common Data
            byteSize += SizeOf (PokerCard.BLANK);                                       // Card Data
            byteSize += sizeof (Int16);                                                 // Card Position (Flop 1-2-3, Turn, River)

            return byteSize;
        }

        #endregion

        #region Player Action

        public static int SizeOf (PlayerBetBlindGameEvent evt) {
            int byteSize = 0;
            byteSize += BaseSizeOf (evt);                                               // Data Signature and Common Data
            byteSize += sizeof (Int32);                                                 // Player ID
            byteSize += sizeof (Int32);                                                 // Chips Spent

            return byteSize;
        }

        public static int SizeOf (PlayerBetCallGameEvent evt) {
            int byteSize = 0;
            byteSize += BaseSizeOf (evt);                                               // Data Signature and Common Data
            byteSize += sizeof (Int32);                                                 // Player ID
            byteSize += sizeof (Int32);                                                 // Chips Spent

            return byteSize;
        }

        public static int SizeOf (PlayerBetCheckGameEvent evt) {
            int byteSize = 0;
            byteSize += BaseSizeOf (evt);                                               // Data Signature and Common Data
            byteSize += sizeof (Int32);                                                 // Player ID

            return byteSize;
        }

        public static int SizeOf (PlayerBetRaiseGameEvent evt) {
            int byteSize = 0;
            byteSize += BaseSizeOf (evt);                                               // Data Signature and Common Data
            byteSize += sizeof (Int32);                                                 // Player ID
            byteSize += sizeof (Int32);                                                 // Chips Spent

            return byteSize;
        }

        public static int SizeOf (PlayerFoldGameEvent evt) {
            int byteSize = 0;
            byteSize += BaseSizeOf (evt);                                               // Data Signature and Common Data
            byteSize += sizeof (Int32);                                                 // Player ID

            return byteSize;
        }

        #endregion

        #region Win Condition

        public static int SizeOf (UpdateMainPrizePotGameEvent evt) {
            int byteSize = 0;
            byteSize += BaseSizeOf (evt);                                               // Data Signature and Common Data
            byteSize += sizeof (Int32);                                                 // Wager per Player

            return byteSize;
        }

        public static int SizeOf (CreateSidePrizePotGameEvent evt) {
            int byteSize = 0;
            byteSize += BaseSizeOf (evt);                                               // Data Signature and Common Data
            byteSize += sizeof (Int32);                                                 // Wager per Player

            return byteSize;
        }

        public static int SizeOf (AllPlayerCardsRevealGameEvent evt) {
            int byteSize = 0;
            byteSize += BaseSizeOf (evt);                                               // Data Signature and Common Data
            byteSize += sizeof (Int16);                                                 // Participating Players Count
            foreach (var handKVP in evt.revealedHands) {
                byteSize += sizeof (Int32);                                             // Participating Player ID [ENUMERATE]
                byteSize += 
                    SizeOf (PokerCard.BLANK) * HoldEmPokerDefines.POKER_PLAYER_DEAL_COUNT; // Participating Player Dealt Cards [ENUMERATE]
            }

            return byteSize;
        }

        public static int SizeOf (PlayerWinGameEvent evt) {
            int byteSize = 0;
            byteSize += BaseSizeOf (evt);                                               // Data Signature and Common Data
            byteSize += sizeof (Int32);                                                 // Player ID
            byteSize += sizeof (Int32);                                                 // Prize Won

            return byteSize;
        }

        #endregion

        #endregion

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

        public static int SizeOf (PlayerBetCheckActionGameInput input) {
            int byteSize = 0;
            byteSize += BaseSizeOf (input);                                             // Data Signature and Common Data

            return byteSize;
        }

        public static int SizeOf (PlayerBetCallActionGameInput input) {
            int byteSize = 0;
            byteSize += BaseSizeOf (input);                                             // Data Signature and Common Data

            return byteSize;
        }

        public static int SizeOf (PlayerBetRaiseActionGameInput input) {
            int byteSize = 0;
            byteSize += BaseSizeOf (input);                                             // Data Signature and Common Data
            byteSize += sizeof (Int32);                                                 // New Stake

            return byteSize;
        }

        public static int SizeOf (PlayerBetFoldActionGameInput input) {
            int byteSize = 0;
            byteSize += BaseSizeOf (input);                                             // Data Signature and Common Data

            return byteSize;
        }

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