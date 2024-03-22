using NUnit.Framework;
using System.Diagnostics;
using TCSHoldEmPoker.Models;
using TCSHoldEmPoker.Models.Define;

public class PokerHandBasicTests {

    [Test]
    public void PokerHand_RankEnumCompare_Test () {
        var fullHouse = PokerHandRankEnum.FULL_HOUSE; // Value 0x07
        var twoPair = PokerHandRankEnum.TWO_PAIR; // Value 0x03

        Assert.IsTrue (fullHouse.CompareTo (twoPair) > 0); // FULL_HOUSE is higher than TWO_PAIR.
        Assert.IsTrue (fullHouse.CompareTo (fullHouse) == 0); // Equal FULL_HOUSE pair.
        Assert.IsTrue (twoPair.CompareTo (fullHouse) < 0); // TWO_PAI is lower than FULL_HOUSE.
    }

    [Test]
    public void PokerHand_HighCardInit_Test () {
        PokerCard[] cardList = {
            PokerCard.CARD_K_C,
            PokerCard.CARD_4_D,
            PokerCard.CARD_2_H,
            PokerCard.CARD_A_H,
            PokerCard.CARD_3_S,
        };
        PokerHand highCard = TestUtils.CreatePokerHand_Debug (cardList); // Expected: HighCard - AH, KC, 4D, 3S, 2H

        Assert.AreEqual (highCard.HandRank, PokerHandRankEnum.HIGH_CARD); // Check Hand Rank.

        // Check card order.
        Assert.AreEqual (highCard.CardOrder[0], PokerCard.CARD_A_H);
        Assert.AreEqual (highCard.CardOrder[1], PokerCard.CARD_K_C);
        Assert.AreEqual (highCard.CardOrder[2], PokerCard.CARD_4_D);
        Assert.AreEqual (highCard.CardOrder[3], PokerCard.CARD_3_S);
        Assert.AreEqual (highCard.CardOrder[4], PokerCard.CARD_2_H);
    }

    [Test]
    public void PokerHand_HighCardMultiCard_Test () {
        PokerCard[] cardList = {
            PokerCard.CARD_8_S,
            PokerCard.CARD_Q_H,
            PokerCard.CARD_7_H,
            PokerCard.CARD_A_C,
            PokerCard.CARD_3_S,
            PokerCard.CARD_4_D,
            PokerCard.CARD_X_D,
        };
        PokerHand highCard = TestUtils.CreatePokerHand_Debug (cardList); // Expected: HighCard - AC, QH, XD, 8S, 7H

        Assert.AreEqual (highCard.HandRank, PokerHandRankEnum.HIGH_CARD); // Check Hand Rank.

        // Check card order.
        Assert.AreEqual (highCard.CardOrder[0], PokerCard.CARD_A_C);
        Assert.AreEqual (highCard.CardOrder[1], PokerCard.CARD_Q_H);
        Assert.AreEqual (highCard.CardOrder[2], PokerCard.CARD_X_D);
        Assert.AreEqual (highCard.CardOrder[3], PokerCard.CARD_8_S);
        Assert.AreEqual (highCard.CardOrder[4], PokerCard.CARD_7_H);
    }

    [Test]
    public void PokerHand_OnePairInit_Test () {
        PokerCard[] cardList = {
            PokerCard.CARD_K_D,
            PokerCard.CARD_7_D,
            PokerCard.CARD_3_H,
            PokerCard.CARD_A_H,
            PokerCard.CARD_3_C,
        };
        PokerHand onePair = TestUtils.CreatePokerHand_Debug (cardList); // Expected: OnePair - 3H, 3C, AH, KD, 7D

        Assert.AreEqual (onePair.HandRank, PokerHandRankEnum.ONE_PAIR); // Check Hand Rank.

        Assert.AreEqual (onePair.CardOrder[0].Value, onePair.CardOrder[1].Value); // Check pairing.

        // Check card order.
        Assert.AreEqual (onePair.CardOrder[0].Value, CardValueEnum.THREE);
        Assert.AreEqual (onePair.CardOrder[1].Value, CardValueEnum.THREE);
        Assert.AreEqual (onePair.CardOrder[2], PokerCard.CARD_A_H);
        Assert.AreEqual (onePair.CardOrder[3], PokerCard.CARD_K_D);
        Assert.AreEqual (onePair.CardOrder[4], PokerCard.CARD_7_D);
    }

