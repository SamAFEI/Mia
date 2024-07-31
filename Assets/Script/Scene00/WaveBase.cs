using Assets.Script.Enemy;
using Assets.Script.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBase : MonoBehaviour
{
    public List<WaveBase> WaveReSets;
    public List<GameObject> Enemys;
    public List<GameObject> Wells;
    public List<GameObject> SpawnPoints;
    public GameObject ActivedPoint;
    public GameObject EndPoint;
    public EnemyController Boss;
    public int WaveCount = 3;
    public float WaveDelay = 3f;
    public float SpawnDelay = 0.1f;
    public bool IsSpawnEnd;
    public bool IsActived;
    public bool IsEnded;
    public bool IsBossLive;
    public string Id;
    public int battlemuisc=0;

    protected virtual void Awake()
    {
        Id = this.name;
    }

    protected virtual void Start()
    {
        SetWellsActive(false);
        if (Boss != null)
        {
            Boss.gameObject.SetActive(false);
        }
    }

    protected virtual void Update()
    {
        if(!IsActived)
        {
            Collider2D HitCollider = ActivedPoint.GetComponent<Collider2D>();
            List<Collider2D> collidersToDamage = new List<Collider2D>();
            ContactFilter2D filter = new ContactFilter2D();
            filter.useTriggers = true;
            Physics2D.OverlapCollider(HitCollider, filter, collidersToDamage);
            foreach (var collider in collidersToDamage)
            {
                if (collider.tag == "Player")
                {
                    IsActived = true;
                    DoEvent();
                }
            }
        }
        if (IsBossLive && Boss.isActiveAndEnabled && Boss.CurrentHP <= 0)
        {
            IsBossLive = false;
        }
        if (IsSpawnEnd && !IsBossLive)
        {
            CheckEnd();
            if (IsEnded)
            {
                SetWellsActive(false);
                AudioManager.Instance.StopBGM();
            }
        }
    }
    protected virtual void SetWellsActive(bool active)
    {
        foreach (GameObject well in Wells)
        {
            well.SetActive(active);
        }
    }
    protected virtual IEnumerator SpawnOneWave()
    {
        for (int i = 0; i < WaveCount; i++)
        {
            StartCoroutine(AudioManager.Instance.PlayStatue());
            foreach (GameObject point in SpawnPoints)
            {
                foreach (GameObject enemy in Enemys)
                {
                    float offset = Random.Range(-1f,1f);
                    Vector2 vector = new Vector2(point.transform.position.x + offset, point.transform.position.y + offset);
                    EffectManager.Instance.DoSpawnFX(vector);
                    Instantiate(enemy, vector, Quaternion.identity);
                    yield return new WaitForSeconds(SpawnDelay);
                }
            }
            yield return new WaitForSeconds(WaveDelay);
        }
        IsSpawnEnd = true;
    }
    protected virtual void DoEvent()
    {
        if (Boss != null && IsBossLive)
        {
            Boss.gameObject.SetActive(true);
        }
        SetWellsActive(true);
        StartCoroutine(SpawnOneWave());
        foreach (WaveBase wave in WaveReSets)
        {
            wave.ReSet();
        }
        AudioManager.Instance.PlayBGM(true, battlemuisc,IsBossLive);
    }
    protected virtual void CheckEnd()
    {
        Collider2D HitCollider = EndPoint.GetComponent<Collider2D>();
        List<Collider2D> colliders = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        Physics2D.OverlapCollider(HitCollider, filter, colliders);
        bool noEnemy = false;
        foreach (var collider in colliders)
        {
            if (collider.tag == "Enemy")
            {
                noEnemy = false;
                break;
            }
            noEnemy = true;
        }
        IsEnded = noEnemy;
    }
    public virtual void ReSet()
    {
        IsSpawnEnd = false;
        IsActived = false;
        IsEnded = false;
    }

}
