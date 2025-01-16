using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Database
{
    public static LinkedList<PoolData> enemyPoolDatas;
    public static int enemySpawnCount = 8;
    public static Vector3 enemySpawnRange = new Vector3(5, 5, 5);
}
