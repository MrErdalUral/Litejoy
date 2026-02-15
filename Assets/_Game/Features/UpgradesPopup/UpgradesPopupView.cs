using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Features.UpgradesPopup
{
    public class UpgradesPopupView : MonoBehaviour,IUpgradesPopupView
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private TMP_Text _upgradeButtonText;
        [SerializeField] private TMP_Text _currentStatsText;
        [SerializeField] private float _fadeDuration = 0.3f;

        private Sequence _showSequence;
        private Sequence _hideSequence;
        
        public void Show()
        {
            if(_hideSequence != null)
                _hideSequence.Complete();
            
            _showSequence ??= CreateShowSequence();
            _showSequence.Restart();
        }

        public void Hide()
        {
            if(_showSequence != null)
                _showSequence.Complete();
            
            _hideSequence ??= CreateHideSequence();
            _hideSequence.Restart();
        }

        private Sequence CreateHideSequence()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(_canvasGroup.DOFade(0, _fadeDuration));
            sequence.AppendCallback(() => gameObject.SetActive(false));
            sequence.SetAutoKill(false);
            return sequence;
        }

        private Sequence CreateShowSequence()
        {
            var sequence = DOTween.Sequence();
            sequence.AppendCallback(() => gameObject.SetActive(true));
            sequence.Append(_canvasGroup.DOFade(1, _fadeDuration));
            sequence.SetAutoKill(false);
            return sequence;
        }

    }
}