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

    private void PlayerDeath()
    {
        if(_player == null)
        {
            _playerAlive = false;
        }
    }


}