    [Test]
    public void PokerHand_OnePairMultiCard_Test () {
        PokerCard[] cardList = {
            PokerCard.CARD_9_D,
            PokerCard.CARD_Q_D,
            PokerCard.CARD_7_H,
            PokerCard.CARD_8_C,
            PokerCard.CARD_3_C,
            PokerCard.CARD_9_H,
            PokerCard.CARD_X_H,
        };
        PokerHand onePair = TestUtils.CreatePokerHand_Debug (cardList); // Expected: OnePair - 9D, 9H, QD, XH, 8C

        Assert.AreEqual (onePair.HandRank, PokerHandRankEnum.ONE_PAIR); // Check Hand Rank.

        Assert.AreEqual (onePair.CardOrder[0].Value, onePair.CardOrder[1].Value); // Check pairing.

        // Check card order.
        Assert.AreEqual (onePair.CardOrder[0].Value, CardValueEnum.NINE);
        Assert.AreEqual (onePair.CardOrder[1].Value, CardValueEnum.NINE);
        Assert.AreEqual (onePair.CardOrder[2], PokerCard.CARD_Q_D);
        Assert.AreEqual (onePair.CardOrder[3], PokerCard.CARD_X_H);
        Assert.AreEqual (onePair.CardOrder[4], PokerCard.CARD_8_C);
    }

    [Test]
    public void PokerHand_TwoPairInit_Test () {
        PokerCard[] cardList = {
            PokerCard.CARD_5_D,
            PokerCard.CARD_4_H,
            PokerCard.CARD_X_D,
            PokerCard.CARD_X_H,
            PokerCard.CARD_4_C,
        };
        PokerHand twoPair = TestUtils.CreatePokerHand_Debug (cardList); // Expected: TwoPair - XD, XH, 4H, 4C, 5D

        Assert.AreEqual (twoPair.HandRank, PokerHandRankEnum.TWO_PAIR); // Check Hand Rank.

        // Check pairings.
        Assert.AreEqual (twoPair.CardOrder[0].Value, twoPair.CardOrder[1].Value);
        Assert.AreEqual (twoPair.CardOrder[2].Value, twoPair.CardOrder[3].Value);

        // Check card order.
        Assert.AreEqual (twoPair.CardOrder[0].Value, CardValueEnum.TEN);
        Assert.AreEqual (twoPair.CardOrder[1].Value, CardValueEnum.TEN);
        Assert.AreEqual (twoPair.CardOrder[2].Value, CardValueEnum.FOUR);
        Assert.AreEqual (twoPair.CardOrder[3].Value, CardValueEnum.FOUR);
        Assert.AreEqual (twoPair.CardOrder[4], PokerCard.CARD_5_D);
    }

    [Test]
    public void PokerHand_TwoPairMultiCard_Test () {
        PokerCard[] cardList = {
            PokerCard.CARD_X_C,
            PokerCard.CARD_2_D,
            PokerCard.CARD_6_S,
            PokerCard.CARD_5_S,
            PokerCard.CARD_A_C,
            PokerCard.CARD_A_H,
            PokerCard.CARD_2_S,
        };
        PokerHand twoPair = TestUtils.CreatePokerHand_Debug (cardList); // Expected: TwoPair - AC, AH, 2D, 2S, XC

        Assert.AreEqual (twoPair.HandRank, PokerHandRankEnum.TWO_PAIR); // Check Hand Rank.

        // Check pairings.
        Assert.AreEqual (twoPair.CardOrder[0].Value, twoPair.CardOrder[1].Value);
        Assert.AreEqual (twoPair.CardOrder[2].Value, twoPair.CardOrder[3].Value);

        // Check card order.
        Assert.AreEqual (twoPair.CardOrder[0].Value, CardValueEnum.ACE);
        Assert.AreEqual (twoPair.CardOrder[1].Value, CardValueEnum.ACE);
        Assert.AreEqual (twoPair.CardOrder[2].Value, CardValueEnum.TWO);
        Assert.AreEqual (twoPair.CardOrder[3].Value, CardValueEnum.TWO);
        Assert.AreEqual (twoPair.CardOrder[4], PokerCard.CARD_X_C);
    }

    [Test]
    public void PokerHand_ThreeOfAKindInit_Test () {
        PokerCard[] cardList = {
            PokerCard.CARD_Q_D,
            PokerCard.CARD_K_S,
            PokerCard.CARD_9_S,
            PokerCard.CARD_9_C,
            PokerCard.CARD_9_D,
        };
        PokerHand threeOfAKind = TestUtils.CreatePokerHand_Debug (cardList); // Expected: ThreeOfAKind - 9S, 9C, 9D, KS, QD

        Assert.AreEqual (threeOfAKind.HandRank, PokerHandRankEnum.THREE_OF_A_KIND); // Check Hand Rank.

        // Check trio.
        Assert.AreEqual (threeOfAKind.CardOrder[0].Value, threeOfAKind.CardOrder[1].Value);
        Assert.AreEqual (threeOfAKind.CardOrder[1].Value, threeOfAKind.CardOrder[2].Value);

        // Check card order.
        Assert.AreEqual (threeOfAKind.CardOrder[0].Value, CardValueEnum.NINE);
        Assert.AreEqual (threeOfAKind.CardOrder[1].Value, CardValueEnum.NINE);
        Assert.AreEqual (threeOfAKind.CardOrder[2].Value, CardValueEnum.NINE);
        Assert.AreEqual (threeOfAKind.CardOrder[3], PokerCard.CARD_K_S);
        Assert.AreEqual (threeOfAKind.CardOrder[4], PokerCard.CARD_Q_D);
    }

