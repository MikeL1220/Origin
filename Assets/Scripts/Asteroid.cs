using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private int _rotationSpeed;

    [SerializeField]
    private GameObject _explosion;


    private SpawnManager _spawnManager;
    private bool _startSpawning;

    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
    }

    private void Update()
   {
        transform.Rotate(0, 0, 1 * _rotationSpeed * Time.deltaTime);

        if(_startSpawning == true)
        {
            _spawnManager.StartSpawning();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.gameObject);
        GameObject explosion = Instantiate(_explosion, transform.position, Quaternion.identity);
        Destroy(this.gameObject, 1.2f);
        Destroy(explosion, 3f);
        _startSpawning = true;
        
    }

   
}
