using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public SpriteRenderer sr;
    private bool hasDashed;
    private bool groundTouch;
    //public AudioManager Audio;
    public Health health;

    [Space]
    [Header("Audio")]
    public AudioManager audio;

    [Space]
    [Header("Layers")]
    public LayerMask groundLayer;

    [Space]
    [Header ("Boolean")]

    public bool onGround;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    public bool wallGrab;
    public bool wallSlide;
    public bool wallJumped;
    public bool isDashing;
    public bool canMove;

    [Space]
    [Header("Other Variable")]

    public float speed = 10;
    public float wallJumpLerp = 5;
    public float dashSpeed = 20;
    public float jumpForce = 10;
    public float slideSpeed = 5;
    public int wallSide;
    public int side;

    public Vector3 respawnPosition;

    [Space]

    [Header("Collision")]

    public float collisionRadius = 0.25f;
    public Vector2 bottomOffset, rightOffset, leftOffset;
    private Color debugCollisionColor = Color.red;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);
        onWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer)
            || Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);

        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);

        wallSide = onRightWall ? -1 : 1;

        anim.SetBool("onGround", onGround);
        anim.SetBool("onWall", onWall);
        anim.SetBool("onRightWall", onRightWall);
        anim.SetBool("wallGrab", wallGrab);
        anim.SetBool("wallSlide", wallSlide);
        anim.SetBool("canMove", canMove);
        anim.SetBool("isDashing", isDashing);

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(x, y);
        
        Walk(dir);
        SetHorizontalMovement(x, y, rb.velocity.y);

        if (Input.GetButton("Fire1") && !hasDashed)
        {
            dash(xRaw, yRaw);
            
        }
        if (Input.GetButton("Jump"))
        {
            SetTrigger("jump");
            if (onGround)
            {
                Jump(Vector2.up);
                PlayJump();
            }
            if (onWall && !onGround)
            {
                WallJump();
                PlayJump();
            }
        }
        if (onWall && Input.GetButton("Fire3") && canMove)
        {
            wallGrab = true;
            wallSlide = false;
        }

        if (Input.GetButtonUp("Fire3") || !onWall || !canMove)
        {
            wallGrab = false;
            wallSlide = false;
        }

        if (onGround && !isDashing)
        {
            wallJumped = false;
            GetComponent<BetterJump>().enabled = true;
        }

        if (wallGrab && !isDashing)
        {
            rb.gravityScale = 0;
            if (x > .2f || x < -.2f)
                rb.velocity = new Vector2(rb.velocity.x, 0);

            float speedModifier = y > 0 ? .5f : 1;

            rb.velocity = new Vector2(rb.velocity.x, y * (speed * speedModifier));
        }
        else
        {
            rb.gravityScale = 3;
        }

        if (onWall && !onGround)
        {
            if (x != 0 && !wallGrab)
            {
                wallSlide = true;
                WallSlide();
            }
        }
        if (onGround && !groundTouch)
        {
            GroundTouch();
            groundTouch = true;
        }

        if (!onGround && groundTouch)
        {
            groundTouch = false;
        }

        if (!onWall || onGround)
            wallSlide = false;

        if (wallGrab || wallSlide || !canMove)
            return;

        if (x > 0)
        {
            side = 1;
            Flip(side);
        }
        if (x < 0)
        {
            side = -1;
            Flip(side);
        }
    }
    void GroundTouch()
    {
        hasDashed = false;
        isDashing = false;

        side = sr.flipX ? -1 : 1;
    }
    private void WallJump()
    {
        if ((side == 1 && onRightWall) || side == -1 && !onRightWall)
        {
            side *= -1;
            Flip(side);
        }

        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));

        Vector2 wallDir = onRightWall ? Vector2.left : Vector2.right;

        Jump(Vector2.up / 1.5f + wallDir / 1.5f);

        wallJumped = true;
    }
    private void Jump(Vector2 dir)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpForce;
    }
    
    private void dash(float x, float y)
    {
        hasDashed = true;

        SetTrigger("dash");

        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(x, y);

        rb.velocity += dir.normalized * dashSpeed;

        PlayRun();

        StartCoroutine(DashWait());
    }
    public void Walk(Vector2 dir, bool standingOnPlatform = false)
    {
        if (!canMove)
            return;

        if (wallGrab)
            return;

        if (!wallJumped)
        {
            //Audio.Play("Walk");
            rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
        }
        else
        {
            //Audio.Play("Walk");
            rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * speed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
        }

        /*if (standingOnPlatform)
        {
            onGround = true;
        }*/
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };

        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
    }
    private void WallSlide()
    {
        if (wallSide != side)
            Flip(side * -1);

        if (!canMove)
            return;

        bool pushingWall = false;
        if ((rb.velocity.x > 0 && onRightWall) || (rb.velocity.x < 0 && onLeftWall))
        {
            pushingWall = true;
        }
        float push = pushingWall ? 0 : rb.velocity.x;

        rb.velocity = new Vector2(push, -slideSpeed);
    }

    public void SetHorizontalMovement(float x, float y, float yVel)
    {
        anim.SetFloat("HorizontalAxis", x);
        anim.SetFloat("VerticalAxis", y);
        anim.SetFloat("VerticalVelocity", yVel);
    }

    public void SetTrigger(string trigger)
    {
        anim.SetTrigger(trigger);
    }

    public void Flip(int side)
    {

        if (wallGrab || wallSlide)
        {
            if (side == -1 && sr.flipX)
                return;

            if (side == 1 && !sr.flipX)
                return;
            
        }

        bool state = (side == 1) ? false : true;
        sr.flipX = state;
    }
    IEnumerator DashWait()
    {
        StartCoroutine(GroundDash());
        
        rb.gravityScale = 0;
        GetComponent<BetterJump>().enabled = false;
        wallJumped = true;
        isDashing = true;

        yield return new WaitForSeconds(.3f);

        rb.gravityScale = 3;
        GetComponent<BetterJump>().enabled = true;
        wallJumped = false;
        isDashing = false;
    }

    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(.15f);
        if (onGround)
            hasDashed = false;
    }
    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }
    public void Death()
    {
        transform.position = respawnPosition;
        health.health -= 1;
        PlayDeath();
        if (health.health == 0)
        {
            Time.timeScale = 0;
            SceneManager.LoadScene("gameover");
        }
    }
    void OnTriggerEnter2D(Collider2D hit)

    {
        if (hit.tag == "CheckPoint")
        {
            respawnPosition = new Vector3(transform.position.x, transform.position.y + 4);
            PlayCollect();
        }
        if (hit.tag == "Fall")
        {
            Death();
        }
        if (hit.tag == "Collect")
        {
            PlayCollect();
        }
        if (hit.tag == "Win")
        {
            Time.timeScale = 0;
            SceneManager.LoadScene("win");
        }
        if (hit.tag == "Win 1")
        {
            Time.timeScale = 0;
            SceneManager.LoadScene("win 1");
        }
        if (hit.tag == "Win 2")
        {
            Time.timeScale = 0;
            SceneManager.LoadScene("Credit");
        }
    }
    
    public void PlayWalk()
    {
        audio.Play("Walk");
    }
    void PlayJump()
    {
        audio.Play("Jump");
    }
    void PlayCollect()
    {
        audio.Play("Collect");
    }
    void PlayDeath()
    {
        audio.Play("Death");
    }
    void PlayGrab()
    {
        audio.Play("Grab");
    }
    void PlayRun()
    {
        audio.Play("Run");
    }
}
