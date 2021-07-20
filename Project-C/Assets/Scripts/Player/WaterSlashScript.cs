using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSlashScript : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float speed;
    public float lifespan;
    public Vector2 offset;
    public int rootDuration;
    public bool dealsDMG;
    public bool enemySpell;

    float direction = -1;
    string targetTag="Enemy";
    // Update is called once per frame
    private void Start()
    {
        if (enemySpell)
            targetTag = "Player";
        if (spriteRenderer.flipX) direction = 1;
        transform.position += new Vector3(offset.x * direction, offset.y, 0);
    }
    void Update()
    {
        lifespan -= Time.deltaTime;
        if (lifespan <= 0)
            Destroy(this.gameObject);
        
        transform.Translate(new Vector2(direction * speed*Time.deltaTime, 0));

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            if (dealsDMG && !enemySpell) collision.gameObject.GetComponent<EnemyBehaviorScript>().TakeDamage();
            if (dealsDMG && enemySpell) collision.gameObject.GetComponent<PlayerScript>().TakeDamage();
            if (rootDuration>0) collision.gameObject.GetComponent<EnemyBehaviorScript>().Stun(rootDuration);
        }
    }
}
