using TCSHoldEmPoker.Models.Define;
using TCSHoldEmPoker.Models;
using System.Collections.Generic;
using System;
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

        public static bool BytesToPlayerStateData (byte[] bytes, out PlayerStateData psd, int startIndex = 0) {
            if (bytes.Length - startIndex < ByteConverterUtils.SIZEOF_PLAYER_STATE_DATA) {
                psd = null;
                return false;
            }


            int i = startIndex;
            // Player ID
            Int32 playerID = BitConverter.ToInt32 (bytes, startIndex: i);
            i += sizeof (Int32);
            // Chips in Hand
            Int32 chipsInHand = BitConverter.ToInt32 (bytes, startIndex: i);

            psd = new () {
                playerID = playerID,
                chipsInHand = chipsInHand,
            };
            return true;
        }

        public static byte[] BytesFromPlayerStateData (PlayerStateData psd) {
            if (psd == null)
                return null;

            byte[] returnBytes = new byte[ByteConverterUtils.SIZEOF_PLAYER_STATE_DATA];

            int i = 0;
            // Player ID
            foreach (byte playerIDByte in BitConverter.GetBytes (psd.playerID)) {
                returnBytes[i] = playerIDByte;
                i++;
            }
            // Chips in Hand
            foreach (byte chipsByte in BitConverter.GetBytes (psd.chipsInHand)) {
                returnBytes[i] = chipsByte;
                i++;
            }

            return returnBytes;
        }

        // SEAT STATE DATA

        public static bool BytesToSeatStateData (byte[] bytes, out SeatStateData ssd, int startIndex = 0) {
            if (bytes.Length - startIndex < ByteConverterUtils.SIZEOF_SEAT_STATE_DATA) {
                ssd = null;
                return false;
            }

            int i = startIndex;
            // Seated Player State Data
            if (!BytesToPlayerStateData (bytes, out var psd, startIndex: i)) {
                ssd = null;
                return false;
            }
            i += ByteConverterUtils.SIZEOF_PLAYER_STATE_DATA;
            // Boolean Property Set 1
            byte boolByte = bytes[i];
            bool[] boolArray = ByteConverterUtils.ConvertByteToBoolArray (boolByte);
            bool didCheck = boolArray[0];
            bool isPlaying = boolArray[1];
            i++;
            // Current Wager
            Int32 currentWager = BitConverter.ToInt32 (bytes, startIndex: i);

            ssd = new () {
                seatedPlayerStateData = psd,
                didCheck = didCheck,
                isPlaying = isPlaying,
                currentWager = currentWager,
            };
            return true;
        }

        public static byte[] BytesFromSeatStateData (SeatStateData ssd) {
            if (ssd == null)
                return null;

            byte[] returnBytes = new byte[ByteConverterUtils.SIZEOF_SEAT_STATE_DATA];

            int i = 0;
            // Seated Player State Data
            foreach (byte psdByte in BytesFromPlayerStateData (ssd.seatedPlayerStateData)) {
                returnBytes[i] = psdByte;
                i++;
            }
            // Boolean Property Set 1
            returnBytes[i] = ByteConverterUtils.ConvertBoolArrayToByte (new bool[] {
                ssd.didCheck, ssd.isPlaying,
                false, false, 
                false, false, 
                false, false,
            });
            i++;
            // Current Wager
            foreach (byte chipsByte in BitConverter.GetBytes (ssd.currentWager)) {
                returnBytes[i] = chipsByte;
                i++;
            }

            return returnBytes;
        }

        #endregion

    }
}