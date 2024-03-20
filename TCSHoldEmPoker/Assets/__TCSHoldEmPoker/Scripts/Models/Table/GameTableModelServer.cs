using System.Collections.Generic;
using TCSHoldEmPoker.Models.Define;

namespace TCSHoldEmPoker.Models {
    public class GameTableModelServer : GameTableModel {

        public delegate void DidGamePhaseChangeHandler (int gameTableID, PokerGamePhase phase);
        public delegate void DidSetTurnSeatIndexHandler (int gameTableID, int seatIndex);
        public delegate void DidRevealAllHandsHandler (int gameTableID, IReadOnlyDictionary<int, IReadOnlyList<PokerCard>> hands);
        public delegate void DidDealCommunityCardHandler (int gameTableID, PokerCard card, int cardIndex);
        public delegate void DidDealCardsToPlayersHandler (int gameTableID, IReadOnlyDictionary<int, IReadOnlyList<PokerCard>> hands);

        #region Properties

        public DidGamePhaseChangeHandler DidGamePhaseChange = delegate { };
        public DidSetTurnSeatIndexHandler DidSetTurnSeatIndex = delegate { };
        public DidRevealAllHandsHandler DidRevealAllHands = delegate { };
        public DidDealCommunityCardHandler DidDealCommunityCard = delegate { };
        public DidDealCardsToPlayersHandler DidDealCardsToPlayers = delegate { };

        // Wagering Properties
        private int SmallBlindWager => MinimumWager / 2;

        // Turning Properties
        private int _nextDealerIndex = 0; // Determines Small Blind for next Ante.
        private int _currentDealerIndex = 0; // Determines Small Blind for current Ante.

        private readonly CardDeck _deck = new ();

        #endregion

        #region Constructor

        public GameTableModelServer (int minWager) {
            _gameTableID = GenerateGameTableID ();

            _minimumWager = minWager;
            _currentTableStake = 0;
            _cashPot = 0;

            _currentTurnSeatIndex = 0;
            for (int i = 0; i < TABLE_CAPACITY; i++) {
                _playerSeats[i] = new ();
            }

            _currentGamePhase = PokerGamePhase.WAITING;
            for (int i = 0; i < COMMUNITY_CARD_COUNT; i++) {
                _communityCards[i] = PokerCard.BLANK;
            }
        }

        #endregion

        #region Methods

        private static int GenerateGameTableID () {
            // TODO: How to generate Table ID???
            return 123456;
        }

        #region Player Query Methods

        private IReadOnlyDictionary<int, IReadOnlyList<PokerCard>> GetAllPlayerHands () {
            Dictionary<int, IReadOnlyList<PokerCard>> playerHands = new ();
            foreach (var seat in _playerSeats) {
                playerHands.Add (seat.SeatedPlayerID, seat.DealtCards);
            }

            return playerHands;
        }

        private bool CheckSeatIndexPlayStatus (int index) {
            if (index < 0 || index >= TABLE_CAPACITY)
                return false;

            return _playerSeats[index].IsPlaying;
        }

        private bool CheckOneLastPlayerLeft (out int solePlayerID) {
            int playerCount = 0;
            solePlayerID = -1;
            foreach (var seat in _playerSeats) {
                if (seat.IsPlaying) {
                    solePlayerID = seat.SeatedPlayerID;
                    playerCount++;
                    if (playerCount > 1)
                        solePlayerID = -1;
                        return false; // Player is NOT alone.
                }
            }

            return playerCount == 1;
        }

        private int GetSeatedPlayerCount () {
            int count = 0;

            foreach (var seat in _playerSeats) {
                if (!seat.IsSeatEmpty)
                    count++;
            }

            return count;
        }

        #endregion

        #region Player Connectivity Methods

        public bool TryPlayerIDJoin (int playerID, int buyInChips) {
            foreach (var seat in _playerSeats) {
                if (seat.IsSeatEmpty && seat.CurrentWager <= 0) {
                    PlayerModel joinedPlayer = new (playerID, buyInChips);
                    seat.SeatPlayer (joinedPlayer);

                    if (_currentGamePhase == PokerGamePhase.WAITING &&
                        GetSeatedPlayerCount () >= 2) {
                        StartNewAnte ();
                    }

                    return true;
                }
            }

            return false;
        }

        public void PlayerIDLeave (int playerID, out int redeemChips) {
            redeemChips = 0;

            if (FindSeatWithPlayerID (playerID, out var seat)) {
                redeemChips = seat.SeatedPlayerChips;
                seat.UnseatPlayer ();
                seat.SurrenderCards ();

                // Check remaining players in middle of game.
                if (_currentGamePhase == PokerGamePhase.PRE_FLOP ||
                    _currentGamePhase == PokerGamePhase.THE_FLOP ||
                    _currentGamePhase == PokerGamePhase.THE_TURN ||
                    _currentGamePhase == PokerGamePhase.THE_RIVER) {
                    CheckAllWagerChecks ();
                }
            }
        }

        #endregion

        #region Game Sequence Methods

