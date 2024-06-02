using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private Enemy _enemyScript; 
    [SerializeField]
    private GameObject _enemy;

    private GameObject _enemyContainer;
    private bool _respawnEnemy = true;

    private float _randomXSpawn;
    private float _ySpawn;

    private bool _playerAlive = true;
    [SerializeField]
    private GameObject _player;

  
    private bool _respawnPowerup = true;

    // [0] = TripleShot, [1] = Speed Boost, [2] = Shield
    [SerializeField]
    private GameObject[] _powerUpID;
    private int _powerUpIDSelector;

    [SerializeField]
    private AudioSource _powerupSound;

    [SerializeField]
    private bool _gameStart;

    [SerializeField]
    private Asteroid _asteroid;

    [SerializeField]
    private GameObject _enemyLaser;

    [SerializeField]
    private GameObject _ammoBox;
    private bool _ammoSpawned;

    [SerializeField]
    private GameObject _repairKit;
    private bool _repairKitSpawned;

    private bool _rarePowerUpSpawned; 

    void Start()
    {
        _asteroid = GetComponent<Asteroid>();
        _enemyScript = GetComponent<Enemy>();
        _enemyContainer = GameObject.Find("Enemy Container");
        _ySpawn = 4;
        
        

    }
    private void Update()
    {
        StartSpawning();
    }

    public void StartSpawning()
    {
        
        if(_asteroid == null)
        {
            StartCoroutine(SpawnEnemy());
            StartCoroutine(PowerUpSpawn());
            StartCoroutine(RarePowerUpSpawn());
            StartCoroutine(AmmoSpawn());
            StartCoroutine(RepairKitSpawn());
           
        }
        
    }


    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(2); 
        if (_respawnEnemy == true && _playerAlive == true)
        {
            _randomXSpawn = Random.Range(-10, 10);
            GameObject newEnemy = Instantiate(_enemy, new Vector3(_randomXSpawn, _ySpawn, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            GameObject enemyLaser = Instantiate(_enemyLaser, new Vector3(newEnemy.transform.position.x, newEnemy.transform.position.y, 0), Quaternion.identity);
            _respawnEnemy = false;
            yield return new WaitForSeconds(5);
            _respawnEnemy = true;
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
            yield return new WaitForSeconds(Random.Range(10, 15));
            _respawnPowerup = true;
        }

    }

    IEnumerator RarePowerUpSpawn()
    {
        yield return new WaitForSeconds(60);
        if(_rarePowerUpSpawned ==false)
        {
            
            int spawnChance = Random.Range(0, 4);
            if (spawnChance >= 3)
            {
                GameObject rarePowerUp = Instantiate(_powerUpID[3], new Vector3(Random.Range(-10, 10), 4, 0), Quaternion.identity);
                _rarePowerUpSpawned = true;
                yield return new WaitForSeconds(60);
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
            yield return new WaitForSeconds(20);
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
            yield return new WaitForSeconds(35);
            _repairKitSpawned = false;
        }
       
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

