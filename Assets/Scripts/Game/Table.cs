using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public List<GameObject> powerups;
    [Header("Spawn Values")]
    public int[] powerup = 
    { 
        26, //Ammo drop
        24, //Health drop
        20, //Shield drop
        15, //Speed drop
        10, //Triple Shot drop
        5 //Wave Shot drop
    };

    public int pTotal;
    public int randomPowerup;

    public List<GameObject> enemies;
    [Header("Spawn Values")]
    public int[] enemy =
    {
        75, //Downward movement
        25 //Strafe movement
    };

    public int eTotal;
    public int randomEnemy;

    private SpawnManager _spawnManager;

    private void Start()
    {
        _spawnManager = GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.Log(gameObject.name + " Spawn Manager reference is NULL");
        }

        Debug.Log("Calculating Tables");

        foreach (var item in powerup)
        {
            pTotal += item;
        }

        foreach (var entity in enemy)
        {
            eTotal += entity;
        }

    }

    public void CalculatePowerup()
    {

        randomPowerup = Random.Range(0, pTotal);

        for (int i = 0; i < powerup.Length; i++)
        {
            if (randomPowerup <= powerup[i])
            {
                GameObject _randomPowerup = powerups[i];
                Debug.Log(_randomPowerup.name);
                _spawnManager.StartCoroutine("PowerupSpawn", _randomPowerup);
                Debug.Log("Succesfully spawned powerup");
                return;
            }
            else
            {
                randomPowerup -= powerup[i];
            }
        }
    }

    public void CalculateEnemy()
    {

        randomEnemy = Random.Range(0, pTotal);

        for (int i = 0; i < enemy.Length; i++)
        {
            if (randomEnemy <= enemy[i])
            {
                GameObject _randomEnemy = enemies[i];
                Debug.Log(_randomEnemy.name);
                _spawnManager.StartCoroutine("EnemySpawn", _randomEnemy);
                Debug.Log("Succesfully spawned enemy");
                return;
            }
            else
            {
                randomEnemy -= enemy[i];
            }
        }
    }
}
