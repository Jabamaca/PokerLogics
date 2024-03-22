using UnityEngine;
using System.Collections.Generic;
using GameUtils.Observing;

using TCSHoldEmPoker.Network;
using TCSHoldEmPoker.Models.Define;
using TCSHoldEmPoker.Network.Events;

public class FakeServer : MonoBehaviour {

    #region Properties

    private readonly PokerGameCoordinator _gameCoordinator = new ();

    #endregion

    #region Unity Internal Methods

    private void OnEnable () {
        AddCoordinatorDelegates ();
    }

    private void OnDisable () {
        RemoveCoordinatorDelegates ();
    }

    #endregion

    #region Methods

    private void AddCoordinatorDelegates () {
        _gameCoordinator.DidPlayerJoin += DidPlayerJoin;
        _gameCoordinator.DidPlayerLeave += DidPlayerLeave;

        _gameCoordinator.DidAnteStart += DidAnteStart;
        _gameCoordinator.DidGamePhaseChange += DidGamePhaseChange;
        _gameCoordinator.DidSetTurnSeatIndex += DidSetTurnSeatIndex;
        _gameCoordinator.DidAnteEnd += DidAnteEnd;

        _gameCoordinator.DidDealCardsToPlayers += DidDealCardsToPlayers;
        _gameCoordinator.DidDealCommunityCard += DidDealCommunityCard;

        _gameCoordinator.DidPlayerBetBlind += DidPlayerBetBlind;
        _gameCoordinator.DidPlayerBetCheck += DidPlayerBetCheck;
        _gameCoordinator.DidPlayerBetCall += DidPlayerBetCall;
        _gameCoordinator.DidPlayerBetCallAllIn += DidPlayerBetCallAllIn;
        _gameCoordinator.DidPlayerBetRaise += DidPlayerBetRaise;
        _gameCoordinator.DidPlayerBetRaiseAllIn += DidPlayerBetRaiseAllIn;
        _gameCoordinator.DidPlayerFold += DidPlayerFold;

        _gameCoordinator.DidGatherWagersToPot += DidGatherWagersToPot;
        _gameCoordinator.DidRevealAllHands += DidRevealAllHands;
        _gameCoordinator.DidPlayerWin += DidPlayerWin;
    }

    private void RemoveCoordinatorDelegates () {
        _gameCoordinator.DidPlayerJoin -= DidPlayerJoin;
        _gameCoordinator.DidPlayerLeave -= DidPlayerLeave;

        _gameCoordinator.DidAnteStart -= DidAnteStart;
        _gameCoordinator.DidGamePhaseChange -= DidGamePhaseChange;
        _gameCoordinator.DidSetTurnSeatIndex -= DidSetTurnSeatIndex;
        _gameCoordinator.DidAnteEnd -= DidAnteEnd;

        _gameCoordinator.DidDealCardsToPlayers -= DidDealCardsToPlayers;
        _gameCoordinator.DidDealCommunityCard -= DidDealCommunityCard;

        _gameCoordinator.DidPlayerBetBlind -= DidPlayerBetBlind;
        _gameCoordinator.DidPlayerBetCheck -= DidPlayerBetCheck;
        _gameCoordinator.DidPlayerBetCall -= DidPlayerBetCall;
        _gameCoordinator.DidPlayerBetCallAllIn -= DidPlayerBetCallAllIn;
        _gameCoordinator.DidPlayerBetRaise -= DidPlayerBetRaise;
        _gameCoordinator.DidPlayerBetRaiseAllIn -= DidPlayerBetRaiseAllIn;
        _gameCoordinator.DidPlayerFold -= DidPlayerFold;

        _gameCoordinator.DidGatherWagersToPot -= DidGatherWagersToPot;
        _gameCoordinator.DidRevealAllHands -= DidRevealAllHands;
        _gameCoordinator.DidPlayerWin -= DidPlayerWin;
    }

    #endregion

    #region Delegate Methods

    #region Player Connectivity Delegates

    private void DidPlayerJoin (int gameTableID, int playerID, int buyInChips) {
        GlobalObserver.NotifyObservers (new PlayerJoinGameEvent () {
            gameTableID = gameTableID,
            playerID = playerID,
            buyInChips = buyInChips,
        });
    }

    private void DidPlayerLeave (int gameTableID, int playerID, int redeemedChips) {
        GlobalObserver.NotifyObservers (new PlayerLeaveGameEvent () {
            gameTableID = gameTableID,
            playerID = playerID,
        });
    }

    #endregion

    #region Game Progression Delegates

