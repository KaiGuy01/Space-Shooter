using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _boost = 1.5f;

    [Header("Projectiles")]
    [SerializeField]
    private GameObject _laser;
    private int _ammoCapacity = 15;
    private int _currentAmmo = 15;

    [Header("Cooldown")]
    [SerializeField]
    private float _fireRate = .5f;
    private float _canFire = -1f;

    [Header("Player Status")]
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private GameObject _leftEngine, _rightEngine;
    private UIManager _uIManager;
    private int _shieldStrength = 3;
    public int score = 0;
    [SerializeField]
    private Animator _anim;

    [Header("Thrusters")]
    [SerializeField]
    private GameObject _thruster;
    [SerializeField]
    private float _thrusterCharge;
    [SerializeField]
    private int _depletionRate = 5;
    [SerializeField]
    private bool _thrusterAvailable = true;


    [Header("Powerups")]
    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField]
    private bool _isWaveShotActive = false;
    [SerializeField]
    private bool _isSpeedActive = false;
    [SerializeField]
    private bool _isShieldActive = false;
    [SerializeField]
    private float _speedMultiplier = 2;

    [SerializeField]
    private GameObject _tripleShot;
    [SerializeField]
    private GameObject _waveShot;
    [SerializeField]
    private GameObject _shieldVisualizer;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip _laserAudio;
    private AudioSource _audioSource;

    #endregion
    void Start()
    {
        //Take the current position, assign start position
        transform.position = new Vector3(0, 0, 0);

        _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_uIManager == null)
        {
            Debug.LogError(this.gameObject + "UI Manager is NULL");
        }

        if (_audioSource == null)
        {
            Debug.LogError(this.gameObject + "Audio Source is NULL");
        }
        else
        {
            _audioSource.clip = _laserAudio;
        }

        RestoreAmmo();
    }

    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            Shoot();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (_isSpeedActive == false && Input.GetKey(KeyCode.LeftShift) && _thrusterAvailable == true) //Enables thrusters if fully charged
        {
            transform.Translate(Vector3.right * horizontalInput * _speed * _boost * Time.deltaTime);
            transform.Translate(Vector3.up * verticalInput * _speed * _boost * Time.deltaTime);

            _thruster.SetActive(true);

            _thrusterCharge = _thrusterCharge - _depletionRate * Time.deltaTime;
            _uIManager.UpdateThruster(_thrusterCharge);
        }
        else
        {
            transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
            transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);
            _thruster.SetActive(false);
            
            if (_thrusterCharge < 100) //Recharges thrusters while shift is not being pressed
            {
                _thrusterCharge = _thrusterCharge + _depletionRate * Time.deltaTime;
                _uIManager.UpdateThruster(_thrusterCharge);
            }
        }

        if (_thrusterCharge < 1)
        {
            StartCoroutine("ThrusterCooldown");
        }

        if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, transform.position.z);
        }
        else if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, transform.position.z);
        }

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -7.5f)
        {
            transform.position = new Vector3(transform.position.x, 0f, 0);
        }


    }

    IEnumerator ThrusterCooldown()
    {
        _thrusterAvailable = false;
        yield return new WaitForSeconds(_depletionRate);
        _thrusterAvailable = true;
    }

    void Shoot()
    {
        _canFire = Time.time + _fireRate;

        if (_currentAmmo > 0)
        {
            if (_isTripleShotActive == true)
            {
                Instantiate(_tripleShot, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                _currentAmmo--;
                _uIManager.UpdateAmmo(_currentAmmo, _ammoCapacity);
            }
            else if (_isWaveShotActive == true)
            {
                Instantiate(_waveShot, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                _currentAmmo--;
                _uIManager.UpdateAmmo(_currentAmmo, _ammoCapacity);
            }
            else
            {
                Instantiate(_laser, new Vector3(transform.position.x, transform.position.y + 1.75f, transform.position.z), Quaternion.identity);
                _currentAmmo--;
                _uIManager.UpdateAmmo(_currentAmmo, _ammoCapacity);
            }

            _audioSource.Play();
        }


    }

    public void Damage(int damageAmount)
    {
        _anim.SetTrigger("Damage");

        if (_isShieldActive == true)
        {
            _shieldStrength--;
            _uIManager.UpdateShield(_shieldStrength);

            switch (_shieldStrength)
            {
                case 2:
                    _shieldVisualizer.GetComponent<SpriteRenderer>().color = Color.yellow;
                    break;
                case 1:
                    _shieldVisualizer.GetComponent<SpriteRenderer>().color = Color.red;
                    break;
                case 0:
                    _isShieldActive = false;
                    _shieldVisualizer.SetActive(false);
                    _shieldStrength = 3;
                    break;
            }
        }
        else
        {
            _lives -= damageAmount;

            _uIManager.UpdateLives(_lives);

            switch (_lives)
            {
                case 0:
                    score = 0;
                    Destroy(this.gameObject);
                    break;
                case 1:
                    _leftEngine.SetActive(true);
                    break;
                case 2:
                    _rightEngine.SetActive(true);
                    break;
            }

        }
    }

    #region Powerups

    public void TripleShotActive()
    {
        if (_isWaveShotActive == false)
        {
            _isTripleShotActive = true;
            StartCoroutine("DisableTripleShot");
        }
    }

    public void WaveShotActive()
    {
        if (_isTripleShotActive == false)
        {
            _isWaveShotActive = true;
            StartCoroutine("DisableWaveShot");
        }
    }

    public void SpeedActive()
    {
        _isSpeedActive = true;
        _speed = _speed * _speedMultiplier;
        StartCoroutine("DisableSpeed");
    }

    public void ShieldActive()
    {
        if (_isShieldActive == true)
        {
            _shieldStrength = 3;
            _shieldVisualizer.GetComponent<SpriteRenderer>().color = new Color(0, 255, 225);
            _uIManager.UpdateShield(_shieldStrength);
        }
        else
        {
            _isShieldActive = true;
            _shieldVisualizer.SetActive(true);
            _shieldVisualizer.GetComponent<SpriteRenderer>().color = new Color(0, 255, 225);
            _uIManager.UpdateShield(_shieldStrength);
        }
    }

    public void RestoreAmmo()
    {
        _currentAmmo = _ammoCapacity;
        _uIManager.UpdateAmmo(_currentAmmo, _ammoCapacity);
    }

    public void RestoreHealth()
    {
        switch (_lives)
        {
            case 1:
                _lives++;
                _leftEngine.SetActive(false);
                _uIManager.UpdateLives(_lives);
                break;
            case 2:
                _lives++;
                _rightEngine.SetActive(false);
                _uIManager.UpdateLives(_lives);
                break;
            case 3:
                Debug.Log("Health is already full.");
                break;
        }
    }

    IEnumerator DisableTripleShot()
    {
        yield return new WaitForSeconds(5);
        _isTripleShotActive = false;
    }

    IEnumerator DisableSpeed()
    {
        yield return new WaitForSeconds(5);
        _speed = _speed / _speedMultiplier;
        _isSpeedActive = false;
    }

    IEnumerator DisableWaveShot()
    {
        yield return new WaitForSeconds(5);
        _isWaveShotActive = false;
    }
    #endregion

    #region Score
    public void Score(int points)
    {
        score += points;
        _uIManager.UpdateScore(score);
    }
    #endregion
}
