using TCSHoldEmPoker.Models;
using System.Collections.Generic;

public static class TestUtils {

    private const string CARD_PRINT_SPLIT = "  ";

    public static string PrintPokerCardList (IEnumerable<PokerCard> cards) {
        string print = "";

        foreach (var card in cards) {
            print += card + CARD_PRINT_SPLIT;
        }

        print = print.Remove (print.Length - CARD_PRINT_SPLIT.Length);

        return print;
    }

    public static string PrintPokerCardSets (params IEnumerable<PokerCard>[] cardSets) {
        string print = "";

        foreach (var cardSet in cardSets) {
            print += PrintPokerCardList (cardSet) + CARD_PRINT_SPLIT;
        }

        print = print.Remove (print.Length - CARD_PRINT_SPLIT.Length);

        return print;
    }

    public static PokerHand CreatePokerHand_Debug (IEnumerable<PokerCard> cards) {
        PokerHand returnHand = PokerHandFactory.GetHighestPokerHandWithCardSet (cards);

        UnityEngine.Debug.Log ("INPUT CARDS - " + TestUtils.PrintPokerCardList (cards));
        UnityEngine.Debug.Log ("RESULT HAND - " + returnHand);

        return returnHand;
    }

    public static PokerHand CreatePokerHandWithSets_Debug (params IEnumerable<PokerCard>[] cardSets) {
        PokerHand returnHand = PokerHandFactory.GetHighestPokerHandWithCardSets (cardSets);

        UnityEngine.Debug.Log ("INPUT CARDS - " + TestUtils.PrintPokerCardSets (cardSets));
        UnityEngine.Debug.Log ("RESULT HAND - " + returnHand);

        return returnHand;
    }
}
