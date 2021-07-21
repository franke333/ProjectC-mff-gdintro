using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float speed;
    public float jumpPower;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;
    float horizontalInput;
    bool jumpInput;
    bool grounded;
    PlayerScript ps;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        sr = this.GetComponent<SpriteRenderer>();
        ps = this.GetComponent<PlayerScript>();
        animator = this.GetComponent<Animator>();
        if (rb == null)
            Debug.LogError("No rigidbody2D connected (PlayerInput)");
        grounded = true;
    }

    // Update is called once per frame
    
    
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput != 0) animator.SetBool("Running", true);
        else animator.SetBool("Running", false);
        jumpInput = Input.GetButton("Jump");
        if (horizontalInput > 0)
            sr.flipX = false;
        else if (horizontalInput < 0)
            sr.flipX = true;
        if (Input.GetButtonDown("Attack"))
            ps.BaseAttack();
        if (Input.GetButtonDown("FireAttack"))
            ps.FireAttack();
        if (Input.GetButtonDown("WaterAttack"))
            ps.WaterAttack();
        if (Input.GetButtonDown("GreenAttack"))
            ps.GreenAttack();
    }

    void FixedUpdate()
    {

        this.gameObject.transform.position += new Vector3(speed, 0, 0)*Time.deltaTime*horizontalInput;
        if(jumpInput && grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }
        grounded = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
    }
}
