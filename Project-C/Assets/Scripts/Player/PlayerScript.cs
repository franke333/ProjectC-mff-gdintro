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
    public Image greenElementSprite;
    public Image waterElementSprite, fireElementSprite, baseAttackSprite;
    public float greenCD, waterCD, fireCD, baseCD;
    public float barrierDuration;
    public bool greenAvalaible, waterAvalaible, fireAvalaible;
    public BoxCollider2D attackCollider;
    public GameObject barrierGO,waterSlashGO;

    private float currCD=0; // cd shared between attacks
    private float currBarrierDur;
    [Header("Weapon GameObjects")]
    public GameObject basicSlashGameObject;

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
                if (health > 0) spriteRenderer.color = Color.white;
            }
        }
        //flip attack collider when character is flipped
        if ((spriteRenderer.flipX && attackCollider.offset.x > 0) || (!spriteRenderer.flipX && attackCollider.offset.x < 0))
        {
            attackCollider.offset *= new Vector2(-1, 1);
            Vector3 pos = basicSlashGameObject.transform.localPosition;
            basicSlashGameObject.transform.localPosition = new Vector3(-1*pos.x,pos.y,pos.z);
            basicSlashGameObject.GetComponent<SpriteRenderer>().flipX = spriteRenderer.flipX;
        }
        UpdateAttacksCD();
        UpdateBarrier();
    }

    private void UpdateAttacksCD()
    {
        if (currCD > 0)
        {
            currCD -= Time.deltaTime;
            if (currCD <= 0)
            {
                baseAttackSprite.color = Color.white;
                if (waterAvalaible) waterElementSprite.color = Color.white;
                if (fireAvalaible) fireElementSprite.color = Color.white;
                if (greenAvalaible) greenElementSprite.color = Color.white;
            }
            else
            {
                baseAttackSprite.color = Color.black;
                if (waterAvalaible) waterElementSprite.color = Color.black;
                if (fireAvalaible) fireElementSprite.color = Color.black;
                if (greenAvalaible) greenElementSprite.color = Color.black;
            }
        }
    }

    private void UpdateBarrier()
    {
        if (currBarrierDur > 0)
        {
            currBarrierDur -= Time.deltaTime;
            if (currBarrierDur <= 0)
            {
                barrierGO.SetActive(false);
            }
        }
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
        if (currBarrierDur > 0) //check for barrier
        {
            currBarrierDur = 0;
            barrierGO.SetActive(false);
        }
        else
        {
            health -= damage;
            spriteRenderer.color = Color.red;
        }
        tookDamage = true;
        RefreshUI();
        if (health <= 0)
            Die();
        
    }

    public void BaseAttack()
    {
        if (currCD <= 0)
        {
            Debug.Log("dealing dmg");
            basicSlashGameObject.GetComponent<SpriteShowScript>().Show();
            currCD = baseCD;
            foreach(var enemy in colliders)
            {
                enemy.GetComponent<EnemyBehaviorScript>().TakeDamage();
            }
        }
    }

    public void WaterAttack()
    {
        if (!waterAvalaible || currCD > 0) return;
        GameObject slash = Instantiate(waterSlashGO,transform.position,transform.rotation);
        slash.GetComponent<SpriteRenderer>().flipX = !spriteRenderer.flipX;
        Debug.Log("casting water");
        currCD = waterCD;
    }

    public void GreenAttack()
    {
        if (!greenAvalaible || currCD > 0) return;

        Debug.Log("casting green");
        currCD = greenCD;
    }

    public void FireAttack()
    {
        if (!fireAvalaible || currCD > 0) return;
        currBarrierDur = barrierDuration;
        barrierGO.SetActive(true);
        Debug.Log("casting fire");
        currCD = fireCD;
       
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
        else if(collision.gameObject.tag == "BlueJewel")
        {
            waterAvalaible = true;
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            waterElementSprite.color = Color.white;
        }
        else if (collision.gameObject.tag == "GreenJewel")
        {
            greenAvalaible = true;
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            greenElementSprite.color = Color.white;
        }
        else if (collision.gameObject.tag == "RedJewel")
        {
            fireAvalaible = true;
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            fireElementSprite.color = Color.white;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (colliders.Contains(collision))
            colliders.Remove(collision);
    }
}
