using System;
using System.Collections;
using System.Collections.Generic;
using BlackCore;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Controller.Hero5 {
    public class Hero5 : HeroBase {
        //Initialize
        public override void Awake () {
            base.Awake ();
            //SpeedAnimationValue = 3f;
            HType = HeroType.near; //Tướng đánh xa
            FirstSetupHero (this.gameObject); //Khởi tạo các values ban đầu cho hero
            SetupSkill ();
        }

        private void SetupSkill () {
            //Khởi tạo các object skill
            Skill1 = new List<GameObject> (); //Object đánh thường
            Skill2 = new List<GameObject> (); //Skill hướng phải, trái
            //Skill3 = new GameObject[1];//Skill hướng trái
            SkillType = new int[2]; //Kiểu skill

            // Đưa vào scene
            for (int i = 0; i < 3; i++) {
                Skill1.Add ((GameObject) Instantiate (Resources.Load<GameObject> (BattleCore.HeroSkillObjectLink + "H5Atk" + (i + 1).ToString ()), new Vector3 (Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity));
                Skill1[i].GetComponent<Hero5Atk> ().Hero = this;
                Skill1[i].SetActive (false);
            }

            //Hiệu ứng chạm đất
            Skill2.Add ((GameObject) Instantiate (Resources.Load<GameObject> (BattleCore.HeroSkillObjectLink + "H5SkillEffect"), new Vector3 (Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity));
            Skill2[0].GetComponent<Hero5Skill> ().Hero = this;
            Skill1[0].SetActive (false);
            //Skill 
            // Skill2[0] = Instantiate(Resources.Load<GameObject>(BattleCore.HeroSkillObjectLink + "H4Atk3"), new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity);
            // Skill2[0].GetComponent<Hero5Atk>().Hero = this;
            // Skill2[0].SetActive(false);
            // Skill2[1] = Instantiate(Resources.Load<GameObject>(BattleCore.HeroSkillObjectLink + "Hero2Skill1R"), new Vector3(Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity);
            // Skill2[0].GetComponent<Hero2Skill1>().Hero = this;
            // Skill2[0].SetActive(false);
            // Skill2[1].GetComponent<Hero2Skill1>().Hero = this;
            // Skill2[1].SetActive(false);
            //Set kiểu đánh gần hay xa cho mỗi skill. Nếu HType = HeroType.far. thì set tất cả cái dưới = 0. Chỉ được thay đổi giá trị cuối của dòng gán bên dưới
            SkillType[0] = HType == HeroType.far ? 0 : 1; //0 = đánh xa. 1 = cận chiến - đánh thường
            SkillType[1] = HType == HeroType.far ? 0 : 1; //0 = đánh xa. 1 = cận chiến - skill 1
            //SkillType[2] = HType == HeroType.far ? 0 : 0;//0 = đánh xa. 1 = cận chiến - skill 2

        }
        //
        // public override void Start()
        // {
        //     base.Start();
        // }

        public override void RefreshTeam (GameObject obj) {
            base.RefreshTeam (obj);
            // SkillExtension.layer = Team.Equals(0) ? 11 : 12;
            // SkillExtension.GetComponent<Collider2D>().enabled = true;
            //SkillExtension.transform.localScale = Team.Equals(0) ? SkillExtension.transform.localScale : new Vector3(transform.position.x - 13.6f, transform.position.y -2f, Module.BASELAYER[2]);
        }
        //Update
        public override void Update () {
            base.Update ();

            //Nội tại nv5: Khi hỗ trợ hoặc hạ gục đối phương, hồi lại 20% máu đã tổn thất
            if (BattleCore.Hero5IntrinsicEnable && BattleCore.Hero5IntrinsicTeam != Team) {
                var hpRegen = (DataValues.vHealth - DataValues.vHealthCurrent) * 20 / 100f;
                DataValues.vHealthCurrent += hpRegen;

                //Gọi hàm show chỉ số từ battle system
                Battle.DamageShow (this.transform.position, 2, Team, hpRegen);
                BattleCore.Hero5IntrinsicEnable = false;
                BattleCore.Hero5IntrinsicTeam = null;
            }
        }
        public override void ActionSkill (int skillnumber) {
            base.ActionSkill (skillnumber);
            switch (skillnumber) {
                case 0: //Normal atk
                    {
                        switch (ComboNormalAtk) {
                            case 0:
                                ShowSkill (Skill1[ComboNormalAtk], new Vector3 (transform.position.x + 3.32f, transform.position.y + 0.7f, Module.BASELAYER[2]), Quaternion.Euler (83f, 61.7f, -125.28f));
                                ComboNormalAtk++;
                                break;
                            case 1:
                                ShowSkill (Skill1[ComboNormalAtk], new Vector3 (transform.position.x + 3.28f, transform.position.y + 1.89f, Module.BASELAYER[2]), Quaternion.Euler (30.8f, 24.1f, -86f));
                                ComboNormalAtk++;
                                break;
                            case 2:
                                ShowSkill (Skill1[ComboNormalAtk], new Vector3 (transform.position.x + 2.7f, transform.position.y + 2.85f, Module.BASELAYER[2]), Quaternion.Euler (180f, 0f, -135.36f));
                                ComboNormalAtk++;
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case 1: //Skill1
                    ShowSkill (Skill2[0], Team.Equals (0) ? new Vector3 (transform.position.x + 13.6f, transform.position.y - 2f, Module.BASELAYER[2]) : new Vector3 (transform.position.x - 13.6f, transform.position.y - 2f, Module.BASELAYER[2]), Quaternion.identity);
                    break;
                default:
                    break;
            }
        }
        //Va chạm
        public override void OnTriggerEnter2D (Collider2D col) {
            base.OnTriggerEnter2D (col);
        }
        public override void OnCollisionEnter2D (Collision2D col) {
            base.OnCollisionEnter2D (col);
        }
    }
}