    private void DidAnteStart (int gameTableID) {
        GlobalObserver.NotifyObservers (new AnteStartGameEvent () {
            gameTableID = gameTableID,
        });
    }

    private void DidGamePhaseChange (int gameTableID, PokerGamePhase phase) {
        GlobalObserver.NotifyObservers (new GamePhaseChangeGameEvent () {
            gameTableID = gameTableID,
            gamePhase = phase,
        });
    }

    private void DidSetTurnSeatIndex (int gameTableID, int seatIndex) {
        GlobalObserver.NotifyObservers (new ChangeTurnSeatIndexGameEvent () {
            gameTableID = gameTableID,
            seatIndex = seatIndex,
        });
    }

    private void DidAnteEnd (int gameTableID) {
        GlobalObserver.NotifyObservers (new AnteEndGameEvent () {
            gameTableID = gameTableID,
        });
    }

    #endregion

    #region Card Distribution Delegates

    private void DidDealCardsToPlayers (int gameTableID, IReadOnlyDictionary<int, IReadOnlyList<PokerCard>> hands) {
        foreach (var playerHandPair in hands) {
            int playerID = playerHandPair.Key;
            var cards = playerHandPair.Value;

            GlobalObserver.NotifyObservers (new PlayerCardsDealGameEvent () {
                gameTableID = gameTableID,
                playerID = playerID,
                cards = cards,
            });
        }
    }

    private void DidDealCommunityCard (int gameTableID, PokerCard card, int cardIndex) {
        GlobalObserver.NotifyObservers (new CommunityCardDealGameEvent () {
            gameTableID = gameTableID,
            cardIndex = cardIndex,
            pokerCard = card,
        });
    }

    #endregion

    #region Player Action Delegates

    private void DidPlayerBetBlind (int gameTableID, int playerID, int chipsSpent) {
        GlobalObserver.NotifyObservers (new PlayerBetBlindGameEvent () {
            gameTableID = gameTableID,
            playerID = playerID,
            chipsSpent = chipsSpent,
        });
    }

    private void DidPlayerBetCheck (int gameTableID, int playerID) {
        GlobalObserver.NotifyObservers (new PlayerBetCheckGameEvent () {
            gameTableID = gameTableID,
            playerID = playerID,
        });
    }

    private void DidPlayerBetCall (int gameTableID, int playerID, int chipsSpent) {
        GlobalObserver.NotifyObservers (new PlayerBetCallGameEvent () {
            gameTableID = gameTableID,
            playerID = playerID,
            chipsSpent = chipsSpent,
            isAllIn = false,
        });
    }

    private void DidPlayerBetCallAllIn (int gameTableID, int playerID, int chipsSpent) {
        GlobalObserver.NotifyObservers (new PlayerBetCallGameEvent () {
            gameTableID = gameTableID,
            playerID = playerID,
            chipsSpent = chipsSpent,
            isAllIn = true,
        });
    }

    private void DidPlayerBetRaise (int gameTableID, int playerID, int chipsSpent) {
        GlobalObserver.NotifyObservers (new PlayerBetRaiseGameEvent () {
            gameTableID = gameTableID,
            playerID = playerID,
            chipsSpent = chipsSpent,
            isAllIn = false,
        });
    }

    private void DidPlayerBetRaiseAllIn (int gameTableID, int playerID, int chipsSpent) {
        GlobalObserver.NotifyObservers (new PlayerBetRaiseGameEvent () {
            gameTableID = gameTableID,
            playerID = playerID,
            chipsSpent = chipsSpent,
            isAllIn = true,
        });
    }

    private void DidPlayerFold (int gameTableID, int playerID) {
        GlobalObserver.NotifyObservers (new PlayerFoldGameEvent () {
            gameTableID = gameTableID,
            playerID = playerID,
        });
    }

    #endregion

    #region Win Condition Delegates

    private void DidGatherWagersToPot (int gameTableID, int newCashPot) {
        GlobalObserver.NotifyObservers (new TableGatherWagersGameEvent () {
            gameTableID = gameTableID,
            newCashPot = newCashPot,
        });
    }

    private void DidRevealAllHands (int gameTableID, IReadOnlyDictionary<int, IReadOnlyList<PokerCard>> hands) {
        GlobalObserver.NotifyObservers (new AllPlayerCardsRevealGameEvent () {
            gameTableID = gameTableID,
            revealedHands = hands,
        });
    }

    private void DidPlayerWin (int gameTableID, int playerID, int chipsWon) {
        GlobalObserver.NotifyObservers (new PlayerWinGameEvent () {
            gameTableID = gameTableID,
            playerID = playerID,
            chipsWon = chipsWon,
        });
    }

    #endregion

    #endregion

}