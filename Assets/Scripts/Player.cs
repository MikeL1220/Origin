using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{


    [SerializeField]
    private float _speed;

    private float _verticalPos;
    private float _horizontalPos;

    [SerializeField]
    private GameObject _laser;
    private float _laserOffset;
    private float _canFire;
    [SerializeField]
    private float _fireRate;
    private bool _fireLaser;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = Vector3.zero;
        _verticalPos = transform.position.y;
        _horizontalPos = transform.position.x;





    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        FireLaser();

    }

    private void Movement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }

        if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }

        if (transform.position.y > 0.8f)
        {
            transform.position = new Vector3(transform.position.x, 0.8f, 0);
        }

        if (transform.position.y < -3.9f)
        {
            transform.position = new Vector3(transform.position.x, -3.9f, 0);
        }
    }

    private void FireLaser()
    {
        
        
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
            {
            _canFire = Time.time + _fireRate;

            Instantiate(_laser, new Vector3(transform.position.x, transform.position.y + _laserOffset, 0), Quaternion.identity);
            
           
            }


        }
    }

