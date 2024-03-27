using System;
using System.Collections.Generic;
using TCSHoldEmPoker.Data;
using TCSHoldEmPoker.Models.Define;

namespace TCSHoldEmPoker.Models {

    public class GameTableModelClient : GameTableModel {

        #region Properties

        private readonly int _clientPlayerID = -1;

        #endregion

        #region Constructors

        public GameTableModelClient (int gameTableID, int clientPlayerID, TableStateData tableStateData) {
            _gameTableID = gameTableID;

            _clientPlayerID = clientPlayerID;

            _minimumWager = tableStateData.minimumWager;
            _currentTableStake = tableStateData.currentTableStake;
            _cashPot = tableStateData.cashPot;

            _currentTurnSeatIndex = tableStateData.currentTurnPlayerIndex;
            int seatIndex = 0;
            foreach (var seatData in tableStateData.seatStateDataOrder) {
                _playerSeats[seatIndex] = new TableSeatModel (seatData);
                seatIndex++;
                if (seatIndex >= TABLE_CAPACITY)
                    break;
            }

            _currentGamePhase = tableStateData.currentGamePhase;
            int cardIndex = 0;
            foreach (var card in tableStateData.communityCardsOrder) {
                _communityCards[cardIndex] = card;
                cardIndex++;
                if (cardIndex >= COMMUNITY_CARD_COUNT)
                    break;
            }
            while (cardIndex < COMMUNITY_CARD_COUNT) {
                _communityCards[cardIndex] = PokerCard.BLANK;
            }
        }

        #endregion

        #region Methods

        #region Player Connectivity Methods

        public void PlayerIDJoin (int playerID, int seatIndex, int buyInChips) {
            if (seatIndex < 0 || seatIndex >= TABLE_CAPACITY ||
                playerID == _clientPlayerID ||
                buyInChips <= 0) {
                return;
            }

            _playerSeats[seatIndex].SeatPlayer (new PlayerModel (playerID, buyInChips));
        }

        public void PlayerIDLeave (int playerID) {
            if (FindSeatWithPlayerID (playerID, out var seat)) {
                seat.UnseatPlayer ();
                seat.SurrenderCards ();
            }
        }

        #endregion

        #region Game Phase Methods

        public void StartNewAnte () {
            RemoveCommunityCards ();
            ReadyPlayersForAnte ();
            _cashPot = 0;
        }

        public void SetGamePhase (PokerGamePhaseEnum gamePhase) {
            RemoveAllChecks ();
            _currentGamePhase = gamePhase;
        }

        public void SetTurnSeatIndex (int seatIndex) {
            _currentTurnSeatIndex = seatIndex;
        }

        public void EndAnte () {
            RemoveCommunityCards ();
            _cashPot = 0;
        }

        #endregion

        #region Card Related Methods

        public void DealCommunityCard (PokerCard card, int orderIndex) {
            if (orderIndex < 0 || orderIndex >= COMMUNITY_CARD_COUNT)
                return;

            _communityCards[orderIndex] = card;
        }

        public void RevealCardsOfPlayerID (int playerID, IEnumerable<PokerCard> cards) {
            if (FindSeatWithPlayerID (playerID, out var seat)) {
                seat.ReceiveCard (cards);
            }
        }

        #endregion

        #region Chips Related Methods

        public void GatherWagersToPot (int newCashPot) {
            for (int i = 0; i < TABLE_CAPACITY; i++) {
                _playerSeats[i].CollectWageredChips (); // Remove seat wagers.
            }

            _currentTableStake = 0;
            _cashPot = newCashPot;
        }

        public void PlayerCheck (int playerID) {
            if (FindSeatWithPlayerID (playerID, out var seat)) {
                seat.DoCheck ();
            }
        }

        public void PlayerBetSpend (int playerID, int spentChips) {
            if (FindSeatWithPlayerID (playerID, out var seat)) {
                seat.SpendToWager (spentChips);
            }
        }

        public void PlayerBetRaiseSpend (int playerID, int spentChips) {
            if (FindSeatWithPlayerID (playerID, out var seat)) {
                RemoveAllChecks ();
                seat.SpendToWager (spentChips);
            }
        }

        public void PlayerFold (int playerID) {
            if (FindSeatWithPlayerID (playerID, out var seat)) {
                seat.FoldHand ();
            }
        }

        public void PlayerWinChips (int playerID, int chipsWon) {
            if (FindSeatWithPlayerID (playerID, out var seat)) {
                seat.GiveChips (chipsWon);
            }

        }

        #endregion

        #endregion

    }

}