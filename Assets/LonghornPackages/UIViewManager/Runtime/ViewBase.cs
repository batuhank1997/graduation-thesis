using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Longhorn.UIViewManager {
    public abstract class ViewBase: MonoBehaviour {
        
        public virtual void OnHidden() {}
        
        public virtual void Hide()
        {
            if (TryGetComponent(out UIObjectTransition uiObjectTransition))
            {
                uiObjectTransition.TurnOffTransition();
                return;
            }
            
            gameObject.SetActive(false);
        }

        public virtual void Show()
        {
            if (TryGetComponent(out UIObjectTransition uiObjectTransition))
            {
                uiObjectTransition.TurnOnTransition();
                return;
            }
            
            gameObject.SetActive(true);
        }
    }
}