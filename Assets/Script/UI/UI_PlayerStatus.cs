using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerStatus : MonoBehaviour
{
    private MiaController player => GameObject.FindObjectOfType<MiaController>();
    private RectTransform myRectTransform => GetComponent<RectTransform>();

    public Slider hpSlider { get; private set; }
    private float hpSmooth;

    private void Awake()
    {
        hpSlider = transform.Find("UI_Health").GetComponent<Slider>();
    }

    private void Start()
    {
        hpSlider.maxValue = player.Data.maxHP;
        hpSlider.value = hpSlider.maxValue;
    }

    public void DoLerpHealth(float _damage)
    {
        hpSmooth = 0;
        StartCoroutine(LerpHealth(_damage));
    }

    private IEnumerator LerpHealth(float _damage)
    {
        float smooth = 10;
        float startHP = hpSlider.value;
        while (hpSmooth < 1)
        {
            hpSmooth += Time.deltaTime * smooth;
            hpSlider.value = Mathf.Lerp(startHP, player.CurrentHP, hpSmooth);
            yield return null;
        }
        yield return new WaitForSeconds(2f);
    }
}
