using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Scarecrow : MonoBehaviour
{
    #region STATE PARAMETERS
    public bool IsHurting { get; protected set; }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #region HURT METHODS
    public virtual IEnumerator Hurt()
    {
        if (IsHurting) { yield break; }
        TimerManager.Instance.DoFrozenTime(0.1f);
        CameraManager.Instance.Shake(1f, 0.1f);
        StartCoroutine(AudioManager.Instance.PlayHit());
        StartCoroutine(StartShark());
        IsHurting = true;
        yield return new WaitForSeconds(0.3f);
        IsHurting = false;
    }
    #endregion
    IEnumerator StartShark()
    {
        for (int i = 0; i < 4; i++)
        {
            this.transform.localPosition = new Vector3(DoShake(i), 0, 0);
            yield return new WaitForSeconds(0.01f);
        }
    }
    float DoShake(int index)
    {
        switch (index)
        {
            case 0: return 0.1f;
            case 1: return 0.0f;
            case 2: return -0.1f;
            case 3: return 0.0f;
            default: return 0.0f;
        }
    }
}
