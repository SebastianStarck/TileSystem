using UnityEngine;
using UnityEngine.EventSystems;

namespace UISystem
{
    public interface IUIEventReceiver {
        public void OnUIEvent(UIEventType ev);
    }
}