using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Player playerStats;

    [SerializeField] private Collider2D interactablePlatform;

    [SerializeField] float jumpForce = 15f;
    [SerializeField] float groundCheckRadius = .5f;
    [SerializeField] float dashSpeed = 20f;
    [SerializeField] float dashTime = 0.1f;
    [SerializeField] float dashCoolDown = 2f;
    [SerializeField] float attackRange = 1f;
    [SerializeField] float startTimeBtwAttack = .3f;
    [SerializeField] float boxSizeX = 2f;
    [SerializeField] float boxSizeY = 2f;

    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlatform;
    [SerializeField] private LayerMask whatIsEnemies;

    [SerializeField] private TrailRenderer tr;

    private float jumpHeightMultiplier = 1f;
    private float movementInputDirection;
    private float timeBtwAttack = 0f;

    [SerializeField]private bool isGrounded;
    private bool isFacingRight = true;
    private bool checkJumpMultiplier;
    private bool isDashing = false;
    [SerializeField] private bool canDash = true;
    private bool isAttacking = false;
    [SerializeField]private bool jump = false;

    private List<string> AttackSFX = new List<string> {"attack1", "attack2"};
   
    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private BoxCollider2D playerCollider;

    public Transform groundCheck;
    public Transform attackPos;
  
    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        anim = transform.GetComponent<Animator>();
        playerStats = transform.GetComponent<Player>();
        transform.rotation = Quaternion.identity;
    }

    private void Update()
    {
        movementInputDirection = Input.GetAxis("Horizontal");
        if (isDashing)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.W) && isGrounded&&!isDashing)
        {
            jump = true;
            anim.SetBool("isJumping", true);
            AudioManager.Instance.PlayClipByName("jump");
            checkJumpMultiplier = true;
            rb.velocity = new Vector2(rb.position.x,jumpForce);
        }

        if (Input.GetKey(KeyCode.S) && interactablePlatform != null)
        {
            StartCoroutine(ColliderControl(interactablePlatform));
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && canDash)
        {
            anim.SetBool("isDashing",true);
            StartCoroutine(Dash());
        }

        if(timeBtwAttack <= 0)
        {
           if (Input.GetMouseButtonDown(0))
           {
               int index = Random.Range(0, AttackSFX.Count);
               if (EventSystem.current.IsPointerOverGameObject())
                    return;
               isAttacking = true;
               anim.SetBool("isAttacking", isAttacking);
               AudioManager.Instance.PlayClipByName(AttackSFX[index]);
               Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
               
               for(int i =0; i<enemiesToDamage.Length; i++)
               {
                    if (!enemiesToDamage[i].CompareTag("Boss") && !enemiesToDamage[i].CompareTag("Fungi") && !enemiesToDamage[i].CompareTag("Fungi Spirit"))
                    {
                        enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(playerStats.CurrAttackDamage);
                    }
                    else if(enemiesToDamage[i].gameObject.CompareTag("Fungi") || enemiesToDamage[i].gameObject.CompareTag("Fungi Spirit"))
                    {
                        if(enemiesToDamage[i] == enemiesToDamage[i].gameObject.GetComponent<Enemy>().attackArea)
                        {
                            enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(playerStats.CurrAttackDamage);
                        }
                    }
               }
           }
           timeBtwAttack = startTimeBtwAttack;
        }
        else
        {
           timeBtwAttack -= Time.deltaTime;
        }

        
    }

    private void FixedUpdate()
    {
        if(movementInputDirection != 0)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

        if (!isDashing)
        {
            rb.velocity = new Vector2(playerStats.CurrPlayerSpeed* movementInputDirection,rb.velocity.y);
        }

        isGrounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(boxSizeX, boxSizeY),0f, whatIsGround) ||
                     Physics2D.OverlapBox(groundCheck.position, new Vector2(boxSizeX, boxSizeY),0f, whatIsPlatform);
        
        interactablePlatform = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsPlatform);
            
        switch (movementInputDirection)
        {
            case > 0:
                isFacingRight = true;
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                break;
            case < 0:
                isFacingRight = false;
                transform.localRotation =Quaternion.Euler(0,180,0);
                break;
        }

        if (checkJumpMultiplier && !Input.GetKeyUp(KeyCode.W))
        {
            checkJumpMultiplier = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpHeightMultiplier);
        }
    }

    public void AttackEnd()
    {
        anim.SetBool("isAttacking", false);
    }
    public void JumpEnd()
    {
        anim.SetBool("isJumping", false);
    }
    public void DashEnd()
    {
        anim.SetBool("isDashing", false);
    }

    public void WalkSound()
    {
        AudioManager.Instance.PlayClipByName("jump");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("interactable platform") )
        {
            if(jump==true)
                AudioManager.Instance.PlayClipByName("landing");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("interactable platform"))
        {
            interactablePlatform = null;
        }
    }

    IEnumerator ColliderControl(Collider2D collider2D)
    {
        if (!collider2D) yield break;
        Physics2D.IgnoreCollision(playerCollider, collider2D);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(playerCollider, collider2D, false);
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        AudioManager.Instance.PlayClipByName("dash");
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        rb.velocity = new Vector2(isFacingRight ? 1 : -1, 0)* dashSpeed;
        Physics2D.IgnoreLayerCollision(7, 8,true);
        tr.emitting = true;
        yield return new WaitForSeconds(dashTime);
        Physics2D.IgnoreLayerCollision(7, 8, false);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(groundCheck.position, new Vector3(boxSizeX,boxSizeY, 0f));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
