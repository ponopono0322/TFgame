using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageChanger : MonoBehaviour
{
    public Image original;
    public Sprite default_image;
    public Sprite clicked_image;
    public GameObject UICanvas_1;
    public GameObject UICanvas_2;
    private bool UIstate = true;
 
    public void ChangeImage()
    {
        
        if (original.sprite != default_image)
        {
            original.sprite = default_image;
        }
        else
        {
            original.sprite = clicked_image;
        }

    }

    public void ShowCanvas()
    {
        if(UIstate == false)
        {
            UIstate = true;
        }
        else
        {
            UIstate = false;
        }
        UICanvas_1.SetActive(UIstate);
        UICanvas_2.SetActive(UIstate);
    }
}
