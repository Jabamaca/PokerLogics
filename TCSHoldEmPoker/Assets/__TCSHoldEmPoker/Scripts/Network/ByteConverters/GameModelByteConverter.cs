using System;
using System.Collections.Generic;
using TCSHoldEmPoker.Models;
using TCSHoldEmPoker.Models.Define;
using TCSHoldEmPoker.Data;

namespace TCSHoldEmPoker.Network.Data {
    public static class GameModelByteConverter {

        #region Methods

        // POKER CARD

        public static int BytesToPokerCard (byte[] bytes, out PokerCard card, int startIndex = 0) {
            int i = startIndex;

            byte cardByte = bytes[i];
            CardValueEnum value = (CardValueEnum)(cardByte & 0xF0);     // 1st half-byte is value.
            CardSuitEnum suit = (CardSuitEnum)(cardByte & 0x0F);        // 2nd half-byte is suit.
            card = new PokerCard (suit, value);
            i += ByteConverterUtils.SIZEOF_CARD_DATA;

            return i;
        }

        public static byte[] BytesFromPokerCard (PokerCard card) {
            return new byte[] {
                (byte)((byte)card.Value | (byte)card.Suit)
            };
        }

        // POKER HAND RANK

        public static int BytesToHandRank (byte[] bytes, out PokerHandRankEnum handRank, int startIndex) {
            int i = startIndex;

            handRank = (PokerHandRankEnum)bytes[i];
            i += ByteConverterUtils.SIZEOF_HAND_RANK;

            return i;
        }

        public static byte[] BytesFromHandRank (PokerHandRankEnum handRank) {
            return new byte[] {
                (byte)handRank
            };
        }

        // POKER GAME PHASE

        public static int BytesToGamePhase (byte[] bytes, out PokerGamePhaseEnum gamePhase, int startIndex) {
            int i = startIndex;

            gamePhase = (PokerGamePhaseEnum)bytes[i];
            i += ByteConverterUtils.SIZEOF_GAME_PHASE;

            return i;
        }

        public static byte[] BytesFromGamePhase (PokerGamePhaseEnum gamePhase) {
            return new byte[] {
                (byte)gamePhase
            };
        }

        // POKER HAND

        public static int BytesToPokerHand (byte[] bytes, out PokerHand hand, int startIndex = 0) {
            int i = startIndex;

            // Hand Rank
            i = BytesToHandRank (bytes, out var handRank, startIndex: i);
            // Card Order
            List<PokerCard> cards = new ();
            for (int c = 0; c < HoldEmPokerDefines.POKER_HAND_SIZE; c++) {
                i = BytesToPokerCard (bytes, out var card, startIndex: i);
                cards.Add (card);
            }

            hand = new PokerHand (handRank, cards);
            return i;
        }

        public static byte[] BytesFromPokerHand (PokerHand hand) {
            byte[] returnBytes = new byte[ByteConverterUtils.SIZEOF_HAND_DATA];

            int i = 0;
            // Hand Rank
            i = ByteConverterUtils.AddBytesToArray (bytesToAdd: BytesFromHandRank (hand.HandRank), bytesLocation: returnBytes, startIndex: i);
            // Card Order
            foreach (var card in hand.CardOrder) {
                i = ByteConverterUtils.AddBytesToArray (bytesToAdd: BytesFromPokerCard (card), bytesLocation: returnBytes, startIndex: i);
            }

            return returnBytes;
        }

        // PLAYER STATE DATA

        public static int BytesToPlayerStateData (byte[] bytes, out PlayerStateData playerStateData, int startIndex = 0) {
            int i = startIndex;
            // Player ID
            i = ByteConverterUtils.BytesToInt32 (bytes, out var playerID, startIndex: i);
            // Chips in Hand
            i = ByteConverterUtils.BytesToInt32 (bytes, out var chipsInHand, startIndex: i);

            playerStateData = new () {
                playerID = playerID,
                chipsInHand = chipsInHand,
            };
            return i;
        }

        public static byte[] BytesFromPlayerStateData (PlayerStateData playerStateData) {
            byte[] returnBytes = new byte[ByteConverterUtils.SIZEOF_PLAYER_STATE_DATA];

            int i = 0;
            // Player ID
            i = ByteConverterUtils.AddBytesToArray (bytesToAdd: BitConverter.GetBytes (playerStateData.playerID),
                bytesLocation: returnBytes, startIndex: i);
            // Chips in Hand
            i = ByteConverterUtils.AddBytesToArray (bytesToAdd: BitConverter.GetBytes (playerStateData.chipsInHand),
                bytesLocation: returnBytes, startIndex: i);

            return returnBytes;
        }

        // SEAT STATE DATA

        public static int BytesToSeatStateData (byte[] bytes, out SeatStateData seatStateData, int startIndex = 0) {
            int i = startIndex;

            // Seated Player State Data
            i = BytesToPlayerStateData (bytes, out var playerStateData, startIndex: i);
            // Boolean Property Set 1
            byte boolByte = bytes[i];
            bool[] boolArray = ByteConverterUtils.BoolArrayFromByte (boolByte);
            bool didCheck = boolArray[0];
            bool isPlaying = boolArray[1];
            i++;
            // Current Wager
            i = ByteConverterUtils.BytesToInt32 (bytes, out var currentWager, startIndex: i);

            seatStateData = new () {
                seatedPlayerStateData = playerStateData,
                didCheck = didCheck,
                isPlaying = isPlaying,
                currentWager = currentWager,
            };
            return i;
        }

