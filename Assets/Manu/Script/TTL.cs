using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTL : MonoBehaviour
{
    [SerializeField] float tempsDeVie = 3;
    float tempsInitial;

    void Start()
    {
        tempsInitial = Time.time;
    }

    void Update()
    {
        if (Time.time > tempsInitial + tempsDeVie)
        {
            Destroy(gameObject);
        }

    }
}
