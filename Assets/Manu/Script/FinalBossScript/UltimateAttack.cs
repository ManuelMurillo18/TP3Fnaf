using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UltimateAttack : Node
{
    GameObject[] friendsPrefab;
    Transform[] friendsSpawnPoint;
    NavMeshAgent agent;
    float duration;
    float elapsedTime = 0f;
    Animator animator;
    bool friendSummoned = false;
    List<GameObject> summonedFriends = new List<GameObject>();
    Interrupt interrupt;
    ManuPlayerComp playerComp;
    float previousBatterieLevel;

    public UltimateAttack(ManuPlayerComp playerComp,Animator animator,NavMeshAgent agent, GameObject[] friendsPrefab, Transform[] friendsSpawnPoint, float duration,Interrupt interrupt, Condition[] conditions, BehaviorTree BT) : base(conditions, BT)
    {
        this.playerComp = playerComp;
        this.agent = agent;
        this.friendsPrefab = friendsPrefab;
        this.friendsSpawnPoint = friendsSpawnPoint;
        this.duration = duration;
        this.animator = animator;
        this.interrupt = interrupt;
    }

    public override void EvaluateAction()
    {
        elapsedTime = 0f;
        if (!friendSummoned)
        {
            base.EvaluateAction();
            for (int i = 0; i < friendsPrefab.Length; i++)
            {
                friendsPrefab[i].SetActive(true);
                friendsPrefab[i].transform.position = friendsSpawnPoint[i].position;
                summonedFriends.Add(friendsPrefab[i]);
            }
            friendSummoned = true;
        }
        
        JAM();
    }

    public void JAM()
    {
        if(interrupt != null)
            interrupt.Stop();
        agent.isStopped = true;
        previousBatterieLevel = playerComp.flashlightBattery;
        playerComp.flashlightBattery = 0f;
        GameManagerManu.Instance.PlayEventMusic();
        playerComp.CameraShake(duration,50f);
        playerComp.TakeDamage(5);
        Debug.Log("JAM");
        
    }

    public override void Tick(float deltaTime)
    {
        if (elapsedTime < duration)
        {
            animator.SetBool("isDancing", true);
            elapsedTime += deltaTime;
        }
        else
        {
            animator.SetBool("isDancing", false);
            agent.isStopped = false;
            foreach (GameObject friend in summonedFriends)
            {
                friend.SetActive(false);
            }
            Debug.Log("JAM finished!");
            elapsedTime = 0;
            friendSummoned = false;
            if (interrupt != null)
                interrupt.Start();
            playerComp.flashlightBattery = previousBatterieLevel;
            FinishAction(true);
        }
    }
}
