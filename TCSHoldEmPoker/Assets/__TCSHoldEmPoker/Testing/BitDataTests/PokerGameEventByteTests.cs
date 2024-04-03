using NUnit.Framework;
using System;
using System.Collections.Generic;
using GameUtils;
using TCSHoldEmPoker.Models.Define;
using TCSHoldEmPoker.Network.Data;
using TCSHoldEmPoker.Network.Define;
using TCSHoldEmPoker.Network.Events;

public class PokerGameEventByteTests {

    private void TestPokerGameEventByteArray (byte[] evtBytes, int expectedSize, NetworkActivityID expectedNetActID) {
        int i = 0;
        UInt16 startSig = BitConverter.ToUInt16 (evtBytes, startIndex: i);
        Assert.AreEqual (startSig, ByteConverterUtils.NETWORK_ACTIVITY_START);
        i += ByteConverterUtils.SIZEOF_NETWORK_ACTIVITY_START;
        NetworkActivityID testID = (NetworkActivityID)BitConverter.ToInt16 (evtBytes, startIndex: i);
        Assert.IsTrue (testID == expectedNetActID);
        UInt16 endSig = BitConverter.ToUInt16 (evtBytes, startIndex: evtBytes.Length - ByteConverterUtils.SIZEOF_NETWORK_ACTIVITY_END);
        Assert.AreEqual (endSig, ByteConverterUtils.NETWORK_ACTIVITY_END);
        Assert.AreEqual (evtBytes.Length, expectedSize);
    }

    private void TestPokerGameEventCommonData (PokerGameEvent evt1, PokerGameEvent evt2) {
        Assert.AreEqual (evt1.gameTableID, evt2.gameTableID);
    }

    [Test]
    public void PokerData_PlayerJoinEventConversion_Test () {
        PlayerJoinGameEvent evt1 = new () {
            gameTableID = 194532,
            playerID = 5342,
            buyInChips = 1000000,
        };

        int expectedSize = ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_PLAYER_JOIN;
        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventPlayerJoin (evt1);
        TestPokerGameEventByteArray (evtBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_PLAYER_JOIN);

        Assert.AreEqual (PokerGameEventByteConverter.BytesToPokerGameEventPlayerJoin (evtBytes, out var evt2), expectedSize);
        TestPokerGameEventCommonData (evt1, evt2);
        Assert.AreEqual (evt1.playerID, evt2.playerID);
        Assert.AreEqual (evt1.buyInChips, evt2.buyInChips);
    }

    [Test]
    public void PokerData_PlayerLeaveEventConversion_Test () {
        PlayerLeaveGameEvent evt1 = new () {
            gameTableID = 194532,
            playerID = 5342,
        };

        int expectedSize = ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_PLAYER_LEAVE;
        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventPlayerLeave (evt1);
        TestPokerGameEventByteArray (evtBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_PLAYER_LEAVE);

        Assert.AreEqual (PokerGameEventByteConverter.BytesToPokerGameEventPlayerLeave (evtBytes, out var evt2), expectedSize);
        TestPokerGameEventCommonData (evt1, evt2);
        Assert.AreEqual (evt1.playerID, evt2.playerID);
    }

    [Test]
    public void PokerData_PlayerCardsDealEventConversion_Test () {
        List<PokerCard> cards = new () {
            PokerCard.CARD_X_D,
            PokerCard.CARD_A_C,
        };

        PlayerCardsDealGameEvent evt1 = new () {
            gameTableID = 1759833,
            playerID = 7392,
            cards = cards,
        };

        int expectedSize = ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_PLAYER_CARD_DEAL;
        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventPlayerCardDeal (evt1);
        TestPokerGameEventByteArray (evtBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_PLAYER_CARD_DEAL);

        Assert.AreEqual (PokerGameEventByteConverter.BytesToPokerGameEventPlayerCardDeal (evtBytes, out var evt2), expectedSize);
        TestPokerGameEventCommonData (evt1, evt2);
        Assert.AreEqual (evt1.playerID, evt2.playerID);
        Assert.IsTrue (ListUtils.CheckEquals (evt1.cards, evt2.cards));
    }

    [Test]
    public void PokerData_CommunityCardDealEventConversion_Test () {
        CommunityCardDealGameEvent evt1 = new () {
            gameTableID = 1759833,
            pokerCard = PokerCard.CARD_A_D,
            cardIndex = 2,
        };

        int expectedSize = ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_COMMUNITY_CARD_DEAL;
        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventCommunityCardDeal (evt1);
        TestPokerGameEventByteArray (evtBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_COMMUNITY_CARD_DEAL);

        Assert.AreEqual (PokerGameEventByteConverter.BytesToPokerGameEventCommunityCardDeal (evtBytes, out var evt2), expectedSize);
        TestPokerGameEventCommonData (evt1, evt2);
        Assert.AreEqual (evt1.pokerCard, evt2.pokerCard);
        Assert.AreEqual (evt1.cardIndex, evt2.cardIndex);
    }

    [Test]
    public void PokerData_AnteStartEventConversion_Test () {
        AnteStartGameEvent evt1 = new () {
            gameTableID = 4859205,
        };

        int expectedSize = ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_ANTE_START;
        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventAnteStart (evt1);
        TestPokerGameEventByteArray (evtBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_ANTE_START);

        Assert.AreEqual (PokerGameEventByteConverter.BytesToPokerGameEventAnteStart (evtBytes, out var evt2), expectedSize);
        TestPokerGameEventCommonData (evt1, evt2);
    }