        private void TriggerGamePhase (PokerGamePhase gamePhase) {
            _currentGamePhase = gamePhase;
            DidGamePhaseChange?.Invoke (GameTableID, _currentGamePhase);
        }

        private void DealCommunityCard (int cardIndex) {
            PokerCard dealtCard = _deck.GetNextCard ();
            _communityCards[cardIndex] = dealtCard;
            DidDealCommunityCard?.Invoke (_gameTableID, dealtCard, cardIndex);
        }

        private void ProceedPlayerTurn () {
            GoToNextPlayingPlayer ();
            DidSetTurnSeatIndex?.Invoke (_gameTableID, _currentTurnSeatIndex);
        }

        private void StartNewAnte () {
            ReadyPlayersForAnte ();

            _cashPot = 0;
            _deck.Shuffle ();

            // Pre-Flop Game Phase.
            TriggerGamePhase (PokerGamePhase.PRE_FLOP);

            DealCardsToPlayers ();
            DetermineBlinds ();

            // 1st Player Turn prompting.
            ProceedPlayerTurn ();
        }

        private void GoToNextPlayingPlayer () {
            for (int i = 0; i < TABLE_CAPACITY; i++) {
                _currentTurnSeatIndex = (_currentTurnSeatIndex + 1) % TABLE_CAPACITY;
                if (CheckSeatIndexPlayStatus(_currentTurnSeatIndex))
                    break; // Found playing player. Stop enum.
            }
        }

        private void ReadyPlayersForAnte () {
            for (int i = 0; i < TABLE_CAPACITY; i++) {
                if (_playerSeats[i] == null)
                    continue;

                _playerSeats[i].SurrenderCards ();
                _playerSeats[i].SetReadyForAnte ();
            }
        }

        private void DealCardsToPlayers () {
            // Deal the 2 cards to each player.
            for (int i = 0; i < CARD_DEAL_COUNT; i++) {
                for (int j = 0; j < TABLE_CAPACITY; j++) {
                    if (!CheckSeatIndexPlayStatus (j))
                        continue; // Skip player seat.

                    _playerSeats[j].ReceiveCard (_deck.GetNextCard ());
                }
            }

            DidDealCardsToPlayers?.Invoke (_gameTableID, GetAllPlayerHands ());
        }

        private void DetermineBlinds () {
            // Small Blind wagering.
            _currentTurnSeatIndex = _nextDealerIndex;
            _currentDealerIndex = _nextDealerIndex; // For after the Flop.
            GoToNextPlayingPlayer (); // Next player after the Dealer is the Small Blind.
            CurrentTurningSeat.RaiseWagerTo (SmallBlindWager);
            _nextDealerIndex = _currentTurnSeatIndex; // Current Small Blind is next Dealer.

            // Big Blind wagering.
            GoToNextPlayingPlayer (); // Next player after the Small Blind is the Big Blind.
            CurrentTurningSeat.RaiseWagerTo (_minimumWager);

            _currentTableStake = _minimumWager;
        }

        private void RevealTheFlop () {
            TriggerGamePhase (PokerGamePhase.THE_FLOP);
            DealTheFlopCards ();

            // Next turning player is the player immediately after the Dealer.
            _currentTurnSeatIndex = _currentDealerIndex;
            ProceedPlayerTurn ();
        }

        private void DealTheFlopCards () {
            _deck.GetNextCard (); // Burn Card.
            DealCommunityCard (cardIndex: 0);
            DealCommunityCard (cardIndex: 1);
            DealCommunityCard (cardIndex: 2);
        }

        private void RevealTheTurn () {
            TriggerGamePhase (PokerGamePhase.THE_TURN);
            DealTheTurnCards ();

            ProceedPlayerTurn ();
        }

        private void DealTheTurnCards () {
            _deck.GetNextCard (); // Burn Card.
            DealCommunityCard (cardIndex: 3);
        }

        private void RevealTheRiver () {
            TriggerGamePhase (PokerGamePhase.THE_RIVER);
            DealTheRiverCards ();

            ProceedPlayerTurn ();
        }

        private void DealTheRiverCards () {
            _deck.GetNextCard (); // Burn Card.
            DealCommunityCard (cardIndex: 4);
        }

        private void HandShowdown () {
            TriggerGamePhase (PokerGamePhase.SHOWDOWN);

            RevealAllHands ();
            DealMissingCommunityCards ();

            if (CalculateWinners (out var winnerSeats, out var winningHand)) {
                TriggerGamePhase (PokerGamePhase.WINNING);

                // Calculate player/s prize.
                int chipPrize = _cashPot / winnerSeats.Count;
                foreach (var seat in winnerSeats) {
                    seat.GiveChips (chipPrize);
                }
            }

            EndOfAnte ();
        }

        private void RevealAllHands () {
            DidRevealAllHands?.Invoke (_gameTableID, GetAllPlayerHands ());
        }

        private void DealMissingCommunityCards () {
            if (_communityCards[0].Equals (PokerCard.BLANK)) { // If NO Flop yet.
                DealTheFlopCards ();
            }
            if (_communityCards[3].Equals (PokerCard.BLANK)) { // If NO Turn yet.
                DealTheTurnCards ();
            }
            if (_communityCards[4].Equals (PokerCard.BLANK)) { // If NO River yet.
                DealTheRiverCards ();
            }
        }

