using System;
using System.Collections.Generic;
using TCSHoldEmPoker.Models.Define;

namespace TCSHoldEmPoker.Models {

    // Connectivity Delegates
    public delegate void DidPlayerJoinHandler (int gameTableID, int playerID, int buyInChips);
    public delegate void DidPlayerLeaveHandler (int gameTableID, int playerID, int redeemedChips);
    public delegate void DidPlayerGetKickedHandler (int gameTableID, int playerID, int redeemedChips);

    // Game Progression Delegates
    public delegate void DidAnteStartHandler (int gameTableID);
    public delegate void DidGamePhaseChangeHandler (int gameTableID, PokerGamePhaseEnum phase);
    public delegate void DidSetTurnSeatIndexHandler (int gameTableID, int seatIndex);
    public delegate void DidAnteEndHandler (int gameTableID);

    // Card Dealing Delegates
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
    public delegate void DidUpdateMainPrizePotHandler (int gameTableID, int wagerPerPlayer);
    public delegate void DidCreateSidePrizePotHandler (int gameTableID, int wagerPerPlayer);
    public delegate void DidRevealAllHandsHandler (int gameTableID, IReadOnlyDictionary<int, IReadOnlyList<PokerCard>> hands);
    public delegate void DidPlayerWinHandler (int gameTableID, int playerID, int chipsWon, PokerHand winningHand);

    public class GameTableModelHost : GameTableModel {

        #region Properties

        // Connectivity Delegates
        public DidPlayerJoinHandler DidPlayerJoin = delegate { };
        public DidPlayerLeaveHandler DidPlayerLeave = delegate { };
        public DidPlayerGetKickedHandler DidPlayerGetKicked = delegate { };

        // Game Progression Delegates
        public DidAnteStartHandler DidAnteStart = delegate { };
        public DidGamePhaseChangeHandler DidGamePhaseChange = delegate { };
        public DidSetTurnSeatIndexHandler DidSetTurnSeatIndex = delegate { };
        public DidAnteEndHandler DidAnteEnd = delegate { };

        // Card Dealing Delegates
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
        public DidUpdateMainPrizePotHandler DidUpdateMainPrizePot = delegate { };
        public DidCreateSidePrizePotHandler DidCreateSidePrizePot = delegate { };
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
            _mainPrizePot = null;
            _sidePrizePots.Clear ();

            _currentTurnSeatIndex = 0;
            for (int i = 0; i < TABLE_CAPACITY; i++) {
                _playerSeats[i] = new ();
            }

            _currentGamePhase = PokerGamePhaseEnum.WAITING;
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
                if (seat.IsPlaying) {
                    playerHands.Add (seat.SeatedPlayerID, seat.DealtCards);
                }
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
                    if (playerCount > 1) {
                        solePlayerID = -1;
                        return false; // Player is NOT alone.
                    }
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
            // Don't join if already joined.
            if (FindSeatWithPlayerID (playerID, out var foundSeat)) {
                return false;
            }

            foreach (var seat in _playerSeats) {
                if (seat.IsSeatEmpty) {
                    PlayerModel joinedPlayer = new (playerID, buyInChips);
                    seat.SeatPlayer (joinedPlayer);

                    DidPlayerJoin?.Invoke (_gameTableID, playerID, buyInChips);

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
                if (_currentGamePhase == PokerGamePhaseEnum.PRE_FLOP ||
                    _currentGamePhase == PokerGamePhaseEnum.THE_FLOP ||
                    _currentGamePhase == PokerGamePhaseEnum.THE_TURN ||
                    _currentGamePhase == PokerGamePhaseEnum.THE_RIVER) {
                    CheckAllWagerChecks ();
                }
            }
        }

        #endregion

        #region Game Sequence Methods

