using NUnit.Framework;
using System;
using System.Collections.Generic;
using GameUtils;
using TCSHoldEmPoker.Models.Define;
using TCSHoldEmPoker.Network.Data;
using TCSHoldEmPoker.Network.Define;
using TCSHoldEmPoker.Network.Events;

public class PokerGameEventByuteTests {

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

        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventPlayerJoin (evt1);
        TestPokerGameEventByteArray (evtBytes, 
            expectedSize: ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_PLAYER_JOIN,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_PLAYER_JOIN);

        Assert.IsTrue (PokerGameEventByteConverter.BytesToPokerGameEventPlayerJoin (evtBytes, out var evt2));
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

        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventPlayerLeave (evt1);
        TestPokerGameEventByteArray (evtBytes,
            expectedSize: ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_PLAYER_LEAVE,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_PLAYER_LEAVE);

        Assert.IsTrue (PokerGameEventByteConverter.BytesToPokerGameEventPlayerLeave (evtBytes, out var evt2));
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

        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventPlayerCardDeal (evt1);
        TestPokerGameEventByteArray (evtBytes,
            expectedSize: ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_PLAYER_CARD_DEAL,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_PLAYER_CARD_DEAL);

        Assert.IsTrue (PokerGameEventByteConverter.BytesToPokerGameEventPlayerCardDeal (evtBytes, out var evt2));
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

        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventCommunityCardDeal (evt1);
        TestPokerGameEventByteArray (evtBytes,
            expectedSize: ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_COMMUNITY_CARD_DEAL,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_COMMUNITY_CARD_DEAL);

        Assert.IsTrue (PokerGameEventByteConverter.BytesToPokerGameEventCommunityCardDeal (evtBytes, out var evt2));
        TestPokerGameEventCommonData (evt1, evt2);
        Assert.AreEqual (evt1.pokerCard, evt2.pokerCard);
        Assert.AreEqual (evt1.cardIndex, evt2.cardIndex);
    }

    [Test]
    public void PokerData_AnteStartEventConversion_Test () {
        AnteStartGameEvent evt1 = new () {
            gameTableID = 4859205,
        };

        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventAnteStart (evt1);
        TestPokerGameEventByteArray (evtBytes,
            expectedSize: ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_ANTE_START,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_ANTE_START);

        Assert.IsTrue (PokerGameEventByteConverter.BytesToPokerGameEventAnteStart (evtBytes, out var evt2));
        TestPokerGameEventCommonData (evt1, evt2);
    }

    [Test]
    public void PokerData_AntePhaseChangeEventConversion_Test () {
        GamePhaseChangeGameEvent evt1 = new () {
            gameTableID = 4568289,
            gamePhase = PokerGamePhaseEnum.THE_RIVER,
        };

        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventAntePhaseChange (evt1);
        TestPokerGameEventByteArray (evtBytes,
            expectedSize: ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_ANTE_PHASE_CHANGE,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_ANTE_PHASE_CHANGE);

        Assert.IsTrue (PokerGameEventByteConverter.BytesToPokerGameEventAntePhaseChange (evtBytes, out var evt2));
        TestPokerGameEventCommonData (evt1, evt2);
        Assert.AreEqual (evt1.gamePhase, evt2.gamePhase);
    }

    [Test]
    public void PokerData_AnteTurnChangeEventConversion_Test () {
        ChangeTurnSeatIndexGameEvent evt1 = new () {
            gameTableID = 564567,
            seatIndex = 0,
        };

        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventAnteTurnChange (evt1);
        TestPokerGameEventByteArray (evtBytes,
            expectedSize: ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_ANTE_TURN_CHANGE,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_ANTE_TURN_CHANGE);

        Assert.IsTrue (PokerGameEventByteConverter.BytesToPokerGameEventAnteTurnChange (evtBytes, out var evt2));
        TestPokerGameEventCommonData (evt1, evt2);
        Assert.AreEqual (evt1.seatIndex, evt2.seatIndex);
    }

    [Test]
    public void PokerData_AnteEndEventConversion_Test () {
        AnteEndGameEvent evt1 = new () {
            gameTableID = 4859205,
        };

        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventAnteEnd (evt1);
        TestPokerGameEventByteArray (evtBytes,
            expectedSize: ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_ANTE_END,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_ANTE_END);

        Assert.IsTrue (PokerGameEventByteConverter.BytesToPokerGameEventAnteEnd (evtBytes, out var evt2));
        TestPokerGameEventCommonData (evt1, evt2);
    }

}