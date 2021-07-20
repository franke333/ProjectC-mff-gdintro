using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorScript : MonoBehaviour
{
    private enum State
    {
        Seek,Attack
    } 
    public enum SlimeType
    {
        Fire,
        Nature,
        Water
    }
    public float speed;
    public float detectionRange;
    public float HP = 2;
    public SlimeType slimeType;
    public GameObject WaterAttackGO, FireAttackGO, NatureAttackGO;
    GameObject player;
    SpriteRenderer sr;
    bool die = false;
    float stateTime;
    State state = State.Seek;
    private float stunDuration = 0;


    //Water
    int waterShot = 0;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        sr = this.gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        stateTime += Time.deltaTime;
        if (die)
            Destroy(this.gameObject);
        if (stunDuration > 0)
        {
            stunDuration -= Time.deltaTime;
            if (stunDuration < 0)
                sr.color = Color.white;
            return;
        }
        float distance = Vector2.Distance(this.transform.position, player.transform.position);
        if (state == State.Seek)
        {
            if (distance <= detectionRange)
            {
                float directionTowardsPlayer = Mathf.Sign((player.transform.position - this.transform.position).x);
                if (directionTowardsPlayer > 0)
                    sr.flipX = true;
                else
                    sr.flipX = false;
                transform.position += new Vector3(1, 0, 0) * directionTowardsPlayer * Time.deltaTime * speed;
            }
            if(stateTime>1f && distance <= detectionRange / 2)
            {
                state = State.Attack;
                stateTime = 0;
            }
        }
        else
        {
            switch (slimeType)
            {
                case SlimeType.Fire:
                    FireAttack();
                    break;
                case SlimeType.Nature:
                    NatureAttack();
                    break;
                case SlimeType.Water:
                    WaterAttack();
                    break;
                default:
                    break;
            }
        }
         

    }


    private void FireAttack()
    {
        if (stateTime > 1f)
        {
            Instantiate(FireAttackGO, transform.position, transform.rotation);
            GameObject second = Instantiate(FireAttackGO, transform.position , transform.rotation);
            second.GetComponent<SpriteRenderer>().flipX = true;
            stateTime = 0;
            state = State.Seek;
        }
    }
    private void NatureAttack()
    {
        if (stateTime > 1.6f)
        {
            float x1 = Random.Range(-4f, 4f);
            float x2 = Random.Range(-2f, 2f);
            Instantiate(NatureAttackGO, transform.position + new Vector3(x1, 0, 0),transform.rotation);
            Instantiate(NatureAttackGO, transform.position + new Vector3(x2, 0, 0), transform.rotation);
            stateTime = 0;
            state = State.Seek;
        }

    }
    private void WaterAttack()
    {
        if (stateTime > 1.1f+0.75*waterShot)
        {
            waterShot++;
            GameObject slash = Instantiate(WaterAttackGO, transform.position, transform.rotation);
            slash.gameObject.GetComponent<SpriteRenderer>().flipX = sr.flipX;
        }
        if (waterShot == 3) //shoot 3, then seek
        {
            state = State.Seek;
            stateTime = 0;
            waterShot = 0;
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


    public void Stun(int duration = 5)
    {
        stunDuration = Mathf.Max(stunDuration,duration);
        sr.color = new Color(0.45f,0.35f,0.19f); //brown
    }
    public void TakeDamage()
    {
        HP -= 1;
        float dir = Mathf.Sign((player.transform.position - this.transform.position).x);
        GetComponent<Rigidbody2D>().velocity = new Vector2(-dir*3,5);
        Debug.Log("Enemy took dmg");
        if (HP <= 0)
            die = true;
    }
}
