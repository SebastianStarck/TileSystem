using System;
using System.Collections.Generic;
using System.Linq;
using Generic;
using UnityEngine;

namespace Animation
{
    public class AnimationClips<TClipType> where TClipType : struct, Enum
    {
        private readonly RuntimeAnimatorController _runtimeAnimController;

        private Dictionary<TClipType, AnimationClip> _animationClips;

        public AnimationClips(RuntimeAnimatorController runtimeAnimController)
        {
            _runtimeAnimController = runtimeAnimController;

            InitializeAnimationClips();
        }

        public AnimationClip GetAnimationClip(TClipType clipType)
        {
            _animationClips.Values.ToArray().Each(entry => Debug.Log(entry));
            var hasClip = _animationClips.TryGetValue(clipType, out var clip);

            return hasClip ? clip : default;
        }

        private void InitializeAnimationClips()
        {
            _animationClips = new Dictionary<TClipType, AnimationClip>();

            if (_runtimeAnimController is AnimatorOverrideController) InitializeFromOverridenController();
            else InitializeFromBaseController();
        }

        private void InitializeFromBaseController()
        {
            AnimationClip[] clips = _runtimeAnimController.animationClips;

            foreach (AnimationClip clip in clips)
            {
                var didParse = Enum.TryParse(clip.name, out TClipType type);
                if (!didParse || _animationClips.ContainsKey(type)) continue;

                _animationClips.Add(type, clip);
            }
        }

        private void InitializeFromOverridenController()
        {
            Debug.Log("foozs");
            var overrideController = _runtimeAnimController as AnimatorOverrideController;
            var overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();

            overrideController.GetOverrides(overrides);

            foreach (var overrideClip in overrides)
            {
                var didParse = Enum.TryParse(overrideClip.Key.name, out TClipType type);
                if (!didParse || _animationClips.ContainsKey(type)) continue;

                _animationClips.Add(type, overrideClip.Value);
            }
        }
    }
}
