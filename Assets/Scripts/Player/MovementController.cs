using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;


[DefaultExecutionOrder(-1)] // Set the execution order to -1 for MovementController
[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class PlayerController : MonoBehaviour
{

    private Animator animator;

    private float rollSpeed = 10.0f;
    private float rollDuration = 0.6f;

    private bool isRolling;

    public float walkSpeed = 5f;
    public float dashForce = 2f;
    public float dashDuration = 0.4f;
    private bool isDashing = false;
    bool moving = false;
   

    private float dashCooldownTimer = Mathf.Infinity;
    [SerializeField] private float dashCooldown;

    [SerializeField]
    AudioSource audioSrc;
    public DefaultPlayerInputs input = null;

    private TouchingDirections touchingDirections;

    public SpriteRenderer spriteRenderer;

    public Vector2 inputVector = Vector2.zero;

    [SerializeField] private Damagable damagable;

    Rigidbody2D rb;

    private int health;
    bool inAir = false;
    bool hitBack = false;
    float hitBackTime = 0.1f;
    float timeInAir = 0f;
    [SerializeField]
    private float hitBackForce = 5f;
    private float playerMass;
    
    private void Awake() {
        touchingDirections = GetComponent<TouchingDirections>();
        input = new DefaultPlayerInputs();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        damagable = GetComponent<Damagable>();
        health = damagable.MaxHealth;
        playerMass = rb.mass;
    }

    private void OnEnable() {
        input.Enable();
        input.Player.Move.performed += OnMovementPerformed;
        input.Player.Move.canceled += OnMovementCancelled;
        input.Player.Dash.performed += OnDashPerformed;
        input.Player.Roll.performed += OnRollPerformed;

    }

    private void OnDisable() {
        input.Disable();
        input.Player.Move.performed -= OnDashPerformed;
        input.Player.Dash.canceled -= OnDashPerformed;
        input.Player.Roll.performed -= OnRollPerformed;

    }


    void FixedUpdate() {

        if (!touchingDirections.IsGrounded)
        {
            audioSrc.Stop();

        }
        else if(touchingDirections.IsGrounded && inAir && moving)
        {
            inAir = false;
            audioSrc.Play();
        }
        inAir = !touchingDirections.IsGrounded;

        if (damagable.Health == 0){
            SceneManager.LoadSceneAsync(5);
       }

        if (isDashing || isRolling) {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            return;
        }
        dashCooldownTimer += Time.deltaTime;

        

        if (health > damagable.Health || hitBack)
        {
            Debug.Log("Hitback");
            HitBack();
            animator.SetTrigger("hurt");
        }
        else if(hitBack == false)
        {

            rb.velocity = new Vector2(inputVector.x * walkSpeed, rb.velocity.y);
        }
        health = damagable.Health;

        
    }
    
    private void HitBack()
    {
        hitBack = true;
        GameObject attacker = damagable.GetObjectThatAttacked();
        
        Debug.Log(attacker);
        
        if (hitBackTime > timeInAir && attacker != null)
        {
            timeInAir += Time.deltaTime;
            rb.velocity = hitBackForce * (new Vector2(rb.position.x - attacker.transform.position.x, Mathf.Abs(rb.position.x - attacker.transform.position.x))) .normalized;
            rb.mass = 0;
           
        }
        else
        {
            rb.mass = playerMass;
            timeInAir = 0;
            hitBack = false;
        }

        
        
            
        
    }

    private void OnMovementPerformed(InputAction.CallbackContext context) {
        inputVector = context.ReadValue<Vector2>();
        animator.SetBool("IsRunning", true);

        audioSrc.Play();

        if (inputVector.x < 0) {
            FlipPlayer(-1f);
        }
        else {
            FlipPlayer(1f);
        }
        moving = true;

    }

    private void OnMovementCancelled(InputAction.CallbackContext context) {
        inputVector = Vector2.zero;
        animator.SetBool("IsRunning", false);
        audioSrc.Stop();
        moving = false;
    }

    private void OnDashPerformed(InputAction.CallbackContext context) {
        if (dashCooldownTimer >= dashCooldown) {
            StartCoroutine(Dash());
            dashCooldownTimer = 0;
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        animator.SetBool("IsDashing", true);
        float playerDirection = transform.localScale.x;
        rb.velocity = new Vector2(playerDirection * (walkSpeed * dashForce), 0);
        yield return new WaitForSeconds(dashDuration);
        animator.SetBool("IsDashing", false);
        isDashing = false;

    }

    public void FlipPlayer(float direction) {
            Vector3 localScale = transform.localScale;
            localScale.x = direction;
            transform.localScale = localScale;
    }



    void OnRollPerformed(InputAction.CallbackContext context) {
      if (touchingDirections.IsGrounded) {
        StartCoroutine(Roll());
      }
    }

    IEnumerator Roll() {
        isRolling = true;
        
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("SkeletonEnemy"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Boss"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Tentacle"), true);
        damagable.SetInvincibility(true);
        animator.SetTrigger("Rolling");
        float playerDirection = transform.localScale.x;
        rb.velocity = new Vector2(playerDirection * rollSpeed, 0);
        yield return new WaitForSeconds(rollDuration);
        damagable.SetInvincibility(false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("SkeletonEnemy"), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Boss"), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Tentacle"), false);

        isRolling = false;
    }

}
