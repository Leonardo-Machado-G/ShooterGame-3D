using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

/*
  Importamos un paquete para que sean controles multiplataforma
  Protegemos el archivo asignandole un rigidBody para que no se olvide de asignar
*/

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    //Variables de movimiento

    float horizontal, vertical;

    //RigidBody, le asignamos esta forma para evitar no usar palabras que unity no deja

    Rigidbody m_rigidbody;

    //Variables de salto, andar y correr

    public float jumpForce = 10.0f, moveSpeed = 5.0f, runSpeed = 10.0f;

    //Estas las escondemos para que solo las veamos nosotros

    private float currentJumpForce, currentMoveSpeed = 0;

    //Le asignamos un animator

    private Animator m_animator;

    //Variable para comprobar si esta en el suelo

    private bool isGrounded;

    //Variables para el movimiento hacia adelante y laterales, velocidad de giro actual y el tiempo que estemos pulsando la tecla

    private float turnAmount, forwardAmount;

    //Cada segundo que gire en el aire girara 180 grados

    [SerializeField] float stationaryTurnAround = 180;
    //Sirve para que yo en el editor lo pueda editar y que sea privado

    //Multiplicamos constantemente por el tiempo de modo que es velocidad = espacio * tiempo

    //Cuando el personaje esta quieto su velocidad de giro sera mas lenta y en movimiento mas rapida

    [SerializeField] float movingTurnSpeed = 360;

    //Asignamos la camera, y creamos el vector de movimiento de la camera y la direccion en que apunta

    public Transform m_camera;
    private Vector3 cameraForward;
    private Vector3 move;
    private bool jump;

    //Vector Normal al suelo
    private Vector3 groundNormal;

    //Distancia de chequeo contra el suelo tirando un rayo
    [SerializeField] float groundCheckDistance = 0.1f;

    //Factor de velocidad para que multiplicar la velocidad a la que se mueve una animacion

    [Range (1.0f , 4.0f)] [SerializeField] float moveSpeedMultiplier = 1.0f;
    //Esto es un slider

    public float m_GravityMultiplier;
    //Constante para que cuando caes de muy alto no traspases el suelo
    //private float m_OrigGroundCheckDistance;


    private void Start()
    {

        //Le asignamos en el start su rigidbody, animator y velocidad actual 

        this.m_rigidbody = GetComponent<Rigidbody>();
        this.currentMoveSpeed = moveSpeed;
        this.m_animator = GetComponent<Animator>();
        //Constante para que cuando caes de muy alto no traspases el suelo
        //this.m_OrigGroundCheckDistance = this.m_GroundCheckDistance;

    }

    private void Update()
    {
        
        //Comprobamos si estamos en el suelo

        CheckGroundStatus();

        /*
          Comprobamos la cantidad de movimiento vertical y horizotal
          CrossPlat nos da el movimiento en los ejes y las transformamos posteriormente
        */

        this.horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        this.vertical = CrossPlatformInputManager.GetAxis("Vertical");

        //Debug.Log("H: " + this.horizontal + ", V: " + this.vertical);

        //Si le damos a la tecla espacio y estamos en el suelo, comienza a saltar
       
        if(Input.GetKeyDown(KeyCode.Space) && this.isGrounded)
        {

            this.m_rigidbody.AddForce(0, this.jumpForce, 0);

        }

        //Si estamos en el suelo y le damos a shift, cambia a correr

        if (Input.GetKeyDown(KeyCode.LeftShift) && this.isGrounded)
        {

            this.currentMoveSpeed = this.runSpeed;

        }

        //Si estamos en el aire o suelo y levantamos shift, cambia a andar

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {

            this.currentMoveSpeed = this.moveSpeed;

        }

    }

    private void FixedUpdate()
    {

        //Si la camara esta asignada ejecuta lo siguiente o cambiando¿?

        if (this.m_camera != null)
        {

            /*
              Forward significa la dirrecion en la que apunta la camara y el vector quiere decir de donde está a donde deberia ir
              El normalizado quiere decir para que el vector vaya de unidad en unidad haciedo que sean iguales
              Si quisiese el doble de velocidad multiplicaria el vector por dos, haciendolo aun mas rapido

              Ajusto la direccion a la que quiero ir con respecto a donde deberia ir la cabeza del personaje
              Para saber exactamente hacia donde tengo que ir
            */

            //Scale multiplica cada componente de cada coordenada, camara x * vector x y crea uno nuevo con estas multiplicaciones
            //el forward solo nos proporciona el eje z, el right el x

            this.cameraForward = Vector3.Scale(this.m_camera.forward, new Vector3(1, 0, 1)).normalized;

            /*
              El movimiento que queremos conseguir será la direccion se obtiene del vertical
              multiplicado por la dirreccion de la camara, donde mira el jugador más
              cuanto tengo que moverlo hacia la izquierda o derecha en funcion de como este girada la camara
            */

            this.move = this.vertical * this.cameraForward + this.horizontal * this.m_camera.right;
        }
        else
        {

            /*
              Movemos el personaje solo en x e y pero no mezclas el angulo de la camara (como en Unity cuando deplazamos solo un eje)
              Son dos vectores que solo poseen x y z (1,0,0), (0,0,1)
            */

            this.move = this.vertical * Vector3.forward + horizontal * Vector3.right;

        }

        /*
          Si hay desplazamiento ejecutamos el vector para transferir el movimiento al jugador
          Sera lo mismo cuando haya que rotar invocando su corresponiente metodo
        */

        if(this.move.magnitude > 0.0f && !PlayerManager.hasDead)
        {

            Move(this.move);

        }

        if (!this.isGrounded)
        {

            HandleAirborneMovement();

        }

        if (isGrounded && this.move.magnitude > 0.0f)
        {

            this.m_animator.speed = this.moveSpeedMultiplier;

        }
        else
        {

            this.m_animator.speed = 1.0f;

        }

    }

    //Comprobamos si estamos en el suelo tirando un rayo contra el suelo

    void CheckGroundStatus()
    {

        //If para visualizar el rayo que vamos trazando, lo llamo gizmo, arriba a la derecha en la escena

#if UNITY_EDITOR
        Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.down * 7.0f, Color.red);
