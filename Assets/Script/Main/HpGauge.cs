using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HpGauge : MonoBehaviour
{
    [SerializeField] private Image hpImage;
    [SerializeField] private Image burnImage;

    public float duration = 0.5f;

    public float strength = 20f;
    public int Vibrate = 100;

    public static float debugDamageRate;
    public static float currentRate = 1.0f;

    public void SetGauge(float targetRate)
    {
        hpImage.DOFillAmount(targetRate, duration).OnComplete(() =>
        {
            burnImage.DOFillAmount(targetRate, duration * 0.5f).SetDelay(0.5f);
        });

        transform.DOShakePosition(duration * 0.5f, strength, Vibrate);

        currentRate = targetRate;
    }

    public void TakeDamage(float rate)
    {
        SetGauge(currentRate - rate);
    }

    // HPゲージをリセットするメソッド
    public void ResetHp()
    {
        currentRate = 1.0f;
        hpImage.fillAmount = 1.0f;
        burnImage.fillAmount = 1.0f;
    }
}