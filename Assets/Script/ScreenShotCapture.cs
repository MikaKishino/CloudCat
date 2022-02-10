using UnityEngine;

/// <summary>
/// スクリーンショットをキャプチャするサンプル
/// </summary>
public class ScreenShotCapture : MonoBehaviour
{
    int i=3;
    private void Update()
    {
        // スペースキーが押されたら
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // スクリーンショットを保存
            i += 1;
            CaptureScreenShot("ipad12_9_" + i.ToString() + ".png");
        }
    }

    // 画面全体のスクリーンショットを保存する
    private void CaptureScreenShot(string filePath)
    {
        ScreenCapture.CaptureScreenshot(filePath, 5);
        Debug.Log("スクショ保存");
    }

}