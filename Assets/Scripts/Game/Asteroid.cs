using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private float _speed = 5f;

    private Animator _anim;
    [SerializeField]
    private AudioClip _clip;

    [SerializeField]
    private SpawnManager _spawnManager;

    void Start()
    {
        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.LogError(this.gameObject + "Animator is NULL");
        }

        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError(this.gameObject + "Spawn Manager is NULL");
        }

        if (_clip == null)
        {
            Debug.LogError(this.gameObject + "Audio Clip is NULL");
        }
    }

    #endregion
    void Update()
    {
        transform.Rotate(Vector3.forward * _speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            _anim.SetTrigger("OnAsteroidDeath");
        }
    }

    void InitializeGame() //Handled by Animation Events
    {
        _spawnManager.StartSpawning();
        Destroy(this.gameObject);
    }

    void PlaySound() //Handled by Animation Events
    {
        AudioSource.PlayClipAtPoint(_clip, this.transform.position);
    }
}
