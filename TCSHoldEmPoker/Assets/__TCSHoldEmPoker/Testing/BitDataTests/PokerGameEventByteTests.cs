using NUnit.Framework;
using System;
using System.Collections.Generic;
using GameUtils;
using TCSHoldEmPoker.Models.Define;
using TCSHoldEmPoker.Network.Data;
using TCSHoldEmPoker.Network.Define;
using TCSHoldEmPoker.Network.Activity.PokerGameEvents;

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

        int expectedSize = ByteConverterUtils.SizeOf (evt1);
        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventPlayerJoin (evt1);
        TestPokerGameEventByteArray (evtBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_PLAYER_JOIN);

        int currentDataIndex = 0;
        PokerGameEventByteConverter.BytesToPokerGameEventPlayerJoin (evtBytes, ref currentDataIndex, out var evt2);
        Assert.AreEqual (currentDataIndex, expectedSize);
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

        int expectedSize = ByteConverterUtils.SizeOf (evt1);
        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventPlayerLeave (evt1);
        TestPokerGameEventByteArray (evtBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_PLAYER_LEAVE);

        int currentDataIndex = 0;
        PokerGameEventByteConverter.BytesToPokerGameEventPlayerLeave (evtBytes, ref currentDataIndex, out var evt2);
        Assert.AreEqual (currentDataIndex, expectedSize);
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

        int currentDataIndex = 0;
        PokerGameEventByteConverter.BytesToPokerGameEventPlayerCardDeal (evtBytes, ref currentDataIndex, out var evt2);
        Assert.AreEqual (currentDataIndex, expectedSize);
        TestPokerGameEventCommonData (evt1, evt2);
        Assert.AreEqual (evt1.playerID, evt2.playerID);
        Assert.IsTrue (ListUtils.CheckEqualsOrder (evt1.cards, evt2.cards));
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

        int currentDataIndex = 0;
        PokerGameEventByteConverter.BytesToPokerGameEventCommunityCardDeal (evtBytes, ref currentDataIndex, out var evt2);
        Assert.AreEqual (currentDataIndex, expectedSize);
        TestPokerGameEventCommonData (evt1, evt2);
        Assert.AreEqual (evt1.pokerCard, evt2.pokerCard);
        Assert.AreEqual (evt1.cardIndex, evt2.cardIndex);
    }

    [Test]
    public void PokerData_AnteStartEventConversion_Test () {
        AnteStartGameEvent evt1 = new () {
            gameTableID = 4859205,
        };

        int expectedSize = ByteConverterUtils.SizeOf (evt1);
        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventAnteStart (evt1);
        TestPokerGameEventByteArray (evtBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_ANTE_START);

        int currentDataIndex = 0;
        PokerGameEventByteConverter.BytesToPokerGameEventAnteStart (evtBytes, ref currentDataIndex, out var evt2);
        Assert.AreEqual (currentDataIndex, expectedSize);
        TestPokerGameEventCommonData (evt1, evt2);
    }

    [Test]
    public void PokerData_AntePhaseChangeEventConversion_Test () {
        GamePhaseChangeGameEvent evt1 = new () {
            gameTableID = 4568289,
            gamePhase = PokerGamePhaseEnum.THE_RIVER,
        };

        int expectedSize = ByteConverterUtils.SizeOf (evt1);
        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventAntePhaseChange (evt1);
        TestPokerGameEventByteArray (evtBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_ANTE_PHASE_CHANGE);

        int currentDataIndex = 0;
        PokerGameEventByteConverter.BytesToPokerGameEventAntePhaseChange (evtBytes, ref currentDataIndex, out var evt2);
        Assert.AreEqual (currentDataIndex, expectedSize);
        TestPokerGameEventCommonData (evt1, evt2);
        Assert.AreEqual (evt1.gamePhase, evt2.gamePhase);
    }

    [Test]
    public void PokerData_AnteTurnChangeEventConversion_Test () {
        ChangeTurnSeatIndexGameEvent evt1 = new () {
            gameTableID = 564567,
            seatIndex = 0,
        };
        int expectedSize = ByteConverterUtils.SizeOf (evt1);
        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventAnteTurnChange (evt1);
        TestPokerGameEventByteArray (evtBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_ANTE_TURN_CHANGE);

        int currentDataIndex = 0;
        PokerGameEventByteConverter.BytesToPokerGameEventAnteTurnChange (evtBytes, ref currentDataIndex, out var evt2);
        Assert.AreEqual (currentDataIndex, expectedSize);
        TestPokerGameEventCommonData (evt1, evt2);
        Assert.AreEqual (evt1.seatIndex, evt2.seatIndex);
    }

    [Test]
    public void PokerData_AnteEndEventConversion_Test () {
        AnteEndGameEvent evt1 = new () {
            gameTableID = 4859205,
        };

        int expectedSize = ByteConverterUtils.SizeOf (evt1);
        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventAnteEnd (evt1);
        TestPokerGameEventByteArray (evtBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_ANTE_END);

        int currentDataIndex = 0;
        PokerGameEventByteConverter.BytesToPokerGameEventAnteEnd (evtBytes, ref currentDataIndex, out var evt2);
        Assert.AreEqual (currentDataIndex, expectedSize);
        TestPokerGameEventCommonData (evt1, evt2);
    }

    [Test]
    public void PokerData_TableUpdateMainPrizeEventConversion_Test () {
        UpdateMainPrizePotGameEvent evt1 = new () {
            gameTableID = 9193821,
            wagerPerPlayer = 17000,
        };

        int expectedSize = ByteConverterUtils.SizeOf (evt1);
        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventTableUpdateMainPrize (evt1);
        TestPokerGameEventByteArray (evtBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_TABLE_UPDATE_MAIN_PRIZE);

        int currentDataIndex = 0;
        PokerGameEventByteConverter.BytesToPokerGameEventTableUpdateMainPrize (evtBytes, ref currentDataIndex, out var evt2);
        Assert.AreEqual (currentDataIndex, expectedSize);
        TestPokerGameEventCommonData (evt1, evt2);
        Assert.AreEqual (evt1.wagerPerPlayer, evt2.wagerPerPlayer);
    }

    [Test]
    public void PokerData_TableCreateSidePrizeEventConversion_Test () {
        CreateSidePrizePotGameEvent evt1 = new () {
            gameTableID = 9193821,
            wagerPerPlayer = 17000,
        };

        int expectedSize = ByteConverterUtils.SizeOf (evt1);
        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventTableCreateSidePrize (evt1);
        TestPokerGameEventByteArray (evtBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_TABLE_CREATE_SIDE_PRIZE);

        int currentDataIndex = 0;
        PokerGameEventByteConverter.BytesToPokerGameEventTableCreateSidePrize (evtBytes, ref currentDataIndex, out var evt2);
        Assert.AreEqual (currentDataIndex, expectedSize);
        TestPokerGameEventCommonData (evt1, evt2);
        Assert.AreEqual (evt1.wagerPerPlayer, evt2.wagerPerPlayer);
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

        int expectedSize = ByteConverterUtils.SizeOf (evt1);
        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventAllPlayerCardsReveal (evt1);
        TestPokerGameEventByteArray (evtBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_ALL_PLAYER_CARDS_REVEAL);

        int currentDataIndex = 0;
        PokerGameEventByteConverter.BytesToPokerGameEventAllPlayerCardsReveal (evtBytes, ref currentDataIndex, out var evt2);
        Assert.AreEqual (currentDataIndex, expectedSize);
        TestPokerGameEventCommonData (evt1, evt2);
        Assert.AreEqual (evt1.revealedHands.Count, evt2.revealedHands.Count);
        foreach (var kvp in evt1.revealedHands) {
            Assert.IsTrue (evt2.revealedHands.TryGetValue (kvp.Key, out var value2));
            Assert.IsTrue (ListUtils.CheckEqualsOrder (kvp.Value, value2));
        }
    }

    [Test]
    public void PokerData_PlayerWinEventConversion_Test () {
        PlayerWinGameEvent evt1 = new () {
            gameTableID = 9193821,
            playerID = 22342,
            chipsWon = 100000,
        };

        int expectedSize = ByteConverterUtils.SizeOf (evt1);
        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventPlayerWin (evt1);
        TestPokerGameEventByteArray (evtBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_PLAYER_WIN);

        int currentDataIndex = 0;
        PokerGameEventByteConverter.BytesToPokerGameEventPlayerWin (evtBytes, ref currentDataIndex, out var evt2);
        Assert.AreEqual (currentDataIndex, expectedSize);
        TestPokerGameEventCommonData (evt1, evt2);
        Assert.AreEqual (evt1.playerID, evt2.playerID);
        Assert.AreEqual (evt1.chipsWon, evt2.chipsWon);
    }

    [Test]
    public void PokerData_PlayerBetBlindEventConversion_Test () {
        PlayerBetBlindGameEvent evt1 = new () {
            gameTableID = 917771,
            playerID = 21425,
            chipsSpent = 1000,
        };

        int expectedSize = ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_BET_BLIND;
        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventPlayerBetBlind (evt1);
        TestPokerGameEventByteArray (evtBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_PLAYER_BET_BLIND);

        int currentDataIndex = 0;
        PokerGameEventByteConverter.BytesToPokerGameEventPlayerBetBlind (evtBytes, ref currentDataIndex, out var evt2);
        Assert.AreEqual (currentDataIndex, expectedSize);
        TestPokerGameEventCommonData (evt1, evt2);
        Assert.AreEqual (evt1.playerID, evt2.playerID);
        Assert.AreEqual (evt1.chipsSpent, evt2.chipsSpent);
    }

    [Test]
    public void PokerData_PlayerBetCheckEventConversion_Test () {
        PlayerBetCheckGameEvent evt1 = new () {
            gameTableID = 389127,
            playerID = 456,
        };

        int expectedSize = ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_BET_CHECK;
        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventPlayerBetCheck (evt1);
        TestPokerGameEventByteArray (evtBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_PLAYER_BET_CHECK);

        int currentDataIndex = 0;
        PokerGameEventByteConverter.BytesToPokerGameEventPlayerBetCheck (evtBytes, ref currentDataIndex, out var evt2);
        Assert.AreEqual (currentDataIndex, expectedSize);
        TestPokerGameEventCommonData (evt1, evt2);
        Assert.AreEqual (evt1.playerID, evt2.playerID);
    }

    [Test]
    public void PokerData_PlayerBetFoldEventConversion_Test () {
        PlayerFoldGameEvent evt1 = new () {
            gameTableID = 89741,
            playerID = 3256,
        };

        int expectedSize = ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_BET_FOLD;
        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventPlayerBetFold (evt1);
        TestPokerGameEventByteArray (evtBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_PLAYER_BET_FOLD);

        int currentDataIndex = 0;
        PokerGameEventByteConverter.BytesToPokerGameEventPlayerBetFold (evtBytes, ref currentDataIndex, out var evt2);
        Assert.AreEqual (currentDataIndex, expectedSize);
        TestPokerGameEventCommonData (evt1, evt2);
        Assert.AreEqual (evt1.playerID, evt2.playerID);
    }

    [Test]
    public void PokerData_PlayerBetCallBasicEventConversion_Test () {
        PlayerBetCallGameEvent evt1 = new () {
            gameTableID = 81111,
            playerID = 88,
            chipsSpent = 1500,
            isAllIn = false,
        };

        int expectedSize = ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_BET_CALL;
        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventPlayerBetCall (evt1);
        TestPokerGameEventByteArray (evtBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_PLAYER_BET_CALL_BASIC);

        int currentDataIndex = 0;
        PokerGameEventByteConverter.BytesToPokerGameEventPlayerBetCallBasic (evtBytes, ref currentDataIndex, out var evt2);
        Assert.AreEqual (currentDataIndex, expectedSize);
        TestPokerGameEventCommonData (evt1, evt2);
        Assert.AreEqual (evt1.playerID, evt2.playerID);
        Assert.AreEqual (evt1.chipsSpent, evt2.chipsSpent);
        Assert.AreEqual (evt1.isAllIn, evt2.isAllIn);
    }

    [Test]
    public void PokerData_PlayerBetCallAllInEventConversion_Test () {
        PlayerBetCallGameEvent evt1 = new () {
            gameTableID = 11231,
            playerID = 8128,
            chipsSpent = 15000,
            isAllIn = true,
        };

        int expectedSize = ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_BET_CALL;
        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventPlayerBetCall (evt1);
        TestPokerGameEventByteArray (evtBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_PLAYER_BET_CALL_ALL_IN);

        int currentDataIndex = 0;
        PokerGameEventByteConverter.BytesToPokerGameEventPlayerBetCallAllIn (evtBytes, ref currentDataIndex, out var evt2);
        Assert.AreEqual (currentDataIndex, expectedSize);
        TestPokerGameEventCommonData (evt1, evt2);
        Assert.AreEqual (evt1.playerID, evt2.playerID);
        Assert.AreEqual (evt1.chipsSpent, evt2.chipsSpent);
        Assert.AreEqual (evt1.isAllIn, evt2.isAllIn);
    }

    [Test]
    public void PokerData_PlayerBetRaiseBasicEventConversion_Test () {
        PlayerBetRaiseGameEvent evt1 = new () {
            gameTableID = 3265,
            playerID = 7444,
            chipsSpent = 20000,
            isAllIn = false,
        };

        int expectedSize = ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_BET_RAISE;
        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventPlayerBetRaise (evt1);
        TestPokerGameEventByteArray (evtBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_PLAYER_BET_RAISE_BASIC);

        int currentDataIndex = 0;
        PokerGameEventByteConverter.BytesToPokerGameEventPlayerBetRaiseBasic (evtBytes, ref currentDataIndex, out var evt2);
        Assert.AreEqual (currentDataIndex, expectedSize);
        TestPokerGameEventCommonData (evt1, evt2);
        Assert.AreEqual (evt1.playerID, evt2.playerID);
        Assert.AreEqual (evt1.chipsSpent, evt2.chipsSpent);
        Assert.AreEqual (evt1.isAllIn, evt2.isAllIn);
    }

    [Test]
    public void PokerData_PlayerBetRaiseAllInEventConversion_Test () {
        PlayerBetRaiseGameEvent evt1 = new () {
            gameTableID = 245631,
            playerID = 45361,
            chipsSpent = 60000,
            isAllIn = true,
        };

        int expectedSize = ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_BET_RAISE;
        byte[] evtBytes = PokerGameEventByteConverter.BytesFromPokerGameEventPlayerBetRaise (evt1);
        TestPokerGameEventByteArray (evtBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_EVENT_PLAYER_BET_RAISE_ALL_IN);

        int currentDataIndex = 0;
        PokerGameEventByteConverter.BytesToPokerGameEventPlayerBetRaiseAllIn (evtBytes, ref currentDataIndex, out var evt2);
        Assert.AreEqual (currentDataIndex, expectedSize);
        TestPokerGameEventCommonData (evt1, evt2);
        Assert.AreEqual (evt1.playerID, evt2.playerID);
        Assert.AreEqual (evt1.chipsSpent, evt2.chipsSpent);
        Assert.AreEqual (evt1.isAllIn, evt2.isAllIn);
    }
}