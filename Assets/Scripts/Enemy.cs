using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float hp;
    public float maxHp;
    Material material;
    public PoolData poolData;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        if (material == null)
            material = GetComponent<Renderer>().material;
        hp = maxHp;
        material.color = Color.white;
    }

    public void UpdateTexture(Texture2D texture)
    {
        if (material == null)
            material = GetComponent<Renderer>().material;
        material.mainTexture= texture;
    }

    public void GetHit(float damage)
    {
        hp -= damage;
        material.color = Color.Lerp(Color.red, Color.white, hp / maxHp);

        if (hp <= 0)
        {
            Database.enemyPoolDatas.Remove(poolData);
            GameObjectPool.Instance.UnLoadObject(poolData);
        }
    }
}
