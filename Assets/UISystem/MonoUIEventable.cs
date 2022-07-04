using UnityEngine;

namespace UISystem
{
    public abstract class MonoUIEventable : MonoBehaviour, IUIEventReceiver
    {
        public event UIEvent UIEvent;

        public virtual void OnUIEvent(UIEventType ev)
        {
            Debug.Log($"Omitted UIEvent message to: {gameObject}");
        }

        internal void EmitUIEvent(UIEventType eventType) => UIEvent.Invoke(eventType);
    }
}