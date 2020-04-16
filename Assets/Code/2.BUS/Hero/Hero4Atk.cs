using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BlackCore;
using UnityEngine;
namespace Controller.Hero4 {
    //Normal atk
    public class Hero4Atk : SkillCore {
        //public Hero4 Hero;
        Vector3 Rot;
        #region Initialize 

        public override void Awake () {
            GetComponent<Collider2D> ().enabled = false;
            base.Awake ();
            //Thiết lập âm thanh
            if (GameSystem.Settings.SoundEnable) {
                SoundClip = new AudioClip[3];
                for (int i = 0; i < SoundClip.Length - 1; i++)
                    SoundClip[i] = Resources.Load<AudioClip> ("Audio/Skill/H4Nor" + (i + 1).ToString ());
                SoundClip[2] = Resources.Load<AudioClip> ("Audio/Skill/H4Skill1");
            }
        }
        //
        public override void Start () {
            base.Start ();
            //Status = status.Stun;//Hiệu ứng choáng
            //TimeStatus = 3f;//Thời gian gây ra hiệu ứng
            // RatioStatus = 0;//Tỉ lệ gây ra hiệu ứng, 1 = 1%
            DataValues = Hero.DataValues; //Gán tất cả giá trị vào skillcore để tính damage khi va chạm với hero khác
            //DamagePercent = 100;//Mặc định là 100% damage gây ra
            //skillcore.SkillType = 0;//Mặc định = 0, vật lý, sửa lại = 1 khi là sát thương phép
            EffectExtension = new List<GameObject> ();
            SetupEffectExtension ("H4AtkHitEffect"); //Khởi tạo hiệu ứng effect riêng cho từng skill của hero (nếu có)
        }
        //Sau khi được active
        private void OnEnable () {
            //Thiết lập âm thanh
            if (GameSystem.Settings.SoundEnable) {
                if (DamagePercent == 100) //Đánh thường, Do code hơi lỗi, gộp cả skill và đánh thường nên phải thêm đoạn này để phân biệt, ko tái sử dụng
                {
                    var rand = UnityEngine.Random.Range (0, SoundClip.Length - 1);
                    StartCoroutine (Battle.PlaySound (SoundClip[rand], 0));
                } //Skill
                else StartCoroutine (Battle.PlaySound (SoundClip[2], 0));
            }
            StartCoroutine (AutoEnableCol (0.1f, gameObject)); //Tự động bật va chạm
            StartCoroutine (AutoDisCol (0.3f, gameObject)); //Tự động bật va chạm
            if (Team.Equals (1)) {
                transform.position = new Vector3 (transform.position.x - ((transform.position.x - Hero.transform.position.x) * 2), transform.position.y, transform.position.z);
                transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            transform.GetChild (0).transform.localScale = transform.localScale;
            CollisionType = 1; //Đưa skill về trạng thái mặc định
            RatioStatus = 0; //Tỉ lệ gây ra hiệu ứng, 1 = 1%
            //herosee = Module.CURRENSEE;
            StartCoroutine (AutoHiden (1f, this.gameObject)); //Ẩn game object sau 1s nếu ko detect dc va chạm
        }

        /// Xử lý va chạm
        private void OnTriggerEnter2D (Collider2D col) {
            if ((Hero.Team.Equals (0) && col.gameObject.layer.Equals (Module.BASELAYERRIGID2D[2])) || (Hero.Team.Equals (1) && col.gameObject.layer.Equals (Module.BASELAYERRIGID2D[1]))) {
                if (CollisionType.Equals (0)) //Nếu kiểu va chạm rồi ẩn
                    Hide (this.gameObject); //Ẩn object sau khi va chạm 

                //Nội tại được sử dụng
                if (Hero.IntrinsicEnable) {
                    PercentHealthAtkEnable = true; //Bật trừ % máu
                    PercentHealthCreated = 10; //Trừ 10% máu
                    Hero.IntrinsicEnable = false;
                }
                CheckExistAndCreateEffectExtension (col.transform.position, EffectExtension); //Hiển thị hiệu ứng trúng đòn lên đối phương
            }
        }

        #endregion
        public override void Hide (GameObject obj) {
            base.Hide (obj);
            gameObject.transform.localScale = new Vector3 (1, 1, 1);
        }
    }
}