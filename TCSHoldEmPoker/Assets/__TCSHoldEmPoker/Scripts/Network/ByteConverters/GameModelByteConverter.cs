using System;
using System.Collections.Generic;
using TCSHoldEmPoker.Data;
using TCSHoldEmPoker.Models;
using TCSHoldEmPoker.Models.Define;

namespace TCSHoldEmPoker.Network.Data {
    public static class GameModelByteConverter {

        #region Methods

        // POKER CARD

        public static void BytesToPokerCard (byte[] bytes, ref int currentDataIndex, out PokerCard card) {
            byte cardByte = bytes[currentDataIndex];
            CardValueEnum value = (CardValueEnum)(cardByte & 0xF0);     // 1st half-byte is value.
            CardSuitEnum suit = (CardSuitEnum)(cardByte & 0x0F);        // 2nd half-byte is suit.
            card = new PokerCard (suit, value);
            currentDataIndex += ByteConverterUtils.SizeOf (card);
        }

        public static byte[] BytesFromPokerCard (PokerCard card) {
            return new byte[] {
                (byte)((byte)card.Value | (byte)card.Suit)
            };
        }

        // POKER HAND RANK

        public static void BytesToHandRank (byte[] bytes, ref int currentDataIndex, out PokerHandRankEnum handRank) {
            handRank = (PokerHandRankEnum)bytes[currentDataIndex];
            currentDataIndex += ByteConverterUtils.SizeOf (handRank);
        }

        public static byte[] BytesFromHandRank (PokerHandRankEnum handRank) {
            return new byte[] {
                (byte)handRank
            };
        }

        // POKER GAME PHASE

        public static void BytesToGamePhase (byte[] bytes, ref int currentDataIndex, out PokerGamePhaseEnum gamePhase) {
            gamePhase = (PokerGamePhaseEnum)bytes[currentDataIndex];
            currentDataIndex += ByteConverterUtils.SizeOf (PokerGamePhaseEnum.SAMPLE);
        }

        public static byte[] BytesFromGamePhase (PokerGamePhaseEnum gamePhase) {
            return new byte[] {
                (byte)gamePhase
            };
        }

        // POKER HAND

        public static void BytesToPokerHand (byte[] bytes, ref int currentDataIndex, out PokerHand hand) {
            // Hand Rank
            BytesToHandRank (bytes, ref currentDataIndex, out var handRank);
            // Card Order
            List<PokerCard> cards = new ();
            for (int c = 0; c < HoldEmPokerDefines.POKER_HAND_SIZE; c++) {
                BytesToPokerCard (bytes, ref currentDataIndex, out var card);
                cards.Add (card);
            }

            hand = new PokerHand (handRank, cards);
        }

        public static byte[] BytesFromPokerHand (PokerHand hand) {
            byte[] returnBytes = new byte[ByteConverterUtils.SizeOf (hand)];

            int currentDataIndex = 0;
            // Hand Rank
            ByteConverterUtils.AddBytesToArray (bytesToAdd: BytesFromHandRank (hand.HandRank), 
                bytesLocation: returnBytes, ref currentDataIndex);
            // Card Order
            foreach (var card in hand.CardOrder) {
                ByteConverterUtils.AddBytesToArray (bytesToAdd: BytesFromPokerCard (card), bytesLocation: returnBytes, ref currentDataIndex);
            }

            return returnBytes;
        }

        // PLAYER STATE DATA

        public static void BytesToPlayerStateData (byte[] bytes, ref int currentDataIndex, out PlayerStateData playerStateData) {
            // Player ID
            ByteConverterUtils.BytesToInt32 (bytes, ref currentDataIndex, out var playerID);
            // Chips in Hand
            ByteConverterUtils.BytesToInt32 (bytes, ref currentDataIndex, out var chipsInHand);

            playerStateData = new () {
                playerID = playerID,
                chipsInHand = chipsInHand,
            };
        }

        public static byte[] BytesFromPlayerStateData (PlayerStateData playerStateData) {
            byte[] returnBytes = new byte[ByteConverterUtils.SizeOf (playerStateData)];

            int currentDataIndex = 0;
            // Player ID
            ByteConverterUtils.AddBytesToArray (bytesToAdd: BitConverter.GetBytes (playerStateData.playerID),
                bytesLocation: returnBytes, ref currentDataIndex);
            // Chips in Hand
            ByteConverterUtils.AddBytesToArray (bytesToAdd: BitConverter.GetBytes (playerStateData.chipsInHand),
                bytesLocation: returnBytes, ref currentDataIndex);

            return returnBytes;
        }

