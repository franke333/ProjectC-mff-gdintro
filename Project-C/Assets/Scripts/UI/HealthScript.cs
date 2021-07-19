using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    public Sprite spriteFull;
    public Sprite spriteEmpty;
    public GameObject spritePrefab;
    private int hpMax=5;
    private int hpCurrent=5;

    private List<GameObject> spriteList;
    
    private void Start()
    {
        spriteList = new List<GameObject>();
        for (int i = 0; i < hpMax; i++)
        {
            GameObject sprite = Instantiate(spritePrefab);
            spriteList.Add(sprite);
            //TODO
        }
        
    }
}
