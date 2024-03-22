using System;

namespace GameUtils.Observing {
    public abstract class Observable : IDisposable {
        public virtual void Dispose () { }
    }

    public sealed class Observable<T> : Observable where T : Observable {
        internal static Action<T> Callbacks;
    }
}
