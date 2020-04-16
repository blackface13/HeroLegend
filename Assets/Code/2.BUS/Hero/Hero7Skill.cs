using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BlackCore;
using System.Collections;
namespace Controller.Hero7
{
    //Skill phun lửa gây sát thương liên tục
    public class Hero7Skill : SkillCore
    {
        private int MaxAtk = 13;//Số lần gây sát thương tối đa
        private float MaxScale = 20f;//Phạm vi tối đa có thể gây sát thương
        private float TimeRespawn = .1f;//Khoảng cách thời gian giữa 2 lần sát thương
        private GameObject ParentObject;//Object giãn nở scale để va chạm (vì đây là hiệu ứng)
        public float SpeedScale;//Tốc độ giãn nở scale cho va chạm
        private Vector3 ScaleOriginal = new Vector3(1, 1, 1);
        //public Hero7 Hero;
        #region Initialize 

        public override void Awake()
        {
            base.Awake();
            //ObjCollision = transform.GetChild(0).gameObject;
           ParentObject = transform.parent.gameObject;
           SpeedScale = 20f;//Tốc độ giãn nở scale cho va chạm
            //Thiết lập âm thanh
            if (GameSystem.Settings.SoundEnable) {
                SoundClip = new AudioClip[2];
                for (int i = 0; i < SoundClip.Length; i++)
                    SoundClip[i] = Resources.Load<AudioClip> ("Audio/Skill/H7Skill" + (i + 1).ToString ());
            }
        }
        //
        public override void Start()
        {
            base.Start();
            //Status = status.Stun;//Hiệu ứng choáng
            TimeStatus = 3f;//Thời gian gây ra hiệu ứng
            //RatioStatus = 100;//Tỉ lệ gây ra hiệu ứng, 1 = 1%
            DataValues = Hero.DataValues;//Gán tất cả giá trị vào skillcore để tính damage khi va chạm với hero khác
            DamagePercent = 20;//Mặc định là 100% damage gây ra
            CollisionType = 1;//Đưa skill về trạng thái mặc định
            SkillType = 1;//Mặc định = 0, vật lý, sửa lại = 1 khi là sát thương phép
            // EffectExtension = new List<GameObject>();
            // SetupEffectExtension("H3AtkHitEffect");//Khởi tạo hiệu ứng effect riêng cho từng skill của hero (nếu có)
        }
        public override void ReSetupLayer(int team)
        {
            base.ReSetupLayer(team);
            ParentObject.transform.localScale = Team.Equals(0) ? transform.localScale : new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            //ParentObject.transform.GetChild(0).localScale = Team.Equals(0) ? transform.localScale : new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            ParentObject.transform.GetChild(1).localScale = Team.Equals(0) ? transform.localScale : new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            ParentObject.transform.GetChild(2).localScale = Team.Equals(0) ? transform.localScale : new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        //Sau khi được active
        private void OnEnable()
        {
            //Thiết lập âm thanh
            if (GameSystem.Settings.SoundEnable) {
                var rand = UnityEngine.Random.Range (0, SoundClip.Length);
                StartCoroutine (Battle.PlaySound (SoundClip[rand], 0));
            }
            ScaleOriginal = new Vector3(1, 1, 1);
            transform.localScale = ScaleOriginal;
            // GetComponent<Collider2D>().enabled = true;
            // StartCoroutine(AutoDisCol(0.2f, gameObject));
            //Vec = gameObject.transform.position;
            //CollisionType = 1;//Đưa skill về trạng thái mặc định
            // RatioStatus = 0;//Tỉ lệ gây ra hiệu ứng, 1 = 1%
            StartCoroutine(AutoHiden(3f, ParentObject));//Ẩn game object nếu xuất hiện quá lâu
            StartCoroutine(LoopingAtk(.1f, 13));//Gây sát thương liên tục
        }

/// <summary>
/// Hàm gây sát thương liên tục riêng cho skill này, hiệu ứng thiêu đốt sẽ xuất hiện ở lần gây dame cuối cùng
/// </summary>
/// <param name="TimeRespawn"></param>
/// <param name="MaxAtk"></param>
/// <returns></returns>
        public override IEnumerator LoopingAtk(float TimeRespawn, int MaxAtk)
        {
var count = 0;
        Start:
            yield return new WaitForSeconds(TimeRespawn);
            if (count >= MaxAtk-1)//ở lần gây dame cuối cùng
this.tag = BattleCore.TagEffectFireBurn;//Set hiệu ứng thiêu đốt
            if (count >= MaxAtk)
                goto End;
            else
            {
                count++;
                GetComponent<Collider2D>().enabled = false;
                GetComponent<Collider2D>().enabled = true;
                goto Start;
            }
        End:
this.tag = "Untagged";//Thiêu đốt xong đưa về trạng thái ban đầu
            GetComponent<Collider2D>().enabled = false;
        }
        /// Xử lý va chạm
        private void OnTriggerEnter2D(Collider2D col)
        {
            if ((Hero.Team.Equals(0) && col.gameObject.layer.Equals(Module.BASELAYERRIGID2D[2])) || (Hero.Team.Equals(1) && col.gameObject.layer.Equals(Module.BASELAYERRIGID2D[1])))
            {
                if (CollisionType.Equals(0))//Nếu kiểu va chạm rồi ẩn
                    Hide(this.gameObject);//Ẩn object sau khi va chạm 
                //CheckExistAndCreateEffectExtension(col.transform.position);//Hiển thị hiệu ứng trúng đòn lên đối phương

            }
        }
        private void Update()
        {
            if (ScaleOriginal.x >= MaxScale)
                ScaleOriginal.x = MaxScale;
            else
                ScaleOriginal.x += SpeedScale * Time.deltaTime;

            transform.localScale = ScaleOriginal;
        }
        #endregion
    }
}
