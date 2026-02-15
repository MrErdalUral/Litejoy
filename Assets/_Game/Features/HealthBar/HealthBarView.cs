using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarView : MonoBehaviour
{
    [SerializeField] private Image _healthBarLayer1;
    [SerializeField] private Image _healthBarLayer2;
    [SerializeField] private float _helthBarAnimationSpeed = 3f;

    public void SetAmount(float amount)
    {
        gameObject.SetActive(amount < 1);
        _healthBarLayer2.fillAmount = amount;
    }

    // Update is called once per frame
    void Update()
    {
        _healthBarLayer1.fillAmount = Mathf.Lerp(_healthBarLayer1.fillAmount, _healthBarLayer2.fillAmount, Time.deltaTime * _helthBarAnimationSpeed);
    }
}