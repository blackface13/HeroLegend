using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSystem : MonoBehaviour
{
    // Use this for initialization
   public List<GameObject> ObjEnemy = new List<GameObject>();
    List<BaseEnemys> ObjEnemyCPN = new List<BaseEnemys>();
    List<GameObject> ObjHero = new List<GameObject>();
    public List<GameObject> ObjSkillHero = new List<GameObject>();
    List<SkillBase> ObjSkillHeroCPN = new List<SkillBase>();
    List<GameObject> ObjLane = new List<GameObject>();
    List<GameObject> ObjSkillEnemy = new List<GameObject>();
    void Start()
    {
        //GameObject[] gos = Resources.FindObjectsOfTypeAll(typeof(UnityEngine.Object)) as GameObject[]; //will return an array of all GameObjects in the scene
        //foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        //{
        //    if (go.layer.Equals(Module.BASELAYERRIGID2D[2]))//Detect Enemy
        //    {
        //        ObjEnemy.Add(go);
        //        ObjEnemyCPN.Add(go.GetComponent<BaseEnemys>());
        //    }
        //    if (go.layer.Equals(Module.BASELAYERRIGID2D[1]))//Detect Hero
        //    {
        //        ObjHero.Add(go);
        //    }
        //    if (go.layer.Equals(Module.BASELAYERRIGID2D[3]))//Detect Skill of Hero
        //    {
        //        ObjSkillHero.Add(go);
        //        ObjSkillHeroCPN.Add(go.GetComponent<SkillBase>());
        //    }
        //    if (go.layer.Equals(Module.BASELAYERRIGID2D[0]))//Detect Lane
        //    {
        //        ObjLane.Add(go);
        //    }
        //    if (go.layer.Equals(Module.BASELAYERRIGID2D[4]))//Detect Skill of Enemy
        //    {
        //        ObjSkillEnemy.Add(go);
        //    }
        //}
        //for (int i = 0; i < ObjSkillHero.Count; i++)
        //    ObjSkillHero[i].SetActive(false);
        //print("Enemy: " + ObjEnemy.Count);
        //print("Hero: " + ObjHero.Count);
        //print("Skill hero: " + ObjSkillHero.Count);
        //print("Lane: " + ObjLane[0].name + ":" + ObjLane[1].name);
        //print("Skill enemy: " + ObjSkillEnemy.Count);
    }

    // Update is called once per frame
    void Update()
    {
        //for (int i = 0; i < ObjSkillHero.Count; i++)//
        //{
        //    if (ObjSkillHero[i].activeSelf && ObjSkillHeroCPN[i].Enable)//If enable
        //    {
        //        for (int x = 0; x < ObjEnemy.Count; x++)
        //            if (ObjEnemy[x].activeSelf == true)//Va chạm với enemy còn sống
        //            {
        //                if (ObjSkillHero[i].transform.position.x > ObjEnemy[x].transform.position.x - ObjEnemyCPN[x].Rectangle.x &&
        //                    ObjSkillHero[i].transform.position.x < ObjEnemy[x].transform.position.x + ObjEnemyCPN[x].Rectangle.x &&
        //                    ObjSkillHero[i].transform.position.y > ObjEnemy[x].transform.position.y - ObjEnemyCPN[x].Rectangle.y &&
        //                    ObjSkillHero[i].transform.position.y < ObjEnemy[x].transform.position.y + ObjEnemyCPN[x].Rectangle.y)
        //                {
        //                    ObjSkillHeroCPN[i].ObjectCollision.Add(ObjEnemy[x]);
        //                    ObjSkillHeroCPN[i].CollisionEnter = true;
        //                    ObjEnemyCPN[x].ObjectCollision = ObjSkillHero[i];
        //                    ObjEnemyCPN[x].CollisionEnter = true;
        //                }
        //            }
        //        if(ObjSkillHero[i].transform.position.y < ObjLane[0].transform.position.y - 5f)//Nếu chạm đất
        //        {
        //            ObjSkillHeroCPN[i].ObjectCollision.Add(ObjLane[0]);
        //            ObjSkillHeroCPN[i].CollisionEnter = true;
        //        }
        //    }
        //}
    }
}
