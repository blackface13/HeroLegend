using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using StartApp;
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

    InterstitialAd VideoGemReward;
    // Start is called before the first frame update
    void Start()
    {
        CreateVideoReward();
        StringBuilder strChange = new StringBuilder("");
        TextLanguage[0].text = Languages.lang[182];
        TextLanguage[1].text = Languages.lang[356];
        TextLanguage[2].text = Languages.lang[183];
        strChange.Append(GemQuantityRequired.ToString()).Append("     = ").Append(GoldQuantityReceived.ToString());
        TextLanguage[3].text = strChange.ToString();
    }

    /// <summary>
    /// Khởi tạo quảng cáo video nhận thưởng
    /// </summary>
    private void CreateVideoReward()
    {
        VideoGemReward = AdSdk.Instance.CreateInterstitial();
        VideoGemReward.RaiseAdVideoCompleted += (sender, e) => {
            UserSystem.IncreaseGems(50, true);
            DataUserController.SaveUserInfor();
        VideoGemReward.LoadAd(InterstitialAd.AdType.Rewarded);
        };
        VideoGemReward.LoadAd(InterstitialAd.AdType.Rewarded);
    }

    /// <summary>
    /// Nút xem video
    /// </summary>
    /// <param name="type">0: video nhận gem
    public void ButtonWatchVideo()
    {
        if (VideoGemReward.IsReady())
        {
            VideoGemReward.ShowAd(); //Hiển thị video quảng cáo
        }
        else
            GameSystem.ControlFunctions.ShowMessage((Languages.lang[181])); //Chờ video tiếp theo dc tải
    }

    /// <summary>
    /// Nút đổi gem lấy vàng
    /// </summary>
    public void ButtonChangeGem()
    {
        if (UserSystem.CheckGems(GemQuantityRequired))
        {
            UserSystem.DecreaseGems(GemQuantityRequired, false);
            UserSystem.IncreaseGolds(GoldQuantityReceived, false);
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