using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kirby : MonoBehaviour
{
    private static Kirby instance;
    public static Kirby Instance { get => (instance) ? instance : GameObject.FindObjectOfType<Kirby>(); }

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
    private bool airControl;

    [SerializeField]
    private float jumpForce;

    public Rigidbody2D MyRigidbody;

    public bool Absorb { get; set; }
    public bool Run { get; set; }
    public bool Jump { get; set; }
    public bool OnGround { get; set; }

    private float distance;

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
        MyRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        movementSpeed = walkSpeed;
    }

    void Update()
    {
        if (gameObject)
        {
            OnGround = IsGrounded();
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
            //jump = true;
        }
        //Absorb
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            //absorb = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            //absorb = false;
            myAnimator.SetBool("absorbing", false);
        }
    }

    private void ResetValues()
    {
        //jump = false;
        if (Mathf.Abs(Input.GetAxis("Horizontal")) < 0.01)
        {
            distance = 0;
            myAnimator.SetFloat("run", distance);
            movementSpeed = walkSpeed;
        }
    }

    private void HandleMovement(float horizontal)
    {
        if (MyRigidbody.velocity.y < 0 && !OnGround)
        {
            myAnimator.SetBool("land", true);
        }
        else
        {
            myAnimator.SetBool("land", false);
        }

        if (Jump && MyRigidbody.velocity.y == 0)
        {
            MyRigidbody.AddForce(new Vector2(0, jumpForce));
            //myAnimator.SetTrigger("jump");
        }

        if (OnGround && distance > 70.0f)
        {
            myAnimator.SetFloat("run", distance);
            movementSpeed = runSpeed;
        }

        if (!Absorb && OnGround)
        {
            MyRigidbody.AddForce(new Vector2(horizontal * movementSpeed, 0));
            HandleRun(horizontal);
        }
        myAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    private void HandleRun(float horizontal)
    {
        if (OnGround && !isAbsorbing())
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
        if (Absorb && !isAbsorbing())
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
        if (!OnGround)
        {
            myAnimator.SetLayerWeight(airLayer, 1);
        }
        else
        {
            myAnimator.SetLayerWeight(airLayer, 0);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapArea(
                new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f),
                new Vector2(transform.position.x + 0.5f, transform.position.y - 0.51f),
                groundLayers
            );
    }
}
