using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsManager : MonoBehaviour
{
    
    //Puntos actuales

    public static int currentPoints;

    //Texto de los puntos

    Text pointsText;

    private void Awake()
    {

        this.pointsText = GetComponent<Text>();
        currentPoints = 0;

    }

    //Funcion principal es cambiar los puntos

    private void Update()
    {

        this.pointsText.text = currentPoints.ToString();

    }

    public static void AddPoints(int points)
    {

        currentPoints += points;

    }

}
