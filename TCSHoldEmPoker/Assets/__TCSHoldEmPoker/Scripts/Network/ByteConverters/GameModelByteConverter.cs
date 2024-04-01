using System;
using System.Collections.Generic;
using TCSHoldEmPoker.Models;
using TCSHoldEmPoker.Models.Define;
using TCSHoldEmPoker.Data;

namespace TCSHoldEmPoker.Network.Data {
    public static class GameModelByteConverter {

        #region Methods

        // POKER CARD

        public static PokerCard ByteToPokerCard (byte cardByte) {
            CardValueEnum value = (CardValueEnum)(cardByte & 0xF0);     // 1st half-byte is value.
            CardSuitEnum suit = (CardSuitEnum)(cardByte & 0x0F);        // 2nd half-byte is suit.

            return new PokerCard (suit, value);
        }

        public static byte ByteFromPokerCard (PokerCard card) {
            return (byte)((byte)card.Value | (byte)card.Suit);
        }

        // POKER HAND

        public static bool BytesToPokerHand (byte[] bytes, out PokerHand hand, int startIndex = 0) {
            if (bytes.Length - startIndex < ByteConverterUtils.SIZEOF_HAND_DATA) {
                hand = null;
                return false;
            }

            int i = startIndex;
            // Hand Rank
            PokerHandRankEnum handRank = (PokerHandRankEnum)bytes[i];
            i += ByteConverterUtils.SIZEOF_HAND_RANK;
            // Card Order
            List<PokerCard> cards = new ();
            for (int c = 0; c < HoldEmPokerDefines.POKER_HAND_SIZE; c++) {
                cards.Add (ByteToPokerCard (bytes[i]));
                i += ByteConverterUtils.SIZEOF_CARD_DATA;
            }

            hand = new PokerHand (handRank, cards);
            return true;
        }

        public static byte[] BytesFromPokerHand (PokerHand hand) {
            if (hand == null)
                return null;

            byte[] returnBytes = new byte[ByteConverterUtils.SIZEOF_HAND_DATA];

            int i = 0;
            // Hand Rank
            returnBytes[i] = (byte)hand.HandRank;
            i += ByteConverterUtils.SIZEOF_HAND_RANK;
            // Card Order
            foreach (var card in hand.CardOrder) {
                returnBytes[i] = ByteFromPokerCard (card);
                i += ByteConverterUtils.SIZEOF_CARD_DATA;
            }

            return returnBytes;
        }

        // PLAYER STATE DATA

        public static bool BytesToPlayerStateData (byte[] bytes, out PlayerStateData playerStateData, int startIndex = 0) {
            if (bytes.Length - startIndex < ByteConverterUtils.SIZEOF_PLAYER_STATE_DATA) {
                playerStateData = null;
                return false;
            }


            int i = startIndex;
            // Player ID
            Int32 playerID = BitConverter.ToInt32 (bytes, startIndex: i);
            i += sizeof (Int32);
            // Chips in Hand
            Int32 chipsInHand = BitConverter.ToInt32 (bytes, startIndex: i);

            playerStateData = new () {
                playerID = playerID,
                chipsInHand = chipsInHand,
            };
            return true;
        }

        public static byte[] BytesFromPlayerStateData (PlayerStateData playerStateData) {
            if (playerStateData == null)
                return null;

            byte[] returnBytes = new byte[ByteConverterUtils.SIZEOF_PLAYER_STATE_DATA];

            int i = 0;
            // Player ID
            foreach (byte playerIDByte in BitConverter.GetBytes (playerStateData.playerID)) {
                returnBytes[i] = playerIDByte;
                i++;
            }
            // Chips in Hand
            foreach (byte chipsByte in BitConverter.GetBytes (playerStateData.chipsInHand)) {
                returnBytes[i] = chipsByte;
                i++;
            }

            return returnBytes;
        }

        // SEAT STATE DATA

        public static bool BytesToSeatStateData (byte[] bytes, out SeatStateData seatStateData, int startIndex = 0) {
            if (bytes.Length - startIndex < ByteConverterUtils.SIZEOF_SEAT_STATE_DATA) {
                seatStateData = null;
                return false;
            }

            int i = startIndex;
            // Seated Player State Data
            if (!BytesToPlayerStateData (bytes, out var playerStateData, startIndex: i)) {
                seatStateData = null;
                return false;
            }
            i += ByteConverterUtils.SIZEOF_PLAYER_STATE_DATA;
            // Boolean Property Set 1
            byte boolByte = bytes[i];
            bool[] boolArray = ByteConverterUtils.BoolArrayFromByte (boolByte);
            bool didCheck = boolArray[0];
            bool isPlaying = boolArray[1];
            i++;
            // Current Wager
            Int32 currentWager = BitConverter.ToInt32 (bytes, startIndex: i);

            seatStateData = new () {
                seatedPlayerStateData = playerStateData,
                didCheck = didCheck,
                isPlaying = isPlaying,
                currentWager = currentWager,
            };
            return true;
        }

