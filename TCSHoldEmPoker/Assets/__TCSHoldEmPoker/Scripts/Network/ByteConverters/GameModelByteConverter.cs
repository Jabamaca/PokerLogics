using TCSHoldEmPoker.Models.Define;
using TCSHoldEmPoker.Models;
using System.Linq;
using System.Collections.Generic;

namespace TCSHoldEmPoker.Network.Data {
    public static class GameModelByteConverter {

        #region Methods

        public static PokerCard ByteToPokerCard (byte cardByte) {
            CardValueEnum value = (CardValueEnum)(cardByte & 0xF0);         // 1st half-byte is value.
            CardSuitEnum suit = (CardSuitEnum)(cardByte & 0x0F);            // 2nd half-byte is suit.

            return new PokerCard (suit, value);
        }

        public static byte ByteFromPokerCard (PokerCard card) {
            return (byte)((byte)card.Value | (byte)card.Suit);
        }

        public static bool BytesToPokerHand (byte[] bytes, out PokerHand hand) {
            if (bytes.Count () != ByteConverterConsts.HAND_DATA_BYTE_SIZE) {
                hand = null;
                return false;
            }

            List<PokerCard> cards = new ();
            for (int i = 1; i <= 5; i++) {
                cards.Add (ByteToPokerCard (bytes[i]));                     // Byte-Indexes 1 to 5  == Card order of priority.
            }

            hand = PokerHandFactory.GetHighestPokerHandWithCardSet (cards); // Byte-Index 0 is auto-set by factory.
            return true;
        }

        public static byte[] BytesFromPokerHand (PokerHand hand) {
            if (hand == null)
                return null;

            byte[] returnBytes = new byte[ByteConverterConsts.HAND_DATA_BYTE_SIZE];

            returnBytes[0] = (byte)hand.HandRank;                           // Byte-Index 0         == HandRank.

            int cardDataIndex = 1;
            foreach (var card in hand.CardOrder) {
                returnBytes[cardDataIndex] = ByteFromPokerCard (card);      // Byte-Indexes 1 to 5  == Card order of priority.
                cardDataIndex++;
            }

            return returnBytes;
        }

        #endregion

    }
}