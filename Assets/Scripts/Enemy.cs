using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed;

    //  private bool _enemyLaserCooldown; 


    private Player _player;

    private UIManager _uiManager;

    private Animator _enemyExplosion;

    private AudioSource _explosionSound;

    private CameraEffects _camera;


    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        _enemyExplosion = GetComponent<Animator>();

        _explosionSound = GameObject.Find("Explosion_Sound").GetComponent<AudioSource>();

        _camera = GameObject.Find("Main Camera").GetComponent<CameraEffects>();

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
            _enemyExplosion.SetTrigger("EnemyDeath");
            this.gameObject.GetComponent<Collider2D>().enabled = false;
            Destroy(this.gameObject, 1.5f);
            _uiManager.UpdateScore();
            _explosionSound.Play();

        }

        if (other.tag == "Player")
        {
            _player.PlayerHealth();
            _enemyExplosion.SetTrigger("EnemyDeath");
            _camera.StartCoroutine("CameraShake");
            _enemySpeed = 0;
            this.gameObject.GetComponent<Collider2D>().enabled = false;
            Destroy(this.gameObject, 1.5f);
            _explosionSound.Play();


        }


    }

}
