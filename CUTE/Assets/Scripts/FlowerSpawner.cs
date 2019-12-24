using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSpawner : MonoBehaviour
{
        // public variables
    public float xMinRange = -25.0f;
    public float xMaxRange = 25.0f;
    public float yMinRange = -17.0f;
    public float yMaxRange = 18.0f;
    public GameObject[] spawnObjects; // what prefabs to spawn

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < 300; i++)
            MakeThingToSpawn();
    }

    void MakeThingToSpawn()
    {
        Vector2 spawnPosition = new Vector2(Random.Range(xMinRange, xMaxRange), Random.Range(yMinRange, yMaxRange));

        // determine which object to spawn
        int objectToSpawn = Random.Range(0, spawnObjects.Length);

        // actually spawn the game object
        GameObject spawnedObject = Instantiate(spawnObjects[objectToSpawn], spawnPosition, Quaternion.identity) as GameObject;
        spawnedObject.transform.parent = gameObject.transform;
        spawnedObject.GetComponent<SpriteRenderer>().sortingLayerName = "Flower";
    }
}
