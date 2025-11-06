using System;
using UnityEngine;

abstract public class BehaviorTree : MonoBehaviour
{
    protected Node root;
    public Node activeNode;

    private bool treeFinished = false;

    abstract protected void InitializeTree();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeTree();
        EvaluateTree();
    }

    // Update is called once per frame
    void Update()
    {
        if (activeNode != null)
        {
            Debug.Log("Ticking: " + activeNode.GetType().ToString());
            activeNode.Tick(Time.deltaTime);
        }
        else if(treeFinished)
        {
            treeFinished = false;
            EvaluateTree();
        }

    }
    //public void OnTreeFinished()
    //{
    //    treeFinished = true;
    //    activeNode = null;
    //}
    public void EvaluateTree()
    {
        root.EvaluateAction();    
    }
    public void Interupt()
    {
        activeNode.Interupt();
        EvaluateTree();
        //if (activeNode != null)
        //    activeNode.Interupt();
        //OnTreeFinished();
    }
}
