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
    private UnityEngine.Vector2 _spawnDelayP = new UnityEngine.Vector2(2, 5);

    [SerializeField]
    private UnityEngine.Vector2 _spawnDelayE = new UnityEngine.Vector2(5, 7.5f);

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

        if (_player == null)
        {
            Debug.LogError(gameObject.name + " Player reference is NULL");
        }
    }

    public void StartSpawning()
    {
        _table.CalculateEnemy();
        _table.CalculatePowerup();
    }

    IEnumerator EnemySpawn(GameObject _randomEnemy)
    {
        GameObject _player = GameObject.FindGameObjectWithTag("Player");
        if (_player != null)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            float _randomX = Random.Range(-8, 8);
            yield return null;
            GameObject newEnemy = Instantiate(_randomEnemy, new UnityEngine.Vector3(_randomX, 7f, transform.position.z), UnityEngine.Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(Random.Range(_spawnDelayE.x, _spawnDelayE.y));
            _table.CalculateEnemy();
        }
    }

    IEnumerator PowerupSpawn(GameObject _randomPowerup)
    {
        GameObject _player = GameObject.FindGameObjectWithTag("Player");
        if (_player != null)
        {
            Debug.Log("Powerup spawn initialized");
            //int _randomPowerup = Random.Range(0, 6);
            float _randomX = Random.Range(-8, 8);
            yield return null;
            Instantiate(_randomPowerup, new UnityEngine.Vector3(_randomX, 7f, transform.position.z), UnityEngine.Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(_spawnDelayP.x, _spawnDelayP.y));
            _table.CalculatePowerup();
        }
    }
}
