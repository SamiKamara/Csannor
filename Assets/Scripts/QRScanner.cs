using System.Collections;
using System;
using UnityEngine;
using TMPro;
using ZXing;
using UnityEngine.UI;

public class QRScanner : MonoBehaviour
{
    public TMP_Text ShiftInfoHolder;
    private WebCamTexture webcamTexture;
    private string QrCode = string.Empty;

    void Start()
    {
        var renderer = GetComponent<RawImage>();
        webcamTexture = new WebCamTexture(512, 512);
        renderer.texture = webcamTexture;
        StartCoroutine(GetQRCode());
    }

    IEnumerator GetQRCode()
    {
        IBarcodeReader barCodeReader = new BarcodeReader();
        webcamTexture.Play();
        var snap = new Texture2D(
            webcamTexture.width,
            webcamTexture.height,
            TextureFormat.ARGB32,
            false
        );

        while (true)
        {
            bool decodeAttempted = false;
            try
            {
                snap.SetPixels32(webcamTexture.GetPixels32());
                var Result = barCodeReader.Decode(
                    snap.GetRawTextureData(),
                    webcamTexture.width,
                    webcamTexture.height,
                    RGBLuminanceSource.BitmapFormat.ARGB32
                );

                if (Result != null)
                {
                    QrCode = Result.Text;
                    if (!string.IsNullOrEmpty(QrCode))
                    {
                        ShiftInfoHolder.text += QrCode + "\n";
                        decodeAttempted = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex.Message);
            }
            if (decodeAttempted)
            {
                webcamTexture.Stop();
                yield return new WaitForSeconds(5);
                QrCode = string.Empty;
                webcamTexture.Play();
            }
            else
            {
                yield return null;
            }
        }
    }

    private void OnGUI()
    {
        int w = Screen.width, h = Screen.height;
        GUIStyle style = new GUIStyle();
        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = h * 2 / 50;
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
        string text = QrCode;
        GUI.Label(rect, text, style);
    }
}
