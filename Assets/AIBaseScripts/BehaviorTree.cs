using UnityEngine;

public abstract class BehaviorTree : MonoBehaviour
{
    protected Node root;
    public Node activeNode;

    private bool treeFinished = false;

    protected abstract void InitializeTree();

    void Start()
    {
        InitializeTree();
        EvaluateTree();
    }

    void Update()
    {
        if (activeNode != null)
        {
            activeNode.Tick(Time.deltaTime);
        }
        else if (treeFinished)
        {

            treeFinished = false;
            EvaluateTree();
        }
    }

    public void EvaluateTree()
    {

        activeNode = null;

        if (root != null)
            root.EvaluateAction();
    }

    public void OnTreeFinished()
    {
        treeFinished = true;
        activeNode = null;
    }

    public void Interupt()
    {
        if (activeNode != null)
            activeNode.Interupt();

        OnTreeFinished();
    }
}