using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//Class gọi tất cả các hàm của các class static
public class ControlFunctions : MonoBehaviour {

    /// <summary>
    /// Hiển thị thông báo lên màn hình
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public void ShowMessage (string text) {
        try {
            StartCoroutine (GameSystem.ShowMessage (text)); //Show tin nhắn từ GameSystem
        } catch (Exception e) {
            print (e);
        }
    }

    /// <summary>
    /// Hiển thị thông báo liên tục, ngăn cách nhau bởi ';'
    /// </summary>
    public IEnumerator ShowMessagecontinuity (string text) {
        var textArray = text.Split (';');
        for (int i = 0; i < textArray.Length; i++) {
            if (!string.IsNullOrEmpty (textArray[i])) {
                StartCoroutine (GameSystem.ShowMessage (textArray[i])); //Show tin nhắn từ GameSystem
                yield return new WaitForSeconds (GlobalVariables.DelayBetween2MessageDisplay);
            }
        }
    }

    /// <summary>
    /// Hiển thị thông báo liên tục
    /// </summary>
    private IEnumerator ShowMessageContinuity(string[] text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            if (!string.IsNullOrEmpty(text[i]))
            {
                StartCoroutine(GameSystem.ShowMessage(text[i])); //Show tin nhắn từ GameSystem
                yield return new WaitForSeconds(GlobalVariables.DelayBetween2MessageDisplay);
            }
        }
    }

    public void ShowManyMessage(string[] text)
    {
        StartCoroutine(ShowMessageContinuity(text));
    }

    /// <summary>
    /// Khởi tạo lấy dữ liệu từ server khi load game
    /// </summary>
    public void SetupServer () {
        StartCoroutine (API.SetupServer ());
    }

    /// <summary>
    /// Khởi chạy âm thanh cho các skill của nhân vật
    /// </summary>
    /// <param name="audioFile">audio clip</param>
    /// <param name="timeDelay">thời gian chờ trước khi chạy âm thanh</param>
    /// <returns></returns>
    public IEnumerator PlaySound (AudioClip audioFile, float timeDelay) {
        if (GameSystem.Settings.SoundEnable) {
            yield return new WaitForSeconds (timeDelay);
            GameSystem.Sound.PlayOneShot (audioFile);
        }
    }
}