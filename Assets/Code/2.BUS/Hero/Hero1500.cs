using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using BlackCore;
namespace Controller.Hero1500
{
    public class Hero1500 : HeroBase
    {
        //Initialize
        public override void Awake()
        {
            base.Awake();
            //SpeedAnimationValue = 3f;
            HType = HeroType.near;//Tướng đánh xa gần
            FirstSetupHero(this.gameObject);//Khởi tạo các values ban đầu cho hero
            SetupSkill();
        }

        private void SetupSkill()
        {
            //Khởi tạo các object skill
            Skill1 = new List<GameObject>();//Object đánh thường
            Skill2 = new List<GameObject>();//Skill hướng phải, trái
            //Skill3 = new GameObject[1];//Skill hướng trái
            SkillType = new int[2];//Kiểu skill

            // Đưa vào scene
            // for (int i = 0; i < 3; i++)
            // {
            //     Skill1.Add((GameObject)Instantiate(Resources.Load<GameObject>(BattleCore.HeroSkillObjectLink + "H1003Atk" + (i + 1).ToString()), new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity));
            //     Skill1[i].GetComponent<Hero1500Atk>().Hero = this;
            //     Skill1[i].SetActive(false);
            // }
            //Hiệu ứng chạm đất
            //Skill2.Add((GameObject)Instantiate(Resources.Load<GameObject>(BattleCore.HeroSkillObjectLink + "H5SkillEffect"), new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity));
//            Skill2[0].GetComponent<Hero1500Skill>().Hero = this;
            //Skill1[0].SetActive(false);
            //Skill 
            // Skill2[0] = Instantiate(Resources.Load<GameObject>(BattleCore.HeroSkillObjectLink + "H4Atk3"), new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity);
            // Skill2[0].GetComponent<Hero1500Atk>().Hero = this;
            // Skill2[0].SetActive(false);
            // Skill2[1] = Instantiate(Resources.Load<GameObject>(BattleCore.HeroSkillObjectLink + "Hero2Skill1R"), new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity);
            // Skill2[0].GetComponent<Hero2Skill1>().Hero = this;
            // Skill2[0].SetActive(false);
            // Skill2[1].GetComponent<Hero2Skill1>().Hero = this;
            // Skill2[1].SetActive(false);
            //Set kiểu đánh gần hay xa cho mỗi skill. Nếu HType = HeroType.far. thì set tất cả cái dưới = 0. Chỉ được thay đổi giá trị cuối của dòng gán bên dưới
            SkillType[0] = HType == HeroType.far ? 0 : 1;//0 = đánh xa. 1 = cận chiến - đánh thường
            SkillType[1] = HType == HeroType.far ? 0 : 1;//0 = đánh xa. 1 = cận chiến - skill 1
            //SkillType[2] = HType == HeroType.far ? 0 : 0;//0 = đánh xa. 1 = cận chiến - skill 2

        }
        //
        // public override void Start()
        // {
        //     base.Start();
        // }

        public override void RefreshTeam(GameObject obj)
        {
            base.RefreshTeam(obj);
            // SkillExtension.layer = Team.Equals(0) ? 11 : 12;
            // SkillExtension.GetComponent<Collider2D>().enabled = true;
            //SkillExtension.transform.localScale = Team.Equals(0) ? SkillExtension.transform.localScale : new Vector3(transform.position.x - 13.6f, transform.position.y -2f, Module.BASELAYER[2]);
        }
        //Update
        public override void Update()
        {
            base.Update();
        }
        public override void ActionSkill(int skillnumber)//Ko dùng với hero này
        {
            base.ActionSkill(skillnumber);
            switch (skillnumber)
            {
                case 0://Normal atk
                    {
                        switch (ComboNormalAtk)
                        {
                            case 0:
                                ShowSkill(Skill1[ComboNormalAtk], new Vector3(transform.position.x + 5f, transform.position.y + 2f, Module.BASELAYER[2]), Quaternion.identity);
                                ComboNormalAtk++;
                                break;
                            case 1:
                                ShowSkill(Skill1[ComboNormalAtk], new Vector3(transform.position.x, transform.position.y + 1.65f, Module.BASELAYER[2]), Quaternion.Euler(39.6f, -180.3f, 124.3f));
                                ComboNormalAtk++;
                                break;
                            case 2:
                                ShowSkill(Skill1[ComboNormalAtk], new Vector3(transform.position.x + 6.18f, transform.position.y + -0.27f, Module.BASELAYER[2]), Quaternion.identity);
                                ComboNormalAtk++;
                                break;
                            default: break;
                        }
                    }
                    break;
                case 1://Skill1
                    ShowSkill(Skill2[0], Team.Equals(0) ? new Vector3(transform.position.x + 13.6f, transform.position.y - 2f, Module.BASELAYER[2]) : new Vector3(transform.position.x - 13.6f, transform.position.y - 2f, Module.BASELAYER[2]), Quaternion.identity);
                    //ShowSkill(Team.Equals(1) ? Skill2[0] : Skill2[1], Team.Equals(1) ? new Vector3(transform.position.x - 8f, transform.position.y + 3.5f, Module.BASELAYER[2]) : new Vector3(transform.position.x + 8f, transform.position.y + 3.5f, Module.BASELAYER[2]), Quaternion.identity);
                    break;
                default: break;
            }
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