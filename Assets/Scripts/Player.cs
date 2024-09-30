using JetBrains.Annotations;
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
    private float _thursterSpeedModifier;
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
    [SerializeField]
    private AudioSource _collectionSound;

    private bool _gameIsOver;

    private CameraEffects _camera;
    [SerializeField]
    private bool _thrusterActive = false;

    void Start()
    {

        transform.position = Vector3.zero;
        _laserOffset = 1.04f;

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _currentSpeed = _baseSpeed;
        _thrusterEngageCoroutine = null;
        _thrusterChargeCoroutine = null;
        _thrusterCharge = _maxThrusters;
        _camera = GameObject.Find("Main Camera").GetComponent<CameraEffects>();
    }





    void Update()
    {



        _laserOffset = 1.04f;

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        _camera = GameObject.Find("Main Camera").GetComponent<CameraEffects>();

        Movement();
        PlayerBounds();
        ThusterActication();
            _uiManager.ThrusterMeter(_thrusterCharge);
            FireLaser();
            PlayerHealthVisual();


        
    }


        private void Movement()
        {
            //Consider changing to Input Manager(Get.Axis) 
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _currentSpeed * Time.deltaTime);



        }
        private void PlayerBounds()
        {
            if (transform.position.x > 11.3f)
            {
                transform.position = new Vector3(-11.3f, transform.position.y, 0);
            }

            if (transform.position.x < -11.3f)
            {
                transform.position = new Vector3(11.3f, transform.position.y, 0);
            }
            //add left hand wrapping 

            if (transform.position.y > -1.2f)
            {
                transform.position = new Vector3(transform.position.x, -1.2f, 0);
            }

            if (transform.position.y < -5.4f)
            {
                transform.position = new Vector3(transform.position.x, -5.4f, 0);
            }

        }


//my attempt at fixing the known thruster bug. 
//if the thursters hit zero and then i let go and let it charge to 1 and the renage the thurtsres its glitches changing values back and forth.

    /*       IEnumerator ThrustersEngage()
           {
               //if the player presses shift 
               //increase the speed of the player
               //set thrusters active to true 
               //while the thrusters are active 
               //decrease the thruster charge 
               //if the thuruster charge reaches 0 
               // set speed back to normal
               //stop decreasing thruster charge 
               //set thruster active to false 


               _currentSpeed = _currentSpeed + _speedBoostModifier;


                   _currentSpeed = _currentSpeed - _speedBoostModifier;
           while (true)
           {
               _thrusterCharge--;
               _thrusterActive = true;
               if (_thrusterCharge <= 0 || Input.GetKeyUp(KeyCode.LeftShift))
               {
                   _thrusterActive = false;
                   break;
               }
               yield return new WaitForSeconds(1);


           }


               yield return new WaitForSeconds(1);

           }
           IEnumerator ThrustersDisengage()
           {
               while (true)
               {
               _thrusterActive = false;
                   _thrusterCharge++;
                   if (_thrusterCharge >= _maxThrusters)
                   {
                       break;
                   }
                   yield return new WaitForSeconds(1);
               }
               yield break;
           }*/

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

        if (_thrusterCharge <= 0)
        {
            _currentSpeed = _baseSpeed;
        }

    }

    IEnumerator ThrustersEngage()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && _thrusterCharge > 0)
        {
            _currentSpeed = _currentSpeed + _thursterSpeedModifier;
        }

        while (Input.GetKey(KeyCode.LeftShift) && _thrusterCharge > 0)
        {
            _thrusterCharge--;

            yield return new WaitForSeconds(1);
        }
        _thrusterEngageCoroutine = null;

    }

    IEnumerator ThrusterCharge()
    {
        if (Input.GetKeyUp(KeyCode.LeftShift) && _currentSpeed > _baseSpeed)
        {
            _currentSpeed = _currentSpeed - _thursterSpeedModifier;
        }

        while (_thrusterCharge < _maxThrusters)
        {
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


        //Divide into 2 seperate dmage and update dmage visual 
        public void Damage()
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
            }
        }
        private void PlayerHealthVisual()
        {


            //this never happens - I belive this is checked when we gain lifr form 2 tro 3 
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
                    Damage();
                    _camera.StartCoroutine("CameraShake");
                    _explosionSound.Play();
                    Destroy(other.gameObject);
                    break;
                case "AmmoBox":
                    _currentAmmo = 15;
                    _uiManager.ResetAmmoCount();
                    _collectionSound.Play();
                    Destroy(other.gameObject);
                    break;
                case "RepairKit":
                    //changed to 1 from 2 
                    if (_lives < 3)
                    {
                        _lives = _lives + 1;
                        _uiManager.UpdateLifeDisplay(_lives);
                    }
                    _collectionSound.Play();
                    Destroy(other.gameObject);
                    break;
            }
        }
    }







