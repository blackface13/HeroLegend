using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code._4.CORE.UnoCard
{
    public class UnoCardSettings : MonoBehaviour
    {
        public Text[] TextUI;
        public GameObject[] ObjectController;
        public Image CurentColor;

        private void Start()
        {
            SetupTextUI();
            GetParameterSetting();
            CurentColor.color = new Color32(DataUserController.User.UnoBGColorR, DataUserController.User.UnoBGColorG, DataUserController.User.UnoBGColorB, 1);
        }

        /// <summary>
        /// Gán ngôn ngữ
        /// </summary>
        private void SetupTextUI()
        {
            TextUI[0].text = Languages.lang[309];// = "Style Options";
            TextUI[1].text = Languages.lang[310];// = "Fast select card";
            TextUI[2].text = Languages.lang[320];// = "Background color";
            TextUI[3].text = Languages.lang[67];// = "Cancel";
            TextUI[4].text = Languages.lang[323];// = "Fast pass round";
            TextUI[5].text = Languages.lang[324];// = "Hỗ trợ hiển thị lá bài được đánh";
            TextUI[6].text = Languages.lang[345];// = "Tự bốc bài khi ko có lá phù hợp";
        }

        /// <summary>
        /// Lấy dữ liệu setting
        /// </summary>
        private void GetParameterSetting()
        {
            ObjectController[1].SetActive(DataUserController.User.UnoSettingFastPush);
            ObjectController[4].SetActive(DataUserController.User.UnoSettingFastPass);
            ObjectController[5].SetActive(DataUserController.User.UnoSettingImgSupport);
            ObjectController[6].SetActive(DataUserController.User.UnoSettingFastGetCard);
        }

        /// <summary>
        /// Các hàm chung
        /// </summary>
        public void GeneralFunctions(int type)
        {
            switch (type)
            {
                case 0://Đóng form
                    DataUserController.SaveUserInfor();
                    ObjectController[0].SetActive(false);
                    break;
                case 1://Đóng UI color picker
                    ObjectController[2].SetActive(false);
                    break;
                case 2://Lưu và đóng UI color picker
                    Color32 getColor = new Color(CurentColor.color.r, CurentColor.color.g, CurentColor.color.b);
                    DataUserController.User.UnoBGColorR = getColor.r;
                    DataUserController.User.UnoBGColorG = getColor.g;
                    DataUserController.User.UnoBGColorB = getColor.b;
                    ObjectController[3].GetComponent<Image>().color = new Color32(DataUserController.User.UnoBGColorR, DataUserController.User.UnoBGColorG, DataUserController.User.UnoBGColorB, 255);
                    ObjectController[2].SetActive(false);
                    break;
                case 5://Chức năng Fast push
                    DataUserController.User.UnoSettingFastPush = !DataUserController.User.UnoSettingFastPush;
                    GetParameterSetting();
                    break;
                case 6://Chức năng chọn màu
                    ObjectController[2].SetActive(true);
                    break;
                case 7://Chức năng Fast pass
                    DataUserController.User.UnoSettingFastPass = !DataUserController.User.UnoSettingFastPass;
                    GetParameterSetting();
                    break;
                case 8://Chức năng hỗ trợ hiển thị
                    DataUserController.User.UnoSettingImgSupport = !DataUserController.User.UnoSettingImgSupport;
                    GetParameterSetting();
                    break;
                case 9://Chức năng Fast get card
                    DataUserController.User.UnoSettingFastGetCard = !DataUserController.User.UnoSettingFastGetCard;
                    GetParameterSetting();
                    break;
            }
        }
    }
}