    [Test]
    public void PokerHand_ThreeOfAKindMultiCard_Test () {
        PokerCard[] cardList = {
            PokerCard.CARD_3_C,
            PokerCard.CARD_2_H,
            PokerCard.CARD_6_H,
            PokerCard.CARD_3_S,
            PokerCard.CARD_7_C,
            PokerCard.CARD_3_D,
            PokerCard.CARD_5_S,
        };
        PokerHand threeOfAKind = TestUtils.CreatePokerHand_Debug (cardList); // Expected: ThreeOfAKind - 3C, 3S, 3D, 7C, 6H

        Assert.AreEqual (threeOfAKind.HandRank, PokerHandRankEnum.THREE_OF_A_KIND); // Check Hand Rank.

        // Check trio.
        Assert.AreEqual (threeOfAKind.CardOrder[0].Value, threeOfAKind.CardOrder[1].Value);
        Assert.AreEqual (threeOfAKind.CardOrder[1].Value, threeOfAKind.CardOrder[2].Value);

        // Check card order.
        Assert.AreEqual (threeOfAKind.CardOrder[0].Value, CardValueEnum.THREE);
        Assert.AreEqual (threeOfAKind.CardOrder[1].Value, CardValueEnum.THREE);
        Assert.AreEqual (threeOfAKind.CardOrder[2].Value, CardValueEnum.THREE);
        Assert.AreEqual (threeOfAKind.CardOrder[3], PokerCard.CARD_7_C);
        Assert.AreEqual (threeOfAKind.CardOrder[4], PokerCard.CARD_6_H);
    }

    [Test]
    public void PokerHand_StraightInit_Test () {
        PokerCard[] cardList = {
            PokerCard.CARD_5_S,
            PokerCard.CARD_4_C,
            PokerCard.CARD_7_S,
            PokerCard.CARD_6_C,
            PokerCard.CARD_3_D,
        };
        PokerHand straight = TestUtils.CreatePokerHand_Debug (cardList); // Expected: Straight - 7S, 6C, 5S, 4C, 3D

        Assert.AreEqual (straight.HandRank, PokerHandRankEnum.STRAIGHT); // Check Hand Rank.

        // Check card order.
        Assert.AreEqual (straight.CardOrder[0].Value, CardValueEnum.SEVEN);
        Assert.AreEqual (straight.CardOrder[1].Value, CardValueEnum.SIX);
        Assert.AreEqual (straight.CardOrder[2].Value, CardValueEnum.FIVE);
        Assert.AreEqual (straight.CardOrder[3].Value, CardValueEnum.FOUR);
        Assert.AreEqual (straight.CardOrder[4].Value, CardValueEnum.THREE);
    }

    [Test]
    public void PokerHand_StraightLowAce_Test () {
        PokerCard[] cardList = {
            PokerCard.CARD_2_D,
            PokerCard.CARD_4_H,
            PokerCard.CARD_5_H,
            PokerCard.CARD_A_C,
            PokerCard.CARD_3_C,
        };
        PokerHand straight = TestUtils.CreatePokerHand_Debug (cardList); // Expected: Straight - 5H, 4H, 3C, 2D, AC

        Assert.AreEqual (straight.HandRank, PokerHandRankEnum.STRAIGHT); // Check Hand Rank.

        // Check card order.
        Assert.AreEqual (straight.CardOrder[0].Value, CardValueEnum.FIVE);
        Assert.AreEqual (straight.CardOrder[1].Value, CardValueEnum.FOUR);
        Assert.AreEqual (straight.CardOrder[2].Value, CardValueEnum.THREE);
        Assert.AreEqual (straight.CardOrder[3].Value, CardValueEnum.TWO);
        Assert.AreEqual (straight.CardOrder[4].Value, CardValueEnum.ACE);
    }

    [Test]
    public void PokerHand_StraightMultiCard_Test () {
        PokerCard[] cardList = {
            PokerCard.CARD_5_D,
            PokerCard.CARD_2_D,
            PokerCard.CARD_A_D,
            PokerCard.CARD_4_H,
            PokerCard.CARD_A_H,
            PokerCard.CARD_3_H,
            PokerCard.CARD_4_D,
        };
        PokerHand straight = TestUtils.CreatePokerHand_Debug (cardList); // Expected: Straight - 5D, 4H, 3H, 2D, AD

        Assert.AreEqual (straight.HandRank, PokerHandRankEnum.STRAIGHT); // Check Hand Rank.

        // Check card order.
        Assert.AreEqual (straight.CardOrder[0].Value, CardValueEnum.FIVE);
        Assert.AreEqual (straight.CardOrder[1].Value, CardValueEnum.FOUR);
        Assert.AreEqual (straight.CardOrder[2].Value, CardValueEnum.THREE);
        Assert.AreEqual (straight.CardOrder[3].Value, CardValueEnum.TWO);
        Assert.AreEqual (straight.CardOrder[4].Value, CardValueEnum.ACE);
    }

