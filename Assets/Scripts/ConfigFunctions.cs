using System.Collections;
using UnityEngine;
using TMPro;

public class ConfigFunctions : MonoBehaviour
{
    public TMP_InputField apiKeyInputField;
    public TMP_InputField senderEmailInputField;
    public TMP_InputField receiverEmailInputField;

    private void OnEnable()
    {
        UpdateUI();
    }

    public void UpdateSendGridAPIKey()
    {
        if (apiKeyInputField != null)
        {
            PlayerPrefs.SetString("InjectedKey", apiKeyInputField.text);
            PlayerPrefs.Save();
            UpdateUI();
            Debug.Log("SendGrid API Key updated successfully.");
        }
    }

    public void UpdateSenderEmail()
    {
        if (senderEmailInputField != null)
        {
            PlayerPrefs.SetString("SenderAddress", senderEmailInputField.text);
            PlayerPrefs.Save();
            UpdateUI();
            Debug.Log("Sender email updated successfully.");
        }
    }

    public void UpdateReceiverEmail()
    {
        if (receiverEmailInputField != null)
        {
            PlayerPrefs.SetString("ReceiverAddress", receiverEmailInputField.text);
            PlayerPrefs.Save();
            UpdateUI();
            Debug.Log("Receiver email updated successfully.");
        }
    }

    private void UpdateUI()
    {
        if (apiKeyInputField != null)
            apiKeyInputField.text = PlayerPrefs.GetString("InjectedKey", "");
        if (senderEmailInputField != null)
            senderEmailInputField.text = PlayerPrefs.GetString("SenderAddress", "");
        if (receiverEmailInputField != null)
            receiverEmailInputField.text = PlayerPrefs.GetString("ReceiverAddress", "");
    }
}
