using Generic;
using UnityEngine;

namespace UISystem
{
    public class UINotificator : MonoBehaviour
    {
        private UIHandler _uiHandler;
        private MonoUIEventable[] _eventables;

        public void Awake()
        {
            _uiHandler = FindObjectOfType<UIHandler>();
            RegisterComponents();
        }

        public void Notify(UIEventType eventType)
        {
            _eventables.Each(eventable => eventable.OnUIEvent(eventType));
        }

        private void RegisterComponents()
        {
            _eventables = FindObjectsOfType<MonoUIEventable>();


            foreach (var component in _eventables) component.UIEvent += OnReceivedEvent;
        }

        private void OnReceivedEvent(UIEventType uiEvent)
        {
            _uiHandler.TriggerEvent(uiEvent);
        }
    }
}