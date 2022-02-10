using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpManager : MonoBehaviour
{

    Canvas helpCanvas;

    void Start()
    {

        helpCanvas = GetComponent<Canvas>();

    }

    void Update()
    {
        //Cambiar tiempo para hacer efecto matrix

        if(Input.GetKeyDown(KeyCode.H))
        {

            helpCanvas.enabled = !helpCanvas.enabled;

            if (helpCanvas.enabled)
            {

                Time.timeScale = 0f;

            }
            else
            {

                Time.timeScale = 1f;

            }

        }

    }

}
