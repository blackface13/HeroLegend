using System;
using System.Collections;
using System.Collections.Generic;
using BlackCore;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Controller.Hero10 {
    public class Hero10 : HeroBase {
        private int DmgReceived = 0; //Đếm số lần chịu sát thương (dùng cho nội tại)
        private float ArmorOriginalTemp = 0f; //Giáp, dùng cho nội tại
        private float MagicResistOriginalTemp = 0f; //Kháng phép, dùng cho nội tại
        //Initialize
        public override void Awake () {
            base.Awake ();
            HType = HeroType.near; //Tướng cận chiến
            FirstSetupHero (this.gameObject); //Khởi tạo các values ban đầu cho hero
            SetupSkill ();
        }

        private void SetupSkill () {
            //Khởi tạo các object skill
            Skill1 = new List<GameObject> (); //Object phi tiêu
            Skill2 = new List<GameObject> (); //Skill
            Skill3 = null;
            SkillType = new int[3]; //Kiểu skill

            for (int i = 0; i < 3; i++) {
                Skill1.Add ((GameObject) Instantiate (Resources.Load<GameObject> (BattleCore.HeroSkillObjectLink + "H10Atk" + (i + 1).ToString ()), new Vector3 (Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity));
                Skill1[i].GetComponent<Hero10Atk> ().Hero = this;
                Skill1[i].SetActive (false);
            }
            Skill2.Add ((GameObject) Instantiate (Resources.Load<GameObject> (BattleCore.HeroSkillObjectLink + "H10Skill"), new Vector3 (Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity));
            Skill2[0].transform.GetChild (0).transform.GetComponent<Hero10Skill> ().Hero = this;
            Skill2[0].SetActive (false);
            //Set kiểu đánh gần hay xa cho mỗi skill. Nếu HType = HeroType.far. thì set tất cả cái dưới = 0. Chỉ được thay đổi giá trị cuối của dòng gán bên dưới
            SkillType[0] = HType == HeroType.far ? 0 : 1; //0 = đánh xa. 1 = cận chiến - đánh thường
            SkillType[1] = HType == HeroType.far ? 0 : 0; //0 = đánh xa. 1 = cận chiến - skill 1
            SkillType[2] = HType == HeroType.far ? 0 : 0; //0 = đánh xa. 1 = cận chiến - skill 2

        }
        //
        // public override void Start()
        // {
        //     base.Start();
        // }

        public override void RefreshTeam (GameObject obj) {
            base.RefreshTeam (obj);
            Skill2[0].transform.GetChild (0).transform.GetComponent<SkillCore> ().ReSetupLayer (Team); //Dành riêng cho skill của hero này, vì object va chạm là object con
            ArmorOriginalTemp = DataValues.vArmor;
            MagicResistOriginalTemp = DataValues.vMagicResist;
        }
        //Update
        public override void Update () {
            base.Update ();
        }
        public override void ActionSkill (int skillnumber) {
            base.ActionSkill (skillnumber);
            switch (skillnumber) {
                case 0: //Normal atk
                    switch (ComboNormalAtk) {
                        case 0:
                            ShowSkill (Skill1[ComboNormalAtk], new Vector3 (transform.position.x + (Team.Equals (0) ? 2.16f : -2.16f), transform.position.y + 1.5f, Module.BASELAYER[2]), Quaternion.Euler (-23f, -151.7f, 55f));
                            ComboNormalAtk++;
                            break;
                        case 1:
                            ShowSkill (Skill1[ComboNormalAtk], new Vector3 (transform.position.x + (Team.Equals (0) ? 3f : -3f), transform.position.y, Module.BASELAYER[2]), Quaternion.Euler (0, 0, 145f));
                            ComboNormalAtk++;
                            break;
                        case 2:
                            ShowSkill (Skill1[ComboNormalAtk], new Vector3 (transform.position.x + (Team.Equals (0) ? 2.16f : -2.16f), transform.position.y + 1.5f, Module.BASELAYER[2]), Quaternion.Euler (-23f, -151.7f, 55f));
                            ComboNormalAtk++;
                            break;
                        default:
                            break;
                    }
                    break;
                case 1: //Skill1
                    IsNoDame = true; //Nhân vật ở trạng thái không nhận sát thương
                    if (CheckExistAndCreateEffectExtension (new Vector3 (this.transform.position.x + (Team.Equals (0) ? 3f : -3f), this.transform.position.y + .4f, Module.BASELAYER[Team.Equals (0) ? 2 : 3]), Skill2, Quaternion.Euler (0, 60f, 0)))
                        Skill2[Skill2.Count - 1].transform.GetChild (0).GetComponent<Hero10Skill> ().Hero = this; //Gán class cho object skill mới tạo
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Viết riêng cho combo normal atk của hero này, sản sinh ra liên tục 3 phi tiêu
        /// </summary>
        /// <returns></returns>
        private IEnumerator ComboAtk () {
            int count = 0;
            Begin:
                var obj1 = GetObjectDontActive (Skill1);
            if (obj1 != null)
                ShowSkill (obj1, this.transform.position, Quaternion.identity);
            count++;
            yield return new WaitForSeconds (.1f);
            if (count < 3)
                goto Begin;
        }

        /// <summary>
        /// Dành cho hero 10: đưa nhân vật về trạng thái nhận sát thương từ đối phương sau khi dùng skill
        /// </summary>
        public override void EndAtk () {
            base.EndAtk ();
            IsNoDame = false;
        }

        //Va chạm
        public override void OnTriggerEnter2D (Collider2D col) {
            //Nội tại: Với mỗi lần chịu sát thương thứ 5, giáp và kháng phép sẽ được tăng gấp đôi
            if (DmgReceived % 5 == 0) {
                DataValues.vArmor = ArmorOriginalTemp * 2;
                DataValues.vMagicResist = MagicResistOriginalTemp * 2;
            } else {
                DataValues.vArmor = ArmorOriginalTemp;
                DataValues.vMagicResist = MagicResistOriginalTemp;
            }

            base.OnTriggerEnter2D (col);

            DmgReceived++; //Tăng số lần nhận sát thương
        }
        public override void OnCollisionEnter2D (Collision2D col) {
            //Nội tại: Với mỗi lần chịu sát thương thứ 5, giáp và kháng phép sẽ được tăng gấp đôi
            if (DmgReceived % 5 == 0) {
                DataValues.vArmor = ArmorOriginalTemp * 2;
                DataValues.vMagicResist = MagicResistOriginalTemp * 2;
            } else {
                DataValues.vArmor = ArmorOriginalTemp;
                DataValues.vMagicResist = MagicResistOriginalTemp;
            }

            base.OnCollisionEnter2D (col);

            DmgReceived++; //Tăng số lần nhận sát thương
        }
    }
}