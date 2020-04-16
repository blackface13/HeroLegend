using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    public int Team;
    Vector3 vec;
    float range_x;
    float gravity;
    // Use this for initialization
    private void Awake()
    {
        gravity = Random.Range(15f, 20f);
        range_x = Team.Equals(1) ? Random.Range(5f, 10f) : Random.Range(-5f, -10f);
    }
    private void OnEnable()
    {
        vec = new Vector3(this.transform.position.x, this.transform.position.y + 3f, this.transform.position.z);
        this.transform.position = vec;
        gravity = Random.Range(15f, 20f);
        range_x = Team.Equals(1) ? Random.Range(5f, 10f) : Random.Range(-5f, -10f);
    }
    public void destroy()
    {
        gameObject.transform.position = new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, gameObject.transform.position.z);
        gameObject.SetActive(false);
        this.GetComponent<Text>().text = "";
    }
    // Update is called once per frame
    void Update()
    {
        if (!Module.PAUSEGAME)
        {
            vec.x += range_x * Time.deltaTime;
            vec.y += gravity * Time.deltaTime;
            gravity -= 30f * Time.deltaTime;
            this.transform.position = vec;
            if (gravity < -15f)
                destroy();
        }
    }
}
