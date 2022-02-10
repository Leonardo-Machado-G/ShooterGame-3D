using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    //Recuento, cerezas, condicion 

    public static int currentCherryCount;
    public int tempCurrentCherryCount;
    public bool isCollectingCherries;
    public static int livesRemaining;

    public Transform[] spawningZones;
    public static bool hasDead;

    private void Awake()
    {
        livesRemaining = 3;
        this.tempCurrentCherryCount = 0;
        currentCherryCount = 0;
        this.isCollectingCherries = false;
        hasDead = false;

    }

    private void Update()
    {
     
        if(HealthManager.currentHealth <= 0 && !hasDead)
        {
            hasDead = true;
            livesRemaining--;

            if (livesRemaining == 2)
            {

                Destroy(GameObject.Find("Life3"));
                GetComponent<Animator>().Play("Die");
                StartCoroutine(RespawnPlayer());

            }

            if (livesRemaining == 1)
            {

                Destroy(GameObject.Find("Life2"));
                GetComponent<Animator>().Play("Die");
                StartCoroutine(RespawnPlayer());

            }

            if (livesRemaining == 0)
            {

                Destroy(GameObject.Find("Life1"));
                GetComponent<Animator>().Play("Die");

            }


        }

        if (this.isCollectingCherries)
        {

            //Cada 60 frames
            //Si esta en la zona de los cerezos se recoge 1 cereza cada 60 frames, que es mas o menos cada 1s

            if(this.tempCurrentCherryCount >= 15)
            {

                PointsManager.AddPoints(5);
                currentCherryCount += 1;
                this.tempCurrentCherryCount = 0;

            }
            else
            {

                this.tempCurrentCherryCount += 1;

            }

        }

    }

    IEnumerator RespawnPlayer()
    {

        int randomPs = Random.Range(0, spawningZones.Length);

        yield return new WaitForSecondsRealtime(4f);
        //Movemos al jugador a la zona de spawning
        this.transform.position = spawningZones[randomPs].transform.position;
        GetComponent<Animator>().Play("Idle_Guard_AR");
        hasDead = false;
        HealthManager.currentHealth = 100;

    }

    //Al entrar comienza a sumar cerezas

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("CherryTree"))
        {

            this.isCollectingCherries = true;
            currentCherryCount += 1;
            PointsManager.AddPoints(5);

        }

    }

    //Al salir de la zona deja de contar
   
    private void OnTriggerExit(Collider other)
    {

        if(other.gameObject.CompareTag("CherryTree"))
        {

            this.isCollectingCherries = false;

        }

    }

}
