﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shikigami_hurt : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
           Shikigami.instance.HP =Shikigami.instance.HP - Shikigami.instance.hurt_value;
            Debug.Log("Shikigami is hurt");
        }
    }

}
