using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float speed;
    public float jumpPower;

    Rigidbody2D rb;
    float horizontalInput;
    bool jumpInput;
    bool grounded;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        if (rb == null)
            Debug.LogError("No rigidbody2D connected (PlayerInput)");
        grounded = true;
    }

    // Update is called once per frame
    
    
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        jumpInput = Input.GetButton("Jump");
        
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
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }
}
