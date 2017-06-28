﻿namespace ParaUnity
{
    using UnityEngine;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;

    public class ParaviewObjectLoader : MonoBehaviour
    {
        private GameObject meshNode;
        private TcpListener listener;

        public void Start()
        {
            ModeManager modeManager = GlobalVariables.modeManager;

            listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            int port = ((IPEndPoint)listener.LocalEndpoint).Port;
            Debug.Log("Started listening on port: " + port);
            GlobalVariables.overlayText.SetText("Started listening on port: " + port);

            string dir = Path.GetTempPath() + "/Unity3DPlugin/";

            if(modeManager.isEditorMode())
                dir += "Editor/" + port;
            else
                dir += "/Embedded/" + System.Diagnostics.Process.GetCurrentProcess().Id;

            Directory.CreateDirectory(dir);

            if (!modeManager.isEditorMode()) {
                string portFile = dir + "/port" + port;
                using (File.Create(portFile)){};
            }
        }

        public void Update()
        {
            if (listener.Pending())
            {
                GlobalVariables.overlayText.SetText("New Connection");
                Socket soc = listener.AcceptSocket();

                Destroy(meshNode);

                string importDir = Loader.GetImportDir(soc);

                if (Directory.Exists(importDir) || File.Exists(importDir))
                {
                    Debug.Log("Importing from:" + importDir);
                    GlobalVariables.overlayText.SetText("Importing from:" + importDir);

                    meshNode = Loader.ImportGameObject(importDir);
                    GlobalVariables.overlayText.SetText("Finished importing");
                    meshNode.transform.position = new Vector3(0, 1, 0);

                    // Register object in globals
                    if (meshNode != null)
                        GlobalVariables.RegisterParaviewObject(meshNode);

                    // Automaticall Resize object
                    meshNode.AddComponent<AutoResize>();

                    meshNode.SetActive(true);
                }
                else
                {
                    Debug.Log("Message was not existing file/directory: " + importDir);
                }

                soc.Disconnect(false);
            }
        }

        void OnApplicationQuit()
        {
            if (listener != null)
                listener.Stop();
            listener = null;
        }
    }
}