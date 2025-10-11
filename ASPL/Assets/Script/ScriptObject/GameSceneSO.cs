using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "Event/GameSceneSO")]
public class GameSceneSO : ScriptableObject
{
    public SceneType sceneType;

    public AssetReference sceneReference;
}
