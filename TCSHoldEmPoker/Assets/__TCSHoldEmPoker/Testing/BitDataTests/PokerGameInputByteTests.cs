using NUnit.Framework;
using System;
using TCSHoldEmPoker.Network.Data;
using TCSHoldEmPoker.Network.Define;
using TCSHoldEmPoker.Network.Inputs;

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

        int expectedSize = ByteConverterUtils.SIZEOF_POKER_GAME_INPUT_PLAYER_REQUEST_JOIN;
        byte[] inputBytes = PokerGameInputByteConverted.BytesFromPokerGameInputPlayerRequestJoin (input1);
        TestPokerGameInputByteArray (inputBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_INPUT_PLAYER_REQUEST_JOIN);

        int currentDataIndex = 0;
        PokerGameInputByteConverted.BytesToPokerGameInputPlayerRequestJoin (inputBytes, ref currentDataIndex, out var input2);
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

        int expectedSize = ByteConverterUtils.SIZEOF_POKER_GAME_INPUT_PLAYER_REQUEST_LEAVE;
        byte[] inputBytes = PokerGameInputByteConverted.BytesFromPokerGameInputPlayerRequestLeave (input1);
        TestPokerGameInputByteArray (inputBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_INPUT_PLAYER_REQUEST_LEAVE);

        int currentDataIndex = 0;
        PokerGameInputByteConverted.BytesToPokerGameInputPlayerRequestLeave (inputBytes, ref currentDataIndex, out var input2);
        Assert.AreEqual (currentDataIndex, expectedSize);
        TestPokerGameInputCommonData (input1, input2);
    }

    [Test]
    public void PokerData_PlayerActionBetCheckInputConversion_Test () {
        PlayerBetCheckActionGameInput input1 = new () {
            gameTableID = 85311,
            playerID = 8999,
        };

        int expectedSize = ByteConverterUtils.SIZEOF_POKER_GAME_INPUT_PLAYER_ACTION_BET_CHECK;
        byte[] inputBytes = PokerGameInputByteConverted.BytesFromPokerGameInputPlayerActionBetCheck (input1);
        TestPokerGameInputByteArray (inputBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_INPUT_PLAYER_ACTION_BET_CHECK);

        int currentDataIndex = 0;
        PokerGameInputByteConverted.BytesToPokerGameInputPlayerActionBetCheck (inputBytes, ref currentDataIndex, out var input2);
        Assert.AreEqual (currentDataIndex, expectedSize);
        TestPokerGameInputCommonData (input1, input2);
    }

    [Test]
    public void PokerData_PlayerActionBetCallInputConversion_Test () {
        PlayerBetCallActionGameInput input1 = new () {
            gameTableID = 13165,
            playerID = 9096,
        };

        int expectedSize = ByteConverterUtils.SIZEOF_POKER_GAME_INPUT_PLAYER_ACTION_BET_CALL;
        byte[] inputBytes = PokerGameInputByteConverted.BytesFromPokerGameInputPlayerActionBetCall (input1);
        TestPokerGameInputByteArray (inputBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_INPUT_PLAYER_ACTION_BET_CALL);

        int currentDataIndex = 0;
        PokerGameInputByteConverted.BytesToPokerGameInputPlayerActionBetCall (inputBytes, ref currentDataIndex, out var input2);
        Assert.AreEqual (currentDataIndex, expectedSize);
        TestPokerGameInputCommonData (input1, input2);
    }

    [Test]
    public void PokerData_PlayerActionBetRaiseInputConversion_Test () {
        PlayerBetRaiseActionGameInput input1 = new () {
            gameTableID = 66331,
            playerID = 6969,
            newStake = 70000,
        };

        int expectedSize = ByteConverterUtils.SIZEOF_POKER_GAME_INPUT_PLAYER_ACTION_BET_RAISE;
        byte[] inputBytes = PokerGameInputByteConverted.BytesFromPokerGameInputPlayerActionBetRaise (input1);
        TestPokerGameInputByteArray (inputBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_INPUT_PLAYER_ACTION_BET_RAISE);

        int currentDataIndex = 0;
        PokerGameInputByteConverted.BytesToPokerGameInputPlayerActionBetRaise (inputBytes, ref currentDataIndex, out var input2);
        Assert.AreEqual (currentDataIndex, expectedSize);
        TestPokerGameInputCommonData (input1, input2);
        Assert.AreEqual (input1.newStake, input2.newStake);
    }

    [Test]
    public void PokerData_PlayerActionBetFoldInputConversion_Test () {
        PlayerBetFoldActionGameInput input1 = new () {
            gameTableID = 66331,
            playerID = 6969,
        };

        int expectedSize = ByteConverterUtils.SIZEOF_POKER_GAME_INPUT_PLAYER_ACTION_BET_FOLD;
        byte[] inputBytes = PokerGameInputByteConverted.BytesFromPokerGameInputPlayerActionBetFold (input1);
        TestPokerGameInputByteArray (inputBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_INPUT_PLAYER_ACTION_BET_FOLD);

        int currentDataIndex = 0;
        PokerGameInputByteConverted.BytesToPokerGameInputPlayerActionBetFold (inputBytes, ref currentDataIndex, out var input2);
        Assert.AreEqual (currentDataIndex, expectedSize);
        TestPokerGameInputCommonData (input1, input2);
    }

}