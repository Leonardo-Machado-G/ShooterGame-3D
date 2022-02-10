using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatleNPC : MonoBehaviour
{
    
    Animator m_Animator;

    public GameObject nextCucumberToDestroy;

    //Golpeo por cereza

    public bool cherriHit = false;
    public float smoothTime = 3.0f;
    private bool hasReachThePlayer = true;
    public Vector3 smoothVelocity = Vector3.zero;
    

    public HealthManager healthManager;


    void Start()
    {

        this.m_Animator = GetComponent<Animator>();

        this.healthManager = GameObject.Find("Health Slider").GetComponent<HealthManager>();

    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {

            this.healthManager.ReduceHealth(20);

            //Cuando colisiona con el jugador le mira y le ataca sin cambiar su rotacion mas
            //solo desplazarse hacia esa direccion en la que ha quedado

            if (!this.cherriHit)
            {

                GameObject thePlayer = collision.gameObject;
                Transform trans = thePlayer.transform;
                this.gameObject.transform.LookAt(trans);
                this.m_Animator.Play("Attack_OnGround");
                BeatlePatrol.isAttacking = true;
                StartCoroutine(DestroyBeatle());

            }
            else
            {
                
                this.m_Animator.Play("Attack_Standing");
                StartCoroutine(DestroyBeatleStanding());

            }

            this.hasReachThePlayer = false;

        }

    }

    private void OnTriggerEnter(Collider collision)
    {

        //Al morir el collider gira con la animacion, se hace desaparecer el collider o se puede usar muñecas de trapo o ragdoll, un muñeco desmontado
        //Formas alternativas de activar animaciones
        
        //Si choco con un pepinillo se activa en animacion y bool del controller, comer y se asigna una variable al objeto

        if (collision.gameObject.CompareTag("Cucumber"))
        {

            this.nextCucumberToDestroy = collision.gameObject;

            this.m_Animator.Play("Eat_OnGround");
            BeatlePatrol.isEating = true;
            StartCoroutine(DestroyCucumber());


        }
        
        if (collision.gameObject.CompareTag("Cherry"))
        {

            BeatlePatrol.isAttacking = true;
            this.cherriHit = true;

        }

    }

    IEnumerator DestroyBeatleStanding()
    {
        
        yield return new WaitForSecondsRealtime(4.0f);
        this.cherriHit = false;
        this.hasReachThePlayer = false;
        Destroy(this.gameObject, 2.0f);
        this.m_Animator.Play("Die_Standing");
        PointsManager.AddPoints(25);

    }

    IEnumerator DestroyBeatle()
    {

        //Se activa durante 4 s reales, destruye al bicho despues de 2 s
        
        yield return new WaitForSecondsRealtime(4.0f);
        BeatlePatrol.isAttacking = false;
        this.hasReachThePlayer = false;
        Destroy(this.gameObject, 2.0f);
        this.m_Animator.Play("Die_OnGround");
        PointsManager.AddPoints(25);

    }

    IEnumerator DestroyCucumber()
    {

        //Se activa durante 3 s, destruye el objeto y dejamos de comer

        yield return new WaitForSeconds(3.0f);
        this.m_Animator.Play("Eat_OnGround");
        Destroy(this.nextCucumberToDestroy.gameObject);
        BeatlePatrol.isEating = false;

    }
    

    private void Update()
    {
        
        if(this.cherriHit)
        {

            //Si la variable esta en true asigna al gameobj, con el tag player
            //Mira a la posicion del player, lo localiza, consigue su posicion, mira al protagonista
            //Se levanta a dos patas

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Transform transPlayer = player.transform;
            this.gameObject.transform.LookAt(transPlayer);


            if(!this.hasReachThePlayer)
            {
                
                this.m_Animator.Play("Run_Standing");

            }
            //Ira a las posicion del jugador el beatle con ref, que se autoconfigura calculando la velocidad (un puntero una referencia en C)

            transform.position = Vector3.SmoothDamp(transform.position, transPlayer.position, ref this.smoothVelocity, this.smoothTime);

        }

    }

}
