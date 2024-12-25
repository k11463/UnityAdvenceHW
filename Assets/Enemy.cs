using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float hp;
    public float maxHp;
    Material matt;

    private void Start()
    {
        matt = GetComponent<Renderer>().material;
        hp = maxHp;
    }

    public void GetHit(float damage)
    {
        hp -= damage;
        matt.color = Color.Lerp(Color.red, Color.white, hp / maxHp);

        if (hp < 0)
        {
            GameManager.RemoveEnemy(gameObject);
            Destroy(gameObject);
        }
    }
}
