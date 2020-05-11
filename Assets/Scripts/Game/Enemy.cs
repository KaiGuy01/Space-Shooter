using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    [SerializeField]
    private Player _player;

    [SerializeField]
    private int _scoreValue = 10;

    private Animator _anim;
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _clip;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private float _fireRate = 3f;
    private float _canFire = -1f;

    [SerializeField]
    private enum _movementType
    {
        downward,
        strafe,
        diagonal
    }

    [SerializeField]
    private _movementType _currentMovement;

    private bool switching = false;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }

        if (_anim == null)
        {
            Debug.LogError("Animator is NULL");
        }

        if (_audioSource == null)
        {
            Debug.LogError(this.gameObject + "Audio Source is NULL");
        }
    }

    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, this.transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            
            for (int i = 0; i < lasers.Length; i++)
            {
                if (gameObject.tag == "Enemy")
                {
                    lasers[i].AssignEnemyLaser(1);
                }
                if (gameObject.tag == "Enemy_Diagonal")
                {
                    lasers[i].AssignEnemyLaser(2);
                }
            }
        }
    }

    void CalculateMovement()
    {
        switch (_currentMovement)
        {
            case _movementType.downward: //Basic downward movement. Most common enemy.
                transform.Translate(Vector3.down * _speed * Time.deltaTime);

                if (transform.position.y <= -7.5f)
                {
                    float randomX = Random.Range(-8f, 8f);

                    transform.position = new Vector3(randomX, 7.5f, transform.position.z);
                }

                GameObject _player = GameObject.FindGameObjectWithTag("Player");

                break;
            case _movementType.strafe: //Side to side movement. Less common enemy.
                transform.Translate(Vector3.down * _speed / 3 * Time.deltaTime);

                if (switching == false)
                {
                    transform.Translate(Vector3.left * _speed * 3 * Time.deltaTime);
                }

                if (switching == true)
                {
                    transform.Translate(Vector3.right * _speed * 3 * Time.deltaTime);
                }

                if (transform.position.x < -7.5f)
                {
                    switching = true;
                }
                else if (transform.position.x > 7.5f)
                {
                    switching = false;
                }

                if (transform.position.y <= -7.5f)
                {
                    float randomX = Random.Range(-8f, 8f);

                    transform.position = new Vector3(randomX, 7.5f, transform.position.z);
                }
                break;

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {

            if (_player != null)
            {
                _player.Damage(1);
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
        }

        else if (other.tag == "Laser") 
        {
            Laser laser = other.GetComponent<Laser>();

            if (laser.GetLaser() == 0) //Only damages enemy if fired by player
            {
                Destroy(other.gameObject);
                _player.Score(_scoreValue);
                _anim.SetTrigger("OnEnemyDeath");
                _speed = 0;
            }
        }
    }

    void Death() //Handled by Animation Events
    {
        Destroy(this.gameObject);
    }

    void PlaySound() //Handled by Animation Events
    {
        AudioSource.PlayClipAtPoint(_clip, this.transform.position);
    }
}
