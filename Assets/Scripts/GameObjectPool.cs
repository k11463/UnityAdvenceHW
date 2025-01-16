using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System;

public class GameObjectPool : MonoBehaviour
{
    static GameObjectPool instance;
    public static GameObjectPool Instance => instance;

    public Dictionary<ObjectType, List<PoolData>> poolDatas;
    GameObjectPool objPool;
    public bool finishLoad = false;

    List<GameObject> initObj = null;
    List<Texture2D> initObjTex = null;

    private void OnEnable()
    {
        EventHandler.FinishAssetLoadedAction  += FinishLoadEnemyAsset;
    }
    private void OnDisEnable()
    {
        EventHandler.FinishAssetLoadedAction -= FinishLoadEnemyAsset;
    }

    private void Awake()
    {
        instance = this;
        poolDatas = new Dictionary<ObjectType, List<PoolData>> ();
        initObj = new List<GameObject>();
        initObjTex = new List<Texture2D> ();
        objPool = GameObjectPool.instance;
    }

    void FinishLoadEnemyAsset(AssetType objType, object obj)
    {
        GameManager gm = GameManager.Instance;
        switch (objType)
        {
            case AssetType.Obj:
                initObj.Add(obj as GameObject);
                finishLoad = true;
                break;
            case AssetType.Texture:
                initObjTex.Add(obj as Texture2D);
                break;
        }
    }

    public IEnumerator InitPool(ObjectType objType, int initCount, string objName, string objStorageName, string objTexName = "")
    {
        AssetManager assetManager = AssetManager.Instance;
        if (objTexName != "")
            assetManager.LoadAsset(AssetType.Texture, objTexName);
        assetManager.LoadAsset(AssetType.Obj, objName);

        yield return finishLoad == true;

        List<PoolData> datas = null;
        if (poolDatas.ContainsKey(objType))
            datas = poolDatas[objType];
        else
        {
            datas = new List<PoolData>();
            poolDatas.Add(objType, datas);
        }

        int i = 0;
        while (true)
        {
            if (i < initCount)
            {
                GameObject go = Instantiate(initObj.Find(o => o.name == objName));
                if (objTexName != "") go.GetComponent<Renderer>().material.mainTexture = initObjTex.Find(t => t.name == objTexName);
                if (objStorageName != "") go.transform.SetParent(GameObject.Find(objStorageName).transform);
                PoolData data = new PoolData();
                go.SetActive(false);
                if (objType == ObjectType.Enemy) go.GetComponent<Enemy>().poolData = data;
                if (objType == ObjectType.Bullet) go.GetComponent<Bullet>().poolData = data;
                data.obj = go;
                data.isUsing = false;
                datas.Add(data);

                i++;
                if (i % 2 == 0)
                    yield return 0;
            }
            else
                break;
        }
    }

    public PoolData LoadObject(ObjectType type)
    {
        if (poolDatas.ContainsKey(type) == false)
            return null;
        List<PoolData> datas = poolDatas[type];
        int datasCount = datas.Count;
        for (int i = 0; i < datasCount; i++)
        {
           if (datas[i].isUsing == false)
            {
                datas[i].isUsing = true;
                datas[i].obj.SetActive(true);
                return datas[i];
            }
        }

        return null;
    }

    public void UnLoadObject(PoolData objData)
    {
        if (objData != null)
        {
            objData.obj.SetActive(false);
            objData.isUsing = false;
        }
    }
}
