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

    [SerializeField]
    private GameObject _tripleShotPowerup;
    private bool _respawnTripleShotPowerup = true; 

    // Start is called before the first frame update
    void Start()
    {
   
        _enemyContainer = GameObject.Find("Enemy Container");
        _ySpawn = 4;

    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(SpawnEnemy());
        StartCoroutine(PowerUpSpawn());
        
    }

   IEnumerator SpawnEnemy()
    {
        if ( _respawnEnemy == true && _playerAlive == true)
        {
            _randomXSpawn = Random.Range(-10, 10);
            GameObject newEnemy = Instantiate(_enemy, new Vector3(_randomXSpawn,_ySpawn,0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;  
            _respawnEnemy = false;
            yield return new WaitForSeconds(5);
            _respawnEnemy = true;
            PlayerDeath();
        }
        
    }
    IEnumerator PowerUpSpawn()
    {
        if(_respawnTripleShotPowerup == true)
        {
            GameObject newTripleShot = Instantiate(_tripleShotPowerup, new Vector3(Random.Range(-10, 10), 4, 0), Quaternion.identity);
            _respawnTripleShotPowerup = false; 
            yield return new WaitForSeconds(Random.Range(10, 15));
            _respawnTripleShotPowerup = true;
        }
        
    }

    private void PlayerDeath()
    {
        if(_player == null)
        {
            _playerAlive = false;
        }
    }


}

