using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class EnemySpawner : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("The types of enemies to spawn.")]
    private Enemy[] m_EnemyTypes;
    #endregion

    #region OnEnable and OnDisable
    private void OnEnable()
    {
        GameManager.GameStartedEvent += StartSpawning;
        GameManager.GameEndedEvent += StopSpawning;
    }

    private void OnDisable()
    {
        GameManager.GameStartedEvent -= StartSpawning;
        GameManager.GameEndedEvent -= StopSpawning;
    }
    #endregion

    #region Spawning Methods
    private void StartSpawning()
    {
        StartCoroutine(SpawnLoop());
        StartCoroutine(SpawnLoop());
        StartCoroutine(SpawnLoop());
    }

    private void StopSpawning()
    {
        StopAllCoroutines();
    }

    private IEnumerator SpawnLoop()
    {
        float maximumTimeToNextSpawn = 10;
        while (true)
        {
            float timeToNextSpawn = Random.Range(0, maximumTimeToNextSpawn);
            if (maximumTimeToNextSpawn < 0.001f)
            {
                maximumTimeToNextSpawn = 0;
                yield return null;
            }
            else
                yield return new WaitForSeconds(timeToNextSpawn);

            maximumTimeToNextSpawn /= 1.05f;

            int enemyType = 0;
            if (Random.Range(0, 1.0f) > 0.75)
                enemyType = 1;
            Vector2 spawnPos = new Vector2(-10, Random.Range(-3.55f, -.75f));
            Instantiate(m_EnemyTypes[enemyType], spawnPos, Quaternion.identity);
        }
    }
    #endregion
}
