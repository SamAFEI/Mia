using Assets.Script.Enemy;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_EnemyStatus : MonoBehaviour
{
    private EnemyController enemy => GetComponentInParent<EnemyController>();
    private RectTransform myRectTransform => GetComponent<RectTransform>();

    public Slider hpSlider { get; private set; }
    private float hpSmooth;

    private void Awake()
    {
        hpSlider = transform.Find("UI_Health").GetComponent<Slider>();
    }

    private void Start()
    {
        hpSlider.maxValue = enemy.MaxHP;
        hpSlider.value = hpSlider.maxValue;
        hpSlider.gameObject.SetActive(false);
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
            hpSlider.gameObject.SetActive(true);
            hpSmooth += Time.deltaTime * smooth;
            hpSlider.value = Mathf.Lerp(startHP, enemy.CurrentHP, hpSmooth);
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        hpSlider.gameObject.SetActive(false);
    }
}
