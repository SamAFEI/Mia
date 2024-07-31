
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    public GameObject ShadowBone;
    public Transform SpawnPosition;
    public Color shadowColor = Color.black;
    public float maxAfterTime = 0.1f;
    public float IntervalTime = 0.01f;
    public float LifeTime = 1f;
    public bool IsSpawn = false;
    float duringAfterTime = 0f;
    float duringIntervalTime = 0f;
    List<AfterImage> listImage;

    private void Start()
    {
        listImage = new List<AfterImage>();
    }
    private void FixedUpdate()
    {
        if (IsSpawn)
        {
            if (duringAfterTime < maxAfterTime)
            {
                duringAfterTime += Time.deltaTime;
                if (duringIntervalTime < IntervalTime)
                {
                    duringIntervalTime += Time.deltaTime;
                }
                else
                {
                    duringIntervalTime = 0f;
                    SpawnOne();
                }
            }
            else
            {
                IsSpawn = false;
            }
        }
        else
        {
            duringAfterTime = 0f;
            duringIntervalTime = 0f;
        }
        for (int i = 0; i < listImage.Count; i++)
        {
            listImage[i].update(Time.deltaTime);
        }
        while(listImage.Count > 0 && listImage[0].isDestroyed)
        {
            listImage.RemoveAt(0);
        }
    }
    void SpawnOne()
    {
        listImage.Add(new AfterImage(ShadowBone, SpawnPosition, shadowColor
                                    , duringAfterTime / maxAfterTime * LifeTime));
    }
}

class AfterImage
{
    GameObject go;
    float lifeTime;
    float duringLifeTime;
    public bool isDestroyed = false;
    SpriteRenderer[] sprList;

    public AfterImage(GameObject go,Transform pos,Color color,float lifeTime)
    {
        this.go = GameObject.Instantiate(go,pos.transform.position,pos.transform.rotation);
        this.go.transform.localScale = pos.localScale;
        this.lifeTime = lifeTime;
        sprList = this.go.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer spr in sprList)
        {
            spr.color = color;
            spr.sortingLayerName = "PerGround";
            spr.sortingOrder = 1;
        }
    }
    public void update(float deltatime)
    {
        if(duringLifeTime < lifeTime)
        {
            duringLifeTime += deltatime;
            foreach(SpriteRenderer spr in sprList)
            {
                float alpha = Mathf.Clamp(spr.color.a - duringLifeTime / lifeTime, 0.3f, 1f);
                spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, alpha);
            }
        }
        else
        {
            GameObject.Destroy(go);
            isDestroyed = true;
        }
    }
}
