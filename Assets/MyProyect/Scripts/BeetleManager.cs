using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeetleManager : MonoBehaviour
{

    //member tag

    private string m_tag = "Beetle";
    public static int currentBeetleCount = 0;
    Text beetleTextCount;
    public GameObject[] beetles;

    void Awake()
    {

        this.beetleTextCount = GetComponent<Text>();
        currentBeetleCount = 1;

    }

    void Update()
    {

        beetles = GameObject.FindGameObjectsWithTag(this.m_tag);
        currentBeetleCount = beetles.Length;
        this.beetleTextCount.text = currentBeetleCount.ToString();

    }
}
