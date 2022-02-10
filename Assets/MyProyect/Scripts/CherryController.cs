using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{

    //Distance de disparo y tiempo de destruccion

    public Rigidbody cherryRb;
    public float throwDistance = 10000f;
    public float timeToDestroy = 4.0f;

    public GameObject player;

    private void Start()
    {

        this.player = GameObject.FindGameObjectWithTag("Player");
        
    }

    private void Update()
    {
        
        //Si le das a la tecla E o del raton activa el metodo si hay municion
        if(Time.timeScale > 0)
        {

            if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
            {

                if (PlayerManager.currentCherryCount > 0)
                {

                    ThrowCherry();

                }

            }

        }

    }

    //Metodo de disparo, crea un rigidbody que instancia la cereza y la pasa a rigidbody

    private void ThrowCherry()
    {

        //Para lanzar la cereza del raton hemos de trazar un rayo dependiendo de donde hayamos hecho click
        //Convertir de 2D a 3D

        //Convierte donde se hace click en la batalla a donde se dirigen los disparos
        //Nos da un rayo, un punto final de la trayectoria del rayo

        Ray cameraToWorldRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit = new RaycastHit();

        if(Physics.Raycast(cameraToWorldRay, out hit))
        {

            Debug.DrawLine(transform.position, hit.point);

            Vector3 directionToFire = hit.point - this.transform.position;

            // Vector3 directionToFire = targetPoint - this.transform.position; Este era antes porque lo redefinio

            //Crea la nueva cereza en el mismo lugar que la otra

            Rigidbody cherryClone = (Rigidbody)Instantiate(this.cherryRb, transform.position, transform.rotation);

            //Con esto se la direccion a la que quiero disparar
            //Esto es coordenadas mundo hace falta trasnferirlo al personaje, desde el origen a donde haces click

            //Vector3 targetPoint = cameraToWorldRay.origin + cameraToWorldRay.direction;

            //Este es el vector del punto seleccionado desde el jugador pero tiene un longitud grande, por lo qye hay que normalizar
            //Le afecta al gravedad
           

            cherryClone.useGravity = true;

            //Elimina cualquier restriccion del rigid, como prohibir rotacion

            cherryClone.constraints = RigidbodyConstraints.None;

            //Le añade una fuerza hacia adelante
            //Por el vector que hemos calculado
            //Hemos de recordar que no es lo mismo disparar desde la pantalla que desde el mundo
            //A su vez nos da las coordenadas de la camara que esta al contrario por lo tanto es una direccion contraria

            cherryClone.AddForce(directionToFire.normalized * this.throwDistance);

            //Finalmente la destruye al paso de un tiempo

            Destroy(cherryClone.gameObject, timeToDestroy);

            //Le resta 1 a la municion

            PlayerManager.currentCherryCount -= 1;

        }
        
    }

}
