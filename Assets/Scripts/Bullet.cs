using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector3 startPos;
    Vector3 targetDirection;
    Vector3 direction;
    public float moveSpeed;
    public LayerMask hitLayer;
    float lifeTime = 3f;
    public GameObject effect;

    public PoolData poolData;
    GameObjectPool objPool;

    public void Shot(Vector3 startPos, Vector3 targetDirection)
    {
        lifeTime = 3f;
        transform.position = startPos;
        var direction = targetDirection - startPos;
        transform.forward = direction;
    }

    private void Awake()
    {
        objPool = GameObjectPool.Instance ;
    }

    private void Update()
    {
        var originPos = transform.position;
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        RaycastHit hit;
        if (Physics.Linecast(originPos, transform.position, out hit, hitLayer))
        {
            GameObject eff = Instantiate(effect);
            eff.transform.position = transform.position;

            GameObject hitObj = hit.collider.gameObject;

            if (hitObj.layer == LayerMask.NameToLayer("Enemy"))
            {
                Enemy enemy = hitObj.GetComponent<Enemy>();
                enemy.GetHit(30f);
            }

            objPool.UnLoadObject(poolData);
        }

        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
            objPool.UnLoadObject(poolData);
    }
}
