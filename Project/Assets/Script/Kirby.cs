using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kirby : MonoBehaviour
{
    private static Kirby instance;
    public static Kirby Instance { get => (instance) ? instance : GameObject.FindObjectOfType<Kirby>(); }

    private Animator myAnimator;
    public float MovementSpeed { get; set; }

    public float walkSpeed;

    public float runSpeed;

    public float flySpeed;

    public float flyMaxRange;

    private bool facingRight;

    [SerializeField]
    private bool airControl;

    public float jumpForce;

    public Rigidbody2D MyRigidbody { get; set; }

    public bool Absorb { get; set; }
    public bool Run { get; set; }
    public bool Jump { get; set; }
    public bool OnGround { get; set; }
    public bool Fly { get; set; }
    public float Distance { get; set; }

    const int airLayer = 1;

    [SerializeField]
    private LayerMask groundLayers;

    private string[] monster = new string[] {
        "UmbrellaOrange",
        "ElectricOrange",
        "Sword"
    };

    private float currentY;

    [SerializeField]
    private GameObject breathPrefab;

    private bool stopFlying = false;

    // Start is called before the first frame update
    void Start()
    {
        facingRight = true;
        MyRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        MovementSpeed = walkSpeed;
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
            HandleLayers();
            ResetValues();
        }
    }

    private void HandleInput()
    {
        //Jump
        if (OnGround && Input.GetKeyDown(KeyCode.Space))
        {
            myAnimator.SetTrigger("jump");
        }
        //Fly
        if (Jump && Input.GetKeyDown(KeyCode.Space))
        {
            currentY = transform.position.y;
            myAnimator.SetTrigger("fly");
            myAnimator.SetBool("flying", true);
        }
        if (Fly && Input.GetKeyDown(KeyCode.Space))
        {
            myAnimator.SetBool("flying", true);
            if ((transform.position.y-currentY) < flyMaxRange)
            {
                MyRigidbody.AddForce(new Vector2(0, flySpeed));
            }
        }
        if (Fly && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            StopFlying();
        }
        //Absorb
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            myAnimator.SetTrigger("absorb");
            myAnimator.SetBool("absorbing", true);
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            myAnimator.SetBool("absorbing", false);
        }
    }

    private void ResetValues()
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) < 0.01)
        {
            Distance = 0;
        }
    }

    private void HandleMovement(float horizontal)
    {
        if (MyRigidbody.velocity.y < 0 && !OnGround && !Fly)
        {
            myAnimator.SetBool("land", true);
        }
        else
        {
            myAnimator.SetBool("land", false);
        }

        if (OnGround && Distance > 70.0f)
        {
            myAnimator.SetBool("run", true);
        }
        else
        {
            myAnimator.SetBool("run", false);
        }
        if (Fly && OnGround)
        {
            StopFlying();
        }

        if (!Absorb && (OnGround || airControl))
        {
            MyRigidbody.AddForce(new Vector2(horizontal * MovementSpeed, 0));
            HandleRun(horizontal);
        }
        myAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    private void HandleRun(float horizontal)
    {
        if (OnGround)
        {
            Distance = Mathf.Min(Distance + Mathf.Abs(horizontal), 75.0f);
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

    public void Breath(int value)
    {
        GameObject tmp = (GameObject)Instantiate(breathPrefab, transform.position, Quaternion.identity);
        if (facingRight)
        {
            tmp.GetComponent<Breath>().Initialize(Vector2.right);
        }
        else
        {
            tmp.GetComponent<Breath>().Initialize(Vector2.left);
        }
    }

    private void StopFlying()
    {
        MyRigidbody.velocity = new Vector2(MyRigidbody.velocity.x, 0);
        Fly = false;
        myAnimator.SetBool("flying", false);
        Breath(0);
    }
}
