using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed;


    private Player _player;

    private UIManager _uiManager;


    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        _uiManager =GameObject.Find("Canvas"). GetComponent<UIManager>();

    }


    void Update()
    {
        EnemyMovement();
    }

    private void EnemyMovement()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if (transform.position.y < -7.7)
        {
            transform.position = new Vector3(Random.Range(-10, 10), 6.2f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
            _uiManager.UpdateScore();
           
        }

        if (other.tag == "Player")
        {
            _player.PlayerHealth();
            Destroy(this.gameObject);
            _uiManager.UpdateLifeCount();

        }
    }

}
