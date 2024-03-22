using System.Collections.Generic;
using TCSHoldEmPoker.Models;
using TCSHoldEmPoker.Models.Define;

namespace TCSHoldEmPoker.Network {
    public class PokerGameCoordinator {

        #region Properties

        // Connectivity Delegates
        public DidPlayerJoinHandler DidPlayerJoin = delegate { };
        public DidPlayerLeaveHandler DidPlayerLeave = delegate { };

        // Game Progression Delegates
        public DidAnteStartHandler DidAnteStart = delegate { };
        public DidGamePhaseChangeHandler DidGamePhaseChange = delegate { };
        public DidSetTurnSeatIndexHandler DidSetTurnSeatIndex = delegate { };
        public DidAnteEndHandler DidAnteEnd = delegate { };

        // Card Distribution Delegates
        public DidDealCardsToPlayersHandler DidDealCardsToPlayers = delegate { };
        public DidDealCommunityCardHandler DidDealCommunityCard = delegate { };

        // Player Action Delegates
        public DidPlayerBetBlindHandler DidPlayerBetBlind = delegate { };
        public DidPlayerBetCheckHandler DidPlayerBetCheck = delegate { };
        public DidPlayerBetCallHandler DidPlayerBetCall = delegate { };
        public DidPlayerBetCallAllInHandler DidPlayerBetCallAllIn = delegate { };
        public DidPlayerBetRaiseHandler DidPlayerBetRaise = delegate { };
        public DidPlayerBetRaiseAllInHandler DidPlayerBetRaiseAllIn = delegate { };
        public DidPlayerFoldHandler DidPlayerFold = delegate { };

        // Win Condition Delegates
        public DidGatherWagersToPotHandler DidGatherWagersToPot = delegate { };
        public DidRevealAllHandsHandler DidRevealAllHands = delegate { };
        public DidPlayerWinHandler DidPlayerWin = delegate { };

        private readonly Dictionary<int, GameTableModelHost> _gameTables = new ();

        #endregion

        #region Internal Methods

        ~PokerGameCoordinator () {
            foreach (var tableKVP in _gameTables) {
                var gameTable = tableKVP.Value;
                RemoveDelegationFromGameTable (gameTable);
            }
        }

        #endregion

        #region Methods

        public bool GetGameTableHostWithID (int gameTableID, out GameTableModelHost gameTableHost) {
            return _gameTables.TryGetValue (gameTableID, out gameTableHost);
        }

        public GameTableModelHost GenerateNewGameTable (int minWager) {
            GameTableModelHost newTableHost = new (minWager);

            _gameTables.Add (newTableHost.GameTableID, newTableHost);
            AddDelegationToGameTable (newTableHost);

            return newTableHost;
        }

        private void AddDelegationToGameTable (GameTableModelHost gameTable) {
            gameTable.DidPlayerJoin += DidPlayerJoin;
            gameTable.DidPlayerLeave += DidPlayerLeave;

            gameTable.DidAnteStart += DidAnteStart;
            gameTable.DidGamePhaseChange += DidGamePhaseChange;
            gameTable.DidSetTurnSeatIndex += DidSetTurnSeatIndex;
            gameTable.DidAnteEnd += DidAnteEnd;

            gameTable.DidDealCardsToPlayers += DidDealCardsToPlayers;
            gameTable.DidDealCommunityCard += DidDealCommunityCard;

            gameTable.DidPlayerBetBlind += DidPlayerBetBlind;
            gameTable.DidPlayerBetCheck += DidPlayerBetCheck;
            gameTable.DidPlayerBetCall += DidPlayerBetCall;
            gameTable.DidPlayerBetCallAllIn += DidPlayerBetCallAllIn;
            gameTable.DidPlayerBetRaise += DidPlayerBetRaise;
            gameTable.DidPlayerBetRaiseAllIn += DidPlayerBetRaiseAllIn;
            gameTable.DidPlayerFold += DidPlayerFold;

            gameTable.DidGatherWagersToPot += DidGatherWagersToPot;
            gameTable.DidRevealAllHands += DidRevealAllHands;
            gameTable.DidPlayerWin += DidPlayerWin;
        }

        private void RemoveDelegationFromGameTable (GameTableModelHost gameTable) {
            gameTable.DidPlayerJoin -= DidPlayerJoin;
            gameTable.DidPlayerLeave -= DidPlayerLeave;

            gameTable.DidAnteStart -= DidAnteStart;
            gameTable.DidGamePhaseChange -= DidGamePhaseChange;
            gameTable.DidSetTurnSeatIndex -= DidSetTurnSeatIndex;
            gameTable.DidAnteEnd -= DidAnteEnd;

            gameTable.DidDealCardsToPlayers -= DidDealCardsToPlayers;
            gameTable.DidDealCommunityCard -= DidDealCommunityCard;

            gameTable.DidPlayerBetBlind -= DidPlayerBetBlind;
            gameTable.DidPlayerBetCheck -= DidPlayerBetCheck;
            gameTable.DidPlayerBetCall -= DidPlayerBetCall;
            gameTable.DidPlayerBetCallAllIn -= DidPlayerBetCallAllIn;
            gameTable.DidPlayerBetRaise -= DidPlayerBetRaise;
            gameTable.DidPlayerBetRaiseAllIn -= DidPlayerBetRaiseAllIn;
            gameTable.DidPlayerFold -= DidPlayerFold;

            gameTable.DidGatherWagersToPot -= DidGatherWagersToPot;
            gameTable.DidRevealAllHands -= DidRevealAllHands;
            gameTable.DidPlayerWin -= DidPlayerWin;
        }

        #endregion

    }
}