        // SEAT STATE DATA

        public static void BytesToSeatStateData (byte[] bytes, ref int currentDataIndex, out SeatStateData seatStateData) {
            // Seated Player State Data
            BytesToPlayerStateData (bytes, ref currentDataIndex, out var playerStateData);
            // Boolean Property Set
            ByteConverterUtils.BytesToBoolArray (bytes, byteCount: 1, ref currentDataIndex, out var boolArray);
            bool didCheck = boolArray[0];
            bool isPlaying = boolArray[1];
            // Current Wager
            ByteConverterUtils.BytesToInt32 (bytes, ref currentDataIndex, out var currentWager);

            seatStateData = new () {
                seatedPlayerStateData = playerStateData,
                didCheck = didCheck,
                isPlaying = isPlaying,
                currentWager = currentWager,
            };
        }

        public static byte[] BytesFromSeatStateData (SeatStateData seatStateData) {
            byte[] returnBytes = new byte[ByteConverterUtils.SizeOf (seatStateData)];

            int currentDataIndex = 0;
            // Seated Player State Data
            ByteConverterUtils.AddBytesToArray (bytesToAdd: BytesFromPlayerStateData (seatStateData.seatedPlayerStateData),
                bytesLocation: returnBytes, ref currentDataIndex);
            // Boolean Property Set 1
            ByteConverterUtils.AddByteToArray (byteToAdd: ByteConverterUtils.BoolArrayToByte (new bool[] {
                seatStateData.didCheck, seatStateData.isPlaying,
                false, false, 
                false, false, 
                false, false,
            }), bytesLocation: returnBytes, ref currentDataIndex);
            // Current Wager
            ByteConverterUtils.AddBytesToArray (bytesToAdd: BitConverter.GetBytes (seatStateData.currentWager),
                bytesLocation: returnBytes, ref currentDataIndex);

            return returnBytes;
        }

        // PRIZE POT STATE DATA

        public static void BytesToPrizePotStateData (byte[] bytes, ref int currentDataIndex, out PrizePotStateData prizePotStateData) {
            // Prize Amount
            ByteConverterUtils.BytesToInt32 (bytes, ref currentDataIndex, out var prizeAmount);
            // Qualified Player Count
            ByteConverterUtils.BytesToInt16 (bytes, ref currentDataIndex, out var playerCount);
            // Revealed Player Hands
            List<Int32> qualifiedPlayerIDs = new ();
            for (int p = 0; p < playerCount; p++) {
                // Player ID (KEY)
                ByteConverterUtils.BytesToInt32 (bytes, ref currentDataIndex, out var playerID);
                qualifiedPlayerIDs.Add (playerID);
            }

            prizePotStateData = new () {
                prizeAmount = prizeAmount,
                qualifiedPlayerIDs = qualifiedPlayerIDs,
            };
        }

        public static byte[] BytesFromPrizePotStateData (PrizePotStateData prizePotStateData) {
            byte[] returnBytes = new byte[ByteConverterUtils.SizeOf (prizePotStateData)];

            int currentIndex = 0;
            // Prize Amount
            ByteConverterUtils.AddBytesToArray (bytesToAdd: BitConverter.GetBytes (prizePotStateData.prizeAmount),
                bytesLocation: returnBytes, ref currentIndex);
            // Qualified  Player Count
            ByteConverterUtils.AddBytesToArray (bytesToAdd: BitConverter.GetBytes ((Int16)prizePotStateData.qualifiedPlayerIDs.Count),
                bytesLocation: returnBytes, ref currentIndex);
            foreach (var playerID in prizePotStateData.qualifiedPlayerIDs) {
                // Player ID
                ByteConverterUtils.AddBytesToArray (bytesToAdd: BitConverter.GetBytes (playerID),
                    bytesLocation: returnBytes, ref currentIndex);
            }

            return returnBytes;
        }

        // TABLE STATE DATA

