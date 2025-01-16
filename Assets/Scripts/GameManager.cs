using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public static GameManager Instance => instance;
    GameObjectPool objPool;

    private void Awake()
    {
        instance = this;
        objPool = GameObjectPool.Instance;
    }

    void Start()
    {
        Database.enemyPoolDatas = new LinkedList<PoolData>();
        StartCoroutine(objPool.InitPool(ObjectType.Enemy, Database.enemySpawnCount, "Enemy", "EnemyStorage", "EnemyTexture"));
        StartCoroutine(Respawn());
    }

    bool respawing = false;

    public IEnumerator Respawn()
    {
        respawing = true;
        int spawnCount = 0;
        while (true)
        {
            if (objPool.finishLoad)
            {
                while (spawnCount < Database.enemySpawnCount)
                {
                    var enemyPoolData = objPool.LoadObject(ObjectType.Enemy);
                    var enemy = enemyPoolData.obj;
                    enemy.GetComponent<Enemy>().Init();
                    Database.enemyPoolDatas.AddLast(enemyPoolData);
                    var startPos = new Vector3(UnityEngine.Random.Range(-Database.enemySpawnRange.x, Database.enemySpawnRange.x), UnityEngine.Random.Range(0, Database.enemySpawnRange.y), UnityEngine.Random.Range(-Database.enemySpawnRange.z, Database.enemySpawnRange.z));
                    enemy.transform.position = startPos;
                    spawnCount++;
                    if (spawnCount % 2 == 0)
                    {
                        yield return 0;
                    }
                }
                respawing = false;
                break;
            }
            else
            {
                yield return 0;
            }
        }
    }

    private void Update()
    {
        if (Database.enemyPoolDatas.Count == 0 && respawing == false)
            StartCoroutine(Respawn());
    }
}
