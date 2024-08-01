using Assets.Script.Manager;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class CheckPoint : MonoBehaviour
{
    private GameObject fire;
    public string Id;
    public bool Activated;
    private void Awake()
    {
        fire = this.transform.Find("Fire").gameObject;
    }
    private void Start()
    {
        ActivateCheckPoint(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<MiaController>() != null)
        {
            if (!Activated)
            {
                SaveManager.Instance.SaveGame();
                TimerManager.Instance.SlowFrozenTime(0.5f);
                AudioManager.Instance.StartCoroutine(AudioManager.Instance.PlayCheckPoint());
                ActivateCheckPoint(true);
            }
        }
    }

    [ContextMenu("Generate checkpoint id")]
    private void GenerateId()
    {
        Id = System.Guid.NewGuid().ToString();
    }

    public void ActivateCheckPoint(bool value)
    {
        Activated = value;
        fire.SetActive(value);
    }
}
