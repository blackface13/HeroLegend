using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EffectStatusController : MonoBehaviour
{
    public Vector3 Speed;
    private void Update()
    { 
        transform.Rotate(Speed * Time.deltaTime);
    }
    // private void OnEnable()
    // {

    // }
}