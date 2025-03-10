using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    [SerializeField] private GameObject _objectForSpawn;
    [SerializeField] private Transform _transformForSpawn;

    public void Spawn()
    {
        GameObject SpawnedObject = Instantiate(_objectForSpawn);
        SpawnedObject.transform.position = _transformForSpawn.position;
    }
}
