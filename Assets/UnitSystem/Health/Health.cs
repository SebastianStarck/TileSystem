using System;
using UnityEngine;

namespace UnitSystem
{
    internal class Health : MonoBehaviour
    {
        private const float healthBasePosition = .5f;
        private const float healthPercentToPositionRatio = .5f;

        private Transform _currentHealthTransform;
        private Transform _missingHealthTransform;
        private TextMesh _currentHealth;

        public void Awake()
        {
            RegisterComponents();
            FaceCamera();
        }
        private void RegisterComponents()
        {
            var ownTransform = transform;

            _currentHealthTransform = ownTransform.GetChild(0).transform;
            _missingHealthTransform = ownTransform.GetChild(1).transform;
            _currentHealth = ownTransform.GetComponentInChildren<TextMesh>();
        }

        public void FaceCamera() => transform.rotation = Quaternion.LookRotation(new Vector3(-90f, Camera.main!.transform.forward.y));

        internal void SetHealthPercentage(float currentHealthPercentage)
        {
            if (currentHealthPercentage <= 0.01f)
            {
                _currentHealthTransform.gameObject.SetActive(false);
                _missingHealthTransform.gameObject.SetActive(false);
                _currentHealth.gameObject.SetActive(false);
                return;
            }

            _currentHealthTransform.gameObject.SetActive(true);
            _missingHealthTransform.gameObject.SetActive(true);
            _currentHealth.gameObject.SetActive(true);

            var missingPercentage = 100 - currentHealthPercentage;
            _currentHealth.text = $"{currentHealthPercentage}%";
            _missingHealthTransform.localPosition = new Vector3(healthBasePosition - missingPercentage / 200, 0f);
            _missingHealthTransform.localScale = new Vector3(missingPercentage / 100, .1f, .11f);
        }
        
    }
}
