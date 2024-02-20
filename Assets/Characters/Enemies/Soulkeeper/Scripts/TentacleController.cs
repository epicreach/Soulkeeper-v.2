using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleController : MonoBehaviour
{

    Damagable damagable;

    int damage = 20;
    bool canAttack = true;
    float timeSinceDeath = 0f;
    [SerializeField]
    float timeToDisapear = 0.5f;
    [SerializeField]
    AudioClip deathClip;
    void Awake() {
        damagable = GetComponent<Damagable>();
    }



    void OnTriggerStay2D(Collider2D other) {

        if (other.CompareTag("Player") && canAttack) {
            Damagable temp = other.GetComponent<Damagable>();
            if (temp != null) {
                temp.Hit(damage);
            }
        }
    }


    public void killTentacle() {
        if (canAttack)
        {
            GetComponent<AudioSource>().PlayOneShot(deathClip);
        }
        canAttack = false;
        timeSinceDeath += Time.deltaTime;
        if (timeSinceDeath > timeToDisapear)
        {
            Destroy(gameObject);
        }
        
    }

    void FixedUpdate() {
        if (damagable.Health <= 0) {
            killTentacle();
            }
    }


}
