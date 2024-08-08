using Fungus;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class TitleScene : MonoBehaviour
{
    public TextMeshProUGUI txPress;
    public VideoPlayer videoPlayer;
    public GameObject video;
    public bool IsPlayEnd;
    public float delayTime;

    private void Start()
    {
        videoPlayer.loopPointReached += PlayEndEvent;
        delayTime = 5f;
    }
    private void Update()
    {
        delayTime -= Time.deltaTime;
        if (Input.anyKeyDown)
        {
            if (IsPlayEnd)
            {
                GameManager.Instance.LoadScene("SceneDemo");
            }
            else if (delayTime < 0)
            {
                PlayEndEvent(videoPlayer);
            }
        }
    }

    private void PlayEndEvent(VideoPlayer vp)
    {
        IsPlayEnd = true;
        videoPlayer.gameObject.SetActive(false);
        video.SetActive(false);
        InvokeRepeating("StartFade", 0f, 2f);
    }

    private void StartFade()
    {
        StartCoroutine(FadeTitle());
    }
    private IEnumerator FadeTitle()
    {
        for (float alpha = 0.3f; alpha < 1f; alpha += Time.deltaTime)
        {
            txPress.color = new Color(txPress.color.r, txPress.color.g, txPress.color.b, alpha);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        for (float alpha = 1f; alpha > 0.3f; alpha -= Time.deltaTime)
        {
            txPress.color = new Color(txPress.color.r, txPress.color.g, txPress.color.b, alpha);
            yield return null;
        }
    }
}
