using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICanvas : MonoBehaviour
{
    public Image DarkScreen;
    public TextMeshProUGUI YouDied;
    public Image EndImage;

    public bool IsEnd;
    public float EndTime = 3f;

    private void Awake()
    {
        InitUIDied();
        EndImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (YouDied.color.a == 1 && Input.anyKeyDown)
        {
            GameManager.Instance.RestarScene();
        }
        if (IsEnd)
        {
            EndTime -= Time.deltaTime;
            if (EndTime < 0)
            {
                if (Input.anyKeyDown)
                {
                    GameManager.Instance.LoadScene("TitleScene");
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (IsEnd)
        {
            EndImage.gameObject.SetActive(true);
            EndImage.color = new Color(1, 1, 1, EndImage.color.a + Time.deltaTime);
        }
    }
    public void SetEnd()
    {
        IsEnd = true;
    }

    public void SwitchOnDiedScreen()
    {
        StartCoroutine(DoDarkScreen());
        //SaveManager.Instance.SaveGame();
    }
    private IEnumerator DoDarkScreen()
    {
        yield return new WaitForSeconds(0.5f);
        float alpha = 0;
        float fadecount = 0;
        DarkScreen.gameObject.SetActive(true);
        while (DarkScreen.color.a < 1)
        {
            fadecount++;
            alpha = Mathf.Lerp(0, 1f, (fadecount / 5f));
            DarkScreen.color = SetColorAlpha(DarkScreen.color, alpha);
            yield return new WaitForSeconds(0.1f);
        }
        StartCoroutine(DoYouDied());
    }
    private IEnumerator DoYouDied()
    {
        float alpha = 0;
        float fadecount = 0;
        YouDied.gameObject.SetActive(true);
        AudioManager.Instance.StartCoroutine(AudioManager.Instance.PlayDeath());
        while (YouDied.color.a < 1)
        {
            fadecount++;
            alpha = Mathf.Lerp(0, 1f, (fadecount / 10f));
            YouDied.color = SetColorAlpha(YouDied.color, alpha);
            yield return new WaitForSeconds(0.1f);
        }
    }
    private IEnumerator DoFadeIn(Color color, int fadecount, float delay = 0f, float alpha = 0)
    {
        yield return new WaitForSeconds(delay);
        DarkScreen.gameObject.SetActive(true);
        while (color.a < 1)
        {
            fadecount++;
            alpha = Mathf.Lerp(0, 1f, (fadecount / 10f));
            color = SetColorAlpha(color, alpha);
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void InitUIDied()
    {
        DarkScreen.color = SetColorAlpha(DarkScreen.color, 0);
        YouDied.color = SetColorAlpha(YouDied.color, 0);
        DarkScreen.gameObject.SetActive(false);
        YouDied.gameObject.SetActive(false);
    }
    public Color SetColorAlpha(Color color, float alpha)
    {
        Color newColor = new Color(color.r, color.g, color.b, alpha);
        return newColor;
    }
}
