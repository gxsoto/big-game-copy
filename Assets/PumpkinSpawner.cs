using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinSpawner : MonoBehaviour
{

    public GameObject goodPumpkinPrefab;  // drag your prefab here in the Inspector
    public GameObject blackPumpkinAttackPrefab;
    public float minX = -8f;
    public float maxX = 8f;
    public float groundY = -3.5f;
    //
    //public float maxY = 4f;
    public float spawnMinY = camBottom + pumpkinHeight / 2f; 
    public float spawnMaxY = Camera.main.transform.position.y; 

    private int batClicksSinceLastPumpkin = 0;
    private int totalGoodPumpkinsSpawned = 0;
    private int goodPumpkinsSinceLastBlack = 0;

    public struct Coords
    {
        public float a;
        public float b;

        public Coords(float a, float b)
        {
            this.a = a;
            this.b = b;
        }
    };

    private List<Coords> pumpkinSpawnPositions = new List<Coords>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        // Watch for bat score changes
        if (Clickable.Score / 10 > totalGoodPumpkinsSpawned)
        {
            SpawnPumpkin();
            totalGoodPumpkinsSpawned++;
            goodPumpkinsSinceLastBlack++;
        }
        if (goodPumpkinsSinceLastBlack >= 5)
        {
            SpawnBlackPumpkin();
            goodPumpkinsSinceLastBlack = 0;
        }
        
    }

    void SpawnPumpkin()
    {
        float randomX = Random.Range(minX, maxX);
        // float randomY = Random.Range(minY, maxY);

        float pumpkinHeight = goodPumpkinPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
        float camBottom = Camera.main.transform.position.y - Camera.main.orthographicSize;

        // float spawnMinY = camBottom + pumpkinHeight / 2f; 
        // float spawnMaxY = Camera.main.transform.position.y; 

        float randomY = Random.Range(spawnMinY, spawnMaxY); ;

        Vector3 spawnPos = new Vector3(randomX, randomY, 0f);
        pumpkinSpawnPositions.Add(new Coords(randomX, randomY));
        // Instantiate(goodPumpkinPrefab, spawnPos, Quaternion.identity);
        GameObject pumpkin = Instantiate(goodPumpkinPrefab, spawnPos, Quaternion.identity);
        pumpkin.transform.localScale = new Vector3(2f, 2f, 1f);
    }
    
    void SpawnBlackPumpkin()
    {
        float randomX = Random.Range(minX, maxX);
        // float randomY = Random.Range(minY, maxY);

        float pumpkinHeight = blackPumpkinAttackPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
        float camBottom = Camera.main.transform.position.y - Camera.main.orthographicSize;

        // float spawnMinY = camBottom + pumpkinHeight / 2f; 
        // float spawnMaxY = Camera.main.transform.position.y; 

        float randomY = Random.Range(spawnMinY, spawnMaxY); ;

        Vector3 spawnPos = new Vector3(randomX, randomY, 0f);
        GameObject blackPumpkin = Instantiate(blackPumpkinAttackPrefab, spawnPos, Quaternion.identity);
        blackPumpkin.transform.localScale = new Vector3(2f, 2f, 1f);
       
    }
}
