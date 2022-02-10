using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatlePatrol : MonoBehaviour
{

    //Indicador de muerte

    public static bool isDead = false;
    public static bool isEating = false;
    public static bool isAttacking = false;

    //Velocidad escarabajo, 
    //1 s de linea recta y hará otro cambiando de direccion
    //El giro estara controlado una vez tengo la direccion

    public float speed = 5.0f;
    public float directionChangeInterval = 1.0f;
    public float maxHeadingChange = 30.0f;

    Animator beatleAnimator;

    //Controlador del NPC
    //Variable para conocer hacia donde nos dirigimos
    //Vector para indicar la rotacion de la siguiente interaccion

    CharacterController controller;
    float heading; //(0 a 360 º)
    Vector3 targetRotation;

    private void Start()
    {

        this.beatleAnimator = GetComponent<Animator>();
        this.controller = GetComponent<CharacterController>();

        //Giramos la cabeza entre 0 y 360º y le cambiamos su angulo con euler
        //Llamamos a la corrutina

        this.heading = Random.Range(0, 360);
        transform.eulerAngles = new Vector3(0, this.heading, 0);
        StartCoroutine(NewHeading());

    }

    IEnumerator NewHeading()
    {

        //La corrutina no te bloquea el codigo si llamasemos al codigo en un metodo normal porque es sincrono

        while(true)
        {

            //Llamamos al metodo que suma y resta 30º maximos a la direccion del cono donde estamos mirando

            NewHeadRoutine();

            yield return new WaitForSeconds(this.directionChangeInterval);
            
        }

    }

    //Decide cuanto girar en funcion del cono de vision con un maximo y un minimo

    void NewHeadRoutine()
    {

        float floor = transform.eulerAngles.y - this.maxHeadingChange;
        float ceil = transform.eulerAngles.y + this.maxHeadingChange;

        //Este abanico sera el minimo cambio que puede tener y el maximo, entonces giraremos la camara

        this.heading = Random.Range(floor, ceil);

        //Insertamos en el vector preparado el giro, pero no podemos hacerlo directamente porque seria instantaneo
        //Hay que hacerlo en el Update

        this.targetRotation = new Vector3(0, this.heading, 0);

    }

    private void Update()
    {
        
        if(!isDead && !isEating && !isAttacking)
        {

            //Cambiamos de forma suave nuestra rotacion actual a donde queremos y el tiempo lo determina cada frame * el giro minimo que podemos hacer

            transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, this.targetRotation, Time.deltaTime * this.directionChangeInterval);
            
        }
        
        //Vector unitario que establece el proximo movimiento y se lo transmite al controller con una velocidad

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        this.controller.SimpleMove(forward * this.speed);

    }

}
