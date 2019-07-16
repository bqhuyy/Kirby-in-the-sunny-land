using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instruction : MonoBehaviour
{
    [SerializeField]
    private GameObject[] guides;

    [SerializeField]
    private GameObject exitDoorPrefab;

    [SerializeField]
    private GameObject mushroomPrefab;

    [SerializeField]
    private GameObject enemyPrefab;

    private int currentStep = 0;
    // Start is called before the first frame update
    void Start()
    {
        guides[currentStep].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {   
        //Go left
        if (currentStep == 0 && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)))
        {
            nextStep();
        }
        //Go right
        if (currentStep == 1 && (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)))
        {
            nextStep();
        }
        //Jump
        if (currentStep == 2 && Input.GetKeyDown(KeyCode.Space))
        {
            nextStep();
        }
        //Fly
        if (currentStep == 3 && Kirby.Instance.Jump && Input.GetKeyDown(KeyCode.Space))
        {
            nextStep();
        }
        //Shoot
        if (currentStep == 4 && Kirby.Instance.Fly && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            nextStep();
            Instantiate(enemyPrefab, new Vector3(-10, 0, 10), Quaternion.identity);
        }
        //Hit the ground
        if (currentStep == 5 && !GameObject.FindGameObjectWithTag("Enemy"))
        {
            nextStep();
            Instantiate(enemyPrefab, new Vector3(-10, 0, 10), Quaternion.identity);
        }
        //Absorb
        if (currentStep == 6 && !GameObject.FindGameObjectWithTag("Enemy"))
        {
            nextStep();
            Instantiate(mushroomPrefab, new Vector3(-20, -0.5f, 10), Quaternion.identity);
        }
        //Eat mushroom
        if (currentStep == 7 && !GameObject.FindGameObjectWithTag("Health"))
        {
            nextStep();
            Instantiate(exitDoorPrefab, new Vector3(-13, 0, 8.5f), Quaternion.identity);
        }
    }

    void nextStep()
    {
        guides[currentStep].SetActive(false);
        guides[++currentStep].SetActive(true);
    }
}
