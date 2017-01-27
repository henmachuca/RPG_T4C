using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {

    private Transform playerTransform;
    private GameObject enemySpawn;
    public GameObject[] enemyType;
    public Collider _area;
    public Collider _ground;

    [Header("Spawn Settings")]
    public int maxEnemies = 1;                  // max number of enemies
    public float minimumSpawnTime = 5;          // minimum time between spawns
    public float maximumSpawnTime = 10;         // maximum time between spawns
    public float spawnDistance = 10;             // distance from the player at which the spawner spawns

    [Header("Debug (variables here should be private)")]
    public int m_numEnemies = 0;                // current number of enemies
    public bool m_playerWithinRange = false;    // is the player within range of the spawner
    public float m_nextSpawnTime = 0;           // the earliest time at which the next spawn will happen

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    //draw purple sphere over spawn area
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        //Use the same vars you use to draw your Overlapsphere to draw your Wire Sphere.
        Gizmos.DrawWireSphere(transform.position, spawnDistance);
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log("Time: "+Time.time);
        // use the GameManager to cache the player
        Transform player = playerTransform;

        // check the distance between the player and the spawner
        if (Vector3.Distance(transform.position, playerTransform.position) <= spawnDistance)
        {
            // the player is within range
            if (m_playerWithinRange)
            {
                // the player has not left the area since the last spawn, don't do anything!
            }
            else
            {
                // check if the current time exceeds the delay
                if (Time.time >= m_nextSpawnTime)
                {
                    // the delay has passed, spawn the enemies!
                    SpawnEnemies();
                }
                else
                {
                    // the delay wasn't finished, reset it!
                    SetRandomDelay();
                }
            }

            // the player is in range
            m_playerWithinRange = true;
        }
        else
        {
            // check if the player was within range on the last frame
            if (m_playerWithinRange)
            {
                // set a random spawn delay (comment this out for instant respawns)
                SetRandomDelay();
            }

            // the player is out of range
            m_playerWithinRange = false;
        }
    }

    /// <summary>
    /// Sets a random delay between the minimumSpawnTime and the maximumSpawnTime.
    /// </summary>
    void SetRandomDelay()
    {
        m_nextSpawnTime = Time.time + Random.Range(minimumSpawnTime, maximumSpawnTime);
    }

    void SpawnEnemies()
    {
        for (int i = m_numEnemies; i < maxEnemies; i++)
        {
            // Generate a random position within the spawn area
            Vector3 pos = MathUtils.PointInsideArea(_area);
            pos = MathUtils.SnapToTheGround(pos, _ground);

            // Make the spawnable look in a random orientation
            Vector3 lookPos = pos + Random.insideUnitSphere;
            lookPos.y = pos.y;

            //SetRandomDelay();

            // Instantiate enemy
            GameObject  enemy = Instantiate(enemyType[Random.Range(0, enemyType.Length)]) as GameObject;
                        enemy.transform.position = pos;
                        enemy.transform.LookAt(lookPos, Vector3.up);

                        if (enemy.GetComponent<Enemy_Master>() != null)
                            enemy.GetComponent<Enemy_Master>().EventEnemyDie += Spawner_EventEnemyDie;

            m_numEnemies++;
        }

    }

    private void Spawner_EventEnemyDie()
    {
        m_numEnemies--;
        if (m_numEnemies < 0)
        {
            m_numEnemies = 0;
        }        
    }
}
