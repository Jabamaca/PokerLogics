using NUnit.Framework;
using System;
using System.Collections.Generic;
using GameUtils;
using TCSHoldEmPoker.Models.Define;
using TCSHoldEmPoker.Network.Data;
using TCSHoldEmPoker.Network.Define;
using TCSHoldEmPoker.Network.Events;

public class PokerGameEventByuteTests {

    [Test]
    public void PokerData_PlayerJoinEventConversion_Test () {
        PlayerJoinGameEvent evt = new () {
            gameTableID = 194532,
            playerID = 5342,
            buyInChips = 1000000,
        };

        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventPlayerJoin (evt);
        int i = 0;
        UInt16 startSig = BitConverter.ToUInt16 (evtBytes, startIndex: i);
        Assert.AreEqual (startSig, ByteConverterUtils.NETWORK_ACTIVITY_START);
        i += ByteConverterUtils.SIZEOF_NETWORK_ACTIVITY_START;
        NetworkActivityID testID = (NetworkActivityID)BitConverter.ToInt16 (evtBytes, startIndex: i);
        Assert.IsTrue (testID == NetworkActivityID.POKER_GAME_EVENT_PLAYER_JOIN);
        UInt16 endSig = BitConverter.ToUInt16 (evtBytes, startIndex: evtBytes.Length - ByteConverterUtils.SIZEOF_NETWORK_ACTIVITY_END);
        Assert.AreEqual (endSig, ByteConverterUtils.NETWORK_ACTIVITY_END);
        Assert.AreEqual (evtBytes.Length, ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_PLAYER_JOIN);

        Assert.IsTrue (PokerGameEventByteConverter.BytesToPokerGameEventPlayerJoin (evtBytes, out var evt2));
        Assert.AreEqual (evt.gameTableID, evt2.gameTableID);
        Assert.AreEqual (evt.playerID, evt2.playerID);
        Assert.AreEqual (evt.buyInChips, evt2.buyInChips);
    }

    [Test]
    public void PokerData_PlayerLeaveEventConversion_Test () {
        PlayerLeaveGameEvent evt = new () {
            gameTableID = 194532,
            playerID = 5342,
        };

        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventPlayerLeave (evt);
        int i = 0;
        UInt16 startSig = BitConverter.ToUInt16 (evtBytes, startIndex: i);
        Assert.AreEqual (startSig, ByteConverterUtils.NETWORK_ACTIVITY_START);
        i += ByteConverterUtils.SIZEOF_NETWORK_ACTIVITY_START;
        NetworkActivityID testID = (NetworkActivityID)BitConverter.ToInt16 (evtBytes, startIndex: i);
        Assert.IsTrue (testID == NetworkActivityID.POKER_GAME_EVENT_PLAYER_LEAVE);
        UInt16 endSig = BitConverter.ToUInt16 (evtBytes, startIndex: evtBytes.Length - ByteConverterUtils.SIZEOF_NETWORK_ACTIVITY_END);
        Assert.AreEqual (endSig, ByteConverterUtils.NETWORK_ACTIVITY_END);
        Assert.AreEqual (evtBytes.Length, ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_PLAYER_LEAVE);

        Assert.IsTrue (PokerGameEventByteConverter.BytesToPokerGameEventPlayerLeave (evtBytes, out var evt2));
        Assert.AreEqual (evt.gameTableID, evt2.gameTableID);
        Assert.AreEqual (evt.playerID, evt2.playerID);
    }

    [Test]
    public void PokerData_PlayerCardsDealEventConversion_Test () {
        List<PokerCard> cards = new () {
            PokerCard.CARD_X_D,
            PokerCard.CARD_A_C,
        };

        PlayerCardsDealGameEvent evt = new () {
            gameTableID = 1759833,
            playerID = 7392,
            cards = cards,
        };

        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventPlayerCardDeal (evt);
        int i = 0;
        UInt16 startSig = BitConverter.ToUInt16 (evtBytes, startIndex: i);
        Assert.AreEqual (startSig, ByteConverterUtils.NETWORK_ACTIVITY_START);
        i += ByteConverterUtils.SIZEOF_NETWORK_ACTIVITY_START;
        NetworkActivityID testID = (NetworkActivityID)BitConverter.ToInt16 (evtBytes, startIndex: i);
        Assert.IsTrue (testID == NetworkActivityID.POKER_GAME_EVENT_PLAYER_CARD_DEAL);
        UInt16 endSig = BitConverter.ToUInt16 (evtBytes, startIndex: evtBytes.Length - ByteConverterUtils.SIZEOF_NETWORK_ACTIVITY_END);
        Assert.AreEqual (endSig, ByteConverterUtils.NETWORK_ACTIVITY_END);
        Assert.AreEqual (evtBytes.Length, ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_PLAYER_CARD_DEAL);

        Assert.IsTrue (PokerGameEventByteConverter.BytesToPokerGameEventPlayerCardDeal (evtBytes, out var evt2));
        Assert.AreEqual (evt.gameTableID, evt2.gameTableID);
        Assert.AreEqual (evt.playerID, evt2.playerID);
        Assert.IsTrue (ListUtils.CheckEquals (evt.cards, evt2.cards));
    }

    [Test]
    public void PokerData_CommunityCardDealEventConversion_Test () {
        CommunityCardDealGameEvent evt = new () {
            gameTableID = 1759833,
            pokerCard = PokerCard.CARD_A_D,
            cardIndex = 2,
        };

        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventCommunityCardDeal (evt);
        int i = 0;
        UInt16 startSig = BitConverter.ToUInt16 (evtBytes, startIndex: i);
        Assert.AreEqual (startSig, ByteConverterUtils.NETWORK_ACTIVITY_START);
        i += ByteConverterUtils.SIZEOF_NETWORK_ACTIVITY_START;
        NetworkActivityID testID = (NetworkActivityID)BitConverter.ToInt16 (evtBytes, startIndex: i);
        Assert.IsTrue (testID == NetworkActivityID.POKER_GAME_EVENT_COMMUNITY_CARD_DEAL);
        UInt16 endSig = BitConverter.ToUInt16 (evtBytes, startIndex: evtBytes.Length - ByteConverterUtils.SIZEOF_NETWORK_ACTIVITY_END);
        Assert.AreEqual (endSig, ByteConverterUtils.NETWORK_ACTIVITY_END);
        Assert.AreEqual (evtBytes.Length, ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_COMMUNITY_CARD_DEAL);

        Assert.IsTrue (PokerGameEventByteConverter.BytesToPokerGameEventCommunityCardDeal (evtBytes, out var evt2));
        Assert.AreEqual (evt.gameTableID, evt2.gameTableID);
        Assert.AreEqual (evt.pokerCard, evt2.pokerCard);
        Assert.AreEqual (evt.cardIndex, evt2.cardIndex);
    }

}