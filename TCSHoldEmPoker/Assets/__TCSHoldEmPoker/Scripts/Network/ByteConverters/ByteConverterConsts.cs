using TCSHoldEmPoker.Models.Define;

namespace TCSHoldEmPoker.Network.Data {
    internal static class ByteConverterConsts {

        // Sizes of Basic Data
        internal const int CARD_DATA_BYTE_SIZE = 1;
        internal const int HAND_RANK_BYTE_SIZE = 1;

        // Sizes of Structured Data
        internal const int HAND_DATA_BYTE_SIZE =
            HAND_RANK_BYTE_SIZE +                                       // Hand Rank
            (HoldEmPokerDefines.POKER_HAND_SIZE * CARD_DATA_BYTE_SIZE); // The 5 Cards

    }
}