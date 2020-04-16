using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BlackCore;
using System.Collections;
namespace Controller.Hero6
{
    //Skill giống ult của lux
    public class Hero6Skill : SkillCore
    {
        private int QltAtk = 2;//Số sát thương gây ra cho mỗi hero
        private int QltAtkCount;
        public float SpeedScale;//Tốc độ giãn nở scale
        private Vector3 ScaleOriginal = new Vector3(1, 3, 1);
        //public Hero6 Hero;
        #region Initialize 

        public override void Awake()
        {
            base.Awake();
            TimeMove = .6f;//Định thời gian move của object này
            SpeedScale = 300f;//Tốc độ giãn nở scale
            //Thiết lập âm thanh
            if (GameSystem.Settings.SoundEnable) {
                SoundClip = new AudioClip[5];
                for (int i = 0; i < SoundClip.Length; i++)
                    SoundClip[i] = Resources.Load<AudioClip> ("Audio/Skill/H6Skill" + (i + 1).ToString ());
            }
        }
        //
        public override void Start()
        {
            base.Start();
            //Status = status.Stun;//Hiệu ứng choáng
            //TimeStatus = 3f;//Thời gian gây ra hiệu ứng
            //RatioStatus = 100;//Tỉ lệ gây ra hiệu ứng, 1 = 1%
            DataValues = Hero.DataValues;//Gán tất cả giá trị vào skillcore để tính damage khi va chạm với hero khác
            DamagePercent = 200;//Mặc định là 100% damage gây ra
            SkillType = 1;//Mặc định = 0, vật lý, sửa lại = 1 khi là sát thương phép
            EffectExtension = new List<GameObject>();
            SetupEffectExtension("H3AtkHitEffect");//Khởi tạo hiệu ứng effect riêng cho từng skill của hero (nếu có)
        }
        public override void ReSetupLayer(int team)
        {
            base.ReSetupLayer(team);
            //transform.localScale = Team.Equals(0) ? transform.localScale : new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        //Sau khi được active
        private void OnEnable()
        {
            //Thiết lập âm thanh
            if (GameSystem.Settings.SoundEnable) {
                var rand = UnityEngine.Random.Range (0, SoundClip.Length);
                StartCoroutine (Battle.PlaySound (SoundClip[rand], 0));
            }
            QltAtkCount = 0;//Reset bộ đếm số lần gây sát thương
            ScaleOriginal = new Vector3(1, 3, 1);
            transform.localScale = ScaleOriginal;
            // GetComponent<Collider2D>().enabled = true;
            // StartCoroutine(AutoDisCol(0.2f, gameObject));
            //Vec = gameObject.transform.position;
            CollisionType = 1;//Đưa skill về trạng thái mặc định
            // RatioStatus = 0;//Tỉ lệ gây ra hiệu ứng, 1 = 1%
            StartCoroutine(AutoHiden(3f, this.gameObject));//Ẩn game object nếu xuất hiện quá lâu
        }
        /// Xử lý va chạm
        private void OnTriggerEnter2D(Collider2D col)
        {
            if ((Hero.Team.Equals(0) && col.gameObject.layer.Equals(Module.BASELAYERRIGID2D[2])) || (Hero.Team.Equals(1) && col.gameObject.layer.Equals(Module.BASELAYERRIGID2D[1])))
            {
                if (CollisionType.Equals(0))//Nếu kiểu va chạm rồi ẩn
                    Hide(this.gameObject);//Ẩn object sau khi va chạm 
                CheckExistAndCreateEffectExtension(col.transform.position, EffectExtension);//Hiển thị hiệu ứng trúng đòn lên đối phương

            }
        }
        private void Update()
        {
            if (Team.Equals(0))
            {
                if (ScaleOriginal.x >= 60f)
                {
                    QltAtkCount++;
                    if (QltAtkCount <= QltAtk)
                    {
                        GetComponent<Collider2D>().enabled = false;
                        GetComponent<Collider2D>().enabled = true;
                    }
                }
                ScaleOriginal.x += SpeedScale * Time.deltaTime;
                if (ScaleOriginal.x >= 75f)
                {
                    if (ScaleOriginal.y <= 0f)
                    {
                        ScaleOriginal.y = 0f;
                        Hide(this.gameObject);//Ẩn object sau khi thu nhỏ 
                    }
                    else
                    {
                        ScaleOriginal.y -= SpeedScale / 20 * Time.deltaTime;
                    }
                }
            }
            else
            {
                if (ScaleOriginal.x <= -60f)
                {
                    QltAtkCount++;
                    if (QltAtkCount <= QltAtk)
                    {
                        GetComponent<Collider2D>().enabled = false;
                        GetComponent<Collider2D>().enabled = true;
                    }
                }
                ScaleOriginal.x -= SpeedScale * Time.deltaTime;
                if (ScaleOriginal.x <= -75f)
                {
                    if (ScaleOriginal.y <= 0f)
                    {
                        ScaleOriginal.y = 0f;
                        Hide(this.gameObject);//Ẩn object sau khi thu nhỏ 
                    }
                    else
                    {
                        ScaleOriginal.y -= SpeedScale / 20 * Time.deltaTime;
                    }
                }
            }
            transform.localScale = ScaleOriginal;
        }
        #endregion
    }
}
