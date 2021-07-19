using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerScript : MonoBehaviour
{
    public float restartTime = 2f;

    private float currentTime = 0;
    private Texture2D texture;

    private enum restartStateEnum
    {
        NONE,FADEIN,FADEOUT
    }

    restartStateEnum restartState=restartStateEnum.NONE;


    private void Start()
    {
        texture = new Texture2D(1, 1);
        restartState = restartStateEnum.FADEOUT;
    }

    public void Update()
    {
        switch (restartState)
        {
            case restartStateEnum.FADEIN:
                currentTime += Time.deltaTime;
                texture.SetPixel(0, 0, new Color(1, 1, 1, currentTime / restartTime));
                texture.Apply();
                if (currentTime>=restartTime)
                {
                    currentTime = 0;
                    restartState = restartStateEnum.NONE;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                break;
            case restartStateEnum.FADEOUT:
                currentTime += Time.deltaTime;
                texture.SetPixel(0, 0, new Color(1, 1, 1, (restartTime - currentTime) / restartTime));
                texture.Apply();
                if (currentTime >= restartTime)
                {
                    currentTime = 0;
                    restartState = restartStateEnum.NONE;
                }
                break;
            default:
                break;
        }
        
    }

    private void OnGUI()
    {
        if (restartState != restartStateEnum.NONE)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
        }
    }

    public void RestartLevel()
    {
        restartState = restartStateEnum.FADEIN;
    }

}
