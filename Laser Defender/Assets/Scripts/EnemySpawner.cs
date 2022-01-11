using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] int startingWave = 0;
    [SerializeField] bool looping = false;

    //Coroutines need Ienumerator as there top, and allow you to do lines of code seperately/waiting until another line has finished or enough time has passed
    //rather than all being executed at the same time. There are three being used as nested coroutines in this file.
    
    // Start is called before the first frame update
    IEnumerator Start()
    {
        do
        {
            //This makes it wait until all waves have spawned(so it starts them again if looping is true)
            yield return StartCoroutine(SpawnAllWaves());
        } while (looping);
    }

    private IEnumerator SpawnAllWaves()
    {
        for (int waveIndex = startingWave; waveIndex < waveConfigs.Count; waveIndex++)
        {
            var currentWave = waveConfigs[waveIndex];
            //This makes it wait for a wave to completely spawn before it starts another
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
        }
    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        for (int enemyCount = 0; enemyCount < waveConfig.GetNumberOfEnemies(); enemyCount++)
        {
            var newEnemy = Instantiate(waveConfig.GetEnemyPrefab(), waveConfig.GetWaypoints()[0].position, Quaternion.identity);
            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
            //This makes it wait a specified amount of time between each spawn of an enemy
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
        }
        

    }
    
}