        public static byte[] BytesFromSeatStateData (SeatStateData seatStateData) {
            if (seatStateData == null)
                return null;

            byte[] returnBytes = new byte[ByteConverterUtils.SIZEOF_SEAT_STATE_DATA];

            int i = 0;
            // Seated Player State Data
            foreach (byte playerStateByte in BytesFromPlayerStateData (seatStateData.seatedPlayerStateData)) {
                returnBytes[i] = playerStateByte;
                i++;
            }
            // Boolean Property Set 1
            returnBytes[i] = ByteConverterUtils.BoolArrayToByte (new bool[] {
                seatStateData.didCheck, seatStateData.isPlaying,
                false, false, 
                false, false, 
                false, false,
            });
            i++;
            // Current Wager
            foreach (byte chipsByte in BitConverter.GetBytes (seatStateData.currentWager)) {
                returnBytes[i] = chipsByte;
                i++;
            }

            return returnBytes;
        }

        // TABLE STATE DATA

        public static bool BytesToTableStateData (byte[] bytes, out TableStateData tableStateData, int startIndex = 0) {
            if (bytes.Length - startIndex < ByteConverterUtils.SIZEOF_TABLE_STATE_DATA) {
                tableStateData = null;
                return false;
            }

            int i = startIndex;
            // Minimum Wager
            Int32 minimumWager = BitConverter.ToInt32 (bytes, startIndex: i);
            i += sizeof (Int32);
            // Current Table Stake
            Int32 currentTableStake = BitConverter.ToInt32 (bytes, startIndex: i);
            i += sizeof (Int32);
            // Cash Pot
            Int32 cashPot = BitConverter.ToInt32 (bytes, startIndex: i);
            i += sizeof (Int32);
            // Seat State Data Order
            List<SeatStateData> seatStateDataOrder = new ();
            for (int s = 0; s < HoldEmPokerDefines.POKER_TABLE_CAPACITY; s++) {
                if (!BytesToSeatStateData (bytes, out var seatStateData, startIndex: i)) {
                    tableStateData = null;
                    return false;
                }
                seatStateDataOrder.Add (seatStateData);
                i += ByteConverterUtils.SIZEOF_SEAT_STATE_DATA;
            }
            // Current Game Phase
            PokerGamePhaseEnum currentGamePhase = (PokerGamePhaseEnum)bytes[i];
            i += ByteConverterUtils.SIZEOF_GAME_PHASE;
            // Current Turning Seat Index
            Int16 currentTurnIndex = bytes[i];
            i++;
            // Community Card Order
            List<PokerCard> communityCardOrder = new ();
            for (int c = 0; c < HoldEmPokerDefines.POKER_COMMUNITY_CARD_COUNT; c++) {
                PokerCard card = ByteToPokerCard (bytes[i]);
                communityCardOrder.Add (card);
                i += ByteConverterUtils.SIZEOF_CARD_DATA;
            }

            tableStateData = new () {
                minimumWager = minimumWager,
                currentTableStake = currentTableStake,
                cashPot = cashPot,
                seatStateDataOrder = seatStateDataOrder,
                currentGamePhase = currentGamePhase,
                currentTurnPlayerIndex = currentTurnIndex,
                communityCardsOrder = communityCardOrder,
            };
            return true;
        }

        public static byte[] BytesFromTableStateData (TableStateData tableStateData) {
            if (tableStateData == null)
                return null;

            byte[] returnBytes = new byte[ByteConverterUtils.SIZEOF_TABLE_STATE_DATA];

            int i = 0;
            // Minimum Wager
            foreach (byte minWagerByte in BitConverter.GetBytes (tableStateData.minimumWager)) {
                returnBytes[i] = minWagerByte;
                i++;
            }
            // Current Table Stake
            foreach (byte tableStakeByte in BitConverter.GetBytes (tableStateData.currentTableStake)) {
                returnBytes[i] = tableStakeByte;
                i++;
            }
            // Cash Pot
            foreach (byte potByte in BitConverter.GetBytes (tableStateData.cashPot)) {
                returnBytes[i] = potByte;
                i++;
            }
            // Seat State Data Order
            for (int s = 0; s < HoldEmPokerDefines.POKER_TABLE_CAPACITY; s++) {
                var seatStateData = tableStateData.seatStateDataOrder[s];
                foreach (byte seatStateByte in BytesFromSeatStateData (seatStateData)) {
                    returnBytes[i] = seatStateByte;
                    i++;
                }
            }
            // Current Game Phase
            returnBytes[i] = (byte)tableStateData.currentGamePhase;
            i++;
            // Current Turn Seat Index
            returnBytes[i] = (byte)tableStateData.currentTurnPlayerIndex;
            i++;
            // Community Card Order
            for (int c = 0; c < HoldEmPokerDefines.POKER_COMMUNITY_CARD_COUNT; c++) {
                var card = tableStateData.communityCardsOrder[c];
                returnBytes[i] = ByteFromPokerCard (card);
                i++;
            }

            return returnBytes;
        }

        #endregion

    }
}