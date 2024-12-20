using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    private Enemy _enemyScript;
    [SerializeField]
    private GameObject _enemy;
    [SerializeField]
    private GameObject _comoEnemy;
    [SerializeField]
    private GameObject _shieldEnemy;

    private GameObject _enemyContainer;
    private bool _respawnBaseEnemy = true;
    private bool _respawnComoEnemy = false;
    private bool _respawnShieldEnemy = false; 

    private float _randomXSpawn;
    private float _ySpawn;

    private bool _playerAlive = true;
    [SerializeField]
    private GameObject _player;


    private bool _respawnPowerup = true;

    // [0] = TripleShot, [1] = Speed Boost, [2] = Shield [3] = Rapid Fire
    [SerializeField]
    private GameObject[] _powerUpID;
    private int _powerUpIDSelector;

    [SerializeField]
    private AudioSource _powerupSound;

    [SerializeField]
    private bool _gameStart;

    [SerializeField]
    private GameObject _asteroidObject;

    [SerializeField]
    private GameObject _enemyLaser;
    [SerializeField]
    private AudioSource _laserSound;

    [SerializeField]
    private GameObject _ammoBox;
    private bool _ammoSpawned;

    [SerializeField]
    private GameObject _repairKit;
    private bool _repairKitSpawned;

    private bool _rarePowerUpSpawned;
    [SerializeField]
    private int _enemyRespawnCooldown; 
    private int _wave = 0;

    [SerializeField]
    private bool _debuffSpawned;
    [SerializeField]
    private GameObject _shockDebuff;

    [SerializeField]
    private float _wave0End;
    [SerializeField]
    private float _wave1Start;
    [SerializeField]
    private float _wave1End;
    [SerializeField]
    private float _wave2Start;
    [SerializeField]
    private float _wave2End;
    [SerializeField]
    private float _wave3Start;
    [SerializeField]
    private float _wave3End;
    [SerializeField] 
    private float _wave4Start;

    [SerializeField]
    private int _powerUpMinSpawnTimer;
    [SerializeField]
    private int _powerUpMaxSpawnTimer;

    [SerializeField]
    private int _rarePowerUpSpawnCooldown;
    [SerializeField]
    private int _rarePowerUpSpawnCooldownChance1to4;

    [SerializeField]
    private int _ammoRespawn;

    [SerializeField]
    private int _repairKitRespawn;

    [SerializeField]
    private int _debuffRespawn;


    void Start()
    {
        _enemyScript = GetComponent<Enemy>();
        _enemyContainer = GameObject.Find("Enemy Container");
        _ySpawn = 4;
       
    }

    private void Update()
    {
        StartSpawning();
        Debug.Log(_wave);
       
    }


    public void StartSpawning()
    {
        //prevent players from starting in wave if they sit in the destroy asteroid screen for longer than 10 minutes 
        if(_asteroidObject != null && Time.timeSinceLevelLoad > 1000.00f) { SceneManager.LoadScene(1); }

        if (_asteroidObject == null)
        {
            StartCoroutine(SpawnBaseEnemy());
            if (_wave >= 1) { StartCoroutine(SpawnShieldEnemy()); }
            if (_wave >= 2) {StartCoroutine(SpawnComoEnemy()); }
            StartCoroutine(PowerUpSpawn());
            StartCoroutine(RarePowerUpSpawn());
            StartCoroutine(AmmoSpawn());
            StartCoroutine(RepairKitSpawn());
            WaveCounter();
            StartCoroutine(DebuffSpawn());


        }

    }
   
    

    private void WaveCounter()
    {
        float gameTime = Time.timeSinceLevelLoad;
        Debug.Log(_wave);
        Debug.Log(gameTime);
       
        if (gameTime <= _wave0End)
        {
            _wave = 0;
           

        }
        else if (gameTime > _wave1Start && gameTime < _wave1End)
        {
            _wave = 1;
            

        }
        else if (gameTime > _wave2Start && gameTime < _wave2End)
        {
            _wave = 2;
        }
        else if (gameTime > _wave3Start && gameTime < _wave3End)
        {
            _wave = 3; 
        }
        else if(gameTime > _wave4Start ){ _wave = 4; }
      

    }

    IEnumerator SpawnBaseEnemy()
    {

        
        if (_respawnBaseEnemy == true && _playerAlive == true)
        {
            
            _randomXSpawn = Random.Range(-10, 10);
            GameObject newEnemy = Instantiate(_enemy, new Vector3(_randomXSpawn, _ySpawn, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            GameObject enemyLaser = Instantiate(_enemyLaser, new Vector3(newEnemy.transform.position.x, newEnemy.transform.position.y, 0), Quaternion.identity);
            _respawnBaseEnemy = false;
            _laserSound.Play();
            yield return new WaitForSeconds(_enemyRespawnCooldown - _wave);
            _respawnBaseEnemy = true;
            PlayerDeath();
        }

    }

    IEnumerator SpawnComoEnemy()
    {
        if ( _respawnComoEnemy == false && _playerAlive == true)
        {
            _randomXSpawn = Random.Range(-10, 10);
            GameObject newEnemy = Instantiate(_comoEnemy, new Vector3(_randomXSpawn, _ySpawn, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            _respawnComoEnemy = true;
            yield return new WaitForSeconds(_enemyRespawnCooldown - _wave);
            _respawnComoEnemy = false;
            PlayerDeath();
        }

    }

    IEnumerator SpawnShieldEnemy()
    {
        if (_respawnShieldEnemy == false && _playerAlive == true)
        {
            _randomXSpawn = Random.Range(-10, 10);
            GameObject newEnemy = Instantiate(_shieldEnemy, new Vector3(_randomXSpawn, _ySpawn, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            _respawnShieldEnemy = true;
            yield return new WaitForSeconds(_enemyRespawnCooldown - _wave);
            _respawnShieldEnemy = false;
            PlayerDeath();
        }

    }

    IEnumerator PowerUpSpawn()
    {
        yield return new WaitForSeconds(2);
        if (_respawnPowerup == true)
        {
            _powerUpIDSelector = Random.Range(0, 3);
            GameObject newPowerUp = Instantiate(_powerUpID[_powerUpIDSelector], new Vector3(Random.Range(-10, 10), 4, 0), Quaternion.identity);
            _respawnPowerup = false;
            yield return new WaitForSeconds(Random.Range(_powerUpMinSpawnTimer, _powerUpMaxSpawnTimer));
            _respawnPowerup = true;
        }

    }

  
    IEnumerator RarePowerUpSpawn()
    {
     
        yield return new WaitForSeconds(_rarePowerUpSpawnCooldown);
        if (_rarePowerUpSpawned == false)
        {

            int spawnChance = Random.Range(0, 4);
            if (spawnChance >= _rarePowerUpSpawnCooldownChance1to4)
            {
                GameObject rarePowerUp = Instantiate(_powerUpID[3], new Vector3(Random.Range(-10, 10), 4, 0), Quaternion.identity);
                _rarePowerUpSpawned = true;
                yield return new WaitForSeconds(_rarePowerUpSpawnCooldown);
                _rarePowerUpSpawned = false;
            }
        }

    }

    
    IEnumerator AmmoSpawn()
    {
        yield return new WaitForSeconds(5);
        if (_ammoSpawned == false)
        {

            GameObject newAmmoBox = Instantiate(_ammoBox, new Vector3(Random.Range(-10, 10), 4, 0), Quaternion.identity);
            _ammoSpawned = true;
            yield return new WaitForSeconds(_ammoRespawn);
            _ammoSpawned = false;
        }

    }

    
    IEnumerator RepairKitSpawn()
    {
        yield return new WaitForSeconds(5);
        if (_repairKitSpawned == false)
        {

            GameObject newRepairKit = Instantiate(_repairKit, new Vector3(Random.Range(-10, 10), 4, 0), Quaternion.identity);
            _repairKitSpawned = true;
            yield return new WaitForSeconds(_repairKitRespawn);
            _repairKitSpawned = false;
        }

    }
    
    IEnumerator DebuffSpawn()
    {
        if (_debuffSpawned == false)
        {
            GameObject newDebuff = Instantiate(_shockDebuff, new Vector3(Random.Range(-10,10),4, 0), Quaternion.identity);
            _debuffSpawned = true;
            yield return new WaitForSeconds(_debuffRespawn);
            _debuffSpawned = false; 
        }
        yield return new WaitForSeconds(_debuffRespawn); 
    }


    private void PlayerDeath()
    {
        if (_player == null)
        {
            _playerAlive = false;
            
        }
    }


    public void PowerUpSound()
    {
        _powerupSound.Play();
    }


}

