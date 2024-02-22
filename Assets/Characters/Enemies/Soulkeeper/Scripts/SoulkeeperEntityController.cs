using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SoulkeeperEntityController : MonoBehaviour
{
    AudioSource audioSrc;
    [SerializeField]
    AudioClip teleportClip;
    Damagable damagable;

    BoxCollider2D collider;
    int health;

    Animator animator;

    void Awake() {
        damagable = GetComponent<Damagable>();
        health = damagable.Health;
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
        audioSrc = GetComponent<AudioSource>();
    }

    void FixedUpdate() {

        if (health != damagable.Health) {
            animator.Play("SoulkeeperSink");
            health = damagable.Health;
            collider.enabled = false;
            audioSrc.PlayOneShot(teleportClip);
            Invoke("Teleport", 1.2f);
        }

        if (damagable.Health <= 0) {
            Destroy(gameObject);
            SceneManager.LoadSceneAsync(4);
        }
    }

    public void Teleport() {
        float currentX = 0;
        float newX = 0;

        newX = Random.Range(5,35);
        transform.position = new Vector2(newX, transform.position.y);
        collider.enabled = true;
    }

}
