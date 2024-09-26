using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageSpawn : MonoBehaviour
{
    public GameObject[] objects;
    [SerializeField]
    Vector3 leftlimit;
    [SerializeField]
    Vector3 rightlimit;
    public float minSpawnCount;
    public float maxSpawnCount;
    private float spawnCount;
    private float totoalObj;
    private Vector3 spawnlocation;
    // Start is called before the first frame update
    void Start()
    {
        
        spawnCount = Random.Range(minSpawnCount, maxSpawnCount);
        SpawnGarbage(spawnCount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnGarbage(float spawnCount)
    {
        for (int i = 0; i < spawnCount; i++)
        {

            spawnlocation.x = Random.Range(leftlimit.x, rightlimit.x);
            spawnlocation.z = 1;
            Instantiate(objects[Random.Range(0, objects.Length - 1)], spawnlocation, Quaternion.identity);

        }
    }

}
