using NUnit.Framework;
using TCSHoldEmPoker.Models;
using GameUtils;

public class TableActivityTests {

    [Test]
    public void PokerHand_DeckShuffle_Test () {
        CardDeck deck = new ();

        int shuffleCount = 10000;
        for (int i = 0; i < shuffleCount; i++) {
            deck.Shuffle ();
            var d1 = deck.ShuffledDeck;
            deck.Shuffle ();
            var d2 = deck.ShuffledDeck;

            Assert.IsFalse (ListUtils.CheckEquals (d1, d2));
        }
    }
}