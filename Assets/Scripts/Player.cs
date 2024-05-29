using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Player : MonoBehaviour
{


    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _thursterSpeed;
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
        

    }


    void Update()
    {
        Movement();
        FireLaser();
       

    }

    private void Movement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _speed += _thursterSpeed; 
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) 
        {
        
            _speed -= _thursterSpeed;   
        }

        if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
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

    private void FireLaser()
    {
        if(_currentAmmo > 0)
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

    public void SpeedBoostPowerupActive()
    {
        _speedBoostActive = true;
        _speed += _speedBoostModifier;

        StartCoroutine("SpeedBoostCooldown");
    }

    IEnumerator SpeedBoostCooldown()
    {
        if (_speedBoostActive == true)
        {
            yield return new WaitForSeconds(_speedBoostCooldown);
            _speedBoostActive = false;
            _speed -= _speedBoostModifier;
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
       
        if(_shieldActive == true)
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

            if(_lives == 3)
            {
                _rightDamage.SetActive(false);
                _leftDamage.SetActive(false);
            }
            else if(_lives == 2) 
            {
                _rightDamage.SetActive(true);
            }
            else if (_lives == 1)
            {
                _leftDamage.SetActive(true);
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
        if  (_gameIsOver == false)
        {
            _gameIsOver = true;
           _uiManager.StartCoroutine("GameOverFlicker");
        }

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "EnemyLaser")
        {
            PlayerHealth();
            Destroy(other.gameObject);
        }
        else if(other.tag == "AmmoBox")
        {
            _currentAmmo = 15;
            _uiManager.ResetAmmoCount();
            Destroy(other.gameObject);
        }
    }


}

