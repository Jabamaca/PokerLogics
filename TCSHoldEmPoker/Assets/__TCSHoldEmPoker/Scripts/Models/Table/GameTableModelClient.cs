using System.Collections.Generic;
using TCSHoldEmPoker.Data;
using TCSHoldEmPoker.Models.Define;

namespace TCSHoldEmPoker.Models {

    public class GameTableModelClient : GameTableModel {

        #region Properties

        private readonly int _clientPlayerID = -1;

        #endregion

        #region Constructors

        public GameTableModelClient (int gameTableID, int clientPlayerID, TableStateData tsd) {
            _gameTableID = gameTableID;

            _clientPlayerID = clientPlayerID;

            _minimumWager = tsd.minimumWager;
            _currentTableStake = tsd.currentTableStake;
            _cashPot = tsd.cashPot;

            _currentTurnSeatIndex = tsd.currentTurnPlayerIndex;
            int seatIndex = 0;
            foreach (var seatData in tsd.seatStateDataOrder) {
                _playerSeats[seatIndex] = new TableSeatModel (seatData);
                seatIndex++;
                if (seatIndex >= TABLE_CAPACITY)
                    break;
            }

            _currentGamePhase = tsd.currentGamePhase;
            int cardIndex = 0;
            foreach (var card in tsd.communityCardsOrder) {
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

            // Pre-Flop Game Phase.
            _currentGamePhase = PokerGamePhase.PRE_FLOP;
        }

        private void ReadyPlayersForAnte () {
            for (int i = 0; i < TABLE_CAPACITY; i++) {
                if (_playerSeats[i] == null)
                    continue;

                _playerSeats[i].SurrenderCards ();
                _playerSeats[i].SetReadyForAnte ();
            }
        }

        private void RemoveCommunityCards () {
            for (int i = 0; i < COMMUNITY_CARD_COUNT; i++) {
                _communityCards[i] = PokerCard.BLANK;
            }
        }

        public void SetGamePhase (PokerGamePhase gamePhase) {
            _currentGamePhase = gamePhase;
        }

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

        #endregion

    }

}