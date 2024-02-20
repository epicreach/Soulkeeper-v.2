using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioScript : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    AudioClip clip;

    Damagable damagable;
    int health;

    void Start()
    {
        damagable = GetComponent<Damagable>();
        health = damagable.Health;
    }

    // Update is called once per frame
    void Update()
    {
        if (damagable.Health < health)
        {
            audioSource.PlayOneShot(clip);

        }
        health = damagable.Health;
    }
}
