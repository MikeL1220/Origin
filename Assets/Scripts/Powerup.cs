using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    private Player _player; 
    
    [SerializeField]
    private GameObject _tripleShotPowerup;

    [SerializeField]
    private int _speed; 


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null )
        {
            Debug.LogError("Player is Null.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        PowerupMovement();
    }

    private void PowerupMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -7.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            _player.TripleShotPowerupActice();
            Destroy(this.gameObject);
        }
    }
}
