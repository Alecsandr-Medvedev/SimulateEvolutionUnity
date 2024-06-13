using UnityEngine;
using System.Diagnostics;
using UnityEngine.UI;
using System.IO;
using System;
using System.Collections;

public class PythonIntegration : MonoBehaviour
{
    [SerializeField]
    private RawImage img;
    private string output = "";

     public void SendDataToPython(string dataToSend, string colors, string title, string labels)
    {
        Process process = new Process();
        process.StartInfo.FileName = Settings.pathToExe;
        process.StartInfo.Arguments = $"{dataToSend} {colors} {title} {labels}";
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.UseShellExecute = false;
        process.OutputDataReceived += new DataReceivedEventHandler(ProcessOutputHandler);
        process.Start();
        process.BeginOutputReadLine();
    }

    private void ProcessOutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
    {
        // ѕолучаем строку Base64 изображени€ из стандартного вывода
        string base64ImageString = outLine.Data;

        // ≈сли строка не пуста€, обрабатываем ее
        if (!string.IsNullOrEmpty(base64ImageString))
        {
            output = base64ImageString;

        }
    }
    

        public void LoadImageFromFile()
    {
        if (output != "")
        {
            byte[] imageBytes = Convert.FromBase64String(output);
            output = "";
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageBytes);
            img.texture = texture;
        }
        else
        {
            UnityEngine.Debug.Log("GraphError");
        }
            
   
    }
}