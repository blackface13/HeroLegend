using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptimizeObjectOutCamera : MonoBehaviour
{
    float SecondCheck = 0.5f;
    float RangeDefault = 30f;
    public GameObject[] ObjectOptimize;//Set in interface
                                       // Use this for initialization
    void Start()
    {
        //StartCoroutine(Deploy(SecondCheck));
    }
    private IEnumerator Deploy(float second)
    {
        for (int i = 0; i < ObjectOptimize.Length; i++)
            if (ObjectOptimize[i].transform.position.x < Camera.main.transform.position.x - (ObjectOptimize[i].transform.position.z + RangeDefault) || ObjectOptimize[i].transform.position.x > Camera.main.transform.position.x + (ObjectOptimize[i].transform.position.z + RangeDefault))
                ObjectOptimize[i].SetActive(false);
            else ObjectOptimize[i].SetActive(true);
        yield return new WaitForSeconds(second);
        StartCoroutine(Deploy(SecondCheck));
    }
}
