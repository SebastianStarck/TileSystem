using System;
using UnityEngine;
namespace UnitSystem.Progress
{
    internal class ProgressBar : MonoBehaviour
    {
        private const float _actionAdvancedPosition = -.5f;

        private Transform _actionBackgroundTransform;
        private Transform _actionAdvancedTransform;
        private TextMesh _currentProgress;

        internal float ProgressRequired;

        public void Awake()
        {
            RegisterComponents();
            FaceCamera();
        }

        private void RegisterComponents()
        {
            var ownTransform = transform;

            _actionBackgroundTransform = ownTransform.GetChild(0).transform;
            _actionAdvancedTransform = ownTransform.GetChild(1).transform;
            _currentProgress = ownTransform.GetComponentInChildren<TextMesh>();
        }

        public void FaceCamera() => transform.rotation = Quaternion.LookRotation(new Vector3(-90f, Camera.main!.transform.forward.y));

        internal void Enable()
        {
            _actionBackgroundTransform.gameObject.SetActive(true);
            _actionAdvancedTransform.gameObject.SetActive(true);
            _currentProgress.gameObject.SetActive(true);
        }

        internal void Disable()
        {
            _actionBackgroundTransform.gameObject.SetActive(false);
            _actionAdvancedTransform.gameObject.SetActive(false);
            _currentProgress.gameObject.SetActive(false);
        }

        internal void SetProgress(float currentProgress)
        {
            var progressPercentage = currentProgress / ProgressRequired * 100;

            _actionAdvancedTransform.localPosition = new Vector3(_actionAdvancedPosition + progressPercentage / 200, 0f);
            _currentProgress.text = $"{(int)progressPercentage}%";
            _actionAdvancedTransform.localScale = new Vector3(progressPercentage / 100, .1f, .11f);
        }
    }
}