    [Test]
    public void PokerHand_FlushInit_Test () {
        PokerCard[] cardList = {
            PokerCard.CARD_3_H,
            PokerCard.CARD_J_H,
            PokerCard.CARD_9_H,
            PokerCard.CARD_7_H,
            PokerCard.CARD_A_H,
        };
        PokerHand flush = TestUtils.CreatePokerHand_Debug (cardList); // Expected: Flush - AH, JH, 9H, 7H, 3H

        Assert.AreEqual (flush.HandRank, PokerHandRankEnum.FLUSH); // Check Hand Rank.

        // Check Suit parity.
        Assert.AreEqual (flush.CardOrder[0].Suit, flush.CardOrder[1].Suit);
        Assert.AreEqual (flush.CardOrder[0].Suit, flush.CardOrder[2].Suit);
        Assert.AreEqual (flush.CardOrder[0].Suit, flush.CardOrder[3].Suit);
        Assert.AreEqual (flush.CardOrder[0].Suit, flush.CardOrder[4].Suit);

        // Check card order.
        Assert.AreEqual (flush.CardOrder[0], PokerCard.CARD_A_H);
        Assert.AreEqual (flush.CardOrder[1], PokerCard.CARD_J_H);
        Assert.AreEqual (flush.CardOrder[2], PokerCard.CARD_9_H);
        Assert.AreEqual (flush.CardOrder[3], PokerCard.CARD_7_H);
        Assert.AreEqual (flush.CardOrder[4], PokerCard.CARD_3_H);
    }

    [Test]
    public void PokerHand_FlushMultiCard_Test () {
        PokerCard[] cardList = {
            PokerCard.CARD_8_S,
            PokerCard.CARD_A_S,
            PokerCard.CARD_2_S,
            PokerCard.CARD_A_D,
            PokerCard.CARD_3_S,
            PokerCard.CARD_K_S,
            PokerCard.CARD_A_H,
        };
        PokerHand flush = TestUtils.CreatePokerHand_Debug (cardList); // Expected: Flush - AS, KS, 8S, 3S, 2S

        Assert.AreEqual (flush.HandRank, PokerHandRankEnum.FLUSH); // Check Hand Rank.

        // Check Suit parity.
        Assert.AreEqual (flush.CardOrder[0].Suit, flush.CardOrder[1].Suit);
        Assert.AreEqual (flush.CardOrder[0].Suit, flush.CardOrder[2].Suit);
        Assert.AreEqual (flush.CardOrder[0].Suit, flush.CardOrder[3].Suit);
        Assert.AreEqual (flush.CardOrder[0].Suit, flush.CardOrder[4].Suit);

        // Check card order.
        Assert.AreEqual (flush.CardOrder[0], PokerCard.CARD_A_S);
        Assert.AreEqual (flush.CardOrder[1], PokerCard.CARD_K_S);
        Assert.AreEqual (flush.CardOrder[2], PokerCard.CARD_8_S);
        Assert.AreEqual (flush.CardOrder[3], PokerCard.CARD_3_S);
        Assert.AreEqual (flush.CardOrder[4], PokerCard.CARD_2_S);
    }

    [Test]
    public void PokerHand_Flush2Suits_Test () {
        PokerCard[] cardList = {
            PokerCard.CARD_9_S,
            PokerCard.CARD_A_H,
            PokerCard.CARD_A_S,
            PokerCard.CARD_2_S,
            PokerCard.CARD_K_H,
            PokerCard.CARD_4_S,
            PokerCard.CARD_J_H,
            PokerCard.CARD_2_H,
            PokerCard.CARD_K_S,
            PokerCard.CARD_3_H,
        };
        PokerHand flush = TestUtils.CreatePokerHand_Debug (cardList); // Expected: Flush - AH, KH, JH, 3H, 2H

        Assert.AreEqual (flush.HandRank, PokerHandRankEnum.FLUSH); // Check Hand Rank.

        // Check Suit parity.
        Assert.AreEqual (flush.CardOrder[0].Suit, flush.CardOrder[1].Suit);
        Assert.AreEqual (flush.CardOrder[0].Suit, flush.CardOrder[2].Suit);
        Assert.AreEqual (flush.CardOrder[0].Suit, flush.CardOrder[3].Suit);
        Assert.AreEqual (flush.CardOrder[0].Suit, flush.CardOrder[4].Suit);

        // Check card order.
        Assert.AreEqual (flush.CardOrder[0], PokerCard.CARD_A_H);
        Assert.AreEqual (flush.CardOrder[1], PokerCard.CARD_K_H);
        Assert.AreEqual (flush.CardOrder[2], PokerCard.CARD_J_H);
        Assert.AreEqual (flush.CardOrder[3], PokerCard.CARD_3_H);
        Assert.AreEqual (flush.CardOrder[4], PokerCard.CARD_2_H);
    }

