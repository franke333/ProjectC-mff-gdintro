using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorScript : MonoBehaviour
{
    public float speed;
    public float detectionRange;
    public float HP = 2;

    GameObject player;
    SpriteRenderer sr;
    bool die=false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(die)
            Destroy(this.gameObject);
        float distance = Vector2.Distance(this.transform.position, player.transform.position);
        if (distance <= detectionRange)
        {
            float directionTowardsPlayer = Mathf.Sign(( player.transform.position- this.transform.position).x);
            if (directionTowardsPlayer > 0)
                sr.flipX = true;
            else
                sr.flipX = false;
            transform.position += new Vector3(1,0,0) * directionTowardsPlayer * Time.deltaTime * speed ;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, detectionRange);
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player)
            collision.gameObject.GetComponent<PlayerScript>().TakeDamage();
    }



    public void TakeDamage()
    {
        HP -= 1;
        Debug.Log("Enemy took dmg");
        if (HP <= 0)
            die = true;
    }
}
