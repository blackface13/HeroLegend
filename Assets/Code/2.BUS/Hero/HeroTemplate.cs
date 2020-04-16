using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
namespace Assets.Code.Controller.Hero.Hero1
{
    public class HeroTemplate : HeroBase
    {
        //Initialize
        public override void Awake()
        {
            base.Awake();
            FirstSetupHero(this.gameObject);//Khởi tạo các values ban đầu cho hero
        }
        //
        // public override void Start()
        // {
        //     base.Start();
        // }
        //Update
        public override void Update()
        {
            base.Update();
        }

        //Va chạm
        public override void OnTriggerEnter2D(Collider2D col)
        {
            base.OnTriggerEnter2D(col);
        }
    }
}