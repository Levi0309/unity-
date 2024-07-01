using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName ="RoomDataSO",menuName ="Map/RoomDataSO")]
public class RoomData_SO : ScriptableObject
{
    public Sprite roomIcon;
    public RoomType roomType;
    public AssetReference sceneToLoad;
}
