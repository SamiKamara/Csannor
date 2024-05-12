using SendGrid;
using SendGrid.Helpers.Mail;
using System.IO;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;
using TMPro;

public class EmailSender : MonoBehaviour
{
    public string ConfigFilePath = "Assets/config.json";
    private EmailConfig config;
    public bool debugSend;
    public string SenderUser;
    public string SenderAddress;
    public string ReceiverUser;
    public string ReceiverAddress;
    public string MessageSubject;
    public TMP_Text MessageContentTMP;

    private void OnEnable()
    {
        LoadConfiguration();
    }

    private void Update()
    {
        if (debugSend)
        {
            SendMail();
            debugSend = false;
        }
    }

    public void SendMail()
    {
        StartCoroutine(SendMailCoroutine());
    }

    private IEnumerator SendMailCoroutine()
    {
        var task = Execute();
        yield return new WaitUntil(() => task.IsCompleted);
        if (task.IsFaulted)
        {
            Debug.LogError($"Failed to send email: {task.Exception?.Message}");
        }
        else
        {
            Debug.Log("Email sent successfully!");
        }
    }

    private async Task Execute()
    {
        if (string.IsNullOrEmpty(config.InjectedKey))
        {
            Debug.LogError("SendGrid API Key is not set.");
            return;
        }

        var client = new SendGridClient(config.InjectedKey);
        var from = new EmailAddress(SenderAddress, SenderUser);
        var subject = MessageSubject;
        var to = new EmailAddress(ReceiverAddress, ReceiverUser);
        var plainTextContent = MessageContentTMP.text;
        var htmlContent = "<strong>" + MessageContentTMP.text + "</strong>";
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

        try
        {
            var response = await client.SendEmailAsync(msg);
            Debug.Log($"Status Code: {response.StatusCode}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to send email: {e.Message}");
        }
    }

    void LoadConfiguration()
    {
        if (File.Exists(ConfigFilePath))
        {
            string json = File.ReadAllText(ConfigFilePath);
            config = JsonUtility.FromJson<EmailConfig>(json);
            SenderAddress = config.SenderAddress;
            ReceiverAddress = config.ReceiverAddress;
        }
        else
        {
            Debug.LogWarning("Configuration file not found: " + ConfigFilePath);
            config = new EmailConfig
            {
                InjectedKey = "",
                SenderAddress = "",
                ReceiverAddress = ""
            };

            string newJson = JsonUtility.ToJson(config, true);
            File.WriteAllText(ConfigFilePath, newJson);
            Debug.Log("A new configuration file was created.");
        }
    }
}
