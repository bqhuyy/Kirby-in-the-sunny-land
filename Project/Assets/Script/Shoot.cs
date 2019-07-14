using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Shoot : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float maxDistance;

    private Rigidbody2D myRigidbody;

    private Vector2 direction;

    private float startX;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        startX = transform.position.x;
    }

    void FixedUpdate()
    {
        myRigidbody.velocity = direction * speed;
    }

    private void Update()
    {
        if (Mathf.Abs(transform.position.x - startX) > maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void Initialize(Vector2 direction)
    {
        this.direction = direction;
    }
}