    [Test]
    public void PokerData_AntePhaseChangeEventConversion_Test () {
        GamePhaseChangeGameEvent evt1 = new () {
            gameTableID = 4568289,
            gamePhase = PokerGamePhaseEnum.THE_RIVER,
        };

        int expectedSize = ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_ANTE_PHASE_CHANGE;
        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventAntePhaseChange (evt1);
        TestPokerGameEventByteArray (evtBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_ANTE_PHASE_CHANGE);

        Assert.AreEqual (PokerGameEventByteConverter.BytesToPokerGameEventAntePhaseChange (evtBytes, out var evt2), expectedSize);
        TestPokerGameEventCommonData (evt1, evt2);
        Assert.AreEqual (evt1.gamePhase, evt2.gamePhase);
    }

    [Test]
    public void PokerData_AnteTurnChangeEventConversion_Test () {
        ChangeTurnSeatIndexGameEvent evt1 = new () {
            gameTableID = 564567,
            seatIndex = 0,
        };
        int expectedSize = ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_ANTE_TURN_CHANGE;
        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventAnteTurnChange (evt1);
        TestPokerGameEventByteArray (evtBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_ANTE_TURN_CHANGE);

        Assert.AreEqual (PokerGameEventByteConverter.BytesToPokerGameEventAnteTurnChange (evtBytes, out var evt2), expectedSize);
        TestPokerGameEventCommonData (evt1, evt2);
        Assert.AreEqual (evt1.seatIndex, evt2.seatIndex);
    }

    [Test]
    public void PokerData_AnteEndEventConversion_Test () {
        AnteEndGameEvent evt1 = new () {
            gameTableID = 4859205,
        };

        int expectedSize = ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_ANTE_END;
        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventAnteEnd (evt1);
        TestPokerGameEventByteArray (evtBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_ANTE_END);

        Assert.AreEqual (PokerGameEventByteConverter.BytesToPokerGameEventAnteEnd (evtBytes, out var evt2), expectedSize);
        TestPokerGameEventCommonData (evt1, evt2);
    }

    [Test]
    public void PokerData_TableGatherWagersEventConversion_Test () {
        TableGatherWagersGameEvent evt1 = new () {
            gameTableID = 9193821,
            newCashPot = 17000,
        };

        int expectedSize = ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_TABLE_GATHER_WAGERS;
        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventTableGatherWagers (evt1);
        TestPokerGameEventByteArray (evtBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_TABLE_GATHER_WAGERS);

        Assert.AreEqual (PokerGameEventByteConverter.BytesToPokerGameEventTableGatherWagers (evtBytes, out var evt2), expectedSize);
        TestPokerGameEventCommonData (evt1, evt2);
        Assert.AreEqual (evt1.newCashPot, evt2.newCashPot);
    }

    [Test]
    public void PokerData_AllPlayerCardsRevealEventConversion_Test () {
        Dictionary<Int32, IReadOnlyList<PokerCard>> revealedHands = new () {
            {
                19323,
                new List<PokerCard> () { PokerCard.CARD_2_C, PokerCard.CARD_6_C }
            },
            {
                594332,
                new List<PokerCard> () { PokerCard.CARD_J_H, PokerCard.CARD_9_S }
            },
            {
                151192,
                new List<PokerCard> () { PokerCard.CARD_Q_H, PokerCard.CARD_A_D }
            },
            {
                98784,
                new List<PokerCard> () { PokerCard.CARD_7_D, PokerCard.CARD_K_C }
            },
            {
                8463,
                new List<PokerCard> () { PokerCard.CARD_8_C, PokerCard.CARD_A_C }
            }
        };

        AllPlayerCardsRevealGameEvent evt1 = new () {
            gameTableID = 9193821,
            revealedHands = revealedHands,
        };

        int expectedSize = ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_ALL_PLAYER_CARDS_REVEAL_BASE +
            (evt1.revealedHands.Count * ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_ALL_PLAYER_CARDS_REVEAL_PLAYER);
        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventAllPlayerCardsReveal (evt1);
        TestPokerGameEventByteArray (evtBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_ALL_PLAYER_CARDS_REVEAL);

        Assert.AreEqual (PokerGameEventByteConverter.BytesToPokerGameEventAllPlayerCardsReveal (evtBytes, out var evt2), expectedSize);
        TestPokerGameEventCommonData (evt1, evt2);
        Assert.AreEqual (evt1.revealedHands.Count, evt2.revealedHands.Count);
        foreach (var kvp in evt1.revealedHands) {
            Assert.IsTrue (evt2.revealedHands.TryGetValue (kvp.Key, out var value2));
            Assert.IsTrue (ListUtils.CheckEquals (kvp.Value, value2));
        }
    }

    [Test]
    public void PokerData_PlayerWinEventConversion_Test () {
        PlayerWinGameEvent evt1 = new () {
            gameTableID = 9193821,
            playerID = 22342,
            chipsWon = 100000,
        };

        int expectedSize = ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_PLAYER_WIN;
        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventPlayerWin (evt1);
        TestPokerGameEventByteArray (evtBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_PLAYER_WIN);

        Assert.AreEqual (PokerGameEventByteConverter.BytesToPokerGameEventPlayerWin (evtBytes, out var evt2), expectedSize);
        TestPokerGameEventCommonData (evt1, evt2);
        Assert.AreEqual (evt1.playerID, evt2.playerID);
        Assert.AreEqual (evt1.chipsWon, evt2.chipsWon);
    }
}