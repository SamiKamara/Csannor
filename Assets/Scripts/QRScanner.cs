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
    private Coroutine qrCoroutine;

    void Start()
    {
        var renderer = GetComponent<RawImage>();
        webcamTexture = new WebCamTexture(512, 512);
        renderer.texture = webcamTexture;
        StartScanning();
    }

    private void OnEnable()
    {
        StartScanning();
    }

    private void OnDisable()
    {
        StopScanning();
    }

    private void StartScanning()
    {
        if (webcamTexture != null)
        {
            webcamTexture.Play();
            if (qrCoroutine == null)
            {
                qrCoroutine = StartCoroutine(GetQRCode());
            }
        }
    }

    private void StopScanning()
    {
        if (webcamTexture != null && webcamTexture.isPlaying)
        {
            webcamTexture.Stop();
        }
        if (qrCoroutine != null)
        {
            StopCoroutine(qrCoroutine);
            qrCoroutine = null;
        }
    }

    IEnumerator GetQRCode()
    {
        IBarcodeReader barCodeReader = new BarcodeReader();
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
                        string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        ShiftInfoHolder.text += "\n" + currentDateTime + "\n" + QrCode + "\n";
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
                yield return new WaitForSeconds(3);
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
        style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = h * 2 / 50;
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
        float textHeight = h * 2 / 100;
        float textWidth = w;
        Rect rect = new Rect((w - textWidth) / 2, (h - textHeight) / 2, textWidth, textHeight);
        string text = QrCode;
        GUI.Label(rect, text, style);
    }

    public void ClearShiftLog()
    {
        ShiftInfoHolder.text = "Shift log: \n";
    }
}
