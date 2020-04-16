/* Created: Bkack Face (bf.blackface@gmail.com)
 * Battle system
 * 2018/12/01
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Thiết lập những thứ cần thiết trước khi bắt đầu trò chơi
public class GameFirstSetup : MonoBehaviour {

    // Use this for initialization
    private void Awake()
    {
        CheckDefaultScreenSize();
        CheckLimitFPS();
    }
    /// <summary>
    /// Set defaul value screen size (Kiểm tra và lưu kích thước mặc định của màn hình thiết bị)
    /// </summary>
    /// <returns></returns>
    private void CheckDefaultScreenSize()
    {
        if (Module.GameLoad("ScreenSizeDefault").Equals(null) || Module.GameLoad("ScreenSizeDefault").Equals(""))
            Module.GameSave("ScreenSizeDefault", Screen.width + ":" + Screen.height);
        //yield return new WaitForSeconds(0);
    }

    /// <summary>
    /// Set limit FPS
    /// </summary>
    private void CheckLimitFPS()
    {
        if(Module.IsLimitFPS)
            Application.targetFrameRate = GameSystem.Settings.FPSLimit;
    }
}
