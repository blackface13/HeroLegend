using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code._4.CORE.UnoCard
{
    public class UnoCardIntroductions : MonoBehaviour
    {
        public GameObject[] ObjectController;
        public Text[] TextUI;
        public GameObject[] ObjectView;
        private void Start()
        {
            SetupTextUI();
        }

        private void SetupTextUI()
        {
            TextUI[6].text = Languages.lang[340];
            TextUI[7].text = Languages.lang[341];
            TextUI[8].text = Languages.lang[342];
            TextUI[9].text = Languages.lang[344];
            TextUI[10].text = Languages.lang[325];
            TextUI[11].text = Languages.lang[326];
            TextUI[12].text = Languages.lang[327];
            TextUI[13].text = Languages.lang[328];
            TextUI[14].text = Languages.lang[329];
            TextUI[15].text = Languages.lang[330];
            TextUI[16].text = Languages.lang[331];
            TextUI[17].text = Languages.lang[343];

            TextUI[18].text = Languages.lang[332];
            TextUI[19].text = Languages.lang[333];
            TextUI[20].text = Languages.lang[334];
            TextUI[21].text = Languages.lang[335];
            TextUI[22].text = Languages.lang[336];
            TextUI[23].text = Languages.lang[337];
            TextUI[24].text = Languages.lang[338];
            TextUI[25].text = Languages.lang[339];
        }

        public void ChangeIntroductions(int order)
        {
            for (int i = 0; i < ObjectView.Length; i++)
            {
                ObjectView[i].SetActive(false);
            }
            ObjectView[order].SetActive(true);
        }

        public void GeneralFunctions(int type)
        {
            switch(type)
            {
                case 0://Đóng UI
                    GameSystem.DisposePrefabUI(10);
                    break;
                default:break;
            }
        }
    }
}
