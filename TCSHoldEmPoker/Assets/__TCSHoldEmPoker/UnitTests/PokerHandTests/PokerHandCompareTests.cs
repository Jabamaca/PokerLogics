using NUnit.Framework;
using System.Collections.Generic;
using TCSHoldEmPoker.Models;
using TCSHoldEmPoker.Models.Define;

public class PokerHandCompareTests {

    [Test]
    public void PokerHand_HighCardCompare_Test () {
        PokerCard[] cardList1 = {
            PokerCard.CARD_A_C,
            PokerCard.CARD_2_D,
            PokerCard.CARD_X_H,
            PokerCard.CARD_6_D,
            PokerCard.CARD_4_S,
        };
        PokerHand highCard1 = TestUtils.CreatePokerHand_Debug (cardList1); // HighCard - AC, XH, 6D, 4S, 2D

        // Same values as cardList1, but different suits.
        PokerCard[] cardList2 = {
            PokerCard.CARD_6_H,
            PokerCard.CARD_2_C,
            PokerCard.CARD_4_D,
            PokerCard.CARD_X_S,
            PokerCard.CARD_A_S,
        };
        PokerHand highCard2 = TestUtils.CreatePokerHand_Debug (cardList2); // HighCard - AS, XS, 6H, 4D, 2C

        PokerCard[] cardList3 = {
            PokerCard.CARD_X_S,
            PokerCard.CARD_J_C,
            PokerCard.CARD_3_C,
            PokerCard.CARD_8_C,
            PokerCard.CARD_7_D,
        };
        PokerHand highCard3 = TestUtils.CreatePokerHand_Debug (cardList3); // HighCard - JC, XS, 8C, 7D, 3D

        Assert.IsTrue (highCard1.CompareTo (highCard2) == 0); // highCard1 and highCard2 ties.
        Assert.IsTrue (highCard1.CompareTo (highCard3) > 0); // highCard1 wins against highCard3.
        Assert.IsTrue (highCard3.CompareTo (highCard2) < 0); // highCard3 loses against highCard2.
    }

    [Test]
    public void PokerHand_KickerCardCompare_Test () {
        PokerHand onePair1 = TestUtils.CreatePokerHand_Debug (new List<PokerCard> () {
            PokerCard.CARD_A_C,
            PokerCard.CARD_2_D,
            PokerCard.CARD_X_H,
            PokerCard.CARD_6_D,
            PokerCard.CARD_2_S,
        }); // HighCard - 2D, 2S, AC, XH, 6D

        PokerHand onePair2 = TestUtils.CreatePokerHand_Debug (new List<PokerCard> () {
            PokerCard.CARD_A_S,
            PokerCard.CARD_2_C,
            PokerCard.CARD_X_D,
            PokerCard.CARD_3_S,
            PokerCard.CARD_2_H,
        }); // HighCard - 2C, 2H, AS, XD, 3S

        Assert.IsTrue (onePair1.CompareTo (onePair2) > 0); // onePair1 wins against onePair2.
    }

    [Test]
    public void PokerHand_HoldEmCompare_Test () {
        List<PokerCard> communityCards = new () {
            PokerCard.CARD_4_H,
            PokerCard.CARD_6_H,
            PokerCard.CARD_7_H,
            PokerCard.CARD_3_C,
            PokerCard.CARD_3_H,
        };

        List<PokerCard> player1Deal = new () {
            PokerCard.CARD_5_D,
            PokerCard.CARD_8_S,
        };

        List<PokerCard> player2Deal = new () {
            PokerCard.CARD_2_H,
            PokerCard.CARD_3_S,
        };

        PokerHand player1Hand = TestUtils.CreatePokerHandWithSets_Debug (communityCards, player1Deal); // Straight - 8S, 7H, 6H, 5D, 4H
        PokerHand player2Hand = TestUtils.CreatePokerHandWithSets_Debug (communityCards, player2Deal); // Flush - 7H, 6H, 4H, 3H, 2H

        Assert.IsTrue (player1Hand.CompareTo (player2Hand) < 0); // player1 loses against player2.
    }
}