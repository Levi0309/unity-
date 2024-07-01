using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class InitLoad : MonoBehaviour
{
    public AssetReference persitance;
    private void Awake()
    {
        Addressables.LoadSceneAsync(persitance);
    }
}
