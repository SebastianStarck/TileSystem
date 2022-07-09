using System;
using BattleSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem
{
    public class UIHandler : MonoBehaviour
    {
        [SerializeField] private bool isAddingUnit;

        private UINotificator _notificator;
        private Button _addUnitButton;
        private AudioSource _audioSource;

        private AudioClip _mouseClick;

        private bool _canDrag = true;
        public bool CanDrag => _canDrag;

        public void Awake()
        {
            _addUnitButton = transform.GetComponentInChildren<Button>();
            _notificator = GetComponent<UINotificator>();
            _audioSource = GetComponent<AudioSource>();
            _mouseClick = AssetLoader.LoadAsset<AudioClip>("click.wav", "SFX");
        }

        public void Update()
        {
            if (Input.GetMouseButtonDown(1)) OnCancelUnitClick();
        }

        // TODO: This could handle a "back" logic
        public void OnCancelUnitClick()
        {
            if (!isAddingUnit) return;

            isAddingUnit = false;
            _addUnitButton.interactable = true;
            // TODO: Make this trigger a new state for BattleManager
            _notificator.Notify(UIEventType.CancelAddUnit);
            _canDrag = true;
        }

        public void OnResolveClick()
        {
            PlayClickEffect();
            _notificator.Notify(UIEventType.Resolve);
            _canDrag = false;
        }

        public void OnClearClick()
        {
            PlayClickEffect();
            _notificator.Notify(UIEventType.Clear);
            _canDrag = true;
        }

        public void OnRestoreClick()
        {
            PlayClickEffect();
            _notificator.Notify(UIEventType.Restore);
        }

        public void OnAddUnitClick()
        {
            PlayClickEffect();
            isAddingUnit = true;
            _addUnitButton.interactable = false;
            _notificator.Notify(UIEventType.AddUnit);
            _canDrag = false;
        }

        public void TriggerEvent(UIEventType uiEvent)
        {
            switch (uiEvent)
            {
                case UIEventType.Click:
                    PlayClickEffect();
                    break;

                case UIEventType.Clear:
                case UIEventType.CancelAddUnit:
                    OnCancelUnitClick();
                    break;
            }
        }

        private void PlayClickEffect() => _audioSource.PlayOneShot(_mouseClick);
    }
}
