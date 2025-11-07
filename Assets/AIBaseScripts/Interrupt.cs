using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Interrupt
{
    Condition[] conditions;
    BehaviorTree tree;
    bool[] conditionsState;

    CancellationTokenSource cts;

    public Interrupt(BehaviorTree tree, Condition[] conditions)
    {
        this.conditions = conditions;
        this.tree = tree;
        conditionsState = new bool[conditions.Length];
        
        Start();
    }

    async void CheckConditions(CancellationToken token) // marche comme un Update
    {
        while (!token.IsCancellationRequested)
        {
            for (int i = 0; i < conditions.Length; i++)
            {
                if (conditions[i].Evalutate() != conditionsState[i])
                {
                    tree.Interupt();
                    UpdateState();
                    return; //juste pour testé le interrupt une fois par frame mettre le return en commentaire 

                }
            }

            await Task.Delay(100);
        }

    }

    void UpdateState()
    {
        for (int i = 0; i < conditions.Length; i++)
        {
            bool currentState = conditions[i].Evalutate();
            if (currentState != conditionsState[i])
            {
                conditionsState[i] = currentState;
            }
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
