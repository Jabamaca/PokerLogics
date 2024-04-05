using TCSHoldEmPoker.Models.Define;
using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.GameEvents {
    public sealed class GamePhaseChangeGameEvent : PokerGameEvent {

        public override PokerGameEventType GameEventType => PokerGameEventType.ANTE_PHASE_CHANGE;

        public PokerGamePhaseEnum gamePhase;

    }
}