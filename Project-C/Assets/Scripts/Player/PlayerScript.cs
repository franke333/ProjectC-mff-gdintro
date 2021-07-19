using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public int health = 3;
    public float eyeFrameTime = 1; //invulnerability period after taking damage
    public List<GameObject> HealthSpritesObjects;
    public Sprite hpFullSprite,hpEmptySprite;

    [Header("Weapon Elements")]
    public Sprite greenElementSprite;
    public Sprite waterElementSprite, fireElementSprite;
    public float greenCD, waterCD, fireCD, baseCD;
    public bool greenAvalaible, waterAvalaible, fireAvalaible;
    public BoxCollider2D attackCollider;

    private float greenCurrCD = 0, waterCurrCD = 0, fireCurrCD = 0, baseCurrCD=0;


    List<Image> healthSprites;
    SpriteRenderer spriteRenderer;
    bool tookDamage = false;
    float timeSinceTakingDamage = 0;
    HashSet<Collider2D> colliders = new HashSet<Collider2D>(); // enemies in range of meele attack
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthSprites = new List<Image>();
        foreach(var sprite in HealthSpritesObjects)
        {
            healthSprites.Add(sprite.GetComponent<Image>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        //invulnerability after taking damage
        if (tookDamage)
        {
            timeSinceTakingDamage += Time.deltaTime;
            if (timeSinceTakingDamage >= eyeFrameTime)
            { 
                tookDamage = false; //end of invulnerability
                timeSinceTakingDamage = 0;
                if(health>0) spriteRenderer.color = Color.white;
            }
        }
        //flip attack collider when character is flipped
        if ((spriteRenderer.flipX && attackCollider.offset.x > 0) || (!spriteRenderer.flipX && attackCollider.offset.x < 0))
            attackCollider.offset *= new Vector2(-1,1);
        if (baseCurrCD > 0) baseCurrCD -= Time.deltaTime;
        if (waterCurrCD > 0) waterCurrCD -= Time.deltaTime;
        if (fireCurrCD > 0) fireCurrCD -= Time.deltaTime;
        if (greenCurrCD > 0) greenCurrCD -= Time.deltaTime;
    }

    private void Die()
    {
        GetComponent<PlayerInput>().speed = 0; //turn off movement
        spriteRenderer.color = Color.black;
        var levelManager = GameObject.FindGameObjectWithTag("GameController");
        levelManager.GetComponent<ManagerScript>().RestartLevel();
    }

    private void RefreshUI()
    {
        for (int i = 0; i < health; i++)
        {
            healthSprites[i].sprite = hpFullSprite;
        }
        for (int i = health; i < healthSprites.Count; i++)
        {
            healthSprites[i].sprite = hpEmptySprite;
        }
    }

    public void TakeDamage(int damage=1)
    {
        if (tookDamage || health==0)
            return; //player is invulnerable -> skip taking damage
        tookDamage = true;
        health -= damage;
        RefreshUI();
        spriteRenderer.color = Color.red;
        if (health <= 0)
            Die();
        
    }

    public void BaseAttack()
    {
        if (baseCurrCD <= 0)
        {
            Debug.Log("dealing dmg");
            baseCurrCD = baseCD;
            foreach(var enemy in colliders)
            {
                enemy.GetComponent<EnemyBehaviorScript>().TakeDamage();
            }
        }
    }

    public void RestoreHealth(int heal)
    {
        int restoreValue = healthSprites.Count - health;
        if (heal < restoreValue) restoreValue = heal; //take min of (missing health,heal)
        health += restoreValue;
        RefreshUI();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Respawn")
        {
            RestoreHealth(3);
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            colliders.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (colliders.Contains(collision))
            colliders.Remove(collision);
    }
}
