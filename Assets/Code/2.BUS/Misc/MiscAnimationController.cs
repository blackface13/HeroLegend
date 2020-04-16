using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscAnimationController : MonoBehaviour
{
    public string AnimName;
    // Start is called before the first frame update
    void Awake()
    {
        this.GetComponent<Animator>().SetTrigger(AnimName);
    }
}
