using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static LinkedList<GameObject> enemies { get; private set; }
    GameObject enemyPrefab;
    public int enemyCount;
    public Vector3 createRange;

    private void Awake()
    {
        StartCoroutine(LoadEnemyPrefab("Enemy"));
    }

    private void Start()
    {
        enemies = new LinkedList<GameObject>();
        for (int i = 0; i < enemyCount; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            var startPos = new Vector3(Random.Range(-createRange.x, createRange.x), Random.Range(0, createRange.y), Random.Range(-createRange.z, createRange.z));
            enemy.transform.position = startPos;
            enemies.AddFirst(enemy);
        }
    }

    IEnumerator LoadEnemyPrefab(string name)
    {
        var loadData = Resources.LoadAsync(name);
        if (loadData == null)
            yield break;

        yield return loadData;

        if (loadData.isDone && loadData.asset != null)
        {
            enemyPrefab = loadData.asset as GameObject;
        }
    }

    public static void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }
}
