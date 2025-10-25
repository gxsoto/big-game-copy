using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinSpawner : MonoBehaviour
{

    public GameObject goodPumpkinPrefab;  // drag your prefab here in the Inspector
    public float minX = -8f;
    public float maxX = 8f;
    public float groundY = -3.5f;

    private int batClicksSinceLastPumpkin = 0;
    private int totalPumpkinsSpawned = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        // Watch for bat score changes
        if (Clickable.Score / 10 > totalPumpkinsSpawned)
        {
            SpawnPumpkin();
            totalPumpkinsSpawned++;
        }
        
    }

    void SpawnPumpkin()
    {
        float randomX = Random.Range(minX, maxX);
        Vector3 spawnPos = new Vector3(randomX, groundY, 0f);
        Instantiate(goodPumpkinPrefab, spawnPos, Quaternion.identity);
    }
}
