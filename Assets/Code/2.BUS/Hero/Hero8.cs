using System;
using System.Collections;
using System.Collections.Generic;
using BlackCore;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Controller.Hero8 {
    public class Hero8 : HeroBase {
        //Initialize
        private float AtkSpeedOriginalTemp = 0f; //Tốc độ đánh, dùng cho nội tại
        private int TotalSecontIntrinsic = 0; //Tổng số giây nội tại
        public override void Awake () {
            base.Awake ();
            HType = HeroType.far; //Tướng đánh xa
            FirstSetupHero (this.gameObject); //Khởi tạo các values ban đầu cho hero
            SetupSkill ();
        }

        private void SetupSkill () {
            //Khởi tạo các object skill
            Skill1 = new List<GameObject> (); //Object phi tiêu
            Skill2 = new List<GameObject> (); //Skill như Q của Sivir
            Skill3 = null;
            SkillType = new int[3]; //Kiểu skill

            // Đưa vào scene
            // for (int i = 0; i < 5; i++)
            // {
            Skill1.Add ((GameObject) Instantiate (Resources.Load<GameObject> (BattleCore.HeroSkillObjectLink + "H8Atk"), new Vector3 (Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity));
            Skill1[0].GetComponent<Hero8Atk> ().Hero = this;
            Skill1[0].SetActive (false);
            //}
            Skill2.Add ((GameObject) Instantiate (Resources.Load<GameObject> (BattleCore.HeroSkillObjectLink + "H8Skill"), new Vector3 (Module.BASEVECTORHIDENOBJECT.x, Module.BASEVECTORHIDENOBJECT.y, Module.BASELAYER[2]), Quaternion.identity));
            Skill2[0].GetComponent<Hero8Skill> ().Hero = this;
            Skill2[0].SetActive (false);
            //Set kiểu đánh gần hay xa cho mỗi skill. Nếu HType = HeroType.far. thì set tất cả cái dưới = 0. Chỉ được thay đổi giá trị cuối của dòng gán bên dưới
            SkillType[0] = HType == HeroType.far ? 0 : 0; //0 = đánh xa. 1 = cận chiến - đánh thường
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
            AtkSpeedOriginalTemp = DataValues.vAtkSpeed;
        }

        //Update
        public override void Update () {
            base.Update ();

            //Nội tại nv8: Khi hỗ trợ hoặc hạ gục đối phương, tốc độ đánh sẽ được tăng gấp đôi trong 5 giây
            if (BattleCore.Hero8IntrinsicEnable && BattleCore.Hero8IntrinsicTeam != Team) {
                TotalSecontIntrinsic++;//Cộng dồn trong trường hợp hạ gục nhiều đối thủ trong 5s
                StartCoroutine (RunIntrinsic ());
                BattleCore.Hero8IntrinsicEnable = false;
                BattleCore.Hero8IntrinsicTeam = null;
            }
        }

        /// <summary>
        /// Chạy nội tại cho nhân vật
        /// Nội tại: Khi hỗ trợ hoặc hạ gục đối phương, tốc độ đánh sẽ được tăng gấp đôi trong 5 giây
        /// </summary>
        /// <returns></returns>
        private IEnumerator RunIntrinsic () {
            DataValues.vAtkSpeed = AtkSpeedOriginalTemp * 2f;
            yield return new WaitForSeconds (5);
            if (TotalSecontIntrinsic.Equals (1))//Nếu = 1 -> reset tốc độ đánh về như cũ
                DataValues.vAtkSpeed = AtkSpeedOriginalTemp;
            TotalSecontIntrinsic--;
        }

        /// <summary>
        /// Thực hiện các đòn tấn công
        /// </summary>
        /// <param name="skillnumber"></param>
        public override void ActionSkill (int skillnumber) {
            base.ActionSkill (skillnumber);
            switch (skillnumber) {
                case 0: //Normal atk
                    //var objdontactive = GetObjectDontActive(Skill1);//Tìm object chưa dc kích hoạt
                    // if (objdontactive != null)
                    //{
                    if (ComboNormalAtk == 2) //Nếu là đòn đánh thứ 3 của combo
                    {
                        StartCoroutine (MultiAtk (Skill1, new Vector3 (this.transform.position.x + (Team.Equals (0) ? 2f : -2f), this.transform.position.y + 1f, this.transform.position.z), .1f, 3, Quaternion.identity));
                        ComboNormalAtk = 0;
                    } else {
                        //Kiểm tra và khởi tạo object skill nếu tất cả các object đều active
                        if (CheckExistAndCreateEffectExtension (new Vector3 (this.transform.position.x + (Team.Equals (0) ? 2f : -2f), this.transform.position.y + 1f, this.transform.position.z), Skill1, Quaternion.identity))
                            Skill1[Skill1.Count - 1].GetComponent<Hero8Atk> ().Hero = this; //Gán class cho object skill mới tạo
                        //ShowSkill(objdontactive, this.transform.position, Quaternion.identity);
                        ComboNormalAtk++;
                    }
                    //}
                    break;
                case 1: //Skill1
                    if (CheckExistAndCreateEffectExtension (new Vector3 (this.transform.position.x + (Team.Equals (0) ? 2f : -2f), this.transform.position.y + 1f, Module.BASELAYER[Team.Equals (0) ? 2 : 3]), Skill2, Quaternion.identity))
                        Skill2[Skill2.Count - 1].GetComponent<Hero8Skill> ().Hero = this; //Gán class cho object skill mới tạo
                    //ShowSkill(Skill2[0], this.transform.position, Quaternion.Euler(70f, 0, 0));
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
                CheckExistAndCreateEffectExtension (new Vector3 (this.transform.position.x + (Team.Equals (0) ? 2f : -2f), this.transform.position.y + 1f, this.transform.position.z), Skill1, Quaternion.identity);

            count++;
            yield return new WaitForSeconds (.1f);
            if (count < 3)
                goto Begin;
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