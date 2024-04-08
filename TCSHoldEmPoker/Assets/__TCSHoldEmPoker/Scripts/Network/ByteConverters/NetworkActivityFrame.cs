using System.Collections.Generic;
using TCSHoldEmPoker.Network.Activity;
using TCSHoldEmPoker.Network.Activity.PokerGameEvents;
using TCSHoldEmPoker.Network.Activity.PokerGameInputs;

namespace TCSHoldEmPoker.Network.Data {
    public class NetworkActivityFrame {

        private readonly List<PokerGameEvent> _pokerGameEventOrderList = new ();
        public IReadOnlyList<PokerGameEvent> PokerGameEventOrderList => _pokerGameEventOrderList;

        private readonly List<PokerGameInput> _pokerGameInputOrderList = new ();
        public IReadOnlyList<PokerGameInput> PokerGameInputOrderList => _pokerGameInputOrderList;

        public PokerGameStateUpdate pokerGameStateUpdate;

        public void AddGameEvent (PokerGameEvent evt) {
            if (evt == null)
                return;

            _pokerGameEventOrderList.Add (evt);
        }

        public void AddGameInput (PokerGameInput input) {
            if (input == null)
                return;

            _pokerGameInputOrderList.Add (input);
        }

    }
}