using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour, IInitializable
{
    [SerializeField] private List<GameObject> _prefabs = new List<GameObject>();
    public void Initialize()
    {
        BuildLevel();
    }
    private void BuildLevel()
    {
        foreach(GameObject prefab in _prefabs)
            Instantiate(prefab);
    }
}
