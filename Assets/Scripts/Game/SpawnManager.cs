using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private GameObject _enemy;

    [SerializeField]
    private UnityEngine.Vector2 _spawnDelay = new UnityEngine.Vector2(2, 5);

    [SerializeField]
    private GameObject _enemyContainer;

    private GameObject _player;

    private Table _table;

    #endregion

    public void Start()
    {
        _table = GetComponent<Table>();

        if (_table == null)
        {
            Debug.LogError(gameObject.name + " Table reference is NULL");
        }
    }

    public void StartSpawning()
    {
        StartCoroutine("EnemySpawn");
        _table.CalculateLoot();
    }

    IEnumerator EnemySpawn()
    {
        _player = GameObject.FindGameObjectWithTag("Player");


        while (_player != null)
        {
            yield return null;
            float _randomX = Random.Range(-8, 8);
            GameObject newEnemy = Instantiate(_enemy, new UnityEngine.Vector3(_randomX, 7f, transform.position.z), UnityEngine.Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(Random.Range(_spawnDelay.x, _spawnDelay.y));
        }
    }

    IEnumerator PowerupSpawn(GameObject _randomPowerup)
    {
        Debug.Log("Powerup spawn initialized");
        //int _randomPowerup = Random.Range(0, 6);
        float _randomX = Random.Range(-8, 8);
        yield return null;
        Instantiate(_randomPowerup, new UnityEngine.Vector3(_randomX, 7f, transform.position.z), UnityEngine.Quaternion.identity);
        yield return new WaitForSeconds(Random.Range(_spawnDelay.x, _spawnDelay.y));
        _table.CalculateLoot();
    }
}
