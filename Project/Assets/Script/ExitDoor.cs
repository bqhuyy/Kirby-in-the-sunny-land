using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    [SerializeField]
    private GameObject exitDoor;

    // Update is called once per frame
    void Update()
    {
        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemy.Length == 0)
        {
            exitDoor.GetComponent<SpriteRenderer>().enabled = true;
            exitDoor.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
