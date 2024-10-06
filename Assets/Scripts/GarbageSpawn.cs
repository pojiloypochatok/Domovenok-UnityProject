using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class GarbageSpawn : MonoBehaviour
{
    public GameObject[] objects;
    [SerializeField]
    Vector3 leftlimit;
    [SerializeField]
    Vector3 rightlimit;
    public int minSpawnCount;
    public int maxSpawnCount;
    private int spawnCount;
    private Vector3 lastspawnlocation;
    private float totoalObj;
    private Vector3 spawnlocation;
    public LayerMask layerMask;
    private List<GameObject> spawnedObjects = new List<GameObject>();
    public static int totalobj = 0;
    public float spawntime;
    private float timebtwspawn;
    void Start()
    {

        spawnCount = Random.Range(minSpawnCount, maxSpawnCount);
        timebtwspawn = spawntime;
        SpawnGarbage(spawnCount);
    }

    void Update()
    {
        if (timebtwspawn <= 0 & totalobj < maxSpawnCount)
        {
            SpawnGarbage(1);
            timebtwspawn = spawntime;
        }
        else
        {
            timebtwspawn -= Time.deltaTime;
        }
    }

    void SpawnGarbage(int spawnCount)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            spawnlocation.x = Random.Range(leftlimit.x, rightlimit.x);
            spawnlocation.z = 1;
            while (CheckForCollision(spawnlocation))
            {
                spawnlocation.x = Random.Range(leftlimit.x, rightlimit.x);
            }
            GameObject clonegarbage = Instantiate(objects[Random.Range(0, objects.Length)], spawnlocation, Quaternion.identity);
            clonegarbage.transform.parent = transform;
            spawnedObjects.Add(clonegarbage);
            totalobj += 1;
        }
    }
    bool CheckForCollision(Vector3 spawnlocation)
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(spawnlocation, new Vector2(15, 15), 0f, layerMask);
        if (colliders.Length > 0)
        {
            return true;
        }
        return false;
    }


}