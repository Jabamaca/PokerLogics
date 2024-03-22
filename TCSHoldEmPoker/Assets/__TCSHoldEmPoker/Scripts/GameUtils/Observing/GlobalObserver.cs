using System;

namespace GameUtils.Observing {
    public static class GlobalObserver {
        public static void AddObserver<T>(Action<T> callback) where T : Observable {
            Observable<T>.Callbacks += callback;
        }

        public static void RemoveObserver<T>(Action<T> callback) where T : Observable {
            Observable<T>.Callbacks -= callback;
        }

        public static void NotifyObservers<T>(T observable) where T : Observable {
            Observable<T>.Callbacks?.Invoke (observable);
        }
    }
}
