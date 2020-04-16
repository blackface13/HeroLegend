using System;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 0: Chế tạo vật phẩm
/// 1: Danh sách hero
/// 2: Chi tiết hero
/// </summary>
public class IntroductionsController : MonoBehaviour {
    public sbyte IntroductionID; //Range sbyte from -128 -> 127
    public Text[] TextValues;
    void OnEnable () {
        TextValues[0].text = Languages.IntroductionTitle[IntroductionID]; //Title 
        TextValues[1].text = Languages.IntroductionDescriptions[IntroductionID]; //Description
    }

    /// <summary>
    /// Đóng form hướng dẫn
    /// </summary>
    public void CloseForm () {
        this.gameObject.SetActive (false);
    }

    /// <summary>
    /// Mở form hướng dẫn và truyền vào ID
    /// </summary>
    /// <param name="introductionID"></param>
    public void OpenForm (int introductionID) {
        IntroductionID = Convert.ToSByte (introductionID);
        this.gameObject.SetActive (true);
    }
}