#endif

        RaycastHit hitInfo;

        //Lanzamos un rayo desde los tobillos del jugador y si el hitinfo detecta la colision nos muestra en que punto colisiona

        if (Physics.Raycast(transform.position + Vector3.up * 1.0f, Vector3.down, out hitInfo, this.groundCheckDistance))
        {

            isGrounded = true;

            //Vector normal a la superficie a la que colisiona

            this.groundNormal = hitInfo.normal;
            this.m_animator.applyRootMotion = true;


        }
        else
        {

            isGrounded = false;

            //Si no hay colision se detecta como un vector hacia arriba

            this.groundNormal = Vector3.up;
            this.m_animator.applyRootMotion = false;

        }

    }

    //Vector de movimiento

    private void Move(Vector3 movement)
    {
        //Necesitamos que todos los vectores esten normalizados y nos den 1 de largo

        if(movement.magnitude > 1.0f)
        {

            //Lo situamos en uno la direccion que nos proporciona

            movement.Normalize();

        }

        /*
          Convierte el movimiento de coordenadas globales a locales
          Debido a que el movimiento se lo aplicamos a un hijo del personaje
          Su contrario es Transform.TransformDirection.
        */

        movement = transform.InverseTransformDirection(movement);
        CheckGroundStatus();

        /*
          Calculamos exactamente el vector que le sale de la cabeza al jugador y lo proyectamos en el suelo
          El vector que es perpendicular al suelo nos indica hacia donde estamos caminando
          Modificamos el vector normal a la superficie sobre la que camina
          A cuanta mayor inclinacion hay en un plano mas cuenta subirlo, como una montaña, un rayo inclinado hacia arriba 
          relacionado con la proyeccion del mismo que representa su movimiento
        */

        movement = Vector3.ProjectOnPlane(movement, this.groundNormal);

        //La cantidad de angulo que tiene que rotar con el vector de movimiento, nos devuelve un angulo conocida la tangente (largo y ancho del triangulo)

        this.turnAmount = Mathf.Atan2(movement.x, movement.z);

        //Es la cantidad de movimiento hacia adelante del eje z

        this.forwardAmount = movement.z;

        //Aplicamos el movimiento

        this.m_rigidbody.velocity = transform.forward * currentMoveSpeed;

        //Aplicamos la rotacion si la hubiese

        ApplyExtraRotation();

    }

    //Vamos a tratar de hacer que la rotacion sea rapida y fluida

    void ApplyExtraRotation()
    {

        //Con el lineal interpolation calculamos la velocidad del giro, entre dos vectores y un tiempo

        float turnSpeed = Mathf.Lerp(this.stationaryTurnAround, this.movingTurnSpeed, this.forwardAmount);

        //Aplicamos la rotacion su velocidad en el eje Y porque giramos la cabeza
        //s = v * t -> cantidad de tiempo desde el ultimo frame * angulo que me quiero mover, la arcotangente * la velocidad para un giro fluido

        transform.Rotate(0, turnSpeed * this.turnAmount * Time.deltaTime, 0);

    }

    //Metodo de la clase monodevelop, cuando se hace un frame de animacion, lo aceleramos correctamente en funcion de la posicion de la animacion

    private void OnAnimatorMove()
    {
        
        //Si esta en el suelo y el tiempo de frame es mayor que 0, asigna al vector vel la posicion del ultimo frame del animator * (a = v/t)
        //Jugaremos con la velocidad del movimiento del personaje

        if (this.isGrounded && Time.deltaTime > 0)
        {

            Vector3 vel = this.m_animator.deltaPosition * this.moveSpeedMultiplier / Time.deltaTime;

            //No varia la velocidad en el eje y

            vel.y = this.m_rigidbody.velocity.y;

            //Le asignamos a vector del rigidbody la velocidad obtenida

            this.m_rigidbody.velocity = vel;

        }

    }

    void HandleAirborneMovement()
    {

        Vector3 extraGravityForce = (Physics.gravity * this.m_GravityMultiplier) - Physics.gravity;
        this.m_rigidbody.AddForce(extraGravityForce);

        //Metodo para asegurarse de que no se traspasa el suelo
        //this.groundCheckDistance = this.m_rigidbody.velocity.y < 0 ? this.m_OrigGroundCheckDistance : 0.01f;

    }

    //ScaleCapsuleForCrouching (crouch), cambia la escala del collidercapsula
    /*
    - ForceMode.Force : acceleration, depends on mass, call in FixedUpdate
    - ForceMode.VelocityChange : changes the velocity, undependant of mass, doesn't say if you call it once or have to keep calling it, but my bet is you call it only once
    - ForceMode.Acceleration : same as force, but ignores mass
    - ForceMode.Impulse : changes the velocity, depends on mass, I found out you should only call this once, otherwise your object will quite quickly reach absurd speeds
     */
}
