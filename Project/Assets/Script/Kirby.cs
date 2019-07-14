using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kirby : MonoBehaviour
{
    private static Kirby instance;
    public static Kirby Instance { get => (instance) ? instance : GameObject.FindObjectOfType<Kirby>(); }
    public Animator MyAnimator { get; set; }
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

    private float currentY;

    [SerializeField]
    private GameObject breathPrefab;

    [SerializeField]
    private BoxCollider2D AbsorbCollider;

    [SerializeField]
    private AudioSource jumpSound;

    public bool IsDead { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        facingRight = true;
        MyRigidbody = GetComponent<Rigidbody2D>();
        MyAnimator = GetComponent<Animator>();
        MovementSpeed = walkSpeed;
    }

    void Update()
    {
        if (gameObject && !IsDead)
        {
            OnGround = IsGrounded();
            HandleInput();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameObject && !IsDead)
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
            MyAnimator.SetTrigger("jump");
        }
        //Fly
        if (Jump && Input.GetKeyDown(KeyCode.Space))
        {
            currentY = transform.position.y;
            MyAnimator.SetTrigger("fly");
            MyAnimator.SetBool("flying", true);
            jumpSound.Play();
        }
        if (Fly && Input.GetKeyDown(KeyCode.Space))
        {
            MyAnimator.SetBool("flying", true);
            jumpSound.Play();
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
            MyAnimator.SetTrigger("absorb");
            MyAnimator.SetBool("absorbing", true);
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            MyAnimator.SetBool("absorbing", false);
            if (AbsorbCollider.enabled)
            {
                AbsorbAttack();
            }
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
            MyAnimator.SetBool("land", true);
        }
        else
        {
            MyAnimator.SetBool("land", false);
        }

        if (OnGround && Distance > 70.0f)
        {
            MyAnimator.SetBool("run", true);
        }
        else
        {
            MyAnimator.SetBool("run", false);
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
        MyAnimator.SetFloat("speed", Mathf.Abs(horizontal));
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
            ChangeDirection();
        }
    }

    private void HandleLayers()
    {
        if (!OnGround)
        {
            MyAnimator.SetLayerWeight(airLayer, 1);
        }
        else
        {
            MyAnimator.SetLayerWeight(airLayer, 0);
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
        GameObject[] sight = GameObject.FindGameObjectsWithTag("Sight");
        if (sight.Length > 0)
        {
            foreach (GameObject obj in sight)
            {
                Physics2D.IgnoreCollision(tmp.GetComponent<Collider2D>(), obj.GetComponent<Collider2D>());
            }
        }
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
        MyAnimator.SetBool("flying", false);
        Breath(0);
    }
    public Vector2 GetDirection()
    {
        return facingRight ? Vector2.right : Vector2.left;
    }

    public void ChangeDirection()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    public IEnumerator TakeDamage()
    {
        yield return null;
    }

    public void AbsorbAttack()
    {
        AbsorbCollider.enabled = !AbsorbCollider.enabled;
    }
}
