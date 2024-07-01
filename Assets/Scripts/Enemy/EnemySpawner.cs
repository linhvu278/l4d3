using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour, IInteractable
{
    int enemyNumber;
    [SerializeField] int maxEnemyNumber;
    //public float spawnInterval;
    [SerializeField] GameObject enemyObject;
    GameObject[] enemies;
    bool canSpawn;

    Vector3 boundsMin, boundsMax;
    [SerializeField] BoxCollider boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        //boxCollider = GetComponent<BoxCollider>();
        boundsMin = boxCollider.bounds.min;
        boundsMax = boxCollider.bounds.max;
        /*
        int i = 0;
        while (i < maxEnemyNumber)
        {
            Instantiate(enemyObject, new Vector3(Random.Range(boundsMin.x, boundsMax.x), 1, Random.Range(boundsMin.z, boundsMax.z)), Quaternion.identity);
            i++;
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        //check if current number of enemies are lower than max amount
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemyNumber = enemies.Length;
        //Debug.Log(enemyNumber);
        /*
        if (enemyNumber == 0)
        {
            int i = 0;
            while (i < maxEnemyNumber)
            {
                Instantiate(enemyObject, new Vector3(Random.Range(boundsMin.x, boundsMax.x), 1, Random.Range(boundsMin.z, boundsMax.z)), Quaternion.identity);
                //StartCoroutine(SpawnEnemy(enemyObject, 1f));
                i++;
            }
        }
        */
        if (enemyNumber < maxEnemyNumber)
            canSpawn = true;
        else canSpawn = false;
        //Debug.Log(enemyNumber);
    }

    public void SpawnEnemy()//(GameObject enemy, float interval)
    {
        if (canSpawn)
        {
            //yield return new WaitForSeconds(interval);
            Instantiate(enemyObject, new Vector3(Random.Range(boundsMin.x, boundsMax.x), 1, Random.Range(boundsMin.z, boundsMax.z)), Quaternion.identity);
        }
    }

    public void OnInteractStart() { SpawnEnemy(); }
    public void OnInteractEnd(){}
    public string InteractText() => "Press E to spawn an enemy";
}