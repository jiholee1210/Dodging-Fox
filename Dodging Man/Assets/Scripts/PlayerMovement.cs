using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpPower = 1f;
    [SerializeField] float acceleration = 50f;
    [SerializeField] float deceleration = 30f;
    [SerializeField] private LayerMask groundLayer;
    
    float inputValue;
    float currentSpeed;
    int playerHp;
    bool isJumping;
    
    private Rigidbody2D rigidbody2D;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        playerHp = 3;
        HpManager.Instance.ResetHp();
    }

    private void FixedUpdate() {
        float targetSpeed = inputValue * speed;
        JumpCheck();

        animator.SetFloat("Speed", Mathf.Abs(inputValue));
        if(Mathf.Abs(targetSpeed) > 0.01f) {
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.fixedDeltaTime);
        }
        else {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.fixedDeltaTime);
        }
        rigidbody2D.linearVelocityX = currentSpeed;
        
        if(inputValue != 0) {
            spriteRenderer.flipX = inputValue < 0;
        }
    }

    private void OnMove(InputValue value) {
        inputValue = value.Get<Vector2>().x;
    }

    private void OnJump() {
        if(!isJumping) {
            isJumping = true;
            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);
            audioSource.PlayOneShot(AudioManager.Instance.jumpClip);
            rigidbody2D.linearVelocityY = 0f;
            rigidbody2D.AddForceY(jumpPower, ForceMode2D.Impulse);
        }
    }

    private void JumpCheck() {
        float verticalVelocity = rigidbody2D.linearVelocityY;
        if(verticalVelocity < 0) {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", true);
        }
    }

    private void CheckWallCollision(Collision2D other) {
        foreach (ContactPoint2D contact in other.contacts)
        {
            // 충돌 법선의 x값이 거의 1이나 -1이면 수평 충돌
            if (Mathf.Abs(contact.normal.x) > 0.9f)
            {
                // 현재 속도와 currentSpeed를 0으로 리셋
                currentSpeed = 0;
                rigidbody2D.linearVelocityX = currentSpeed;
                break;
            }
        }
    }

    public void OnHit(Vector2 hitPosition) {
        StartCoroutine(Hurt(transform.position));
    }

    private IEnumerator Hurt(Vector2 hitPosition) {
            animator.SetBool("isDamaged", true);
            playerHp -= 1;
            HpManager.Instance.UpdateHp(playerHp);
            gameObject.layer = LayerMask.NameToLayer("DamagedPlayer");
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);

            rigidbody2D.linearVelocity = Vector3.zero;
            rigidbody2D.position = hitPosition;
            rigidbody2D.simulated = false;

            yield return new WaitForSeconds(0.5f);
        if(playerHp > 0) {
            animator.SetBool("isDamaged", false);
            rigidbody2D.simulated = true;

            yield return new WaitForSeconds(1f);

            gameObject.layer = LayerMask.NameToLayer("Player");
            spriteRenderer.color = new Color(1, 1, 1, 1f);
        }
        else {
            GameManager.Instance.EndGame();
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        //Debug.Log(other.gameObject.layer);
        if (((1 << other.gameObject.layer) & groundLayer) != 0) {
            //Debug.Log("경계나 벽에 박음");
            CheckWallCollision(other);
        }
    }

    private void OnCollisionStay2D(Collision2D other) {
        //Debug.Log(other.gameObject.layer);
        if (((1 << other.gameObject.layer) & groundLayer) != 0) {
            //Debug.Log("경계나 벽에 박음");
            CheckWallCollision(other);
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        if (((1 << other.gameObject.layer) & groundLayer) != 0)
        {
            Debug.Log("땅에 닿음");
            isJumping = false;
            animator.SetBool("isFalling", false);
            animator.SetBool("isJumping", false);
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (((1 << other.gameObject.layer) & groundLayer) != 0)
        {
            animator.SetBool("isFalling", false);
            animator.SetBool("isJumping", false);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        
        if (((1 << other.gameObject.layer) & groundLayer) != 0) {
            Debug.Log("땅에서 떨어짐");
        }
    }
}
