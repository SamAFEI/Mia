using Assets.Script.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public List<GameObject> Enemys;
    public List<GameObject> SpawnPoints;
    public List<GameObject> SpawnBossPoints;
    public List<GameObject> Boss;
    public bool IsLevel1Complete;
    public bool IsLevel2Complete;
    public bool IsLevel3Complete;
    public bool IsLeve11Active;
    public bool IsLeve12Active;
    public bool IsLeve13Active;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else if (this != Instance)
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (GameManager.Instance.IsPaused) { return; }
    }

    public void SetLevel1Active()
    {
        IsLeve11Active = true;
    }

    protected virtual IEnumerator SpawnOneWave()
    {
        StartCoroutine(AudioManager.Instance.PlayStatue());
        foreach (GameObject point in SpawnPoints)
        {
            foreach (GameObject enemy in Enemys)
            {
                float offset = Random.Range(-1f, 1f);
                Vector2 vector = new Vector2(point.transform.position.x + offset, point.transform.position.y + offset);
                EffectManager.Instance.DoSpawnFX(vector);
                Instantiate(enemy, vector, Quaternion.identity);
                yield return new WaitForSeconds(Random.Range(0.1f,0.6f));
            }
        }
        yield return new WaitForSeconds(30f);
    }
}
