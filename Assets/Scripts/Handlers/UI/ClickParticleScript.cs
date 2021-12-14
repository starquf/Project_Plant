using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickParticleScript : MonoBehaviour
{
    private List<ParticleSystem> particles = new List<ParticleSystem>();

    private void Start()
    {
        GetComponentsInChildren(particles);
        print("test");
    }

    private void Update()
    {
        GetClick();
    }

    private void GetClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            for (int i = 0; i < particles.Count; i++)
                particles[i].Play();
        }
    }
}
