using System;

namespace Generic
{
    public struct Timer
    {
        private readonly Action _onAttackReady;
        private readonly float _attackDelay;

        public float CurrentProgress
        {
            get;
            private set;
        }

        public Timer(float attackDelay, Action onAttackReady)
        {
            _attackDelay = attackDelay;
            _onAttackReady = onAttackReady;
            CurrentProgress = 0f;
        }

        public float Progress(float progress)
        {
            CurrentProgress += progress;

            if (CurrentProgress > _attackDelay)
            {
                _onAttackReady();
                Restart();
            }

            return CurrentProgress;
        }

        private void Restart() => CurrentProgress -= _attackDelay;
    }
}
