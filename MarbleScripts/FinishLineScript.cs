using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLineScript : MonoBehaviour
{
    private float timer = 0f;
    private bool finished = false;

    private void Update()
    {
        if (!finished)
            timer += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.tag == "Player" && !finished)
        {
            finished = true;
            Debug.Log(timer);
        }
    }
}