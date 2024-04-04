using NUnit.Framework;
using System;
using System.Collections.Generic;
using GameUtils;
using TCSHoldEmPoker.Models.Define;
using TCSHoldEmPoker.Network.Data;
using TCSHoldEmPoker.Network.Define;
using TCSHoldEmPoker.Network.Inputs;
using TCSHoldEmPoker.Network.Events;

public class PokerGameInputByteTests {

    private void TestPokerGameInputByteArray (byte[] inputBytes, int expectedSize, NetworkActivityID expectedNetActID) {
        int i = 0;
        UInt16 startSig = BitConverter.ToUInt16 (inputBytes, startIndex: i);
        Assert.AreEqual (startSig, ByteConverterUtils.NETWORK_ACTIVITY_START);
        i += ByteConverterUtils.SIZEOF_NETWORK_ACTIVITY_START;
        NetworkActivityID testID = (NetworkActivityID)BitConverter.ToInt16 (inputBytes, startIndex: i);
        Assert.IsTrue (testID == expectedNetActID);
        UInt16 endSig = BitConverter.ToUInt16 (inputBytes, startIndex: inputBytes.Length - ByteConverterUtils.SIZEOF_NETWORK_ACTIVITY_END);
        Assert.AreEqual (endSig, ByteConverterUtils.NETWORK_ACTIVITY_END);
        Assert.AreEqual (inputBytes.Length, expectedSize);
    }

    private void TestPokerGameInputCommonData (PokerGameInput input1, PokerGameInput input2) {
        Assert.AreEqual (input1.gameTableID, input2.gameTableID);
        Assert.AreEqual (input1.playerID, input2.playerID);
    }

    [Test]
    public void PokerData_PlayerJoinRequestInputConversion_Test () {
        PlayerJoinRequestGameInput input1 = new () {
            gameTableID = 24211,
            playerID = 9087,
            buyInChips = 500000,
        };

        int expectedSize = ByteConverterUtils.SIZEOF_POKER_GAME_INPUT_PLAYER_JOIN_REQUEST;
        byte[] inputBytes = PokerGameInputByteConverted.BytesFromPokerGameInputPlayerJoinRequest (input1);
        TestPokerGameInputByteArray (inputBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_INPUT_PLAYER_REQUEST_JOIN);

        int currentDataIndex = 0;
        PokerGameInputByteConverted.BytesToPokerGameInputPlayerJoinRequest (inputBytes, ref currentDataIndex, out var input2);
        Assert.AreEqual (currentDataIndex, expectedSize);
        TestPokerGameInputCommonData (input1, input2);
        Assert.AreEqual (input1.buyInChips, input2.buyInChips);
    }

    [Test]
    public void PokerData_PlayerLeaveRequestInputConversion_Test () {
        PlayerLeaveRequestGameInput input1 = new () {
            gameTableID = 95211,
            playerID = 222,
        };

        int expectedSize = ByteConverterUtils.SIZEOF_POKER_GAME_INPUT_PLAYER_LEAVE_REQUEST;
        byte[] inputBytes = PokerGameInputByteConverted.BytesFromPokerGameInputPlayerLeaveRequest (input1);
        TestPokerGameInputByteArray (inputBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_INPUT_PLAYER_REQUEST_LEAVE);

        int currentDataIndex = 0;
        PokerGameInputByteConverted.BytesToPokerGameInputPlayerLeaveRequest (inputBytes, ref currentDataIndex, out var input2);
        Assert.AreEqual (currentDataIndex, expectedSize);
        TestPokerGameInputCommonData (input1, input2);
    }

}