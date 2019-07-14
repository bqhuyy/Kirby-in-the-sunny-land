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

    private bool immortal = false;
    [SerializeField]
    private float immortalTime;

    private float immortalCounter = 0;

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
        BecomeImmortal();
    }

    private void BecomeImmortal()
    {
        if (immortalCounter > 0.01)
        {
            StartCoroutine(IndicateImmortal());
            immortalCounter -= Time.deltaTime;
        }
        else
        {
            immortal = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (damageSources.Contains(collision.tag) && !IsDead)
        {
            if (!immortal)
            {
                health.CurrentVal -= 10;
                if (!IsDead)
                {
                    Kirby.Instance.MyAnimator.SetTrigger("damage");
                    immortal = true;
                    immortalCounter = immortalTime;
                }
                else
                {
                    Kirby.Instance.MyAnimator.SetTrigger("die");
                    //Kirby.Instance.IsDead = true;
                }
            }
        }
    }

    private IEnumerator IndicateImmortal()
    {
        while (immortal)
        {
            Kirby.Instance.GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(.1f);
            Kirby.Instance.GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(.1f);
        }
    }
}
