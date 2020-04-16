using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase : MonoBehaviour
{
    public GameObject Ctrl; // Tạo biến control để điều khiển var của Battle System
    public GameObject Player;
    public BaseEnemys BaseEnemy;
    public System_Battle SystemBattle;
    public BaseHeroes BaseHero;
    public int DamePer = 100;//Số lượng % dame gây ra (10 = 10%) sửa ở mỗi skill cho phù hợp
    public virtual void Awake()
    {
        Ctrl = GameObject.FindGameObjectWithTag("ControlScene");
        SystemBattle = Ctrl.GetComponent<System_Battle>();
    }
    public virtual void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    public void ShowSkill(GameObject obj, Vector3 vec, Quaternion quater)
    {
        obj.transform.position = vec;
        obj.transform.rotation = quater;
        obj.SetActive(true);
    }
   public virtual IEnumerator AutoHiden(float time)
    {
        yield return new WaitForSeconds(time);
        Hide();
    }
    public virtual void Hide()
    {
        gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]);
        gameObject.transform.localEulerAngles = new Vector3();
    }
}
