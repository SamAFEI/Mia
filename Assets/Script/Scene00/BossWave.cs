using Assets.Script.Enemy;
using Assets.Script.Manager;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class BossWave : MonoBehaviour
{
    public UICanvas uiCanvas;
    public List<GameObject> Enemys;
    public List<GameObject> SpawnPoints;
    public GameObject EndPoint;
    public GameObject BossObj;
    public EnemyController Boss;
    public BossWave NextWave;
    public bool IsBossActived;
    public bool IsEnded;
    public bool IsBossLive;
    public int battlemuisc = 0;

    private void Start()
    {
        IsBossLive = true;
    }
    private void Update()
    {
        if (Boss != null && Boss.CurrentHP <= 0)
        {
            IsBossLive = false;
        }
        if (!IsBossLive)
        {
            CheckEnd();
            if (IsEnded)
            {
                AudioManager.Instance.StopBGM();
                if (NextWave != null)
                { return; }
                else
                {
                    uiCanvas.IsEnd = true;
                }
            }
        }
    }
    public virtual void DoEvent()
    {
        StartCoroutine(SpawnOneWave());
        AudioManager.Instance.PlayBGM(true, battlemuisc, IsBossLive);
    }

    protected virtual IEnumerator SpawnOneWave()
    {
        while (IsBossLive)
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
                    yield return new WaitForSeconds(Random.Range(0.1f, 0.6f));
                }
            }
            if (!IsBossActived)
            {
                yield return new WaitForSeconds(10f);
                if (BossObj != null)
                {
                    GameObject point = SpawnPoints[Random.Range(0, SpawnPoints.Count - 1)];
                    float offset = Random.Range(-1f, 1f);
                    Vector2 vector = new Vector2(point.transform.position.x + offset, point.transform.position.y + offset);
                    EffectManager.Instance.DoSpawnFX(vector);
                    Boss = Instantiate(BossObj, vector, Quaternion.identity).GetComponent<EnemyController>();
                }
                IsBossActived = true;
            }
            yield return new WaitForSeconds(30f);
        }
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
}
