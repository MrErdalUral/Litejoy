using System;
using DG.Tweening;
using UnityEngine;

namespace _Game.Features.Bosses
{
    public class BossView : MonoBehaviour,IDisposable
    {
        private Tweener _shakeSequence;

        public void Shake()
        {
            _shakeSequence ??= transform.DOShakeRotation(0.3f,strength:20f).SetAutoKill(false);
            if(!_shakeSequence.IsPlaying())
                _shakeSequence.Restart();
        }

        public void Dispose()
        {
            Destroy(gameObject);
        }
    }
}