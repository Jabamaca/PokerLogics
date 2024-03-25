using NUnit.Framework;
using TCSHoldEmPoker.Models.Define;

public class PokerGameTest {

    [Test]
    public void PokerGame_FullGame_Test () {

        TestingPokerGameHost tableHost = new (minWager: 100);

        tableHost.TableHost.TryPlayerIDJoin (playerID: 1001, buyInChips: 10000);
        Assert.IsFalse (tableHost.TableHost.TryPlayerIDJoin (playerID: 1001, buyInChips: 0)); // PlayerID can't join again.
        tableHost.TableHost.StartNewAnte ();
        Assert.IsTrue (tableHost.TableHost.CurrentGamePhase == PokerGamePhase.WAITING); // Can't start, only 1 player joined.

        tableHost.TableHost.TryPlayerIDJoin (playerID: 2002, buyInChips: 10000);
        tableHost.TableHost.TryPlayerIDJoin (playerID: 3003, buyInChips: 10000);

        // 3-Player Game
        tableHost.TableHost.StartNewAnte ();

        // 1st player after Big Blind.
        tableHost.TableHost.PlayerCheckOrCall (tableHost.CurrentTurnID);
        Assert.IsTrue (tableHost.TableHost.CurrentGamePhase == PokerGamePhase.PRE_FLOP); // Still PRE-FLOP, Blinds must still Check or Call.

        int smallBlindID = tableHost.CurrentTurnID;
        tableHost.TableHost.PlayerCheckOrCall (tableHost.CurrentTurnID);
        int bigBlindID = tableHost.CurrentTurnID;
        tableHost.TableHost.PlayerCheckOrCall (tableHost.CurrentTurnID);
        Assert.IsTrue (tableHost.TableHost.CurrentGamePhase == PokerGamePhase.THE_FLOP); // Still PRE-FLOP, Blinds must still Check or Call.
        Assert.IsTrue (tableHost.CurrentTurnID == smallBlindID); // Small Blind goes first after THE FLOP Reveal.
    }

}
