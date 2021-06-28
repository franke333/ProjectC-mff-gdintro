using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorScript : MonoBehaviour
{
    public float speed;
    public float detectionRange;


    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(this.transform.position, player.transform.position);
        if (distance <= detectionRange)
        {
            float directionTowardsPlayer = Mathf.Sign(( player.transform.position- this.transform.position).x);
            transform.position += new Vector3(1,0,0) * directionTowardsPlayer * Time.deltaTime * speed ;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, detectionRange);
        
    }
}
