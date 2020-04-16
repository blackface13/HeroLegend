using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lưu file log khi xảy ra xung đột trong phần mềm
/// </summary>
public static class ErrorCode {
    public static List<string> Error = new List<string> ();
    /// <summary>
    /// Khởi tạo, gọi hàm này tại scene load game
    /// </summary>
    public static void Initialize () {
        if (Error.Count <= 0) {
            Error.Add ("E0001");//Lỗi attack của hero 1
        }
    }

    public static void WriteErrorLog (short errorIndex) {

    }
}