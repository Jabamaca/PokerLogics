using NUnit.Framework;
using System.Security.Cryptography.X509Certificates;
using TCSHoldEmPoker.Models.Define;

public class PokerGameTest {

    [Test]
    public void PokerGame_FullGame_Test () {

        TestingPokerGameHost tableHost = new (minWager: 1000);

        tableHost.TableHost.TryPlayerIDJoin (playerID: 1001, buyInChips: 10000);
        Assert.IsFalse (tableHost.TableHost.TryPlayerIDJoin (playerID: 1001, buyInChips: 0)); // PlayerID can't join again.
        tableHost.TableHost.StartNewAnte ();
        Assert.IsTrue (tableHost.TableHost.CurrentGamePhase == PokerGamePhaseEnum.WAITING); // Can't start, only 1 player joined.

        tableHost.TableHost.TryPlayerIDJoin (playerID: 2002, buyInChips: 10000);
        tableHost.TableHost.TryPlayerIDJoin (playerID: 3003, buyInChips: 10000);

        // 3-Player Game
        tableHost.TableHost.StartNewAnte ();

        // PRE-FLOP
        // 1st player after Big Blind.
        tableHost.TableHost.PlayerCheckOrCall (tableHost.CurrentTurnID);                        // ***MOVE*** Player-1 CALL Spend:1000 [0 --> 1000]

        Assert.IsTrue (tableHost.TableHost.CurrentGamePhase == PokerGamePhaseEnum.PRE_FLOP);        // Still PRE-FLOP, Blinds must still Check or Call.

        int smallBlindID = tableHost.CurrentTurnID;                                             // Player-2 == Small Blind
        tableHost.TableHost.PlayerCheckOrCall (tableHost.CurrentTurnID);                        // ***MOVE*** Player-2 CALL Spend:500 [500 --> 1000]

        int currentTurnChecker = tableHost.CurrentTurnID;
        tableHost.TableHost.PlayerCheckOrCall (smallBlindID);                                   // Player-2 INVALID MOVE ******************************************
        Assert.AreEqual (currentTurnChecker, tableHost.CurrentTurnID);

        int bigBlindID = tableHost.CurrentTurnID;                                               // Player-3 == Big Blind
        tableHost.TableHost.PlayerCheckOrCall (tableHost.CurrentTurnID);                        // ***MOVE*** Player-3 CHECK Spend:0 [1000 --> 1000]

        // THE FLOP
        Assert.IsTrue (tableHost.TableHost.CurrentGamePhase == PokerGamePhaseEnum.THE_FLOP);        // Proceed to THE FLOP.

        Assert.IsTrue (tableHost.CurrentTurnID == smallBlindID);                                // Small Blind goes first after THE FLOP Reveal.
        tableHost.TableHost.PlayerCheckOrCall (tableHost.CurrentTurnID);                        // ***MOVE*** Player-2 CHECK Spend:0 [0 --> 0]
        tableHost.TableHost.PlayerCheckOrCall (tableHost.CurrentTurnID);                        // ***MOVE*** Player-3 CHECK Spend:0 [0 --> 0]
        tableHost.TableHost.PlayerRaise (tableHost.CurrentTurnID, newStake: 1000);              // ***MOVE*** Player-1 RAISE Spend:1000 [1000 --> 1000]

        Assert.IsTrue (tableHost.TableHost.CurrentGamePhase == PokerGamePhaseEnum.THE_FLOP);        // Still THE FLOP.

        currentTurnChecker = tableHost.CurrentTurnID;
        tableHost.TableHost.PlayerCheckOrCall (bigBlindID);                                     // Player-3 INVALID MOVE ******************************************
        Assert.AreEqual (currentTurnChecker, tableHost.CurrentTurnID);

        tableHost.TableHost.PlayerRaise (tableHost.CurrentTurnID, newStake: 2000);              // ***MOVE*** Player-2 Raise Spend:2000 [0 --> 2000]

        currentTurnChecker = tableHost.CurrentTurnID;
        tableHost.TableHost.PlayerRaise (tableHost.CurrentTurnID, newStake: 1500);              // Player-3 INVALID MOVE ******************************************
        Assert.AreEqual (currentTurnChecker, tableHost.CurrentTurnID);

        tableHost.TableHost.PlayerCheckOrCall (tableHost.CurrentTurnID);                        // ***MOVE*** Player-3 CALL Spend:2000 [0 --> 2000]

        Assert.IsTrue (tableHost.TableHost.CurrentGamePhase == PokerGamePhaseEnum.THE_FLOP);        // Still THE FLOP.

        tableHost.TableHost.PlayerCheckOrCall (tableHost.CurrentTurnID);                        // ***MOVE*** Player-1 CALL Spend:1000 [1000 --> 2000]

        // THE TURN
        Assert.IsTrue (tableHost.TableHost.CurrentGamePhase == PokerGamePhaseEnum.THE_TURN);        // Proceed to THE TURN.

        tableHost.TableHost.PlayerCheckOrCall (tableHost.CurrentTurnID);                        // ***MOVE*** Player-2 CHECK Spend:0 [0 --> 0]
        tableHost.TableHost.PlayerCheckOrCall (tableHost.CurrentTurnID);                        // ***MOVE*** Player-3 CHECK Spend:0 [0 --> 0]
        tableHost.TableHost.PlayerCheckOrCall (tableHost.CurrentTurnID);                        // ***MOVE*** Player-1 CHECK Spend:0 [0 --> 0]

        // THE RIVER
        Assert.IsTrue (tableHost.TableHost.CurrentGamePhase == PokerGamePhaseEnum.THE_RIVER);        // Proceed to THE RIVER.

        tableHost.TableHost.PlayerRaise (tableHost.CurrentTurnID, newStake: 1000);              // ***MOVE*** Player-2 Raise Spend:1000 [0 --> 1000]
        tableHost.TableHost.PlayerRaise (tableHost.CurrentTurnID, newStake: 2000);              // ***MOVE*** Player-3 Raise Spend:2000 [0 --> 2000]
        tableHost.TableHost.PlayerRaise (tableHost.CurrentTurnID, newStake: 3000);              // ***MOVE*** Player-1 Raise Spend:3000 [0 --> 3000]

        Assert.IsTrue (tableHost.TableHost.CurrentGamePhase == PokerGamePhaseEnum.THE_RIVER);        // Still THE RIVER.

        tableHost.TableHost.PlayerCheckOrCall (tableHost.CurrentTurnID);                        // ***MOVE*** Player-2 CHECK Spend:2000 [1000 --> 3000]
        tableHost.TableHost.PlayerCheckOrCall (tableHost.CurrentTurnID);                        // ***MOVE*** Player-3 CHECK Spend:1000 [2000 --> 3000]
    }

