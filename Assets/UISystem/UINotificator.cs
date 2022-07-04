using Generic;
using UnityEngine;

namespace UISystem
{
    /// <summary>
    /// Mediator class between UIHandler ~ MonoUIEventable
    /// </summary>
    public class UINotificator : MonoBehaviour
    {
        private UIHandler _uiHandler;
        /// <summary>
        /// Subscribed 
        /// </summary>
        private MonoUIEventable[] _eventables;

        public void Awake()
        {
            _uiHandler = FindObjectOfType<UIHandler>();
            RegisterComponents();
        }


        private void RegisterComponents()
        {
            _eventables = FindObjectsOfType<MonoUIEventable>();

            foreach (var component in _eventables) component.UIEvent += OnReceivedEvent;
        }

        public void Notify(UIEventType eventType) => _eventables.Each(eventable => eventable.OnUIEvent(eventType));
        private void OnReceivedEvent(UIEventType uiEvent) => _uiHandler.TriggerEvent(uiEvent);
    }
}
