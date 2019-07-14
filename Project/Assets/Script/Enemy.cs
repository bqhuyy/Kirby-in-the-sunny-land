using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator MyAnimator { get; private set; }

    public Rigidbody2D MyRigidbody { get; set; }

    [SerializeField]
    private Transform shootPos;

    [SerializeField]
    private float movementSpeed;

    private bool facingRight;

    public bool Attack { get; set; }

    [SerializeField]
    private GameObject shootPrefab;

    private IEnemyState currentState;

    public GameObject Target { get; set; }

    [SerializeField]
    private int health;

    public bool IsDead
    {
        get
        {
            return health <= 0;
        }
    }

    [SerializeField]
    private GameObject breathPrefab;

    // Start is called before the first frame update
    void Start()
    {
        facingRight = false;
        MyAnimator = GetComponent<Animator>();
        MyRigidbody = GetComponent<Rigidbody2D>();
        ChangeState(new IdleState());

        }

    // Update is called once per frame
    void Update()
    {
        if (!IsDead)
        {
            currentState.Execute();
            LookAtTarget();
        }
    }

    private void LookAtTarget()
    {
        if (Target != null)
        {
            float xDir = Target.transform.position.x - transform.position.x;
            if (xDir < 0 && facingRight || xDir > 0 && !facingRight)
            {
                ChangeDirection();
            }
        }
    }

    public void ChangeDirection()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    public void ChangeState(IEnemyState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter(this);
    }

    public void Move()
    {
        if (!Attack)
        {
            MyAnimator.SetFloat("speed", 1);
            transform.Translate(GetDirection() * movementSpeed * Time.deltaTime);
        }
    }

    public Vector2 GetDirection()
    {
        return facingRight ? Vector2.right : Vector2.left;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Breath" && !IsDead)
        {
            StartCoroutine(TakeDamage());
        }
        currentState.OnTriggerEnter(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsDead && collision.gameObject.name == "Kirby")
        {
            Destroy(gameObject);
        }
    }

    private void Shoot(int value)
    {
        if (facingRight)
        {
            GameObject tmp = (GameObject)Instantiate(shootPrefab, shootPos.position, Quaternion.Euler(new Vector3(0, 0, 180)));
            tmp.GetComponent<Shoot>().Initialize(Vector2.right);
        }
        else
        {
            GameObject tmp = (GameObject)Instantiate(shootPrefab, shootPos.position, Quaternion.identity);
            tmp.GetComponent<Shoot>().Initialize(Vector2.left);
        }
    }

    public IEnumerator TakeDamage()
    {
        health -= 10;
        if (!IsDead)
        {

        }
        else
        {
            MyAnimator.SetTrigger("die");
            yield return null;
        }
    }
}
