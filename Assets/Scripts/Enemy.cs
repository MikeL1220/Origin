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
    [SerializeField]
    private bool _newEnemyMove;

    private bool _moveDirection = false;


    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        _enemyExplosion = GetComponent<Animator>();

        _explosionSound = GameObject.Find("Explosion_Sound").GetComponent<AudioSource>();

        _camera = GameObject.Find("Main Camera").GetComponent<CameraEffects>();
        StartCoroutine(NewEnemyMovement());

    }


    void Update()
    {
        EnemyMovement();
        
    }



    private void EnemyMovement()
    {
        
        //use a random number gegnerator to determine if enemies move side to side 
        if (_newEnemyMove == true && _moveDirection == false) 
        {
            transform.Translate(new Vector3(-1,-1,0) * _enemySpeed * Time.deltaTime);
        }
        else if(_newEnemyMove == true && _moveDirection ==true)
        {
            transform.Translate(new Vector3(1, -1, 0) * _enemySpeed * Time.deltaTime);
        }
        else { transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime); }
        
        

        if (transform.position.y < -7.7)
        {
            transform.position = new Vector3(Random.Range(-10, 10), 6.2f, 0);
        }
    }

  IEnumerator NewEnemyMovement()
    {
        int RandomInt = Random.Range(1, 3);
        _newEnemyMove = true;
        //false is left and true is right 
        _moveDirection = false;
        if(RandomInt == 1)
        {
            _moveDirection = false;
        }
        else if (RandomInt == 2) { _moveDirection = true; }  

        yield return new WaitForSeconds(6);
        _newEnemyMove = false;
        yield return new WaitForSeconds(7); 

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
            _player.Damage();
            _enemyExplosion.SetTrigger("EnemyDeath");
            _camera.StartCoroutine("CameraShake");
            _enemySpeed = 0;
            this.gameObject.GetComponent<Collider2D>().enabled = false;
            Destroy(this.gameObject, 1.5f);
            _explosionSound.Play();


        }


    }

}
