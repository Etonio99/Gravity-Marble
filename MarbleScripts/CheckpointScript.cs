using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    private bool activated = false;
    private ParticleSystem particles;

    private Vector3 respawnAngle;

    private void Start()
    {
        particles = transform.GetChild(1).GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.tag == "Player" && !activated)
        {
            activated = true;
            particles.Play();
            MarbleScript _ms = _other.GetComponent<OnMarbleScript>().controller;
            respawnAngle = _ms.transform.eulerAngles;
            _ms.SetRespawnPoint(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z));
            _ms.SetRespawnAngle(respawnAngle);
        }
    }
}