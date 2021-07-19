using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteShowScript : MonoBehaviour
{
    public float showTime;

    float timeRemain = 0;
    SpriteRenderer sr;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (timeRemain > 0)
        {
            timeRemain -= Time.deltaTime;
            if (timeRemain <= 0)
                sr.enabled = false;
        }
    }

    public void Show()
    {
        sr.enabled = true;
        timeRemain = showTime;
    }

}
