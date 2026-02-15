using System;
using DG.Tweening;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Features.UpgradesPopup
{
    public class UpgradesPopupView : MonoBehaviour, IUpgradesPopupView
    {
        [Header("UI References")] [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private TMP_Text _upgradeButtonText;
        [SerializeField] private TMP_Text _currentHealthText;
        [SerializeField] private TMP_Text _currentLevelText;

        [Header("Animation Params")] [SerializeField]
        private float _fadeDuration = 0.3f;

        private Sequence _showSequence;
        private Sequence _hideSequence;

        public IObservable<Unit> OnCloseButtonClickedObservable => _closeButton.OnClickAsObservable();
        public IObservable<Unit> OnUpgradeButtonClickedObservable => _upgradeButton.OnClickAsObservable();
        public IObservable<bool> OnPointerHoverUpgradeButtonObservable => _upgradeButton
            .OnPointerEnterAsObservable()
            .Select(_ => true)
            .Merge(_upgradeButton.OnPointerExitAsObservable().Select(_ => false));

        public void UpdateHealthUpgradeText(float amount, bool isGreen = false)
        {
            if (isGreen)
            {
                _upgradeButtonText.text = $"Health: <color=green>{100 + amount}%</color> ";
                return;
            }
            
            _upgradeButtonText.text = $"Health: {100 + amount}%";
        }

        public void UpdateLevelText(int level)
        {
            _currentLevelText.text = $"Level: {level}";
        }

        public void UpdateUpgradeButtonText(bool isMaxLevel, int cost)
        {
            if (isMaxLevel)
            {
                _upgradeButtonText.text = "Max Level";
                _upgradeButton.interactable = false;
                return;
            }
            
            _upgradeButtonText.text = $"Level Up ({cost}$)";
        }

        public void Show()
        {
            if (_hideSequence != null)
                _hideSequence.Complete();

            _showSequence ??= CreateShowSequence();
            _showSequence.Restart();
        }

        public void Hide()
        {
            if (_showSequence != null)
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