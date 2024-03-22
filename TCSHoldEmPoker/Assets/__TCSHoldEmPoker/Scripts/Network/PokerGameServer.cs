using System.Collections.Generic;
using TCSHoldEmPoker.Models;
using TCSHoldEmPoker.Models.Define;

namespace TCSHoldEmPoker.Network {
    public class PokerGameServer {

        #region Properties

        private readonly Dictionary<int, GameTableModelHost> _gameTables = new ();

        #endregion

        #region Internal Mehtods

        ~PokerGameServer () {
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

        #region Delegate Methods

        #region Player Connectivity Delegates

        private void DidPlayerJoin (int gameTableID, int playerID, int buyInChips) {

        }

        private void DidPlayerLeave (int gameTableID, int playerID, int redeemedChips) {
        
        }

        #endregion

        #region Game Progression Delegates

        private void DidAnteStart (int gameTableID) {
        
        }

        private void DidGamePhaseChange (int gameTableID, PokerGamePhase phase) {
        
        }

        private void DidSetTurnSeatIndex (int gameTableID, int seatIndex) {
        
        }

        private void DidAnteEnd (int gameTableID) {
        
        }

        #endregion

        #region Card Distribution Delegates

        private void DidDealCardsToPlayers (int gameTableID, IReadOnlyDictionary<int, IReadOnlyList<PokerCard>> hands) {
        
        }

        private void DidDealCommunityCard (int gameTableID, PokerCard card, int cardIndex) {
        
        }

        #endregion

        #region Player Action Delegates

        private void DidPlayerBetBlind (int gameTableID, int playerID, int chipsSpent) {
        
        }

        private void DidPlayerBetCheck (int gameTableID, int playerID) {
        
        }

        private void DidPlayerBetCall (int gameTableID, int playerID, int chipsSpent) {
        
        }

        private void DidPlayerBetCallAllIn (int gameTableID, int playerID, int chipsSpent) {
        
        }

        private void DidPlayerBetRaise (int gameTableID, int playerID, int chipsSpent) {
        
        }

        private void DidPlayerBetRaiseAllIn (int gameTableID, int playerID, int chipsSpent) {

        }

        private void DidPlayerFold (int gameTableID, int playerID) {
        
        }

        #endregion

        #region Win Condition Delegates

        private void DidGatherWagersToPot (int gameTableID, int newCashPot) {
        
        }

        private void DidRevealAllHands (int gameTableID, IReadOnlyDictionary<int, IReadOnlyList<PokerCard>> hands) {
        
        }

        private void DidPlayerWin (int gameTableID, int playerID, int chipsWon) {
        
        }

        #endregion

        #endregion

    }
}