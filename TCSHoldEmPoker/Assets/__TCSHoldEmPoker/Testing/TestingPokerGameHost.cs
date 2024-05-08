using UnityEngine;
using System.Collections.Generic;
using TCSHoldEmPoker.Models;
using TCSHoldEmPoker.Models.Define;

public class TestingPokerGameHost {

    #region Properties

    private readonly GameTableModelHost _tableHost;
    public GameTableModelHost TableHost => _tableHost;

    public int CurrentTurnID => _tableHost.GetTurningPlayerID ();

    #endregion

    #region Constructor

    public TestingPokerGameHost (int minWager) {
        _tableHost = new (minWager);
        AddTableDelegates ();
    }

    #endregion

    #region Internal Methods

    ~TestingPokerGameHost () {
        RemoveTableDelegates ();
    }

    #endregion

    #region Methods

    private void AddTableDelegates () {
        _tableHost.DidPlayerJoin += DidPlayerJoin;
        _tableHost.DidPlayerLeave += DidPlayerLeave;
        _tableHost.DidPlayerGetKicked += DidPlayerGetKicked;

        _tableHost.DidAnteStart += DidAnteStart;
        _tableHost.DidGamePhaseChange += DidGamePhaseChange;
        _tableHost.DidSetTurnSeatIndex += DidSetTurnSeatIndex;
        _tableHost.DidAnteEnd += DidAnteEnd;

        _tableHost.DidDealCardsToPlayers += DidDealCardsToPlayers;
        _tableHost.DidDealCommunityCard += DidDealCommunityCard;

        _tableHost.DidPlayerBetBlind += DidPlayerBetBlind;
        _tableHost.DidPlayerBetCheck += DidPlayerBetCheck;
        _tableHost.DidPlayerBetCall += DidPlayerBetCall;
        _tableHost.DidPlayerBetCallAllIn += DidPlayerBetCallAllIn;
        _tableHost.DidPlayerBetRaise += DidPlayerBetRaise;
        _tableHost.DidPlayerBetRaiseAllIn += DidPlayerBetRaiseAllIn;
        _tableHost.DidPlayerFold += DidPlayerFold;

        _tableHost.DidUpdateMainPrizePot += DidUpdateMainPrizePot;
        _tableHost.DidCreateSidePrizePot += DidCreateSidePrizePot;
        _tableHost.DidRevealAllHands += DidRevealAllHands;
        _tableHost.DidPlayerWin += DidPlayerWin;
    }

    private void RemoveTableDelegates () {
        _tableHost.DidPlayerJoin -= DidPlayerJoin;
        _tableHost.DidPlayerLeave -= DidPlayerLeave;
        _tableHost.DidPlayerGetKicked -= DidPlayerGetKicked;

        _tableHost.DidAnteStart -= DidAnteStart;
        _tableHost.DidGamePhaseChange -= DidGamePhaseChange;
        _tableHost.DidSetTurnSeatIndex -= DidSetTurnSeatIndex;
        _tableHost.DidAnteEnd -= DidAnteEnd;

        _tableHost.DidDealCardsToPlayers -= DidDealCardsToPlayers;
        _tableHost.DidDealCommunityCard -= DidDealCommunityCard;

        _tableHost.DidPlayerBetBlind -= DidPlayerBetBlind;
        _tableHost.DidPlayerBetCheck -= DidPlayerBetCheck;
        _tableHost.DidPlayerBetCall -= DidPlayerBetCall;
        _tableHost.DidPlayerBetCallAllIn -= DidPlayerBetCallAllIn;
        _tableHost.DidPlayerBetRaise -= DidPlayerBetRaise;
        _tableHost.DidPlayerBetRaiseAllIn -= DidPlayerBetRaiseAllIn;
        _tableHost.DidPlayerFold -= DidPlayerFold;

        _tableHost.DidUpdateMainPrizePot -= DidUpdateMainPrizePot;
        _tableHost.DidCreateSidePrizePot -= DidCreateSidePrizePot;
        _tableHost.DidRevealAllHands -= DidRevealAllHands;
        _tableHost.DidPlayerWin -= DidPlayerWin;
    }

    #endregion

    #region Delegate Methods

    #region Player Connectivity Delegates

    private void DidPlayerJoin (int gameTableID, int playerID, int buyInChips) {
        Debug.Log (gameTableID + " PLAYER_JOIN PlayerID:" + playerID + " Chips:" + buyInChips);
    }

    private void DidPlayerLeave (int gameTableID, int playerID, int redeemedChips) {
        Debug.Log (gameTableID + " PLAYER_LEAVE PlayerID:" + playerID + " Chips:" + redeemedChips);
    }

    private void DidPlayerGetKicked (int gameTableID, int playerID, int redeemedChips) {
        Debug.Log (gameTableID + " PLAYER_KICKED PlayerID:" + playerID + " Chips:" + redeemedChips);
    }

    #endregion

    #region Game Progression Delegates

    private void DidAnteStart (int gameTableID) {
        Debug.Log (gameTableID + " ANTE_START");
    }

    private void DidGamePhaseChange (int gameTableID, PokerGamePhaseEnum phase) {
        Debug.Log (gameTableID + " PHASE_CHANGE" + " Phase:" + TestUtils.PhaseString (phase));
    }