    [Test]
    public void PokerHand_FullHouseInit_Test () {
        PokerCard[] cardList = {
            PokerCard.CARD_J_H,
            PokerCard.CARD_X_D,
            PokerCard.CARD_J_C,
            PokerCard.CARD_X_H,
            PokerCard.CARD_J_S,
        };
        PokerHand fullHouse = TestUtils.CreatePokerHand_Debug (cardList); // Expected: FullHouse - JH, JC, JS, XD, XH

        Assert.AreEqual (fullHouse.HandRank, PokerHandRankEnum.FULL_HOUSE); // Check Hand Rank.

        // Check trio.
        Assert.AreEqual (fullHouse.CardOrder[0].Value, fullHouse.CardOrder[1].Value);
        Assert.AreEqual (fullHouse.CardOrder[0].Value, fullHouse.CardOrder[2].Value);

        // Check pair.
        Assert.AreEqual (fullHouse.CardOrder[3].Value, fullHouse.CardOrder[4].Value);

        // Check card order.
        Assert.AreEqual (fullHouse.CardOrder[0].Value, CardValueEnum.JACK);
        Assert.AreEqual (fullHouse.CardOrder[1].Value, CardValueEnum.JACK);
        Assert.AreEqual (fullHouse.CardOrder[2].Value, CardValueEnum.JACK);
        Assert.AreEqual (fullHouse.CardOrder[3].Value, CardValueEnum.TEN);
        Assert.AreEqual (fullHouse.CardOrder[4].Value, CardValueEnum.TEN);
    }

    [Test]
    public void PokerHand_FullHouseMultiCard_Test () {
        PokerCard[] cardList = {
            PokerCard.CARD_2_S,
            PokerCard.CARD_A_H,
            PokerCard.CARD_A_D,
            PokerCard.CARD_2_C,
            PokerCard.CARD_K_C,
            PokerCard.CARD_2_H,
            PokerCard.CARD_K_S,
        };
        PokerHand fullHouse = TestUtils.CreatePokerHand_Debug (cardList); // Expected: FullHouse - 2S, 2C, 2H, AH, AD

        Assert.AreEqual (fullHouse.HandRank, PokerHandRankEnum.FULL_HOUSE); // Check Hand Rank.

        // Check trio.
        Assert.AreEqual (fullHouse.CardOrder[0].Value, fullHouse.CardOrder[1].Value);
        Assert.AreEqual (fullHouse.CardOrder[0].Value, fullHouse.CardOrder[2].Value);

        // Check pair.
        Assert.AreEqual (fullHouse.CardOrder[3].Value, fullHouse.CardOrder[4].Value);

        // Check card order.
        Assert.AreEqual (fullHouse.CardOrder[0].Value, CardValueEnum.TWO);
        Assert.AreEqual (fullHouse.CardOrder[1].Value, CardValueEnum.TWO);
        Assert.AreEqual (fullHouse.CardOrder[2].Value, CardValueEnum.TWO);
        Assert.AreEqual (fullHouse.CardOrder[3].Value, CardValueEnum.ACE);
        Assert.AreEqual (fullHouse.CardOrder[4].Value, CardValueEnum.ACE);
    }

    [Test]
    public void PokerHand_FourOfAKindInit_Test () {
        PokerCard[] cardList = {
            PokerCard.CARD_3_D,
            PokerCard.CARD_3_S,
            PokerCard.CARD_3_C,
            PokerCard.CARD_A_C,
            PokerCard.CARD_3_H,
        };
        PokerHand fourOfAKind = TestUtils.CreatePokerHand_Debug (cardList); // Expected: FourOfAKind - 3D, 3S, 3C, 3H, AC

        Assert.AreEqual (fourOfAKind.HandRank, PokerHandRankEnum.FOUR_OF_A_KIND); // Check Hand Rank.

        // Check quad.
        Assert.AreEqual (fourOfAKind.CardOrder[0].Value, fourOfAKind.CardOrder[1].Value);
        Assert.AreEqual (fourOfAKind.CardOrder[0].Value, fourOfAKind.CardOrder[2].Value);
        Assert.AreEqual (fourOfAKind.CardOrder[0].Value, fourOfAKind.CardOrder[3].Value);

        // Check card order.
        Assert.AreEqual (fourOfAKind.CardOrder[0].Value, CardValueEnum.THREE);
        Assert.AreEqual (fourOfAKind.CardOrder[1].Value, CardValueEnum.THREE);
        Assert.AreEqual (fourOfAKind.CardOrder[2].Value, CardValueEnum.THREE);
        Assert.AreEqual (fourOfAKind.CardOrder[2].Value, CardValueEnum.THREE);
        Assert.AreEqual (fourOfAKind.CardOrder[4], PokerCard.CARD_A_C);
    }

