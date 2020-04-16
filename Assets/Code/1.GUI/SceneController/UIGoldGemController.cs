using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGoldGemController : MonoBehaviour {
    public GameObject[] Obj;
    public Text[] TextLanguage;
    // Start is called before the first frame update
    void Start () {
TextLanguage[0].text = Languages.lang[182];
TextLanguage[1].text = Languages.lang[183];
TextLanguage[2].text = Languages.lang[183];
    }

    /// <summary>
    /// Nút xem video
    /// </summary>
    /// <param name="type">0: video nhận gem, 1 video nhận vàng</param>
    public void ButtonWatchVideo (int type) {
        if (type.Equals (0)) { //Video gems
            if (ADS.rewardBasedVideoGems.IsLoaded ()) {
                ADS.rewardBasedVideoGems.Show (); //Hiển thị video quảng cáo
            } else
                GameSystem.ControlFunctions.ShowMessage( (Languages.lang[181])); //Chờ video tiếp theo dc tải
        } else { //Video gold
            if (ADS.rewardBasedVideoGold.IsLoaded ()) {
                ADS.rewardBasedVideoGold.Show (); //Hiển thị video quảng cáo
            } else
                GameSystem.ControlFunctions.ShowMessage( (Languages.lang[181])); //Chờ video tiếp theo dc tải
        }
    }

/// <summary>
/// Nút đóng UI
/// </summary>
    public void ButtonCloseUI()
    {
        GameSystem.DisposePrefabUI (3);
    }
}