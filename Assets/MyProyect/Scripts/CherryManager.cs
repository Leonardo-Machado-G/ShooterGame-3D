using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CherryManager : MonoBehaviour
{

    //member tag
    
    Text cherriesTextCount;

    void Start()
    {

        this.cherriesTextCount = GetComponent<Text>();

    }

    void Update()
    {
        
        this.cherriesTextCount.text = PlayerManager.currentCherryCount.ToString();

    }
}
