using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolTool : MonoBehaviour
{
    public GameObject objPrefab;
    private ObjectPool<GameObject> pool;
    private void Awake()
    {
        pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(objPrefab),
            actionOnGet:(obj)=>obj.SetActive(true),
            actionOnRelease:(obj)=>obj.SetActive(false),
            actionOnDestroy:(obj)=>Destroy(obj),
            collectionCheck:false,
            defaultCapacity:10,
            maxSize:20

            );

        preFillPool(8);
    }
    private void preFillPool(int count) 
    {
        GameObject[] preFillPools = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            preFillPools[i] = pool.Get();
        }
        foreach (var item in preFillPools)
        {
            pool.Release(item);
        }
    }
    public GameObject GetObjectFromPool() 
    {
        return pool.Get();
    }
    /// <summary>
    ///  Õ∑≈ªÿ»•
    /// </summary>
    /// <param name="releaseObj"></param>
    public void ReturnObjectToPool(GameObject releaseObj) 
    {
        pool.Release(releaseObj);
    }
}
