using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinSpawner : MonoBehaviour
{

    public GameObject goodPumpkinPrefab;  // drag your prefab here in the Inspector
    public float minX = -8f;
    public float maxX = 8f;
    public float groundY = -3.5f;

    private int totalPumpkinsSpawned = 0;
    public int clicksPerPumpkin = 10;      // spawn every 10 bat clicks
    private int nextBatClickThreshold = 10;

    public GameObject puffEffectPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        // Watch for bat click changes
        if (Clickable.BatClicks >= nextBatClickThreshold)
        {
            SpawnPumpkin();
            nextBatClickThreshold += clicksPerPumpkin;
            totalPumpkinsSpawned++;
        }
        
    }

    void SpawnPumpkin()
    {
        float randomX = Random.Range(minX, maxX);
        Vector3 spawnPos = new Vector3(randomX, groundY, 0f);
        // Instantiate(goodPumpkinPrefab, spawnPos, Quaternion.identity);

        GameObject pumpkin = Instantiate(goodPumpkinPrefab, spawnPos, Quaternion.identity);
        Instantiate(puffEffectPrefab, spawnPos, Quaternion.identity);

        // Add 5 points for spawning a good pumpkin
        Clickable.Score += 5;

        // 🎃 Randomize overall size
        float baseScale = Random.Range(1.0f, 1.5f);

        // 🎲 Randomly stretch horizontally or vertically a bit
        float stretchX = baseScale * Random.Range(0.9f, 1.3f);
        float stretchY = baseScale * Random.Range(0.9f, 1.3f);

        // Apply to the pumpkin
        pumpkin.transform.localScale = new Vector3(stretchX, stretchY, 1f);

        // 🍁 Adjust sorting order based on Y position (lower = in front)
        SpriteRenderer sr = pumpkin.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            // Multiply Y by -100 to turn position into layer depth
            sr.sortingOrder = Mathf.RoundToInt(-pumpkin.transform.position.y * 100);
        }
    }
}