        private void TriggerGamePhase (PokerGamePhaseEnum gamePhase) {
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

        public void StartNewAnte () {
            if (_currentGamePhase != PokerGamePhaseEnum.WAITING ||
                GetSeatedPlayerCount () < 2) {
                return;
            }

            ReadyPlayersForAnte ();
            _sidePrizePots.Clear ();
            _mainPrizePot = new PrizePotModel (GetPlayerIDsWithChips ());
            _deck.Shuffle ();

            DidAnteStart?.Invoke (_gameTableID);

            // Pre-Flop Game Phase.
            TriggerGamePhase (PokerGamePhaseEnum.PRE_FLOP);

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
            RemoveAllChecks (); // Blinds don't check automatically.
        }

        private void RevealTheFlop () {
            TriggerGamePhase (PokerGamePhaseEnum.THE_FLOP);
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
            TriggerGamePhase (PokerGamePhaseEnum.THE_TURN);
            DealTheTurnCards ();

            ProceedPlayerTurn ();
        }

        private void DealTheTurnCards () {
            _deck.GetNextCard (); // Burn Card.
            DealCommunityCard (cardIndex: 3);
        }

        private void RevealTheRiver () {
            TriggerGamePhase (PokerGamePhaseEnum.THE_RIVER);
            DealTheRiverCards ();

            ProceedPlayerTurn ();
        }

        private void DealTheRiverCards () {
            _deck.GetNextCard (); // Burn Card.
            DealCommunityCard (cardIndex: 4);
        }

        private void HandShowdown () {
            TriggerGamePhase (PokerGamePhaseEnum.SHOWDOWN);

            RevealAllHands ();
            DealMissingCommunityCards ();

            CalculateWinners ();

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

        private void CalculateWinners () {
            // List all best hands for players.
            Dictionary<int, PokerHand> playerHands = new ();
            foreach (var seat in _playerSeats) {
                if (seat.IsPlaying) {
                    PokerHand playerHand = PokerHandFactory.GetHighestPokerHandWithCardSets (_communityCards, seat.DealtCards);
                    playerHands.Add (seat.SeatedPlayerID, playerHand);
                }
            }

            TriggerGamePhase (PokerGamePhaseEnum.WINNING);

            // Get winner of Side Prize Pots first.
            foreach (var sidePrizePot in _sidePrizePots) {
                CalculatePotWinners (sidePrizePot, playerHands);
            }
            // Get winner of Main Prize Pot, then.
            CalculatePotWinners (_mainPrizePot, playerHands);
        }

        private void CalculatePotWinners (PrizePotModel prizePot, IReadOnlyDictionary<int, PokerHand> playerHands) {
            PokerHand winningHand = null;
            List<int> winnerPlayerIDs = new ();
            foreach (var playerID in prizePot.qualifiedPlayerIDs) {
                PokerHand currentPlayerHand = playerHands[playerID];

                int handComp = currentPlayerHand.CompareTo (winningHand);
                if (winnerPlayerIDs.Count <= 0 || handComp > 0) { // No winner yet, or found better hand.

                    winningHand = currentPlayerHand;
                    winnerPlayerIDs.Clear ();
                    winnerPlayerIDs.Add (playerID);

                } else if (handComp == 0) { // Found equally winning hand.
                    winnerPlayerIDs.Add (playerID);
                }
            }

            int distributedPrize = prizePot.prizeAmount / winnerPlayerIDs.Count;
            foreach (var playerID in winnerPlayerIDs) {
                PlayerIDWin (playerID, chipsWon: distributedPrize, winningHand);
            }
        }

        private void WinByDefault (int playerID) {
            GatherWagersToPot ();

            TriggerGamePhase (PokerGamePhaseEnum.WINNING);

            PlayerIDWin (playerID, _mainPrizePot.prizeAmount, BlankHandMaker.BlankHand);
            foreach (var sidePrizePot in _sidePrizePots) {
                PlayerIDWin (playerID, sidePrizePot.prizeAmount, BlankHandMaker.BlankHand);
            }

            EndOfAnte ();
        }

        private void PlayerIDWin (int playerID, int chipsWon, PokerHand winningHand) {
            if (FindSeatWithPlayerID (playerID, out var seat)) {
                seat.GiveChips (chipsWon);
                DidPlayerWin?.Invoke (_gameTableID, seat.SeatedPlayerID, chipsWon, winningHand);
            }
        }

        private void EndOfAnte () {
            RemoveCommunityCards ();
            _sidePrizePots.Clear ();
            _mainPrizePot = null;
            DidAnteEnd?.Invoke (_gameTableID);

            KickPlayersWithNoChips ();
            TriggerGamePhase (PokerGamePhaseEnum.WAITING);
        }

        private void KickPlayersWithNoChips () {
            foreach (var seat in _playerSeats) {
                if (!seat.IsSeatEmpty && seat.SeatedPlayerChips <= 0) {
                    int kickedPlayerID = seat.SeatedPlayerID;
                    int remainingChips = seat.SeatedPlayerChips;
                    seat.UnseatPlayer ();
                    DidPlayerGetKicked (_gameTableID, kickedPlayerID, remainingChips);
                }
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
                case PokerGamePhaseEnum.PRE_FLOP:
                    RevealTheFlop ();
                    break;
                case PokerGamePhaseEnum.THE_FLOP:
                    RevealTheTurn ();
                    break;
                case PokerGamePhaseEnum.THE_TURN:
                    RevealTheRiver ();
                    break;
                case PokerGamePhaseEnum.THE_RIVER:
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
                if (seat.CurrentWager >= _currentTableStake || seat.SeatedPlayerChips == 0) {
                    seat.DoCheck ();
                    DidPlayerBetCheck?.Invoke (_gameTableID, seat.SeatedPlayerID);

                } else if (_currentTableStake >= seat.SeatedPlayerChips + seat.CurrentWager) {
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
                _mainPrizePot.DisqualifyPlayerIDFromPot (seat.SeatedPlayerID);
                foreach (var sidePrizePot in _sidePrizePots) {
                    sidePrizePot.DisqualifyPlayerIDFromPot (seat.SeatedPlayerID);
                }

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

                if (newStake >= seat.SeatedPlayerChips + seat.CurrentWager) {
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
            if (GetSortedAllInSeats (out var allInSeats)) {
                foreach (var allInSeat in allInSeats) {
                    if (allInSeat.CurrentWager <= 0) {
                        _mainPrizePot.DisqualifyPlayerIDFromPot (allInSeat.SeatedPlayerID);
                        continue;
                    }
                    int allInWager = allInSeat.CurrentWager;
                    foreach (int playerID in _mainPrizePot.qualifiedPlayerIDs) {
                        if (FindSeatWithPlayerID (playerID, out var seat)) {
                            _mainPrizePot.AddPrizeToPot (seat.CollectWageredChips (allInWager));
                        }
                    }
                    var sidePrizePot = _mainPrizePot;
                    _sidePrizePots.Add (sidePrizePot);
                    DidCreateSidePrizePot (_gameTableID, wagerPerPlayer: allInWager);
                    // NEW Main Prize Pot
                    _mainPrizePot = new PrizePotModel (_mainPrizePot.qualifiedPlayerIDs);
                    _mainPrizePot.DisqualifyPlayerIDFromPot (allInSeat.SeatedPlayerID);
                }
            }

            // Gather ramaining wagers to Main Prize Pot.
            int wagerPerPlayer = 0;
            foreach (int playerID in _mainPrizePot.qualifiedPlayerIDs) {
                if (FindSeatWithPlayerID (playerID, out var seat)) {
                    int playerWager = seat.CollectWageredChips ();
                    if (wagerPerPlayer == 0)
                        wagerPerPlayer = playerWager; // Save
                    _mainPrizePot.AddPrizeToPot (playerWager);
                }
            }
            DidUpdateMainPrizePot (_gameTableID, wagerPerPlayer);

            _currentTableStake = 0;
        }

        #endregion

        #endregion

    }
}