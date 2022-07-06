using UnityEngine;

namespace UISystem
{
    /// <summary>
    /// Abstract class to be extended instead of MonoBehaviour
    /// Allows emission and reception of UI events 
    /// </summary>
    public abstract class MonoUIEventable : MonoBehaviour, IUIEventReceiver, IUIEventEmitter
    {
        /// <summary>
        /// Inherited exposed event for gameObjects to emit events
        /// @See UINotificator.cs
        /// TODO: Add out event enum
        /// </summary>
        public event UIEvent UIEvent;

        internal void EmitUIEvent(UIEventType eventType) => UIEvent?.Invoke(eventType);

        /// <summary>
        /// Callback method to be called on UI message dispatch
        /// </summary>
        /// <param name="ev">Received event from UI</param>
        /// TODO: Add in event enum
        public virtual void OnUIEvent(UIEventType ev)
        {
            Debug.Log($"Omitted UIEvent message to: {gameObject}");
        }

    }
}
