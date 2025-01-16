//using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour
{
    static AssetManager instance;
    public static AssetManager Instance => instance;

  

    public Dictionary<AssetType, List<AssetData>> assetDatas;

    private void Awake()
    {
        instance = this;
        assetDatas = new Dictionary<AssetType, List<AssetData>>();
    }

    public void LoadAsset(AssetType assetType, string assetName)
    {
        List<AssetData> datas = null;

        // 字典裡是否有包含當前Type?    有：繼續執行讀取動作    沒有：新建一個該Type的List
        if (assetDatas.ContainsKey(assetType))
        {
            datas = assetDatas[assetType];
            // 取得當前Type的資料數量
            var datasCount = datas.Count;
            for (int i = 0; i < datasCount; i++)
            {
                if (datas[i].name == assetName)
                {
                    EventHandler.FinishAssetLoaded(assetType, datas[i].obj);
                    return;
                }
            }
        }
        else
        {
            datas = new List<AssetData>();
            assetDatas.Add(assetType, datas);
        }

        StartCoroutine(LoadGameObject(assetType, assetName, datas));
    }

    IEnumerator LoadGameObject(AssetType assetType, string assetName, List<AssetData> dataList)
    {
        ResourceRequest rr = null;
        if (assetType == AssetType.Obj || assetType == AssetType.Effect)
        {
            rr = Resources.LoadAsync<GameObject>(assetName);
        }
        else if (assetType == AssetType.Texture)
        {
            rr = Resources.LoadAsync<Texture>(assetName);
        }

        if (rr == null)
            yield break;

        yield return rr;

        if (rr.isDone && rr.asset != null)
        {
            AssetData data = new AssetData();
            data.name = assetName;
            data.obj = rr.asset;
            dataList.Add(data);
            EventHandler.FinishAssetLoaded(assetType, rr.asset);
        }
    }
}