        public static byte[] BytesFromSeatStateData (SeatStateData seatStateData) {
            byte[] returnBytes = new byte[ByteConverterUtils.SIZEOF_SEAT_STATE_DATA];

            int i = 0;
            // Seated Player State Data
            i = ByteConverterUtils.AddBytesToArray (bytesToAdd: BytesFromPlayerStateData (seatStateData.seatedPlayerStateData),
                bytesLocation: returnBytes, startIndex: i);
            // Boolean Property Set 1
            i = ByteConverterUtils.AddByteToArray (byteToAdd: ByteConverterUtils.BoolArrayToByte (new bool[] {
                seatStateData.didCheck, seatStateData.isPlaying,
                false, false, 
                false, false, 
                false, false,
            }), bytesLocation: returnBytes, startIndex: i);
            // Current Wager
            i = ByteConverterUtils.AddBytesToArray (bytesToAdd: BitConverter.GetBytes (seatStateData.currentWager),
                bytesLocation: returnBytes, startIndex: i);

            return returnBytes;
        }

        // TABLE STATE DATA

        public static int BytesToTableStateData (byte[] bytes, out TableStateData tableStateData, int startIndex = 0) {
            int i = startIndex;
            // Minimum Wager
            i = ByteConverterUtils.BytesToInt32 (bytes, out var minimumWager, startIndex: i);
            // Current Table Stake
            i = ByteConverterUtils.BytesToInt32 (bytes, out var currentTableStake, startIndex: i);
            // Cash Pot
            i = ByteConverterUtils.BytesToInt32 (bytes, out var cashPot, startIndex: i);
            // Seat State Data Order
            List<SeatStateData> seatStateDataOrder = new ();
            for (int s = 0; s < HoldEmPokerDefines.POKER_TABLE_CAPACITY; s++) {
                i = BytesToSeatStateData (bytes, out var seatStateData, startIndex: i);
                seatStateDataOrder.Add (seatStateData);
            }
            // Current Game Phase
            i = BytesToGamePhase (bytes, out var currentGamePhase, startIndex: i);
            // Current Turning Seat Index
            i = ByteConverterUtils.BytesToInt16 (bytes, out var currentTurnIndex, startIndex: i);
            // Community Card Order
            List<PokerCard> communityCardOrder = new ();
            for (int c = 0; c < HoldEmPokerDefines.POKER_COMMUNITY_CARD_COUNT; c++) {
                i = BytesToPokerCard (bytes, out var card, startIndex: i);
                communityCardOrder.Add (card);
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
            return i;
        }

        public static byte[] BytesFromTableStateData (TableStateData tableStateData) {
            byte[] returnBytes = new byte[ByteConverterUtils.SIZEOF_TABLE_STATE_DATA];

            int i = 0;
            // Minimum Wager
            i = ByteConverterUtils.AddBytesToArray (bytesToAdd: BitConverter.GetBytes (tableStateData.minimumWager),
                bytesLocation: returnBytes, startIndex: i);
            // Current Table Stake
            i = ByteConverterUtils.AddBytesToArray (bytesToAdd: BitConverter.GetBytes (tableStateData.currentTableStake),
                bytesLocation: returnBytes, startIndex: i);
            // Cash Pot
            i = ByteConverterUtils.AddBytesToArray (bytesToAdd: BitConverter.GetBytes (tableStateData.cashPot),
                bytesLocation: returnBytes, startIndex: i);
            // Seat State Data Order
            for (int s = 0; s < HoldEmPokerDefines.POKER_TABLE_CAPACITY; s++) {
                var seatStateData = tableStateData.seatStateDataOrder[s];
                i = ByteConverterUtils.AddBytesToArray (bytesToAdd: BytesFromSeatStateData (seatStateData),
                    bytesLocation: returnBytes, startIndex: i);
            }
            // Current Game Phase
            i = ByteConverterUtils.AddBytesToArray (bytesToAdd: BytesFromGamePhase (tableStateData.currentGamePhase),
                bytesLocation: returnBytes, startIndex: i);
            // Current Turn Seat Index
            i = ByteConverterUtils.AddBytesToArray (bytesToAdd: BitConverter.GetBytes (tableStateData.currentTurnPlayerIndex),
                bytesLocation: returnBytes, startIndex: i);
            // Community Card Order
            for (int c = 0; c < HoldEmPokerDefines.POKER_COMMUNITY_CARD_COUNT; c++) {
                var card = tableStateData.communityCardsOrder[c];
                i = ByteConverterUtils.AddBytesToArray (bytesToAdd: BytesFromPokerCard (card), 
                    bytesLocation: returnBytes, startIndex: i);
            }

            return returnBytes;
        }

        #endregion

    }
}