    [Test]
    public void PokerHand_FourOfAKindMultiCard_Test () {
        PokerCard[] cardList = {
            PokerCard.CARD_A_C,
            PokerCard.CARD_A_D,
            PokerCard.CARD_5_C,
            PokerCard.CARD_5_D,
            PokerCard.CARD_5_H,
            PokerCard.CARD_A_S,
            PokerCard.CARD_5_S,
        };
        PokerHand fourOfAKind = TestUtils.CreatePokerHand_Debug (cardList); // Expected: FourOfAKind - 5C, 5D, 5H, 5S, AC

        Assert.AreEqual (fourOfAKind.HandRank, PokerHandRankEnum.FOUR_OF_A_KIND); // Check Hand Rank.

        // Check quad.
        Assert.AreEqual (fourOfAKind.CardOrder[0].Value, fourOfAKind.CardOrder[1].Value);
        Assert.AreEqual (fourOfAKind.CardOrder[0].Value, fourOfAKind.CardOrder[2].Value);
        Assert.AreEqual (fourOfAKind.CardOrder[0].Value, fourOfAKind.CardOrder[3].Value);

        // Check card order.
        Assert.AreEqual (fourOfAKind.CardOrder[0].Value, CardValueEnum.FIVE);
        Assert.AreEqual (fourOfAKind.CardOrder[1].Value, CardValueEnum.FIVE);
        Assert.AreEqual (fourOfAKind.CardOrder[2].Value, CardValueEnum.FIVE);
        Assert.AreEqual (fourOfAKind.CardOrder[2].Value, CardValueEnum.FIVE);
        Assert.AreEqual (fourOfAKind.CardOrder[4], PokerCard.CARD_A_C);
    }

    [Test]
    public void PokerHand_StraightFlushInit_Test () {
        PokerCard[] cardList = {
            PokerCard.CARD_X_C,
            PokerCard.CARD_J_C,
            PokerCard.CARD_9_C,
            PokerCard.CARD_7_C,
            PokerCard.CARD_8_C,
        };
        PokerHand strFlush = TestUtils.CreatePokerHand_Debug (cardList); // Expected: Straight Flush - JC, XC, 9C, 8C, 7C

        Assert.AreEqual (strFlush.HandRank, PokerHandRankEnum.STRAIGHT_FLUSH); // Check Hand Rank.

        // Check Suit parity.
        Assert.AreEqual (strFlush.CardOrder[0].Suit, strFlush.CardOrder[1].Suit);
        Assert.AreEqual (strFlush.CardOrder[0].Suit, strFlush.CardOrder[2].Suit);
        Assert.AreEqual (strFlush.CardOrder[0].Suit, strFlush.CardOrder[3].Suit);
        Assert.AreEqual (strFlush.CardOrder[0].Suit, strFlush.CardOrder[4].Suit);

        // Check card order.
        Assert.AreEqual (strFlush.CardOrder[0].Value, CardValueEnum.JACK);
        Assert.AreEqual (strFlush.CardOrder[1].Value, CardValueEnum.TEN);
        Assert.AreEqual (strFlush.CardOrder[2].Value, CardValueEnum.NINE);
        Assert.AreEqual (strFlush.CardOrder[3].Value, CardValueEnum.EIGHT);
        Assert.AreEqual (strFlush.CardOrder[4].Value, CardValueEnum.SEVEN);
    }

    [Test]
    public void PokerHand_StraightFlushLowAce_Test () {
        PokerCard[] cardList = {
            PokerCard.CARD_2_D,
            PokerCard.CARD_4_D,
            PokerCard.CARD_3_D,
            PokerCard.CARD_A_D,
            PokerCard.CARD_5_D,
        };
        PokerHand strFlush = TestUtils.CreatePokerHand_Debug (cardList); // Expected: Straight Flush - 5D, 4D, 3D, 2D, AD

        Assert.AreEqual (strFlush.HandRank, PokerHandRankEnum.STRAIGHT_FLUSH); // Check Hand Rank.

        // Check Suit parity.
        Assert.AreEqual (strFlush.CardOrder[0].Suit, strFlush.CardOrder[1].Suit);
        Assert.AreEqual (strFlush.CardOrder[0].Suit, strFlush.CardOrder[2].Suit);
        Assert.AreEqual (strFlush.CardOrder[0].Suit, strFlush.CardOrder[3].Suit);
        Assert.AreEqual (strFlush.CardOrder[0].Suit, strFlush.CardOrder[4].Suit);

        // Check card order.
        Assert.AreEqual (strFlush.CardOrder[0].Value, CardValueEnum.FIVE);
        Assert.AreEqual (strFlush.CardOrder[1].Value, CardValueEnum.FOUR);
        Assert.AreEqual (strFlush.CardOrder[2].Value, CardValueEnum.THREE);
        Assert.AreEqual (strFlush.CardOrder[3].Value, CardValueEnum.TWO);
        Assert.AreEqual (strFlush.CardOrder[4].Value, CardValueEnum.ACE);
    }