        private bool CalculateWinners (out List<TableSeatModel> winnerSeats, out PokerHand winningHand) {
            winnerSeats = new ();
            winningHand = BlankPokerHand.BlankHand;
            foreach (var seat in _playerSeats) {
                if (seat.IsPlaying) {
                    PokerHand seatHand = PokerHandFactory.GetHighestPokerHandWithCardSets (_communityCards, seat.DealtCards);
                    int handComp = seatHand.CompareTo (winningHand);

                    if (handComp > 0) { // Winning hand overtake.
                        winningHand = seatHand;
                        winnerSeats.Clear ();
                        winnerSeats.Add (seat);

                    } else if (handComp == 0) { // Current winning hand is tied with.
                        winnerSeats.Add (seat);
                    }
                }
            }

            return winningHand.HandRank != PokerHandRankEnum.NULL;
        }

        private void WinByDefault (int playerID) {
            GatherWagersToPot ();

            TriggerGamePhase (PokerGamePhase.WINNING);

            if (FindSeatWithPlayerID (playerID, out var seat)) {
                seat.GiveChips (_cashPot);
            }

            EndOfAnte ();
        }

        private void EndOfAnte () {
            _cashPot = 0;
            // Remove community cards.
            for (int i = 0; i < COMMUNITY_CARD_COUNT; i++) {
                _communityCards[i] = PokerCard.BLANK;
            }

            if (GetSeatedPlayerCount () < 2) {
                TriggerGamePhase (PokerGamePhase.WAITING);
            } else {
                StartNewAnte ();
            }
        }

        private void CheckAllWagerChecks () {
            if (CheckOneLastPlayerLeft (out int soloPlayer)) {
                WinByDefault (soloPlayer);
                return;
            }
            
            if (CheckIfAllBetted (out bool onMaxBet)) {
                GatherWagersToPot ();

                if (onMaxBet) { 
                    // Max betting reached. Proceed to Showdown.
                    HandShowdown ();
                } else {
                    ProceedToNextPhase ();
                }
            } else {
                ProceedPlayerTurn ();
            }
        }

        private void ProceedToNextPhase () {
            switch (_currentGamePhase) {
                case PokerGamePhase.PRE_FLOP:
                    RevealTheFlop ();
                    break;
                case PokerGamePhase.THE_FLOP:
                    RevealTheTurn ();
                    break;
                case PokerGamePhase.THE_TURN:
                    RevealTheRiver ();
                    break;
                case PokerGamePhase.THE_RIVER:
                    HandShowdown ();
                    break;
            }
        }

        private bool CheckIfAllBetted (out bool onMaxBet) {
            int playersWithChips = 0;
            foreach (var seat in _playerSeats) {
                if (seat.IsPlaying) {
                    if (!seat.DidCheck) {
                        onMaxBet = false;
                        return false;
                    }
                    if (seat.SeatedPlayerChips > 0) {
                        playersWithChips++;
                    }
                }
            }

            onMaxBet = playersWithChips <= 1;
            return true;
        }

        #endregion

        #region Player Input Methods

        public void PlayerCheckOrCall (int playerID) {
            if (CurrentTurningSeat.SeatedPlayerID != playerID)
                return; // Not playerID's turn.

            if (FindSeatWithPlayerID (playerID, out var turningSeat)) {

                if (_currentTableStake == 0 || turningSeat.SeatedPlayerChips == 0) {
                    turningSeat.DoCheck ();
                } else if (_currentTableStake > turningSeat.SeatedPlayerChips) {
                    turningSeat.WagerAllIn ();
                } else {
                    turningSeat.RaiseWagerTo (_currentTableStake);
                }

                CheckAllWagerChecks ();
            }
        }

        public void PlayerFold (int playerID) {
            if (CurrentTurningSeat.SeatedPlayerID != playerID)
                return; // Not playerID's turn.

            if (FindSeatWithPlayerID (playerID, out var turningSeat)) {
                turningSeat.FoldHand ();

                CheckAllWagerChecks ();
            }
        }

        public void PlayerRaise (int playerID, int newStake) {
            if (CurrentTurningSeat.SeatedPlayerID != playerID)
                return; // Not playerID's turn.

            if (newStake <= _currentTableStake)
                return; // Invalid Raise.

            if (FindSeatWithPlayerID (playerID, out var turningSeat)) {
                RemoveAllChecks ();

                turningSeat.RaiseWagerTo (newStake);
                _currentTableStake = newStake;

                CheckAllWagerChecks ();
            }
        }

        #endregion

        #region Wagering Methods

        private void GatherWagersToPot () {
            for (int i = 0; i < TABLE_CAPACITY; i++) {
                _cashPot += _playerSeats[i].CollectWageredChips ();
            }

            _currentTableStake = 0;
        }

        private void RemoveAllChecks () {
            for (int i = 0; i < TABLE_CAPACITY; i++) {
                _playerSeats[i].Uncheck ();
            }
        }

        #endregion

        #endregion

    }
}