    [Test]
    public void PokerGame_AutoWin_Test () {
        TestingPokerGameHost tableHost = new (minWager: 1000);

        tableHost.TableHost.TryPlayerIDJoin (playerID: 1001, buyInChips: 10000);
        tableHost.TableHost.TryPlayerIDJoin (playerID: 2002, buyInChips: 10000);
        tableHost.TableHost.TryPlayerIDJoin (playerID: 3003, buyInChips: 10000);
        tableHost.TableHost.TryPlayerIDJoin (playerID: 4004, buyInChips: 10000);
        tableHost.TableHost.TryPlayerIDJoin (playerID: 5005, buyInChips: 10000);

        // 3-Player Game
        tableHost.TableHost.StartNewAnte ();

        // PRE-FLOP
        // 1st player after Big Blind.
        tableHost.TableHost.PlayerCheckOrCall (tableHost.CurrentTurnID);                        // ***MOVE*** Player-1 CALL Spend:1000 [0 --> 1000]
        tableHost.TableHost.PlayerCheckOrCall (tableHost.CurrentTurnID);                        // ***MOVE*** Player-2 CALL Spend:1000 [0 --> 1000]
        tableHost.TableHost.PlayerCheckOrCall (tableHost.CurrentTurnID);                        // ***MOVE*** Player-3 CALL Spend:1000 [0 --> 1000]
        tableHost.TableHost.PlayerCheckOrCall (tableHost.CurrentTurnID);                        // ***MOVE*** Player-4 CALL Spend:500 [500 --> 1000]
        tableHost.TableHost.PlayerCheckOrCall (tableHost.CurrentTurnID);                        // ***MOVE*** Player-5 CHECK Spend:0 [1000 --> 1000]

        Assert.AreEqual (tableHost.TableHost.CashPot, 5000);                                    // Check POT == 5000

        // THE-FLOP
        Assert.IsTrue (tableHost.TableHost.CurrentGamePhase == PokerGamePhaseEnum.THE_FLOP);        // Proceed to THE FLOP.

        tableHost.TableHost.PlayerCheckOrCall (tableHost.CurrentTurnID);                        // ***MOVE*** Player-4 CHECK Spend:0 [0 --> 0]
        tableHost.TableHost.PlayerRaise (tableHost.CurrentTurnID, newStake: 2000);              // ***MOVE*** Player-5 RAISE Spend:2000 [0 --> 2000]
        tableHost.TableHost.PlayerFold (tableHost.CurrentTurnID);                               // ***MOVE*** Player-1 FOLD Spend:0 [0 --> 0]
        tableHost.TableHost.PlayerCheckOrCall (tableHost.CurrentTurnID);                        // ***MOVE*** Player-2 CALL Spend:2000 [0 --> 2000]
        tableHost.TableHost.PlayerCheckOrCall (tableHost.CurrentTurnID);                        // ***MOVE*** Player-3 CALL Spend:2000 [0 --> 2000]
        tableHost.TableHost.PlayerCheckOrCall (tableHost.CurrentTurnID);                        // ***MOVE*** Player-4 CALL Spend:2000 [0 --> 2000]

        Assert.AreEqual (tableHost.TableHost.CashPot, 13000);                                    // Check POT == 13000

        // THE TURN
        Assert.IsTrue (tableHost.TableHost.CurrentGamePhase == PokerGamePhaseEnum.THE_TURN);        // Proceed to THE TURN.

        tableHost.TableHost.PlayerCheckOrCall (tableHost.CurrentTurnID);                        // ***MOVE*** Player-5 CHECK Spend:0 [0 --> 0]
        tableHost.TableHost.PlayerRaise (tableHost.CurrentTurnID, newStake: 1000);              // ***MOVE*** Player-2 RAISE Spend:1000 [0 --> 1000]
        tableHost.TableHost.PlayerCheckOrCall (tableHost.CurrentTurnID);                        // ***MOVE*** Player-3 CALL Spend:1000 [0 --> 1000]
        tableHost.TableHost.PlayerCheckOrCall (tableHost.CurrentTurnID);                        // ***MOVE*** Player-4 CALL Spend:1000 [0 --> 1000]
        tableHost.TableHost.PlayerFold (tableHost.CurrentTurnID);                               // ***MOVE*** Player-5 FOLD Spend:0 [0 --> 0]

        Assert.AreEqual (tableHost.TableHost.CashPot, 16000);                                    // Check POT == 16000

        // THE RIVER
        Assert.IsTrue (tableHost.TableHost.CurrentGamePhase == PokerGamePhaseEnum.THE_RIVER);        // Proceed to THE RIVER.

        tableHost.TableHost.PlayerRaise (tableHost.CurrentTurnID, newStake: 5000);              // ***MOVE*** Player-2 RAISE Spend:5000 [0 --> 5000]
        tableHost.TableHost.PlayerFold (tableHost.CurrentTurnID);                               // ***MOVE*** Player-3 FOLD Spend:0 [0 --> 0]
        tableHost.TableHost.PlayerFold (tableHost.CurrentTurnID);                               // ***MOVE*** Player-4 FOLD Spend:0 [0 --> 0]
    }

