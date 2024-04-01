using NUnit.Framework;
using System;
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
        i += sizeof (Int16);
        NetworkActivityID testID = (NetworkActivityID)BitConverter.ToInt16 (evtBytes, startIndex: i);
        Assert.IsTrue (testID == NetworkActivityID.POKER_GAME_EVENT_PLAYER_JOIN);
        UInt16 endSig = BitConverter.ToUInt16 (evtBytes, startIndex: evtBytes.Length - ByteConverterUtils.SIZEOF_NETWORK_ACTIVITY_END);
        Assert.AreEqual (endSig, ByteConverterUtils.NETWORK_ACTIVITY_END);

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
        i += sizeof (Int16);
        NetworkActivityID testID = (NetworkActivityID)BitConverter.ToInt16 (evtBytes, startIndex: i);
        Assert.IsTrue (testID == NetworkActivityID.POKER_GAME_EVENT_PLAYER_LEAVE);
        UInt16 endSig = BitConverter.ToUInt16 (evtBytes, startIndex: evtBytes.Length - ByteConverterUtils.SIZEOF_NETWORK_ACTIVITY_END);
        Assert.AreEqual (endSig, ByteConverterUtils.NETWORK_ACTIVITY_END);

        Assert.IsTrue (PokerGameEventByteConverter.BytesToPokerGameEventPlayerLeave (evtBytes, out var evt2));
        Assert.AreEqual (evt.gameTableID, evt2.gameTableID);
        Assert.AreEqual (evt.playerID, evt2.playerID);

    }

}