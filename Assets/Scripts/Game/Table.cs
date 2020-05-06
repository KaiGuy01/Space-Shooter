using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public List<GameObject> powerups;
    public int[] table = { 
        26, //Ammo drop
        24, //Health drop
        20, //Shield drop
        15, //Speed drop
        10, //Triple Shot drop
        5 //Wave Shot drop
    };

    public int total;
    public int randomNumber;

    private SpawnManager _spawnManager;

    private void Start()
    {
        _spawnManager = GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.Log(gameObject.name + " Spawn Manager reference is NULL");
        }

        Debug.Log("Calculating Loot Table");

        foreach (var item in table)
        {
            total += item;
        }

    }

    public void CalculateLoot()
    {

        randomNumber = Random.Range(0, total);

        for (int i = 0; i < table.Length; i++)
        {
            if (randomNumber <= table[i])
            {
                GameObject _randomPowerup = powerups[i];
                Debug.Log(_randomPowerup.name);
                _spawnManager.StartCoroutine("PowerupSpawn", _randomPowerup);
                Debug.Log("Succesfully spawned powerup");
                return;
            }
            else
            {
                randomNumber -= table[i];
            }
        }
    }
}
