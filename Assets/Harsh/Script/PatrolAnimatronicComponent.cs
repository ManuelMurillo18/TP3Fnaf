using System;
using UnityEngine;

public class PatrolAnimatronicComponent : MonoBehaviour
{
    public void JumpScare(GameObject player)
    {
        player.GetComponent<PlayerComponent>().JumpScare();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
