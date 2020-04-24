using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class UIGoldGemController : MonoBehaviour
{
    public GameObject[] Obj;
    public Text[] TextLanguage;

    [LabelText("Số gem cần đổi")]
    [Required]
    public int GemQuantityRequired;

    [LabelText("Số gold nhận dc")]
    [Required]
    public int GoldQuantityReceived;

    // Start is called before the first frame update
    void Start()
    {
        StringBuilder strChange = new StringBuilder("");
        TextLanguage[0].text = Languages.lang[182];
        TextLanguage[1].text = Languages.lang[183];
        TextLanguage[2].text = Languages.lang[356];
        strChange.Append(GemQuantityRequired.ToString()).Append("     = ").Append(GoldQuantityReceived.ToString());
        TextLanguage[3].text = strChange.ToString();
    }

    /// <summary>
    /// Nút xem video
    /// </summary>
    /// <param name="type">0: video nhận gem
    public void ButtonWatchVideo()
    {
        if (ADS.rewardBasedVideoGems.IsLoaded())
        {
            ADS.rewardBasedVideoGems.Show(); //Hiển thị video quảng cáo
        }
        else
            GameSystem.ControlFunctions.ShowMessage((Languages.lang[181])); //Chờ video tiếp theo dc tải
    }

    /// <summary>
    /// Nút đổi gem lấy vàng
    /// </summary>
    public void ButtonChangeGem(int gemQuantity)
    {
        if (UserSystem.CheckGems(gemQuantity))
        {
            UserSystem.DecreaseGems(GemQuantityRequired);
            UserSystem.IncreaseGolds(GoldQuantityReceived);
            DataUserController.SaveUserInfor();
            GameSystem.ControlFunctions.ShowMessage(Languages.lang[357]);
        }
    }

    /// <summary>
    /// Nút đóng UI
    /// </summary>
    public void ButtonCloseUI()
    {
        GameSystem.DisposePrefabUI(3);
    }
}