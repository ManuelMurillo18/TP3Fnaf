using UnityEngine;
using UnityEngine.AI;

public class FinalBossComponent : MonoBehaviour
{
    //************************ animation ************************//
    AnimatronicState currentState = AnimatronicState.Idle;
    Animator animator;

    //************************ agent ************************//
    NavMeshAgent agent;

    //************************ SFX ************************//
    FootstepAudio footstepAudio;
    [SerializeField] AudioClip jumpscare_sfx;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        footstepAudio = GetComponent<FootstepAudio>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeSoundOnVelocity();
    }


    void ChangeSoundOnVelocity()
    {
        if (footstepAudio == null)
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
        footstepAudio.StopSound();
        SFXManager.Instance.PlaySFX(jumpscare_sfx, transform, 0.5f);

    }
}
