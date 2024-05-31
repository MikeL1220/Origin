using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{

    [SerializeField]
    private int _collectableSpeed; 

    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        transform.Translate(Vector3.down * _collectableSpeed * Time.deltaTime);

        if (transform.position.y < -7.5f)
        {
            Destroy(this.gameObject);
        }
    }

}
