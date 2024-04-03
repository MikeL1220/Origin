using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed;

    
    private Player player; 

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        
    }

    // Update is called once per frame
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

    private void OnTriggerEnter(Collider other)
    {
       if(other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }

       if (other.tag == "Player")
        {
            player.PlayerHealth();
            Destroy(this.gameObject);
        
        }
    }

}
