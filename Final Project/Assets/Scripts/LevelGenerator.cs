using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] prefabs; // unique parts of the road
    public GameObject bg;    // background
    public float zSpawn = 0; // road spawn
    public float bgSpawn;  // background spawn; z-axis offset
    public float bgY = 17; // y-axis offset
    public float partLength = 40.85f; // length of the one part
    private Vector3 distance;  // between player and background
    public int partCount = 5; // number of parts to generate
    private int index;
    public Transform playerTransform;
    private List<GameObject> currentParts = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        bgSpawn = partLength * partCount + partLength / 2;
        bg.transform.position = new Vector3(0, bgY, bgSpawn);
        distance = bg.transform.position - playerTransform.position;
        for (int i = 0; i< partCount; i++)
        {
            SpawnRoad(); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        bg.transform.position = playerTransform.position + distance;
        if (zSpawn - playerTransform.position.z < partLength*(partCount-2))  // the player has passed one part of the road,
        {                        
            SpawnRoad();                                                     // so the next part is going to be generated
            Destroy(currentParts[0]);                                        // and the old part is destroyed
            currentParts.RemoveAt(0);
        }
    }
    private void SpawnRoad()
    {
        zSpawn += partLength;
        index = Random.Range(1, prefabs.Length);    // choose a part randomly
        GameObject part = Instantiate(prefabs[index], transform.forward * zSpawn, transform.rotation);  // put the part in place
        currentParts.Add(part);
    }
}
