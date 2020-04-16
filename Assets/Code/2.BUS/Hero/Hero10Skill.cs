using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BlackCore;
using UnityEngine;
namespace Controller.Hero10 {
    //Skill chặn sát thương như Braum
    public class Hero10Skill : SkillCore {
        private GameObject ParentObject; //Object cha (vì đây là hiệu ứng)
        //public Hero10 Hero;
        #region Initialize 

        public override void Awake () {
            base.Awake ();
            ParentObject = transform.parent.gameObject;
            if (GameSystem.Settings.SoundEnable) {
                SoundClip = new AudioClip[7];
                for (int i = 0; i < SoundClip.Length; i++)
                    SoundClip[i] = Resources.Load<AudioClip> ("Audio/Skill/H10Skill" + (i + 1).ToString ());
            }
        }
        //
        public override void Start () {
            base.Start ();
            //Status = status.Stun;//Hiệu ứng choáng
            //TimeStatus = 3f;//Thời gian gây ra hiệu ứng
            //RatioStatus = 100;//Tỉ lệ gây ra hiệu ứng, 1 = 1%
            DataValues = Hero.DataValues; //Gán tất cả giá trị vào skillcore để tính damage khi va chạm với hero khác
            DamagePercent = 20; //Mặc định là 100% damage gây ra
            CollisionType = 1; //Đưa skill về trạng thái mặc định
            //skillcore.SkillType = 0;//Mặc định = 0, vật lý, sửa lại = 1 khi là sát thương phép
            // EffectExtension = new List<GameObject>();
            // SetupEffectExtension("H3AtkHitEffect");//Khởi tạo hiệu ứng effect riêng cho từng skill của hero (nếu có)
        }
        public override void ReSetupLayer (int team) {
            base.ReSetupLayer (team);
            ParentObject.transform.localScale = Team.Equals (0) ? transform.localScale : new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            //ParentObject.transform.GetChild(0).localScale = Team.Equals(0) ? transform.localScale : new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            ParentObject.transform.GetChild (1).localScale = Team.Equals (0) ? transform.localScale : new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            ParentObject.transform.GetChild (2).localScale = Team.Equals (0) ? transform.localScale : new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        //Sau khi được active
        private void OnEnable () {
            //Thiết lập âm thanh
            if (GameSystem.Settings.SoundEnable) {
                //Chạy âm thanh khởi động khiên đỡ
                var rand = UnityEngine.Random.Range (0, 4);
                StartCoroutine (Battle.PlaySound (SoundClip[rand], 0));
                //Chạy âm thanh quá trình đang đỡ
                rand = UnityEngine.Random.Range (4, SoundClip.Length);
                StartCoroutine (Battle.PlaySound (SoundClip[rand], .3f));
            }
            GetComponent<Collider2D> ().enabled = true;
            // StartCoroutine(AutoDisCol(0.2f, gameObject));
            //Vec = gameObject.transform.position;
            //CollisionType = 1;//Đưa skill về trạng thái mặc định
            // RatioStatus = 0;//Tỉ lệ gây ra hiệu ứng, 1 = 1%
            StartCoroutine (AutoHiden (5f, ParentObject)); //Ẩn game object nếu xuất hiện quá lâu
            //StartCoroutine(LoopingAtk(.1f, 13));//Gây sát thương liên tục
            StartCoroutine (AutoDisCol (2f, gameObject));
        }
        /// Xử lý va chạm với skill của đối phương
        private void OnTriggerEnter2D (Collider2D col) {
            if ((Hero.Team.Equals (0) && col.gameObject.layer.Equals (Module.BASELAYERRIGID2D[4])) || (Hero.Team.Equals (1) && col.gameObject.layer.Equals (Module.BASELAYERRIGID2D[3]))) {
                //col.gameObject.SetActive(false);
                try {
                    if (col.gameObject.GetComponent<SkillCore> ().Hero.HeroID.Equals (10))//Không thao tác gì khi va chạm với skill chặn của hero10 đối phương
                     { } else if (col.gameObject.GetComponent<SkillCore> ().Hero.HeroID.Equals (7))
                        col.gameObject.GetComponent<SkillCore> ().Hide (col.gameObject.transform.parent.gameObject); //Với skill của hero 7 thì ẩn parent của object
                    else if (col.gameObject.GetComponent<SkillCore> ().Hero.HeroID.Equals (2))
                        col.gameObject.GetComponent<SkillCore> ().Hide (col.gameObject.transform.parent.gameObject.transform.parent.gameObject); //Với skill của hero 2 thì ẩn parent của parent của object
                    else
                        col.gameObject.GetComponent<SkillCore> ().Hide (col.gameObject); //Ẩn object skill của đối phương sau khi va chạm với khiên chắn
                } catch {

                }
                //CheckExistAndCreateEffectExtension(col.transform.position);//Hiển thị hiệu ứng trúng đòn lên đối phương
            }
        }
        #endregion
    }
}