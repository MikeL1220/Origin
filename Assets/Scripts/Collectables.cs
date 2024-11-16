using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{

    [SerializeField]
    private int _collectableSpeed;
    [SerializeField]
    private int _debuffSpeed;

    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        if (tag == "debuff")
        {
            transform.Translate(Vector3.down * _debuffSpeed * Time.deltaTime);
        }
        else { transform.Translate(Vector3.down * _collectableSpeed * Time.deltaTime); }
        

        if (transform.position.y < -7.5f)
        {
            Destroy(this.gameObject);
        }
    }

}
