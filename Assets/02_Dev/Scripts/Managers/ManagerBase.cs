using UnityEngine;
using Longhorn.Core;

namespace Managers {
    
    public abstract class ManagerBase<T, TStateType> : Singleton<T> where T : Component {
        protected TStateType State { get; set; }

        public static event OnStateChangeHandler OnStateChange;

        public delegate void OnStateChangeHandler();

        // Sets state to a new value and invokes OnStateChange delegate
        public virtual void SetState(TStateType newState) {
            State = newState;
            OnStateChange?.Invoke();
        }
        
        public virtual TStateType GetState()
        {
            return State;
        }
    }
}