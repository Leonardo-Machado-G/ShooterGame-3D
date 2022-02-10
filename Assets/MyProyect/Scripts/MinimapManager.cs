using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapManager : MonoBehaviour
{

    RawImage minimap;

    void Start()
    {

        this.minimap = GetComponent<RawImage>();

    }

    
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.M))
        {

            this.minimap.enabled = !minimap.enabled;

        }

    }

}
