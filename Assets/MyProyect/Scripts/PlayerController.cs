using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //Controladores tanto de String como bool para crear cambiamos en las animaciones

    public string idleState, walkState, runState, jumpState, throwState, dieState;
    public bool isWalking, isRunning, isJumping, isIdle, isDead, forward, backward, left, right;

    //Animator

    Animator m_animator;

    void Start()
    {

        //Por defecto estamos andando y asignamos el animator

        this.m_animator = GetComponent<Animator>();
        this.isIdle = true;

    }

    private void Update()
    {

        if(PlayerManager.livesRemaining == 0)
        {

            this.m_animator.Play("Die");

        }

        //DOWN STATES

        if (Input.GetKeyDown(KeyCode.W))
        {

            if(!isRunning)
            {

                this.isWalking = true;
                this.isIdle = false;
                this.forward = true;
                this.m_animator.SetBool(this.idleState, false);
                this.m_animator.SetBool(this.walkState, true);

            }

        }

        if(Input.GetKeyDown(KeyCode.A))
        {

            this.isWalking = true;
            this.isIdle = false;
            this.left = true;
            this.m_animator.SetBool(this.idleState, false);
            this.m_animator.SetBool(this.walkState, true);

        }

        if (Input.GetKeyDown(KeyCode.S))
        {

            this.isWalking = true;
            this.isIdle = false;
            this.backward = true;
            this.m_animator.SetBool(this.idleState, false);
            this.m_animator.SetBool(this.walkState, true);

        }

        if (Input.GetKeyDown(KeyCode.D))
        {

            this.isWalking = true;
            this.isIdle = false;
            this.right = true;
            this.m_animator.SetBool(this.idleState, false);
            this.m_animator.SetBool(this.walkState, true);

        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {

            if(this.isWalking)
            {

                this.isRunning = true;
                this.m_animator.SetBool(this.runState, true);
                this.m_animator.SetBool(this.walkState, false);

            }

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {

            Jump();

        }

        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {

            Throw();

        }


        //Action UP (acabar una accion)

        if (Input.GetKeyUp(KeyCode.W))
        {

            this.forward = false;

            if(!this.backward && !this.left && !this.right)
            {

                StopMotion();

            }

        }

        if (Input.GetKeyUp(KeyCode.A))
        {

            this.left = false;

            if (!this.backward && !this.forward && !this.right)
            {

                StopMotion();

            }

        }

        if (Input.GetKeyUp(KeyCode.S))
        {

            this.backward = false;

            if (!this.left && !this.forward && !this.right)
            {

                StopMotion();

            }


        }

        if (Input.GetKeyUp(KeyCode.D))
        {

            this.right = false;

            if (!this.backward && !this.forward && !this.forward)
            {

                StopMotion();

            }


        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {

            if(this.isRunning && this.isWalking)
            {

                this.isRunning = false;
                this.m_animator.SetBool(this.walkState, true);
                this.m_animator.SetBool(this.runState, false);

            }

        }
        
    }

    void StopMotion()
    {
        
        this.isWalking = false;
        this.isRunning = false;
        this.isIdle = true;

        this.m_animator.SetBool(this.runState, false);
        this.m_animator.SetBool(this.walkState, false);
        this.m_animator.SetBool(this.idleState, true);
        
    }

    void Jump()
    {

        this.m_animator.SetBool(this.jumpState, true);
        this.m_animator.SetBool(this.runState, false);
        this.m_animator.SetBool(this.walkState, false);
        this.m_animator.SetBool(this.idleState, false);
        StartCoroutine(ConsumeJump());

    }

    void Throw()
    {

        this.m_animator.SetBool(this.throwState, true);
        this.m_animator.SetBool(this.runState, false);
        this.m_animator.SetBool(this.walkState, false);
        this.m_animator.SetBool(this.idleState, false);
        StartCoroutine(ConsumeThrow());

    }

    IEnumerator ConsumeJump()
    {

        yield return new WaitForSeconds(0.66f);
        this.m_animator.SetBool(this.jumpState, false);

        ReturnToMoveState();

    }

    IEnumerator ConsumeThrow()
    {

        yield return new WaitForSeconds(0.66f);
        this.m_animator.SetBool(this.throwState, false);

        ReturnToMoveState();

    }

    void ReturnToMoveState()
    {

        if (this.isRunning)
        {

            this.m_animator.SetBool(this.runState, true);

        }
        else if (this.isWalking)
        {

            this.m_animator.SetBool(this.walkState, true);

        }
        else if (this.isIdle)
        {

            this.m_animator.SetBool(this.idleState, true);

        }

    }

}
