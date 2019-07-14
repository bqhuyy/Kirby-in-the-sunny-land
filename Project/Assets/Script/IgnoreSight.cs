using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreSight : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] sight = GameObject.FindGameObjectsWithTag("Sight");
        if (sight.Length > 0)
        {
            foreach (GameObject obj in sight)
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), obj.GetComponent<Collider2D>());
            }
        }
    }
}
