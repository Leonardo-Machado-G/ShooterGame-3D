using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CucumberManager : MonoBehaviour
{

    //member tag

    private string m_tag = "Cucumber";
    public static int currentCucumberCount = 0;
    Text cucumberTextCount;
    public GameObject[] cucumbers;

    void Awake()
    {

        this.cucumberTextCount = GetComponent<Text>();
        currentCucumberCount = 1;

    }

    void Update()
    {

        cucumbers = GameObject.FindGameObjectsWithTag(this.m_tag);
        currentCucumberCount = cucumbers.Length;
        this.cucumberTextCount.text = currentCucumberCount.ToString();

    }
}
