using System;
using System.Runtime.CompilerServices;
using FishNet.Component.Animating;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Transporting.Tugboat;
using MelonLoader;
using MelonLoader.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Get_To_Multiplayer
{
    public class Manager : MonoBehaviour
    {
        public void Start()
        {
            string text = MelonEnvironment.UserDataDirectory + "/panel.pf";
            this.asset = AssetBundle.LoadFromFile(text);
            this.SpawnPanel();
        }

       
        public void SpawnPanel()
        {
            GameObject gameObject = this.asset.LoadAsset<GameObject>("Panel");
            this.Canvas = GameObject.Instantiate<GameObject>(gameObject);
            this.hold = this.Canvas.AddComponent<InfoHolder>();
            Toggle component = this.Canvas.transform.Find("Canvas").Find("PanelMultiplayer").Find("HOST").Find("Tag").GetComponent<Toggle>();
            Toggle component2 = this.Canvas.transform.Find("Canvas").Find("PanelMultiplayer").Find("HOST").Find("Casual").GetComponent<Toggle>();
            component.onValueChanged.AddListener(new UnityAction<bool>(this.Canvas.GetComponent<InfoHolder>().SetTagBool));
            component2.onValueChanged.AddListener(new UnityAction<bool>(this.Canvas.GetComponent<InfoHolder>().SetCasualBool));
            this.Canvas.transform.Find("Canvas").Find("PanelMultiplayer").Find("HOST").Find("Start").GetComponent<Button>().onClick.AddListener(delegate ()
            {
                this.OnHost();
            });
            this.Canvas.transform.Find("Canvas").Find("PanelMultiplayer").Find("JOIN").Find("JoinGame").GetComponent<Button>().onClick.AddListener(delegate ()
            {
                this.OnClient();
            });
            this.AddButtonsModel();
            this.AddButtonsSkates();
        }

        // Token: 0x06000015 RID: 21 RVA: 0x00002670 File Offset: 0x00000870
        public void OnHost()
        {
            this.Tag = this.hold.Tag;
            NetworkManager networkManager = GameObject.FindObjectOfType<NetworkManager>();
            networkManager.gameObject.GetComponent<Tugboat>().SetServerBindAddress(this.Canvas.transform.Find("Canvas").Find("PanelMultiplayer").Find("HOST").Find("IP").GetComponent<TMP_InputField>().text, 0);
            networkManager.gameObject.GetComponent<Tugboat>().SetPort(ushort.Parse(this.Canvas.transform.Find("Canvas").Find("PanelMultiplayer").Find("HOST").Find("Port").GetComponent<TMP_InputField>().text));
            networkManager.ServerManager.StartConnection();
            networkManager.ClientManager.StartConnection(this.Canvas.transform.Find("Canvas").Find("PanelMultiplayer").Find("HOST").Find("IP").GetComponent<TMP_InputField>().text);
            if (this.Tag)
            {
                Debug.Log("Playing Tag!!");
                base.Invoke("InvokeChangeColorModel", 5f);
                return;
            }
            Debug.Log("Playing Casual!!");
        }

        private void InvokeChangeColorModel()
        {
            this.ChangeColorModel("Red");
        }

        public void OnClient()
        {
            NetworkManager networkManager = GameObject.FindObjectOfType<NetworkManager>();
            networkManager.gameObject.GetComponent<Tugboat>().SetPort(ushort.Parse(this.Canvas.transform.Find("Canvas").Find("PanelMultiplayer").Find("JOIN").Find("Port").GetComponent<TMP_InputField>().text));
            networkManager.ClientManager.StartConnection(this.Canvas.transform.Find("Canvas").Find("PanelMultiplayer").Find("JOIN").Find("IP").GetComponent<TMP_InputField>().text);
        }

        public void AddButtonsModel()
        {
            Button[] componentsInChildren = this.Canvas.transform.Find("Canvas").Find("PanelCharacter").Find("Model").transform.GetComponentsInChildren<Button>();
            Button[] array = componentsInChildren;
            for (int i = 0; i < array.Length; i++)
            {
                Button button = array[i];
                MelonLogger.Msg("Setting button");
                button.onClick.AddListener(delegate ()
                {
                    this.ChangeColorModel(button.name);
                });
                button.onClick.AddListener(delegate ()
                {
                    this.ChangeOutlineOfModel(button.GetComponent<RectTransform>());
                });
            }
        }

        public void ChangeOutlineOfModel(RectTransform transform)
        {
            if (base.GetComponent<NetworkManager>().IsClientStarted)
            {
                RectTransform component = this.Canvas.transform.Find("Canvas").Find("PanelCharacter").Find("Model").Find("Outline").GetComponent<RectTransform>();
                component.gameObject.SetActive(true);
                component.position = transform.position;
            }
        }

        public void ChangeOutlineOfSkates(RectTransform transform)
        {
            if (base.GetComponent<NetworkManager>().IsClientStarted)
            {
                RectTransform component = this.Canvas.transform.Find("Canvas").Find("PanelCharacter").Find("Skates").Find("Outline").GetComponent<RectTransform>();
                component.gameObject.SetActive(true);
                component.position = transform.position;
            }
        }

        public void AddButtonsSkates()
        {
            Button[] componentsInChildren = this.Canvas.transform.Find("Canvas").Find("PanelCharacter").Find("Skates").transform.GetComponentsInChildren<Button>();
            Button[] array = componentsInChildren;
            for (int i = 0; i < array.Length; i++)
            {
                Button button = array[i];
                MelonLogger.Msg("Setting button");
                button.onClick.AddListener(delegate ()
                {
                    this.ChangeColorSkates(button.name);
                });
                button.onClick.AddListener(delegate ()
                {
                    this.ChangeOutlineOfSkates(button.GetComponent<RectTransform>());
                });
            }
        }

        public void ChangeColorModel(string Name)
        {
            if (base.GetComponent<NetworkManager>().IsClientStarted)
            {
                Player[] array = GameObject.FindObjectsOfType<Player>();
                foreach (Player player in array)
                {
                    if (player.GetComponent<NetworkObject>().IsOwner)
                    {
                        player.transform.Find("Model").transform.Find("model_RIG_UPDATED").GetComponent<NetworkAnimator>().SetTrigger(Name);
                        MelonLogger.Msg("Changed to color: " + Name);
                    }
                }
            }
        }

        public void ChangeColorSkates(string Name)
        {
            if (base.GetComponent<NetworkManager>().IsClientStarted)
            {
                Player[] array = GameObject.FindObjectsOfType<Player>();
                foreach (Player player in array)
                {
                    NetworkObject component = player.GetComponent<NetworkObject>();
                    if (component != null && component.IsOwner)
                    {
                        Transform transform = player.transform.Find("Model");
                        if (transform != null)
                        {
                            string[] array3 = new string[]
                            {
                                "kneepad_LOW.L",
                                "kneepad_LOW.R",
                                "rollerblade_LOW.L",
                                "rollerblade_LOW.R",
                                "shoe_LOW.L",
                                "shoe_LOW.R"
                            };
                            foreach (string text in array3)
                            {
                                Transform transform2 = transform.Find(text);
                                if (transform2 != null)
                                {
                                    NetworkAnimator component2 = transform2.GetComponent<NetworkAnimator>();
                                    if (component2 != null)
                                    {
                                        component2.SetTrigger(Name);
                                    }
                                    else
                                    {
                                        MelonLogger.Warning("NetworkAnimator not found on " + text);
                                    }
                                }
                                else
                                {
                                    MelonLogger.Warning("Child object " + text + " not found under Model");
                                }
                            }
                            MelonLogger.Msg("Changed to color: " + Name);
                        }
                        else
                        {
                            MelonLogger.Error("Model not found for player");
                        }
                    }
                }
            }
        }

        public string username = "player";

        private GameObject Canvas;

        private AssetBundle asset;

        public bool Tag;

        public InfoHolder hold;
    }
}
