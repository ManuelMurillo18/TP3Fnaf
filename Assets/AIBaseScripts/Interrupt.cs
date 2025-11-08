using System;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class Interrupt
{
    Condition[] conditions;
    BehaviorTree behaviorTree;
    bool[] conditionsState;

    CancellationTokenSource cts;


    float cooldown;  // délai minimum entre deux interruptions
    float lastInterruptTime = -10f;
    public Interrupt(BehaviorTree behaviorTree, Condition[] conditions, float cooldown = 2f)
    {
        this.behaviorTree = behaviorTree;
        this.conditions = conditions;
        this.cooldown = cooldown;
        conditionsState = new bool[conditions.Length];

        Start();
    }

    //async private void CheckConditions(CancellationToken token)
    //{
    //    while (!token.IsCancellationRequested)
    //    {
    //        for (int index = 0; index < conditions.Length; ++index)
    //        {
    //            if (conditions[index].Evaluate() != conditionsState[index])
    //            {
    //                // Empêche l’interruption trop fréquente
    //                if (Time.time - lastInterruptTime >= cooldown)
    //                {
    //                    lastInterruptTime = Time.time;
    //                    behaviorTree.Interupt();
    //                }

    //                //Debug.Log("Interrupting Behavior Tree due to condition change.");
    //                //behaviorTree.Interupt();
    //                UpdateState();
    //                break;
    //            }
    //        }
    //        await Task.Delay(100);
    //    }
    //}

    async private void CheckConditions(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            for (int index = 0; index < conditions.Length; ++index)
            {
                bool current = conditions[index].Evaluate();
                // Interrompt uniquement si la condition passe de false à true
                if (!conditionsState[index] && current)
                {
                    if (Time.time - lastInterruptTime >= cooldown)
                    {
                        lastInterruptTime = Time.time;
                        behaviorTree.Interupt();
                    }
                    UpdateState();
                    break;
                }
                // Met à jour l'état même si pas d'interruption
                conditionsState[index] = current;
            }
            await Task.Delay(1000);
        }
    }

    private void UpdateState()
    {
        for (int index = 0; index < conditions.Length; ++index)
        {
            conditionsState[index] = conditions[index].Evaluate();
        }
    }

    public void Start()
    {
        cts = new CancellationTokenSource();
        UpdateState();
        CheckConditions(cts.Token);
    }

    public void Stop()
    {
        cts.Cancel();
    }
}