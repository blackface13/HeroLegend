using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BlackCore;
using UnityEngine;
namespace Controller.Hero1003 {
    //Normal atk
    public class Hero1003Atk : SkillCore {
        //public Hero1003 Hero;
        Vector3 Rot;
        public int SlotCombo; //Phân biệt đòn đánh thứ bao nhiêu
        #region Initialize 

        public override void Awake () {
            GetComponent<Collider2D> ().enabled = false;
            base.Awake ();
            //Thiết lập âm thanh
            if (GameSystem.Settings.SoundEnable) {
                SoundClip = new AudioClip[4];
                for (int i = 0; i < SoundClip.Length; i++)
                    SoundClip[i] = Resources.Load<AudioClip> ("Audio/Skill/H5Nor" + (i + 1).ToString ());
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
            //SetupEffectExtension("H5AtkHitEffect");//Khởi tạo hiệu ứng effect riêng cho từng skill của hero (nếu có)
        }
        //Sau khi được active
        private void OnEnable () {
            //Thiết lập âm thanh
            if (GameSystem.Settings.SoundEnable) {
                var rand = UnityEngine.Random.Range (0, SoundClip.Length);
                StartCoroutine (Battle.PlaySound (SoundClip[rand], 0));
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
            StartCoroutine (AutoHiden (2f, this.gameObject)); //Ẩn game object sau 1s nếu ko detect dc va chạm
            if (SlotCombo.Equals (2)) //Gây sát thương liên tục 3 lần nết là đòn đánh thứ 3
               StartCoroutine(LoopingAtk (0.05f, 3));
        }
        /// Xử lý va chạm
        private void OnTriggerEnter2D (Collider2D col) {
            if ((Hero.Team.Equals (0) && col.gameObject.layer.Equals (Module.BASELAYERRIGID2D[2])) || (Hero.Team.Equals (1) && col.gameObject.layer.Equals (Module.BASELAYERRIGID2D[1]))) {
                if (CollisionType.Equals (0)) //Nếu kiểu va chạm rồi ẩn
                    Hide (this.gameObject); //Ẩn object sau khi va chạm
                // CheckExistAndCreateEffectExtension(col.transform.position, EffectExtension);//Hiển thị hiệu ứng trúng đòn lên đối phương
            }
        }

        #endregion
        public override void Hide (GameObject obj) {
            base.Hide (obj);
            gameObject.transform.localScale = new Vector3 (1, 1, 1);
        }
    }
}