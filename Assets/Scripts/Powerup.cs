using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    private Player _player;

    private bool _shieldStatus;

    private SpawnManager _spawnManager;

    [SerializeField]
    private int _speed;

    //0 = no powerup  1 = tripleshot 2 = speed boost  3 = shield
    [SerializeField]
    private int _powerUpID;

    

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("Player is Null.");
        }
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is Null"); 
        }  
    }

    void Update()
    {
        PowerupMovement();
    }

    private void PowerupMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -7.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            switch(_powerUpID)
            {
                case 1:
                    _player.TripleShotPowerupActive();
                    break;
                case 2:
                    _player.SpeedBoostPowerupActive();
                    break;
                case 3: _player.ShieldPowerupActive(_shieldStatus);
                    break;
                default:
                    break;
                    
            }
            _spawnManager.PowerUpSound();
            Destroy(this.gameObject);
        }
    }
}
