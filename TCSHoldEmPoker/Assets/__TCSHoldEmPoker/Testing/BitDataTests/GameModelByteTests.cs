using NUnit.Framework;
using TCSHoldEmPoker.Network.Data;
using TCSHoldEmPoker.Models;
using TCSHoldEmPoker.Models.Define;

public class GameModelByteTests {

    [Test] 
    public void PokerData_ByteToPokerCardConvert_Test () {
        byte cardByte = 0x54; // (byte)CardValueEnum.FIVE | (byte)CardSuitEnum.SPADES;

        PokerCard cardTest = GameModelByteConverter.ByteToPokerCard (cardByte);
        Assert.AreEqual (cardTest.Value, CardValueEnum.FIVE);
        Assert.AreEqual (cardTest.Suit, CardSuitEnum.SPADES);
    }

    [Test]
    public void PokerData_PokerCardToByteConvert_Test () {
        PokerCard card1 = PokerCard.CARD_2_H;
        byte cardByte = GameModelByteConverter.ByteFromPokerCard (card1);
        PokerCard card2 = GameModelByteConverter.ByteToPokerCard (cardByte);

        Assert.IsTrue (card1.Equals (card2));
    }

    [Test]
    public void PokerData_PokerHandConversion_Test () {
        PokerCard[] cards = new PokerCard[] {
            PokerCard.CARD_2_D,
            PokerCard.CARD_2_C,
            PokerCard.CARD_2_H,
            PokerCard.CARD_5_H,
            PokerCard.CARD_5_C,
        };

        PokerHand hand1 = PokerHandFactory.GetHighestPokerHandWithCardSet (cards);
        byte[] handBytes = GameModelByteConverter.BytesFromPokerHand (hand1);
        Assert.IsTrue (GameModelByteConverter.BytesToPokerHand (handBytes, out var hand2));

        Assert.AreEqual (hand1.CompareTo (hand2), 0); // Check Compare equal.
    }

    [Test]
    public void PokerData_PokerHandErrorData_Test () {
        
    }
}