    [Test]
    public void PokerHand_StraightFlushMultiCard_Test () {
        PokerCard[] cardList = {
            PokerCard.CARD_5_S,
            PokerCard.CARD_3_S,
            PokerCard.CARD_Q_S,
            PokerCard.CARD_A_S,
            PokerCard.CARD_2_S,
            PokerCard.CARD_K_S,
            PokerCard.CARD_4_S,
        };
        PokerHand strFlush = TestUtils.CreatePokerHand_Debug (cardList); // Expected: Straight Flush - 5S, 4S, 3S, 2S, AS

        Assert.AreEqual (strFlush.HandRank, PokerHandRankEnum.STRAIGHT_FLUSH); // Check Hand Rank.

        // Check Suit parity.
        Assert.AreEqual (strFlush.CardOrder[0].Suit, strFlush.CardOrder[1].Suit);
        Assert.AreEqual (strFlush.CardOrder[0].Suit, strFlush.CardOrder[2].Suit);
        Assert.AreEqual (strFlush.CardOrder[0].Suit, strFlush.CardOrder[3].Suit);
        Assert.AreEqual (strFlush.CardOrder[0].Suit, strFlush.CardOrder[4].Suit);

        // Check card order.
        Assert.AreEqual (strFlush.CardOrder[0].Value, CardValueEnum.FIVE);
        Assert.AreEqual (strFlush.CardOrder[1].Value, CardValueEnum.FOUR);
        Assert.AreEqual (strFlush.CardOrder[2].Value, CardValueEnum.THREE);
        Assert.AreEqual (strFlush.CardOrder[3].Value, CardValueEnum.TWO);
        Assert.AreEqual (strFlush.CardOrder[4].Value, CardValueEnum.ACE);
    }

    [Test]
    public void PokerHand_StraightFlush2Suits_Test () {
        PokerCard[] cardList = {
            PokerCard.CARD_2_C,
            PokerCard.CARD_4_C,
            PokerCard.CARD_J_H,
            PokerCard.CARD_9_H,
            PokerCard.CARD_3_C,
            PokerCard.CARD_K_H,
            PokerCard.CARD_X_H,
            PokerCard.CARD_5_C,
            PokerCard.CARD_A_C,
            PokerCard.CARD_Q_H,
        };
        PokerHand strFlush = TestUtils.CreatePokerHand_Debug (cardList); // Expected: Flush - KH, QH, JH, XH, 9H

        Assert.AreEqual (strFlush.HandRank, PokerHandRankEnum.STRAIGHT_FLUSH); // Check Hand Rank.

        // Check Suit parity.
        Assert.AreEqual (strFlush.CardOrder[0].Suit, strFlush.CardOrder[1].Suit);
        Assert.AreEqual (strFlush.CardOrder[0].Suit, strFlush.CardOrder[2].Suit);
        Assert.AreEqual (strFlush.CardOrder[0].Suit, strFlush.CardOrder[3].Suit);
        Assert.AreEqual (strFlush.CardOrder[0].Suit, strFlush.CardOrder[4].Suit);

        // Check card order.
        Assert.AreEqual (strFlush.CardOrder[0].Value, CardValueEnum.KING);
        Assert.AreEqual (strFlush.CardOrder[1].Value, CardValueEnum.QUEEN);
        Assert.AreEqual (strFlush.CardOrder[2].Value, CardValueEnum.JACK);
        Assert.AreEqual (strFlush.CardOrder[3].Value, CardValueEnum.TEN);
        Assert.AreEqual (strFlush.CardOrder[4].Value, CardValueEnum.NINE);
    }

