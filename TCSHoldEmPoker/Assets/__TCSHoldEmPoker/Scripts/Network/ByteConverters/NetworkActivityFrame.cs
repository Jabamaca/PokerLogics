using System.Collections.Generic;
using TCSHoldEmPoker.Network.GameEvents;
using TCSHoldEmPoker.Network.GameInputs;

namespace TCSHoldEmPoker.Network.Data {
    public class NetworkActivityFrame {

        private readonly List<PokerGameEvent> _gameEventOrderList = new ();
        public IReadOnlyList<PokerGameEvent> GameEventOrderList => _gameEventOrderList;

        private readonly List<PokerGameInput> _gameInputOrderList = new ();
        public IReadOnlyList<PokerGameInput> GameInputOrderList => _gameInputOrderList;

        public void AddGameEvent (PokerGameEvent evt) {
            if (evt == null)
                return;

            _gameEventOrderList.Add (evt);
        }

        public void AddGameInput (PokerGameInput input) {
            if (input == null)
                return;

            _gameInputOrderList.Add (input);
        }

    }
}