using System;
using System.Runtime.CompilerServices;
using FishNet.Component.Spawning;
using FishNet.Managing;
using FishNet.Managing.Transporting;
using FishNet.Object;
using FishNet.Transporting.Tugboat;
using MelonLoader;
using MelonLoader.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

[assembly: MelonInfo(typeof(Get_To_Multiplayer.Base), "Get To Multiplayer", "1.6.8", "Nanodogs Studios")]
[assembly: MelonGame("Isto", "Get To Work")]

namespace Get_To_Multiplayer
{
    public class Base : MelonMod
    {
        private GameObject Network;

        public static AssetBundle asset;

        public static AssetBundle assetpl;

        private bool loaded;

        public override void OnUpdate()
        {
            if (SceneManager.GetActiveScene().name == "2-TitleScreen")
            {
                if (!this.loaded)
                {
                    string text = MelonEnvironment.UserDataDirectory + "/manager.pf";
                    string text2 = MelonEnvironment.UserDataDirectory + "/newplayer.pf";
                    Base.asset = AssetBundle.LoadFromFile(text);
                    Base.assetpl = AssetBundle.LoadFromFile(text2);
                    GameObject gameObject = Base.assetpl.LoadAsset<GameObject>("Player");
                    gameObject.AddComponent<Player>();
                    this.Network = GameObject.Instantiate<GameObject>(Base.asset.LoadAsset<GameObject>("Manager"));
                    this.Network.GetComponent<TransportManager>().Transport = this.Network.GetComponent<Tugboat>();
                    this.Network.GetComponent<NetworkManager>().SpawnablePrefabs.Clear();
                    this.Network.GetComponent<NetworkManager>().SpawnablePrefabs.AddObject(gameObject.GetComponent<NetworkObject>(), false, true);
                    this.Network.GetComponent<PlayerSpawner>().SetPlayerPrefab(gameObject.GetComponent<NetworkObject>());
                    Manager manager = this.Network.AddComponent<Manager>();
                    Debug.Log("Adding MainMP");
                    this.loaded = true;
                }
                if (GameObject.Find("Panel(Clone)") == null)
                {
                    this.Network.GetComponent<Manager>().SpawnPanel();
                }
            }
        }
    }
}
