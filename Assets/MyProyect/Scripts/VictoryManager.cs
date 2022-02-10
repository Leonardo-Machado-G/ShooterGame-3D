using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VictoryManager : MonoBehaviour
{

    Text victoryText;

    private void Awake()
    {

        this.victoryText = GetComponent<Text>();
        this.victoryText.text = "";
        //victoryText.enabled = false;


    }

    private void Update()
    {
        
        if(BeetleManager.currentBeetleCount == 0)
        {

            //victoryText.enabled = true;
            this.victoryText.text = "You Win!\n";

        }

        if(CucumberManager.currentCucumberCount == 0 || PlayerManager.livesRemaining == 0)
        {

            this.victoryText.text = "FRACASADO";

        }

    }

}
