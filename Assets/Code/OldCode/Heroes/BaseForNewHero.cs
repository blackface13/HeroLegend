using UnityEngine;
using System.Collections;

public class BaseForNewHero : BaseHeroes
{
    #region Variables
    private GameObject[] SkillObject;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        FloatNumber[2] = Module.BASESKILL1SPEEDDEFAULT;//Skill 1 - Tốc độ lướt
        BolNumber[0] = false;
        MainHeroes = GameObject.FindGameObjectWithTag("MainHeroes");
    }

    /// <summary>
    /// Attack
    /// </summary>
    /// <returns></returns>
    public override void Attack(bool pressup, bool pressdown)
    {
        if (!BolNumber[0])
        {
            if (!BolNumber[1])
            {
                MainHeroes.GetComponent<CharacterControl>().Anim.SetTrigger("Atk1");
                BolNumber[1] = true;
                FloatNumber[0] = FloatNumber[1];
                return;
            }
            if (!BolNumber[2])
            {
                MainHeroes.GetComponent<CharacterControl>().Anim.SetTrigger("Atk2");
                BolNumber[2] = true;
                FloatNumber[0] = FloatNumber[1];
                return;
            }
            if (!BolNumber[3])
            {
                MainHeroes.GetComponent<CharacterControl>().Anim.SetTrigger("Atk3");
                BolNumber[3] = true;
                FloatNumber[0] = FloatNumber[1];
                return;
            }
            //StartCoroutine(CountNormalAtk(1));
        }
    }
}
