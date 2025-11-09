using System;
using UnityEngine;
using UnityEngine.AI;

public class PatrolAnimatronicComponent : MonoBehaviour
{
    //************************ animation ************************//
    AnimatronicState currentState = AnimatronicState.Idle;
    Animator animator;

    //************************ value ************************//
    [SerializeField] float walkSpeed = 10f;
    [SerializeField] float runSpeed = 20f;

    //************************ agent ************************//
    NavMeshAgent agent;

    //************************ SFX ************************//
    FootstepAudio footstepAudio;
    [SerializeField] AudioClip jumpscare_sfx;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        footstepAudio = GetComponent<FootstepAudio>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeAnimationOnVelocity();
        ChangeSoundOnVelocity();
    }

    public void SetCanRun(bool canRun)
    {
        if (canRun)
        {
            agent.speed = runSpeed;
        }
        else
        {
            agent.speed = walkSpeed;
        }
    }

    void ChangeAnimationOnVelocity()
    {
        if (animator == null)
            return;
        float curentSpeed = agent.velocity.magnitude;
        if (curentSpeed < 0.1f)
        {
            SetAnimatronicState(AnimatronicState.Idle);
           // footstepAudio.StopSound();
        }
        else if(curentSpeed > 0.1f && curentSpeed < 10f)
        {
           // footstepAudio.PlayWalkSound();
            SetAnimatronicState(AnimatronicState.Walking);
        }
        else if (curentSpeed >= 10f)
        {
           // footstepAudio.PlayRunSound();
            SetAnimatronicState(AnimatronicState.Running);
        }
    }

    void ChangeSoundOnVelocity()
    {
        if(footstepAudio == null)
            return;
        float curentSpeed = agent.velocity.magnitude;
        if (curentSpeed < 0.3f)
        {
            footstepAudio.StopSound();
        }
        else if (curentSpeed > 0.1f && curentSpeed < 20f)
        {
            footstepAudio.PlayWalkSound();
        }
        //else if (curentSpeed >= 20f)
        //{
        //    footstepAudio.PlayRunSound();
        //}
    }

    public void JumpScare()
    {
        animator.SetTrigger("jumpscare");
        SFXManager.Instance.PlaySFX(jumpscare_sfx, transform,0.5f);

    }

    public void SetAnimatronicState(AnimatronicState newState)
    {
        if (currentState == newState)
            return;
        currentState = newState;
        switch (currentState)
        {
            case AnimatronicState.Idle:
                animator.SetBool("isWalking", false);
                break;
            case AnimatronicState.Walking:
                animator.SetBool("isWalking", true);
                animator.SetFloat("animationSpeed", 1f);
                agent.speed = walkSpeed;
                break;
            case AnimatronicState.Running:
                animator.SetFloat("animationSpeed", 2f);
                agent.speed = runSpeed;
                break;
            default:
                break;
        }
    }
}

public enum AnimatronicState
{
    Idle,
    Walking,
    Running
}
