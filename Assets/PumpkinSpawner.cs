using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinSpawner : MonoBehaviour
{

    public GameObject goodPumpkinPrefab;  // drag your prefab here in the Inspector
    public GameObject blackPumpkinAttackPrefab;
    public GameObject pumpkinHurtPrefab;

    public float minX = -8f;
    public float maxX = 8f;
    public float groundY = -3.5f;
    private bool blackPumpkinActive = false;

    private int batClicksSinceLastPumpkin = 0;
    private int totalGoodPumpkinsSpawned = 0;
    private int goodPumpkinsSinceLastBlack = 0;

    public struct PumpkinInfo
    {
        public Vector2 position;
        public GameObject pumpkinObject;

        public PumpkinInfo(Vector2 pos, GameObject obj)
        {
            position = pos;
            pumpkinObject = obj;
        }
    }   

    private List<PumpkinInfo> goodPumpkins = new List<PumpkinInfo>();
    private List<PumpkinInfo> currBlackPumpkins = new List<PumpkinInfo>();

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
        if (goodPumpkinsSinceLastBlack >= 2)
        {
            SpawnBlackPumpkin();
            goodPumpkinsSinceLastBlack = 0;
        }
        
    }

    void SpawnPumpkin()
    {
        float randomX = Random.Range(minX, maxX);

        float pumpkinHeight = goodPumpkinPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
        float camBottom = Camera.main.transform.position.y - Camera.main.orthographicSize;

        float spawnMinY = camBottom + pumpkinHeight / 2f; 
        float spawnMaxY = Camera.main.transform.position.y; 

        float randomY = Random.Range(spawnMinY, spawnMaxY); 

        Vector3 spawnPos = new Vector3(randomX, randomY, 0f);
        GameObject pumpkin = Instantiate(goodPumpkinPrefab, spawnPos, Quaternion.identity);
        pumpkin.transform.localScale = new Vector3(2f, 2f, 1f);
        goodPumpkins.Add(new PumpkinInfo(new Vector2(spawnPos.x, spawnPos.y), pumpkin));
        Debug.LogWarning("added good pumpkin");
    }

    void SpawnBlackPumpkin()
    {
        
        //pumpkin doesnt spawn below the camera
        float pumpkinHeight = blackPumpkinAttackPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
        float camBottom = Camera.main.transform.position.y - Camera.main.orthographicSize;
        // choose a good pumpkin to attack
        int randomIndex = Random.Range(0, goodPumpkins.Count);
        PumpkinInfo target = goodPumpkins[randomIndex];

        //check out its coords so we spawn the hurt pumpkin in the exact same spot
        Vector3 oldPos = target.pumpkinObject.transform.position;
        Destroy(target.pumpkinObject);
        GameObject hurtPumpkin = Instantiate(pumpkinHurtPrefab, oldPos, Quaternion.identity);
        hurtPumpkin.transform.localScale = new Vector3(2f, 2f, 1f);

        // Update our good pumpkins list
        goodPumpkins[randomIndex] = new PumpkinInfo(new Vector2(oldPos.x, oldPos.y), hurtPumpkin);

        //spawn black pumpkin to the right since it bites leftwards
        float newX = Mathf.Clamp(oldPos.x + 1.5f, minX, maxX);
        float newY = oldPos.y;
        Vector3 spawnPos = new Vector3(newX, newY, 0f);
        GameObject blackPumpkin = Instantiate(blackPumpkinAttackPrefab, spawnPos, Quaternion.identity);
        blackPumpkin.transform.localScale = new Vector3(2f, 2f, 1f);
        goodPumpkins.RemoveAt(randomIndex);

        //save both in case i need to kill both later
        // currBlackPumpkins.Add(new PumpkinInfo(new Vector2(oldPos.x, oldPos.y), hurtPumpkin));
        currBlackPumpkins.Add(new PumpkinInfo(new Vector2(spawnPos.x, spawnPos.y), blackPumpkin));

        // timer to kill the pumpkin(s)
        totalGoodPumpkinsSpawned--;
        StartCoroutine(timer(hurtPumpkin, blackPumpkin));
    }
    
    IEnumerator timer(GameObject hurtPumpkin, GameObject blackPumpkin)
{
    float timer = 5f;

    while (timer > 0)
    {
        // if black pumpkin was destroyed early (user tapped it)
        if (blackPumpkin == null)
        {
            // bring back good pumpkin
            Vector3 pos = hurtPumpkin.transform.position;
            Destroy(hurtPumpkin);
            GameObject goodPumpkin = Instantiate(goodPumpkinPrefab, pos, Quaternion.identity);
            goodPumpkin.transform.localScale = new Vector3(2f, 2f, 1f);
            goodPumpkins.Add(new PumpkinInfo(new Vector2(pos.x, pos.y), goodPumpkin));

            //remove balxk pumpkin from list
            for (int i = currBlackPumpkins.Count - 1; i >= 0; i--)
            {
                if (currBlackPumpkins[i].pumpkinObject == blackPumpkin || currBlackPumpkins[i].pumpkinObject == hurtPumpkin)
                {
                    currBlackPumpkins.RemoveAt(i);
                }
            }

            totalGoodPumpkinsSpawned++;
            yield break;
        }

        timer -= Time.deltaTime;
        yield return null;
    }

    // time is up, kill both pumpkins
    if (hurtPumpkin != null) {Destroy(hurtPumpkin);}
    if (blackPumpkin != null) {Destroy(blackPumpkin);}
    
    //remove pumpkins from lists and update count
    totalGoodPumpkinsSpawned--;
    for (int i = goodPumpkins.Count - 1; i >= 0; i--) // go backwards to safely remove items
    {
        if (goodPumpkins[i].pumpkinObject == hurtPumpkin)
        {
            goodPumpkins.RemoveAt(i);
            break; // stop after removing, since there should only be one match
        }
    }
    for (int i = currBlackPumpkins.Count - 1; i >= 0; i--)
    {
        if (currBlackPumpkins[i].pumpkinObject == hurtPumpkin || currBlackPumpkins[i].pumpkinObject == blackPumpkin)
        {
            currBlackPumpkins.RemoveAt(i);
        }
    }
}

}
