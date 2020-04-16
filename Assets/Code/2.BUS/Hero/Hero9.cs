using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using BlackCore;
namespace Controller.Hero9
{
    public class Hero9 : HeroBase
    {
        //Initialize
        public override void Awake()
        {
            base.Awake();
            HType = HeroType.far;//Tướng đánh xa
            FirstSetupHero(this.gameObject);//Khởi tạo các values ban đầu cho hero
            SetupSkill();
        }

        private void SetupSkill()
        {
            //Khởi tạo các object skill
            Skill1 = new List<GameObject>();//Object phi tiêu
            Skill2 = new List<GameObject>();//Skill như Q của Sivir
            Skill3 = null;
            SkillType = new int[3];//Kiểu skill

            Skill1.Add((GameObject)Instantiate(Resources.Load<GameObject>(BattleCore.HeroSkillObjectLink + "H9Atk"), new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity));
            Skill1[0].GetComponent<Hero9Atk>().Hero = this;
            Skill1[0].SetActive(false);
            Skill2.Add((GameObject)Instantiate(Resources.Load<GameObject>(BattleCore.HeroSkillObjectLink + "H9Skill"), new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.Euler(73.74f, 0, 0)));
            Skill2[0].transform.GetChild(0).GetComponent<Hero9Skill>().Hero = this;
            Skill2[0].SetActive(false);
            //Set kiểu đánh gần hay xa cho mỗi skill. Nếu HType = HeroType.far. thì set tất cả cái dưới = 0. Chỉ được thay đổi giá trị cuối của dòng gán bên dưới
            SkillType[0] = HType == HeroType.far ? 0 : 0;//0 = đánh xa. 1 = cận chiến - đánh thường
            SkillType[1] = HType == HeroType.far ? 0 : 0;//0 = đánh xa. 1 = cận chiến - skill 1
            SkillType[2] = HType == HeroType.far ? 0 : 0;//0 = đánh xa. 1 = cận chiến - skill 2

        }
        //
        // public override void Start()
        // {
        //     base.Start();
        // }

        public override void RefreshTeam(GameObject obj)
        {
            base.RefreshTeam(obj);
            Skill2[0].transform.GetChild(0).transform.GetComponent<SkillCore>().ReSetupLayer(Team);//Dành riêng cho skill của hero này, vì object va chạm là object con
        }
        //Update
        public override void Update()
        {
            base.Update();
        }
        public override void ActionSkill(int skillnumber)
        {
            base.ActionSkill(skillnumber);
            switch (skillnumber)
            {
                case 0://Normal atk
                    if (ComboNormalAtk == 2)//Nếu là đòn đánh thứ 3 của combo
                    {
                        StartCoroutine(MultiAtk(Skill1, new Vector3(this.transform.position.x + (Team.Equals(0) ? 1.6f : -1.6f), this.transform.position.y + .5f, this.transform.position.z), .1f, 3, Quaternion.identity));
                        ComboNormalAtk = 0;
                    }
                    else
                    {
                        if (CheckExistAndCreateEffectExtension(new Vector3(this.transform.position.x + (Team.Equals(0) ? 1.6f : -1.6f), this.transform.position.y + .5f, this.transform.position.z), Skill1, Quaternion.identity))
                            Skill1[Skill1.Count - 1].GetComponent<Hero9Atk>().Hero = this;//Gán class cho object skill mới tạo
                        //ShowSkill(objdontactive, this.transform.position, Quaternion.identity);
                        ComboNormalAtk++;
                    }
                    break;
                case 1://Skill1
                    if (CheckExistAndCreateEffectExtension(new Vector3(this.transform.position.x, this.transform.position.y -1.5f, Module.BASELAYER[Team.Equals(0) ? 2 : 3]), Skill2, Quaternion.Euler(73.74f, 0, 0)))
                        Skill2[Skill2.Count - 1].transform.GetChild(0).GetComponent<Hero9Skill>().Hero = this;//Gán class cho object skill mới tạo
                        break;
                default: break;
            }
        }
        /// <summary>
        /// Viết riêng cho combo normal atk của hero này, sản sinh ra liên tục 3 phi tiêu
        /// </summary>
        /// <returns></returns>
        private IEnumerator ComboAtk()
        {
            int count = 0;
        Begin:
            var obj1 = GetObjectDontActive(Skill1);
            if (obj1 != null)
                ShowSkill(obj1, this.transform.position, Quaternion.identity);
            count++;
            yield return new WaitForSeconds(.1f);
            if (count < 3)
                goto Begin;
        }
        //Va chạm
        public override void OnTriggerEnter2D(Collider2D col)
        {
            base.OnTriggerEnter2D(col);
        }
        public override void OnCollisionEnter2D(Collision2D col)
        {
            base.OnCollisionEnter2D(col);
        }
    }
}