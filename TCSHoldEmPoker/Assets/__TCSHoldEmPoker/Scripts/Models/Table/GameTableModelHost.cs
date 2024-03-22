using System;
using System.Collections.Generic;
using TCSHoldEmPoker.Models.Define;

namespace TCSHoldEmPoker.Models {
    public class GameTableModelHost : GameTableModel {

        // Connectivity Delegates
        public delegate void DidPlayerJoinHandler (int gameTableID, int playerID, int buyInChips);
        public delegate void DidPlayerLeaveHandler (int gameTableID, int playerID, int redeemedChips);

        // Game Progression Delegates
        public delegate void DidAnteStartHandler (int gameTableID);
        public delegate void DidGamePhaseChangeHandler (int gameTableID, PokerGamePhase phase);
        public delegate void DidSetTurnSeatIndexHandler (int gameTableID, int seatIndex);
        public delegate void DidAnteEndHandler (int gameTableID);

        // Card Distribution Delegates
        public delegate void DidDealCardsToPlayersHandler (int gameTableID, IReadOnlyDictionary<int, IReadOnlyList<PokerCard>> hands);
        public delegate void DidDealCommunityCardHandler (int gameTableID, PokerCard card, int cardIndex);

        // Player Action Delegates
        public delegate void DidPlayerBetBlindHandler (int gameTableID, int playerID, int chipsSpent);
        public delegate void DidPlayerBetCheckHandler (int gameTableID, int playerID);
        public delegate void DidPlayerBetCallHandler (int gameTableID, int playerID, int chipsSpent);
        public delegate void DidPlayerBetCallAllInHandler (int gameTableID, int playerID, int chipsSpent);
        public delegate void DidPlayerBetRaiseHandler (int gameTableID, int playerID, int chipsSpent);
        public delegate void DidPlayerBetRaiseAllInHandler (int gameTableID, int playerID, int chipsSpent);
        public delegate void DidPlayerFoldHandler (int gameTableID, int playerID);

        // Win Condition Delegates
        public delegate void DidGatherWagersToPotHandler (int gameTableID, int newCashPot);
        public delegate void DidRevealAllHandsHandler (int gameTableID, IReadOnlyDictionary<int, IReadOnlyList<PokerCard>> hands);
        public delegate void DidPlayerWinHandler (int gameTableID, int playerID, int chipsWon);

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

        // Wagering Properties
        private int SmallBlindWager => MinimumWager / 2;

        // Turning Properties
        private int _nextDealerIndex = 0; // Determines Small Blind for next Ante.
        private int _currentDealerIndex = 0; // Determines Small Blind for current Ante.

        private readonly CardDeck _deck = new ();

        #endregion

        #region Constructor

        public GameTableModelHost (int minWager) {
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

                    DidPlayerJoin?.Invoke (_gameTableID, playerID, buyInChips);

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

                DidPlayerLeave?.Invoke (_gameTableID, playerID, redeemChips);

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

            DidAnteStart?.Invoke (_gameTableID);

            // Pre-Flop Game Phase.
            TriggerGamePhase (PokerGamePhase.PRE_FLOP);

            DealCardsToPlayers ();
            CollectBlindWagers ();

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

        private void CollectBlindWagers () {
            // Small Blind wagering.
            _currentTurnSeatIndex = _nextDealerIndex;
            _currentDealerIndex = _nextDealerIndex; // For after the Flop.
            GoToNextPlayingPlayer (); // Next player after the Dealer is the Small Blind.
            CurrentTurningSeat.RaiseWagerTo (SmallBlindWager, out int smallBlindSpent);
            DidPlayerBetBlind?.Invoke (_gameTableID, CurrentTurningSeat.SeatedPlayerID, smallBlindSpent);
            _nextDealerIndex = _currentTurnSeatIndex; // Current Small Blind is next Dealer.

            // Big Blind wagering.
            GoToNextPlayingPlayer (); // Next player after the Small Blind is the Big Blind.
            CurrentTurningSeat.RaiseWagerTo (_minimumWager, out int bigBlindSpent);
            DidPlayerBetBlind?.Invoke (_gameTableID, CurrentTurningSeat.SeatedPlayerID, bigBlindSpent);

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
                    PlayerSeatWin (seat, chipPrize);
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
                PlayerSeatWin (seat, _cashPot);
            }

            EndOfAnte ();
        }

        private void PlayerSeatWin (TableSeatModel seat, int chipsWon) {
            seat.GiveChips (chipsWon);
            DidPlayerWin?.Invoke (_gameTableID, seat.SeatedPlayerID, chipsWon);
        }

        private void EndOfAnte () {
            _cashPot = 0;
            RemoveCommunityCards ();
            DidAnteEnd?.Invoke (_gameTableID);

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
                    RemoveAllChecks ();
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

            TryPlayerAction (playerID, (seat) => {
                if (_currentTableStake == 0 || seat.SeatedPlayerChips == 0) {
                    seat.DoCheck ();
                    DidPlayerBetCheck?.Invoke (_gameTableID, seat.SeatedPlayerID);

                } else if (_currentTableStake >= seat.SeatedPlayerChips) {
                    seat.WagerAllIn (out int chipsSpent);
                    DidPlayerBetCallAllIn?.Invoke (_gameTableID, seat.SeatedPlayerID, chipsSpent);

                } else {
                    seat.RaiseWagerTo (_currentTableStake, out int chipsSpent);
                    DidPlayerBetCall?.Invoke (_gameTableID, seat.SeatedPlayerID, chipsSpent);
                }
            });
        }

        public void PlayerFold (int playerID) {
            if (CurrentTurningSeat.SeatedPlayerID != playerID)
                return; // Not playerID's turn.

            TryPlayerAction (playerID, (seat) => {
                seat.FoldHand ();
                DidPlayerFold.Invoke (_gameTableID, seat.SeatedPlayerID);
            });
        }

        public void PlayerRaise (int playerID, int newStake) {
            if (CurrentTurningSeat.SeatedPlayerID != playerID)
                return; // Not playerID's turn.

            if (newStake <= _currentTableStake)
                return; // Invalid Raise.

            TryPlayerAction (playerID, (seat) => {
                RemoveAllChecks ();

                if (newStake >= seat.SeatedPlayerChips) {
                    seat.WagerAllIn (out int chipsSpent);
                    DidPlayerBetRaiseAllIn?.Invoke (_gameTableID, seat.SeatedPlayerID, chipsSpent);
                } else {
                    seat.RaiseWagerTo (newStake, out int chipsSpent);
                    DidPlayerBetRaise?.Invoke (_gameTableID, seat.SeatedPlayerID, chipsSpent);
                }
                _currentTableStake = newStake;
            });
        }

        private void TryPlayerAction (int playerID, Action<TableSeatModel> action) {
            if (FindSeatWithPlayerID (playerID, out var seat)) {
                if (!seat.IsPlaying)
                    return; // Folded or waiting player can't interact.

                action (seat);

                CheckAllWagerChecks ();
            }
        }

        #endregion

        #region Wagering Methods

        private void GatherWagersToPot () {
            int gatheredWagers = 0;
            for (int i = 0; i < TABLE_CAPACITY; i++) {
                gatheredWagers += _playerSeats[i].CollectWageredChips ();
            }

            if (gatheredWagers > 0) {
                _cashPot += gatheredWagers;
                DidGatherWagersToPot?.Invoke (_gameTableID, _cashPot);
            }

            _currentTableStake = 0;
        }

        #endregion

        #endregion

    }
}