using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private int _laserSpeed;


    void Update()
    {
        LaserMovement();
    }
    private void LaserMovement()
    {
        if (tag == "EnemyLaser")
        {
            transform.Translate(Vector3.down * _laserSpeed * Time.deltaTime);
            if (transform.position.y < -10)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);

            if (transform.position.y > 6)
            {
                Destroy(this.gameObject);
            }
        }
    }


}
