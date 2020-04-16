//Điều khiển hiệu ứng skill 4
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class H2Skill4_Eff : SkillBase
{
    private bool herosee;
    public override void Awake()
    {
        base.Awake();
        if (BaseHero != null)
            herosee = BaseHero.BolNumber[5];//Set theo hướng nhìn của char, ko phải hướng bấm
    }
    public override void Start()
    {
        base.Start();
        BaseHero = Player.GetComponent<BaseHeroes>();
        herosee = BaseHero.BolNumber[5];//Set theo hướng nhìn của char, ko phải hướng bấm
    }
    /// <summary>
    /// Enable this object
    /// </summary>
    private void OnEnable()
    {
        if (BaseHero != null)
        {
            herosee = BaseHero.BolNumber[5];//Set theo hướng nhìn của char, ko phải hướng bấm
        }
        if (herosee)//Phải
        {
            if (Player != null)
            {
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                transform.GetChild(0).transform.localScale = new Vector3(-1, transform.GetChild(0).transform.localScale.y, transform.GetChild(0).transform.localScale.z);
            }
        }

        transform.GetChild(0).transform.localScale = transform.localScale;
    }
    /// <summary>
    /// Disable this object
    /// </summary>
    private void OnDisable()
    {
        transform.localScale = new Vector3(1, 1, 1);
        transform.GetChild(0).transform.localScale = new Vector3(1, 1, 1);
    }
}
