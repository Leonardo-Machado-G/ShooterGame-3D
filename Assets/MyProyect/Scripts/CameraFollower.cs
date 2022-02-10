using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    
    //Solo habrá una cámara a no ser que sean juegos con pantallas divididas

    public static CameraFollower sharedInstance;

    //Para saber a quien seguimos (humanoide)

    public GameObject followTarget;

    //Corrige los cambios bruscos tanto de giro como de movimiento
    public float movementSmoothness = 1.0f;
    public float rotationSmoothness = 1.0f;

    //Variable para seguir al jugador una vez resucita
    public bool canFollow = true;


    //Lo primero que se ejecuta antes de nada

    private void Awake()
    {

        sharedInstance = this;

    }

    //Lo ultimo que se ejecuta en cada frame
    //Lo ultimo que hace el ciclo cuando pinta un frame es mover la camara al jugador
    private void LateUpdate()
    {
        
        //Si el personaje esta muerto no seguira o si el object deja de existir o no existe

        if(this.followTarget == null || this.canFollow == false)
        {

            return;

        }

        //Interpolacion lineal, dados dos puntos buscar una curva que pase por ambos
        //Lineal interpolation

        //Estamos en una posicion y queremos ir a donde esta el jugador, el gameobject, con la ultima vez que se actualiza el frame * movimiento suave
        //Factor de suavidad desde el tiempo "refresco" de frame (incremento de tiempo desde la ultima frame que renderizamos)
        //Traslacion

        transform.position = Vector3.Lerp(transform.position, followTarget.transform.position, Time.deltaTime * movementSmoothness);

        //Interpolation para ir de un punto a otro y lo haces en forma de linea
        //Slerp lo hace en forma de esfera, busca la esfera
        //Se hace con cuaterniones que son matrices 4x4, cuando trasladamos rotamos etc, todo esta en un matriz 4x4
        //En la diagonal de la matriz esta la escala, x y z, en los triangulos externos esta la rotacion de x y z y la traslacion esta en la 4 columna

        /*
         
         x11 x12 x13 x14                                          s_X rxy rxz p_x
         x21 x22 x23 x24                  ------>                 rxy s_y ryz p_y                                                                                
         x31 x32 x33 x34                                          rxz ryz s_z p_z                                                          
         x41 x42 x43 x44                                           0   0   0   1
         
        En los cuaterniones el x44 = 1 / x14 = posicion x, x24 posicion en y, x34, posicion en z
        En x11 = es el factor de escala en x, y lo mismo con x22 y x33 / x41 = 0 = x42 = x43
        x12 = factor de rotacion de XY, x13 = XZ, x23 = YZ y la matriz es simetrica de modo que se corresponde el triagulo inferior con el superior
         
         */


        //Rotacion
        //Velocidad de rotacion
        transform.rotation = Quaternion.Slerp(transform.rotation, followTarget.transform.rotation, Time.deltaTime * movementSmoothness);

    }
    
}
