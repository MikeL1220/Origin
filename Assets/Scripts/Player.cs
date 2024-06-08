using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Player : MonoBehaviour
{


    [SerializeField]
    private float _currentSpeed;
    [SerializeField]
    private float _baseSpeed;
    [SerializeField]
    private float _thursterSpeed;
    [SerializeField]
    private int _maxThrusters;
    [SerializeField]
    private int _thrusterCharge;
    private Coroutine _thrusterEngageCoroutine;
    private Coroutine _thrusterChargeCoroutine;
    [SerializeField]
    private float _speedBoostModifier;

    [SerializeField]
    private GameObject _laser;
    private float _laserOffset;
    private float _canFire;
    [SerializeField]
    private float _fireRate;
    [SerializeField]
    private int _maxAmmo;
    [SerializeField]
    private int _currentAmmo;

    [SerializeField]
    private int _lives;

    private int _powerUpID;

    [SerializeField]
    private int _tripleShotCooldown;
    [SerializeField]
    private GameObject _tripleShot;
    private bool _tripleShotActive;


    [SerializeField]
    private int _speedBoostCooldown;
    private bool _speedBoostActive;

    [SerializeField]
    private GameObject _shield;
    private bool _shieldActive;
    [SerializeField]
    private int _shieldHealth;

    private UIManager _uiManager;

    [SerializeField]
    private GameObject _rightDamage;
    [SerializeField]
    private GameObject _leftDamage;

    [SerializeField]
    private AudioSource _laserSound;
    [SerializeField]
    private AudioSource _explosionSound;

    private bool _gameIsOver;



    void Start()
    {
        transform.position = Vector3.zero;
        _laserOffset = 1.04f;

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _currentSpeed = _baseSpeed;
        _thrusterEngageCoroutine = null;
        _thrusterChargeCoroutine = null;


    }


    void Update()
    {
        Movement();
        _uiManager.ThrusterMeter(_thrusterCharge);
        FireLaser();
        ThusterActication();




    }

    private void Movement()
    {

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * _currentSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * _currentSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * _currentSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * _currentSpeed * Time.deltaTime);
        }

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }

        if (transform.position.y > -1.2f)
        {
            transform.position = new Vector3(transform.position.x, -1.2f, 0);
        }

        if (transform.position.y < -5.4f)
        {
            transform.position = new Vector3(transform.position.x, -5.4f, 0);
        }

    }
    private void ThusterActication()
    {
        if (Input.GetKey(KeyCode.LeftShift) && _thrusterEngageCoroutine == null)
        {
            _thrusterEngageCoroutine = StartCoroutine(ThrustersEngage());

        }

        if (Input.GetKeyUp(KeyCode.LeftShift) && _thrusterChargeCoroutine == null) 
        {
            _thrusterChargeCoroutine = StartCoroutine(ThrusterCharge());
        
        }

    }

    IEnumerator ThrustersEngage()
    {

        while (Input.GetKey(KeyCode.LeftShift))
        { 
            _currentSpeed = 5;
            _thrusterCharge--;

            yield return new WaitForSeconds(1);
        }
        _thrusterEngageCoroutine = null; 
        
    }

    IEnumerator ThrusterCharge()
    {


        while (true)
        {
            _currentSpeed = 3;
            _thrusterCharge++;
            if (_thrusterCharge > _maxThrusters || Input.GetKey(KeyCode.LeftShift))
            {
                _thrusterChargeCoroutine = null;
                break;
            }

            yield return new WaitForSeconds(1);

        }
        _thrusterChargeCoroutine = null; 

        
    }


        private void FireLaser()
        {
            if (_currentAmmo > 0)
            {
                if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
                {
                    _canFire = Time.time + _fireRate;


                    if (_tripleShotActive == true)
                    {
                        Instantiate(_tripleShot, new Vector3(transform.position.x, transform.position.y + _laserOffset, 0), Quaternion.identity);

                    }
                    else
                    {
                        Instantiate(_laser, new Vector3(transform.position.x, transform.position.y + _laserOffset, 0), Quaternion.identity);

                    }
                    _laserSound.Play();
                    _currentAmmo--;
                    _uiManager.ReduceAmmoCount();


                }

            }




        }
        public void TripleShotPowerupActive()
        {
            _tripleShotActive = true;
            StartCoroutine("TripleShotCooldown");
        }

        IEnumerator TripleShotCooldown()
        {
            if (_tripleShotActive == true)
            {
                yield return new WaitForSeconds(_tripleShotCooldown);
                _tripleShotActive = false;
            }
        }

        public void RapidFirePowerupActive()
        {
            StartCoroutine(RapidFireLasers());
        }

        IEnumerator RapidFireLasers()
        {

            for (int i = 0; i < 100; i++)
            {
                Instantiate(_laser, new Vector3(transform.position.x, transform.position.y + _laserOffset, 0), Quaternion.identity);
                yield return new WaitForSeconds(.1f);
                _laserSound.Play();
            }



        }

        public void SpeedBoostPowerupActive()
        {
            _speedBoostActive = true;
            _currentSpeed += _speedBoostModifier;

            StartCoroutine("SpeedBoostCooldown");
        }

        IEnumerator SpeedBoostCooldown()
        {
            if (_speedBoostActive == true)
            {
                yield return new WaitForSeconds(_speedBoostCooldown);
                _speedBoostActive = false;
                _currentSpeed -= _speedBoostModifier;
            }
        }

        public void ShieldPowerupActive(bool shieldActive)
        {
            _shieldActive = shieldActive;
            _shieldActive = true;
            _shield.SetActive(true);
            _shieldHealth = 3;
            _uiManager.ShieldHealthVisualizer(_shieldHealth);
        }

        public void PlayerHealth()
        {

            if (_shieldActive == true)
            {
                _shieldHealth--;
                _uiManager.ShieldHealthVisualizer(_shieldHealth);
                if (_shieldHealth == 0)
                {
                    _shield.SetActive(false);
                    _shieldActive = false;
                }

            }
            else
            {
                _lives--;
                _uiManager.UpdateLifeDisplay(_lives);
                _explosionSound.Play();

                if (_lives == 3)
                {
                    _rightDamage.SetActive(false);
                    _leftDamage.SetActive(false);
                }
                else if (_lives == 2)
                {
                    _rightDamage.SetActive(true);
                    _leftDamage.SetActive(false);
                }
                else if (_lives == 1)
                {
                    _leftDamage.SetActive(true);
                    _rightDamage.SetActive(true);
                }

            }
            if (_lives == 0)
            {

                GameOver();
                Destroy(this.gameObject);

            }

        }

        private void GameOver()
        {
            if (_gameIsOver == false)
            {
                _gameIsOver = true;
                _uiManager.StartCoroutine("GameOverFlicker");
            }

        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            switch ((other.tag))
            {
                case "EnemyLaser":
                    PlayerHealth();
                    Destroy(other.gameObject);
                    break;
                case "AmmoBox":
                    _currentAmmo = 15;
                    _uiManager.ResetAmmoCount();
                    Destroy(other.gameObject);
                    break;
                case "RepairKit":
                    _lives = _lives + 2;
                    PlayerHealth();
                    Destroy(other.gameObject);
                    break;
            }

        }
    }





