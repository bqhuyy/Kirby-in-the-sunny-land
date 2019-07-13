using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Kirby_KinematicControl : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    private Animator myAnimator;

    private float movementSpeed = 1.0f;

    [SerializeField]
    private float walkSpeed = 1.0f;

    [SerializeField]
    private float runSpeed = 1.0f;

    private bool facingRight;

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    [SerializeField]
    private bool isGrounded;

    private bool jump;

    [SerializeField]
    private bool airControl;

    [SerializeField]
    private float jumpForce;

    private float distance;

    private bool run;

    private bool absorb;

    const int airLayer = 1;

    [SerializeField]
    private LayerMask groundLayers;

    private string[] monster = new string[] {
        "UmbrellaOrange",
        "ElectricOrange",
        "Sword"
    };

    // Start is called before the first frame update
    void Start()
    {
        facingRight = true;
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        movementSpeed = walkSpeed;
    }

    void Update()
    {
        if (gameObject)
        {
            isGrounded = Physics2D.OverlapArea(
                new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f),
                new Vector2(transform.position.x + 0.5f, transform.position.y -0.51f),
                groundLayers
            );
            HandleInput();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameObject)
        {
            float horizontal = Input.GetAxis("Horizontal");
            HandleMovement(horizontal);
            Flip(horizontal);
            HandleAttack();
            HandleLayers();
            ResetValues();
        }
    }

    private void HandleInput()
    {
        //Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }
        //Absorb
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            absorb = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            absorb = false;
            myAnimator.SetBool("absorbing", false);
        }
    }

    private void ResetValues()
    {
        jump = false;
        if (Mathf.Abs(Input.GetAxis("Horizontal")) < 0.01)
        {
            distance = 0;
            myAnimator.SetFloat("run", distance);
            movementSpeed = walkSpeed;
        }
    }

    private void HandleMovement(float horizontal)
    {
        if (myRigidbody.velocity.y < 0 && !isGrounded)
        {
            myAnimator.SetBool("land", true);
        } else
        {
            myAnimator.SetBool("land", false);
        }

        if (isGrounded && jump)
        {
            myRigidbody.AddForce(new Vector2(0, jumpForce));
            myAnimator.SetTrigger("jump");
        }

        if (isGrounded && distance > 70.0f)
        {
            myAnimator.SetFloat("run", distance);
            movementSpeed = runSpeed;
        }

        if (!isAbsorbing())
        {
            myRigidbody.AddForce(new Vector2(horizontal * movementSpeed, 0));
            HandleRun(horizontal);
        }
        myAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    private void HandleRun(float horizontal)
    {
        if (isGrounded && !isAbsorbing())
        {
            distance = Mathf.Min(distance + Mathf.Abs(horizontal), 75.0f);
        }
    }

    private bool isAbsorbing()
    {
        return this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Absorb") || this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Absorbing");
    }

    private void HandleAttack()
    {
        if (absorb && !isAbsorbing())
        {
            myAnimator.SetTrigger("absorb");
            myAnimator.SetBool("absorbing", true);
        }
    }

    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;

            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    private void HandleLayers()
    {
        if (!isGrounded)
        {
            myAnimator.SetLayerWeight(airLayer, 1);
        }
        else
        {
            myAnimator.SetLayerWeight(airLayer, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (monster.Contains(collision.gameObject.name))
        {
            Destroy(gameObject);
        }
    }
}
