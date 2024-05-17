using System.Collections;
using System;
using UnityEngine;
using TMPro;
using ZXing;
using UnityEngine.UI;
using System.Collections.Generic;

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

    private const float lineSpacing = 35.0f;

    private void OnGUI()
    {
        int w = Screen.width, h = Screen.height;
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = h * 2 / 50;
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);

        string text = QrCode;
        string[] lines = SplitTextIntoLines(text, 20);

        float textHeight = h * 2 / 100;
        float totalHeight = textHeight * lines.Length + lineSpacing * (lines.Length - 1);
        float startY = (h - totalHeight) / 2;

        for (int i = 0; i < lines.Length; i++)
        {
            Rect rect = new Rect(0, startY + i * (textHeight + lineSpacing), w, textHeight);
            GUI.Label(rect, lines[i], style);
        }
    }

    private string[] SplitTextIntoLines(string text, int maxLineLength)
    {
        List<string> lines = new List<string>();

        for (int i = 0; i < text.Length; i += maxLineLength)
        {
            if (i + maxLineLength < text.Length)
                lines.Add(text.Substring(i, maxLineLength));
            else
                lines.Add(text.Substring(i));
        }

        return lines.ToArray();
    }

    public void ClearShiftLog()
    {
        ShiftInfoHolder.text = "Shift log: \n";
    }
}
