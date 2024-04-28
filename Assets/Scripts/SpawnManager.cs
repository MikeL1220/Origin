using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
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




    void Start()
    {
        _asteroid = GetComponent<Asteroid>();   
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