    [Test]
    public void PokerGame_AutoWinShort_Test () {
        TestingPokerGameHost tableHost = new (minWager: 1000);

        tableHost.TableHost.TryPlayerIDJoin (playerID: 1001, buyInChips: 10000);
        tableHost.TableHost.TryPlayerIDJoin (playerID: 2002, buyInChips: 10000);
        tableHost.TableHost.TryPlayerIDJoin (playerID: 3003, buyInChips: 10000);
        tableHost.TableHost.TryPlayerIDJoin (playerID: 4004, buyInChips: 10000);
        tableHost.TableHost.TryPlayerIDJoin (playerID: 5005, buyInChips: 10000);

        // 3-Player Game
        tableHost.TableHost.StartNewAnte ();

        // PRE-FLOP
        // 1st player after Big Blind.
        tableHost.TableHost.PlayerCheckOrCall (tableHost.CurrentTurnID);                        // ***MOVE*** Player-1 CALL Spend:1000 [0 --> 1000]
        tableHost.TableHost.PlayerCheckOrCall (tableHost.CurrentTurnID);                        // ***MOVE*** Player-2 CALL Spend:1000 [0 --> 1000]
        tableHost.TableHost.PlayerCheckOrCall (tableHost.CurrentTurnID);                        // ***MOVE*** Player-3 CALL Spend:1000 [0 --> 1000]
        tableHost.TableHost.PlayerCheckOrCall (tableHost.CurrentTurnID);                        // ***MOVE*** Player-4 CALL Spend:500 [500 --> 1000]
        tableHost.TableHost.PlayerCheckOrCall (tableHost.CurrentTurnID);                        // ***MOVE*** Player-5 CHECK Spend:0 [1000 --> 1000]

        Assert.AreEqual (tableHost.TableHost.CashPot, 5000);                                    // Check POT == 5000

        // THE-FLOP
        Assert.IsTrue (tableHost.TableHost.CurrentGamePhase == PokerGamePhaseEnum.THE_FLOP);        // Proceed to THE FLOP.

        tableHost.TableHost.PlayerCheckOrCall (tableHost.CurrentTurnID);                        // ***MOVE*** Player-4 CHECK Spend:0 [0 --> 0]
        tableHost.TableHost.PlayerRaise (tableHost.CurrentTurnID, newStake: 2000);              // ***MOVE*** Player-5 RAISE Spend:2000 [0 --> 2000]
        tableHost.TableHost.PlayerFold (tableHost.CurrentTurnID);                               // ***MOVE*** Player-1 FOLD Spend:0 [0 --> 0]
        tableHost.TableHost.PlayerCheckOrCall (tableHost.CurrentTurnID);                        // ***MOVE*** Player-2 CALL Spend:2000 [0 --> 2000]
        tableHost.TableHost.PlayerCheckOrCall (tableHost.CurrentTurnID);                        // ***MOVE*** Player-3 CALL Spend:2000 [0 --> 2000]
        tableHost.TableHost.PlayerRaise (tableHost.CurrentTurnID, newStake: 5000);              // ***MOVE*** Player-4 RAISE Spend:5000 [0 --> 5000]
        tableHost.TableHost.PlayerFold (tableHost.CurrentTurnID);                               // ***MOVE*** Player-5 FOLD Spend:0 [0 --> 0]
        tableHost.TableHost.PlayerFold (tableHost.CurrentTurnID);                               // ***MOVE*** Player-2 FOLD Spend:0 [0 --> 0]
        tableHost.TableHost.PlayerFold (tableHost.CurrentTurnID);                               // ***MOVE*** Player-3 FOLD Spend:0 [0 --> 0]
    }
}
