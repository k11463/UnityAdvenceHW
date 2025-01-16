using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventHandler
{
    public static Action<AssetType, UnityEngine.Object> FinishAssetLoadedAction;
    public static void FinishAssetLoaded(AssetType assetType, UnityEngine.Object obj) => FinishAssetLoadedAction?.Invoke(assetType, obj);

    public static Action<PoolData> EnemyDeadAction;
}

public enum ObjectType
{
    None = -1,
    Enemy = 0,
    Npc,
    Bullet
}
public class PoolData
{
    public bool isUsing;
    public GameObject obj;
}

public enum AssetType
{
    None = -1,
    Obj = 0,
    Effect,
    Material,
    Texture
}
public class AssetData
{
    public string name;
    public UnityEngine.Object obj;
}