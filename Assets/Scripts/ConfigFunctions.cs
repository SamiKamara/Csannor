using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class ConfigFunctions : MonoBehaviour
{
    public string ConfigFilePath = "Assets/config.json";
    private EmailConfig config;

    public TMP_InputField apiKeyInputField;
    public TMP_InputField senderEmailInputField;
    public TMP_InputField receiverEmailInputField;

    private void OnEnable()
    {
        LoadConfiguration();
        UpdateUI();
    }

    public void UpdateSendGridAPIKey()
    {
        if (apiKeyInputField != null)
        {
            config.InjectedKey = apiKeyInputField.text;
            SaveConfiguration();
            UpdateUI();
            Debug.Log("SendGrid API Key updated successfully.");
        }
    }

    public void UpdateSenderEmail()
    {
        if (senderEmailInputField != null)
        {
            config.SenderAddress = senderEmailInputField.text;
            SaveConfiguration();
            UpdateUI();
            Debug.Log("Sender email updated successfully.");
        }
    }

    public void UpdateReceiverEmail()
    {
        if (receiverEmailInputField != null)
        {
            config.ReceiverAddress = receiverEmailInputField.text;
            SaveConfiguration();
            UpdateUI();
            Debug.Log("Receiver email updated successfully.");
        }
    }

    private void LoadConfiguration()
    {
        if (File.Exists(ConfigFilePath))
        {
            string json = File.ReadAllText(ConfigFilePath);
            config = JsonUtility.FromJson<EmailConfig>(json);
        }
        else
        {
            Debug.LogWarning($"Configuration file not found: {ConfigFilePath}. Creating a new one with default empty values.");
            config = new EmailConfig
            {
                InjectedKey = "",
                SenderAddress = "",
                ReceiverAddress = ""
            };

            SaveConfiguration();
        }

        UpdateUI();
    }

    private void SaveConfiguration()
    {
        string json = JsonUtility.ToJson(config, true);
        File.WriteAllText(ConfigFilePath, json);
        Debug.Log("Configuration file saved.");
    }

    public void UpdateUI()
    {
        if (apiKeyInputField != null)
            apiKeyInputField.text = config.InjectedKey;
        if (senderEmailInputField != null)
            senderEmailInputField.text = config.SenderAddress;
        if (receiverEmailInputField != null)
            receiverEmailInputField.text = config.ReceiverAddress;
    }
}
