using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_HI : MonoBehaviour
{
    public GameObject sprite;
    public GameObject build_range;
    public bool building = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("manger in building : " + building);
    }

    public void build_on()
    {
        sprite.SetActive(true);
        build_range.SetActive(true);
        building = true;
    }
    public void build_off()
    {
        build_range.SetActive(false);
        building = false;
    }
}