    [Test]
    public void PokerHand_RoyalFlushInit_Test () {
        PokerCard[] cardList = {
            PokerCard.CARD_X_C,
            PokerCard.CARD_J_C,
            PokerCard.CARD_A_C,
            PokerCard.CARD_K_C,
            PokerCard.CARD_Q_C,
        };
        PokerHand royalFlush = TestUtils.CreatePokerHand_Debug (cardList); // Expected: Straight Flush - AC, KC, QC, JC, XC

        Assert.AreEqual (royalFlush.HandRank, PokerHandRankEnum.ROYAL_FLUSH); // Check Hand Rank.

        // Check Suit parity.
        Assert.AreEqual (royalFlush.CardOrder[0].Suit, royalFlush.CardOrder[1].Suit);
        Assert.AreEqual (royalFlush.CardOrder[0].Suit, royalFlush.CardOrder[2].Suit);
        Assert.AreEqual (royalFlush.CardOrder[0].Suit, royalFlush.CardOrder[3].Suit);
        Assert.AreEqual (royalFlush.CardOrder[0].Suit, royalFlush.CardOrder[4].Suit);

        // Check card order.
        Assert.AreEqual (royalFlush.CardOrder[0].Value, CardValueEnum.ACE);
        Assert.AreEqual (royalFlush.CardOrder[1].Value, CardValueEnum.KING);
        Assert.AreEqual (royalFlush.CardOrder[2].Value, CardValueEnum.QUEEN);
        Assert.AreEqual (royalFlush.CardOrder[3].Value, CardValueEnum.JACK);
        Assert.AreEqual (royalFlush.CardOrder[4].Value, CardValueEnum.TEN);
    }

    [Test]
    public void PokerHand_RoyalFlushMultiCard_Test () {
        PokerCard[] cardList = {
            PokerCard.CARD_X_H,
            PokerCard.CARD_9_H,
            PokerCard.CARD_Q_H,
            PokerCard.CARD_A_H,
            PokerCard.CARD_8_H,
            PokerCard.CARD_K_H,
            PokerCard.CARD_J_H,
        };
        PokerHand royalFlush = TestUtils.CreatePokerHand_Debug (cardList); // Expected: Straight Flush - AH, KH, QH, JH, TH

        Assert.AreEqual (royalFlush.HandRank, PokerHandRankEnum.ROYAL_FLUSH); // Check Hand Rank.

        // Check Suit parity.
        Assert.AreEqual (royalFlush.CardOrder[0].Suit, royalFlush.CardOrder[1].Suit);
        Assert.AreEqual (royalFlush.CardOrder[0].Suit, royalFlush.CardOrder[2].Suit);
        Assert.AreEqual (royalFlush.CardOrder[0].Suit, royalFlush.CardOrder[3].Suit);
        Assert.AreEqual (royalFlush.CardOrder[0].Suit, royalFlush.CardOrder[4].Suit);

        // Check card order.
        Assert.AreEqual (royalFlush.CardOrder[0].Value, CardValueEnum.ACE);
        Assert.AreEqual (royalFlush.CardOrder[1].Value, CardValueEnum.KING);
        Assert.AreEqual (royalFlush.CardOrder[2].Value, CardValueEnum.QUEEN);
        Assert.AreEqual (royalFlush.CardOrder[3].Value, CardValueEnum.JACK);
        Assert.AreEqual (royalFlush.CardOrder[4].Value, CardValueEnum.TEN);
    }

    [Test]
    public void PokerHand_RoyalFlush2Suits_Test () {
        PokerCard[] cardList = {
            PokerCard.CARD_A_C,
            PokerCard.CARD_X_C,
            PokerCard.CARD_J_H,
            PokerCard.CARD_Q_H,
            PokerCard.CARD_J_C,
            PokerCard.CARD_K_H,
            PokerCard.CARD_X_H,
            PokerCard.CARD_K_C,
            PokerCard.CARD_Q_C,
            PokerCard.CARD_A_H,
        };
        PokerHand royalFlush = TestUtils.CreatePokerHand_Debug (cardList); // Expected: Flush - KC, QC, JC, XC, 9C

        Assert.AreEqual (royalFlush.HandRank, PokerHandRankEnum.ROYAL_FLUSH); // Check Hand Rank.

        // Check Suit parity.
        Assert.AreEqual (royalFlush.CardOrder[0].Suit, royalFlush.CardOrder[1].Suit);
        Assert.AreEqual (royalFlush.CardOrder[0].Suit, royalFlush.CardOrder[2].Suit);
        Assert.AreEqual (royalFlush.CardOrder[0].Suit, royalFlush.CardOrder[3].Suit);
        Assert.AreEqual (royalFlush.CardOrder[0].Suit, royalFlush.CardOrder[4].Suit);

        // Check card order.
        Assert.AreEqual (royalFlush.CardOrder[0].Value, CardValueEnum.ACE);
        Assert.AreEqual (royalFlush.CardOrder[1].Value, CardValueEnum.KING);
        Assert.AreEqual (royalFlush.CardOrder[2].Value, CardValueEnum.QUEEN);
        Assert.AreEqual (royalFlush.CardOrder[3].Value, CardValueEnum.JACK);
        Assert.AreEqual (royalFlush.CardOrder[4].Value, CardValueEnum.TEN);
    }
}