    private void DidSetTurnSeatIndex (int gameTableID, int seatIndex) {
        Debug.Log (gameTableID + " TURN_CHANGE SeatIndex:" + seatIndex + " PlayerID:" + _tableHost.GetPlayerIDAtIndex (seatIndex));
    }

    private void DidAnteEnd (int gameTableID) {
        Debug.Log (gameTableID + " ANTE_END");
    }

    #endregion

    #region Card Distribution Delegates

    private void DidDealCardsToPlayers (int gameTableID, IReadOnlyDictionary<int, IReadOnlyList<PokerCard>> hands) {
        foreach (var playerHandPair in hands) {
            int playerID = playerHandPair.Key;
            var cards = playerHandPair.Value;

            string debugLog = gameTableID + " PLAYER_CARD_DEAL PlayerID:" + playerID + " Cards: ";
            foreach (var card in cards) {
                debugLog += "[" + card + "]";
            }
            Debug.Log (debugLog);
        }
    }

    private void DidDealCommunityCard (int gameTableID, PokerCard card, int cardIndex) {
        string debugLog = gameTableID + " COMMUNITY_REVEAL Index:" + cardIndex + " Card:" + card + " CC: ";
        foreach (var commCard in _tableHost.CommunityCards) {
            debugLog += "[" + commCard + "]";
        }
        Debug.Log (debugLog);
    }

    #endregion

    #region Player Action Delegates

    private void DidPlayerBetBlind (int gameTableID, int playerID, int chipsSpent) {
        if (_tableHost.TryGetPlayerChipsInHand (playerID, out int netChips)) {
            Debug.Log (gameTableID + " PLAYER_BET_BLIND PlayerID:" + playerID + " Spent Chips:" + chipsSpent + " Net Chips:" + netChips);
        }
    }

    private void DidPlayerBetCheck (int gameTableID, int playerID) {
        if (_tableHost.TryGetPlayerChipsInHand (playerID, out int netChips)) {
            Debug.Log (gameTableID + " PLAYER_BET_CHECK PlayerID:" + playerID + " Net Chips:" + netChips);
        }
    }

    private void DidPlayerBetCall (int gameTableID, int playerID, int chipsSpent) {
        if (_tableHost.TryGetPlayerChipsInHand (playerID, out int netChips)) {
            Debug.Log (gameTableID + " PLAYER_BET_CALL PlayerID:" + playerID + " Spent Chips:" + chipsSpent + " Net Chips:" + netChips);
        }
    }

    private void DidPlayerBetCallAllIn (int gameTableID, int playerID, int chipsSpent) {
        if (_tableHost.TryGetPlayerChipsInHand (playerID, out int netChips)) {
            Debug.Log (gameTableID + " PLAYER_BET_CALL PlayerID:" + playerID + " Spent Chips:" + chipsSpent + " Net Chips:" + netChips + " ALL IN!!!");
        }
    }

    private void DidPlayerBetRaise (int gameTableID, int playerID, int chipsSpent) {
        if (_tableHost.TryGetPlayerChipsInHand (playerID, out int netChips)) {
            Debug.Log (gameTableID + " PLAYER_BET_RAISE PlayerID:" + playerID + " Spent Chips:" + chipsSpent + " Net Chips:" + netChips);
        }
    }

    private void DidPlayerBetRaiseAllIn (int gameTableID, int playerID, int chipsSpent) {
        if (_tableHost.TryGetPlayerChipsInHand (playerID, out int netChips)) {
            Debug.Log (gameTableID + " PLAYER_BET_RAISE PlayerID:" + playerID + " Spent Chips:" + chipsSpent + " Net Chips:" + netChips + " ALL IN!!!");
        }
    }

    private void DidPlayerFold (int gameTableID, int playerID) {
        if (_tableHost.TryGetPlayerChipsInHand (playerID, out int netChips)) {
            Debug.Log (gameTableID + " PLAYER_FOLDED PlayerID:" + playerID + " Net Chips:" + netChips);
        }
    }

    #endregion

    #region Win Condition Delegates

    private void DidUpdateMainPrizePot (int gameTableID, int wagerPerPlayer) {
        Debug.Log (gameTableID + " GATHER_POT Update Main Pot: WAGER PER PLAYER = " + wagerPerPlayer);
    }

    private void DidCreateSidePrizePot (int gameTableID, int wagerPerPlayer) {
        Debug.Log (gameTableID + " GATHER_POT Create Side Pot: WAGER PER PLAYER = " + wagerPerPlayer);
    }

    private void DidRevealAllHands (int gameTableID, IReadOnlyDictionary<int, IReadOnlyList<PokerCard>> hands) {
        foreach (var playerHandPair in hands) {
            int playerID = playerHandPair.Key;
            var cards = playerHandPair.Value;

            string debugLog = gameTableID + " PLAYER_CARD_REVEAL PlayerID:" + playerID + " Cards: ";
            foreach (var card in cards) {
                debugLog += "[" + card + "]";
            }
            Debug.Log (debugLog);
        }
    }

    private void DidPlayerWin (int gameTableID, int playerID, int chipsWon, PokerHand winningHand) {
        if (_tableHost.TryGetPlayerChipsInHand (playerID, out int netChips)) {
            Debug.Log (gameTableID + " WINNER!!! WINNER!!! PlayerID:" + playerID + " Chips Won:" + chipsWon + " Net Chips:" + netChips + " Winning Hand: " + winningHand);
        }
    }

    #endregion

    #endregion

}