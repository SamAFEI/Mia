using Assets.Script.Enemy;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BOSSStatus : MonoBehaviour
{
    public EnemyController boss;
    private RectTransform myRectTransform => GetComponent<RectTransform>();

    public Slider hpSlider { get; private set; }
    public TextMeshProUGUI txBoss { get; private set; }
    private float hpSmooth;

    private void Awake()
    {
        hpSlider = transform.Find("UI_Health").GetComponent<Slider>();
        txBoss = transform.Find("UI_Name").GetComponent<TextMeshProUGUI>();
        SetUIStateActive(false);
    }

    public void DoLerpHealth(float _damage)
    {
        hpSmooth = 0;
        StartCoroutine(LerpHealth(_damage));
    }

    private IEnumerator LerpHealth(float _damage)
    {
        float smooth = 5;
        float startHP = hpSlider.value;
        while (hpSmooth < 1)
        {
            hpSmooth += Time.deltaTime * smooth;
            hpSlider.value = Mathf.Lerp(startHP, boss.CurrentHP, hpSmooth);
            yield return null;
        }
        yield return new WaitForSeconds(2f);
    }

    public void SetUIStateActive(bool _value)
    {
        hpSlider.gameObject.SetActive(_value);
        txBoss.gameObject.SetActive(_value);

        if (_value)
        {
            hpSlider.maxValue = boss.Data.maxHP;
            hpSlider.value = hpSlider.maxValue;
            txBoss.text = boss.name;
            StartCoroutine(LerpBarScale());
        }
    }

    private IEnumerator LerpBarScale()
    {
        float smooth = 0.01f;
        hpSlider.transform.localScale = new Vector3(0, 1, 1);
        float ScaleSmooth = 0;
        while (ScaleSmooth < 1)
        {
            ScaleSmooth += Time.deltaTime * smooth;
            hpSlider.transform.localScale = new Vector3(Mathf.Lerp(hpSlider.transform.localScale.x, 1, ScaleSmooth), 1, 1);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
    }
}
