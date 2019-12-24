using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemy, frenemy;
    public bool testFrenemy;
    public float minSecondsBetweenSpawn = 4, maxSecondsBetweenSpawn = 7;
    private float savedTime, timeBetweenSpawn;

    void Start()
    {
        savedTime = Time.time;
        timeBetweenSpawn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - savedTime < timeBetweenSpawn)
            return;
        int rand = Random.Range(1, 4);
        if (rand <= 2 && !testFrenemy)
            Instantiate(enemy, transform.position, transform.rotation);
        else
            Instantiate(frenemy, transform.position, transform.rotation);
        savedTime = Time.time;
        timeBetweenSpawn = Random.Range(minSecondsBetweenSpawn, maxSecondsBetweenSpawn);
    }
}
