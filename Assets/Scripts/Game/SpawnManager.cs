using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private GameObject _enemy;

    [SerializeField]
    private GameObject[] _powerups;

    [SerializeField]
    private float _spawnDelay = 5f;

    [SerializeField]
    private GameObject _enemyContainer;

    private GameObject _player;

    #endregion

    public void StartSpawning()
    {
        StartCoroutine("EnemySpawn");
        StartCoroutine("PowerupSpawn");
    }

    IEnumerator EnemySpawn()
    {
        _player = GameObject.FindGameObjectWithTag("Player");


        while (_player != null)
        {
            yield return null;
            float _randomX = Random.Range(-8, 8);
            GameObject newEnemy = Instantiate(_enemy, new Vector3(_randomX, 7f, transform.position.z), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_spawnDelay);
        }
    }

    IEnumerator PowerupSpawn()
    {

        while (_player != null)
        {
            int _randomPowerup = Random.Range(0, 3);
            float _randomX = Random.Range(-8, 8);
            yield return null;
            Instantiate(_powerups[_randomPowerup], new Vector3(_randomX, 7f, transform.position.z), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(5f, 10f));
        }
    }
}
