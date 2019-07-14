using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Stat health;

    [SerializeField]
    private List<string> damageSources;

    public bool IsDead
    {
        get
        {
            return health.CurrentVal <= 0;
        }
    }

    private void Awake()
    {
        health.Initialize();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (damageSources.Contains(collision.tag) && !IsDead)
        {
            health.CurrentVal -= 10;
            if (!IsDead)
            {
                Kirby.Instance.MyAnimator.SetTrigger("damage");
            }
            else
            {
                Kirby.Instance.MyAnimator.SetTrigger("die");
                Kirby.Instance.IsDead = true;
            }
        }
    }
}
