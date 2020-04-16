using System;
using System.Collections;
using System.Collections.Generic;
using BlackCore;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Controller.Hero2 {
    public class Hero2 : HeroBase {
        //Initialize
        private float AtkSpeedOriginalTemp = 0f; //Tốc độ đánh, dùng cho nội tại
        public override void Awake () {
            base.Awake ();
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
                Skill1.Add ((GameObject) Instantiate (Resources.Load<GameObject> (BattleCore.HeroSkillObjectLink + "H2Atk" + (i + 1).ToString ()), new Vector3 (Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity));
                Skill1[i].GetComponent<Hero2Atk> ().Hero = this;
                Skill1[i].SetActive (false);
            }
            //Skill 
            Skill2.Add ((GameObject) Instantiate (Resources.Load<GameObject> (BattleCore.HeroSkillObjectLink + "Hero2Skill1L"), new Vector3 (Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity));
            Skill2.Add ((GameObject) Instantiate (Resources.Load<GameObject> (BattleCore.HeroSkillObjectLink + "Hero2Skill1R"), new Vector3 (Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity));
            Skill2[0].GetComponent<Hero2Skill1> ().Hero = this;
            Skill2[0].SetActive (false);
            Skill2[1].GetComponent<Hero2Skill1> ().Hero = this;
            Skill2[1].SetActive (false);
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
            AtkSpeedOriginalTemp = DataValues.vAtkSpeed;
        }

        //Update
        public override void Update () {
            base.Update ();

            //Nội tại nhân vật: Dưới 50% máu, tốc độ đánh tăng 1.5 lần, dưới 30% máu, tốc độ đánh tăng 2 lần, dưới 10% máu, tốc độ đánh tăng 3 lần
            if (DataValues.vHealthCurrent <= DataValues.vHealth * 0.1f) { //Máu < 10%
                DataValues.vAtkSpeed = AtkSpeedOriginalTemp * 3f;
            } else if (DataValues.vHealthCurrent <= DataValues.vHealth * 0.3f) { //Máu < 30%
                DataValues.vAtkSpeed = AtkSpeedOriginalTemp * 2f;
            } else if (DataValues.vHealthCurrent <= DataValues.vHealth / 2) { //Máu < 50%
                DataValues.vAtkSpeed = AtkSpeedOriginalTemp * 1.5f;
            } else {
                DataValues.vAtkSpeed = AtkSpeedOriginalTemp;
            }
        }
        public override void ActionSkill (int skillnumber) {
            base.ActionSkill (skillnumber);
            switch (skillnumber) {
                case 0: //Normal atk
                    {
                        switch (ComboNormalAtk) {
                            case 0:
                                ShowSkill (Skill1[ComboNormalAtk], new Vector3 (transform.position.x + 1.45f, transform.position.y + 0.71f, Module.BASELAYER[2]), Quaternion.Euler (36f, 48f, -94.59f));
                                ComboNormalAtk++;
                                break;
                            case 1:
                                ShowSkill (Skill1[ComboNormalAtk], new Vector3 (transform.position.x + 0.4f, transform.position.y + 0.71f, Module.BASELAYER[2]), Quaternion.Euler (-63.16f, -34.7f, -14.05f));
                                ComboNormalAtk++;
                                break;
                            case 2:
                                ShowSkill (Skill1[ComboNormalAtk], new Vector3 (transform.position.x + 0.4f, transform.position.y - 0.43f, Module.BASELAYER[2]), Quaternion.Euler (73.35f, 0f, 0f));
                                ComboNormalAtk++;
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case 1: //Skill1
                    ShowSkill (Team.Equals (1) ? Skill2[0] : Skill2[1], Team.Equals (1) ? new Vector3 (transform.position.x - 8f, transform.position.y + 3.5f, Module.BASELAYER[2]) : new Vector3 (transform.position.x + 8f, transform.position.y + 3.5f, Module.BASELAYER[2]), Quaternion.identity);
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