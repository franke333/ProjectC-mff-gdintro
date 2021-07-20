using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotemScript : MonoBehaviour
{
    public List<Sprite> spritesRGB;
    public Text winText;
    bool jewelB, jewelR, jewelG;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerScript ps = collision.GetComponent<PlayerScript>();
            jewelB = ps.waterAvalaible;
            jewelG = ps.greenAvalaible;
            jewelR = ps.fireAvalaible;
            int index = 0;
            if (jewelR) index += 1;
            if (jewelG) index += 2;
            if (jewelB) index += 4;
            GetComponent<SpriteRenderer>().sprite = spritesRGB[index];
            if (index == 7)
                winText.gameObject.SetActive(true);
        }
    }
}
