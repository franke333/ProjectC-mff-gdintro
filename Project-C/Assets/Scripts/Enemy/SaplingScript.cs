using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaplingScript : MonoBehaviour
{
    public float timeToRise;
    public BoxCollider2D box;
    public float offsetY;
    float currTime=0;

    private void Start()
    {
        transform.position += new Vector3(0, offsetY, 0);
    }
    // Update is called once per frame
    void Update()
    {
        currTime += Time.deltaTime;
        if (currTime > timeToRise)
        {
            box.enabled = true;
            transform.position += new Vector3(0, Time.deltaTime * 10, 0);
        }
        if (currTime > timeToRise + 0.6f)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.GetComponent<PlayerScript>().TakeDamage();
    }
}