        public static void BytesToTableStateData (byte[] bytes, ref int currentDataIndex, out TableStateData tableStateData) {
            // Minimum Wager
            ByteConverterUtils.BytesToInt32 (bytes, ref currentDataIndex, out var minimumWager);
            // Current Table Stake
            ByteConverterUtils.BytesToInt32 (bytes, ref currentDataIndex, out var currentTableStake);
            // Main Prize Pot
            BytesToPrizePotStateData (bytes, ref currentDataIndex, out var mainPrizePotData);
            // Side Prize Pot Count
            ByteConverterUtils.BytesToInt16 (bytes, ref currentDataIndex, out var sidePrizePotCount);
            // Side Prize Pots
            List<PrizePotStateData> sidePrizePotDataList = new ();
            for (int sppi = 0; sppi < sidePrizePotCount; sppi++) {
                BytesToPrizePotStateData (bytes, ref currentDataIndex, out var sidePrizePotData);
                sidePrizePotDataList.Add (sidePrizePotData);
            }
            // Seat State Data Order
            List<SeatStateData> seatStateDataOrder = new ();
            for (int ssdi = 0; ssdi < HoldEmPokerDefines.POKER_TABLE_CAPACITY; ssdi++) {
                BytesToSeatStateData (bytes, ref currentDataIndex, out var seatStateData);
                seatStateDataOrder.Add (seatStateData);
            }
            // Current Game Phase
            BytesToGamePhase (bytes, ref currentDataIndex, out var currentGamePhase);
            // Current Turning Seat Index
            ByteConverterUtils.BytesToInt16 (bytes, ref currentDataIndex, out var currentTurnIndex);
            // Community Card Order
            List<PokerCard> communityCardOrder = new ();
            for (int c = 0; c < HoldEmPokerDefines.POKER_COMMUNITY_CARD_COUNT; c++) {
                BytesToPokerCard (bytes, ref currentDataIndex, out var card);
                communityCardOrder.Add (card);
            }

            tableStateData = new () {
                minimumWager = minimumWager,
                currentTableStake = currentTableStake,
                mainPrizeStateData = mainPrizePotData,
                sidePrizeStateDataList = sidePrizePotDataList,
                seatStateDataOrder = seatStateDataOrder,
                currentGamePhase = currentGamePhase,
                currentTurnPlayerIndex = currentTurnIndex,
                communityCardsOrder = communityCardOrder,
            };
        }

        public static byte[] BytesFromTableStateData (TableStateData tableStateData) {
            byte[] returnBytes = new byte[ByteConverterUtils.SizeOf (tableStateData)];

            int currentDataIndex = 0;
            // Minimum Wager
            ByteConverterUtils.AddBytesToArray (bytesToAdd: BitConverter.GetBytes (tableStateData.minimumWager),
                bytesLocation: returnBytes, ref currentDataIndex);
            // Current Table Stake
            ByteConverterUtils.AddBytesToArray (bytesToAdd: BitConverter.GetBytes (tableStateData.currentTableStake),
                bytesLocation: returnBytes, ref currentDataIndex);
            // Main Prize Pot
            ByteConverterUtils.AddBytesToArray (bytesToAdd: BytesFromPrizePotStateData (tableStateData.mainPrizeStateData),
                bytesLocation: returnBytes, ref currentDataIndex);
            // Side Prize Pot Count
            ByteConverterUtils.AddBytesToArray (bytesToAdd: BitConverter.GetBytes ((Int16)tableStateData.sidePrizeStateDataList.Count),
                bytesLocation: returnBytes, ref currentDataIndex);
            // Side Prize Pots
            foreach (var sidePrizePot in tableStateData.sidePrizeStateDataList) {
                ByteConverterUtils.AddBytesToArray (bytesToAdd: BytesFromPrizePotStateData (sidePrizePot),
                    bytesLocation: returnBytes, ref currentDataIndex);
            }
            // Seat State Data Order
            for (int s = 0; s < HoldEmPokerDefines.POKER_TABLE_CAPACITY; s++) {
                var seatStateData = tableStateData.seatStateDataOrder[s];
                ByteConverterUtils.AddBytesToArray (bytesToAdd: BytesFromSeatStateData (seatStateData),
                    bytesLocation: returnBytes, ref currentDataIndex);
            }
            // Current Game Phase
            ByteConverterUtils.AddBytesToArray (bytesToAdd: BytesFromGamePhase (tableStateData.currentGamePhase),
                bytesLocation: returnBytes, ref currentDataIndex);
            // Current Turn Seat Index
            ByteConverterUtils.AddBytesToArray (bytesToAdd: BitConverter.GetBytes (tableStateData.currentTurnPlayerIndex),
                bytesLocation: returnBytes, ref currentDataIndex);
            // Community Card Order
            for (int c = 0; c < HoldEmPokerDefines.POKER_COMMUNITY_CARD_COUNT; c++) {
                var card = tableStateData.communityCardsOrder[c];
                ByteConverterUtils.AddBytesToArray (bytesToAdd: BytesFromPokerCard (card), 
                    bytesLocation: returnBytes, ref currentDataIndex);
            }

            return returnBytes;
        }

        #endregion

    }
}