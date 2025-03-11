using System;
using System.Runtime.CompilerServices;
using FishNet.Object;
using Isto.GTW.Player;
using LaymsCoolNotifs;
using RootMotion.Demos;
using TMPro;
using UnityEngine;

namespace Get_To_Multiplayer
{
    public class Player : NetworkBehaviour
    {
        public override void OnStartClient()
        {
            base.OnStartClient();
            GameObject.DontDestroyOnLoad(base.gameObject);
            this.PlayerUsername = "Player" + base.GetComponent<NetworkObject>().ObjectId.ToString();
            if (this.joinNotif == null)
            {
                GameObject gameObject = Notifications.CreateNotification("a player has joined the server.", true, Color.yellow, 24);
                GameObject.Destroy(gameObject, 5f);
            }
        }

        public void Update()
        {
            GameObject.DontDestroyOnLoad(base.gameObject);
            NetworkObject component = base.GetComponent<NetworkObject>();
            if (component != null && component.IsOwner)
            {
                this.InitializePlayerComponents();
                if (this.playerPhysics != null && this.playerModel != null)
                {
                    this.SyncPlayerTransform();
                    this.SyncPlayerLimbs();
                    this.playerRoot = this.playerModel.transform.Find("CharacterRoot/MainChar_V4");
                    Transform transform = base.transform.Find("Model");
                    if (this.playerRoot != null && transform != null)
                    {
                        Transform transform2 = transform.Find("model_RIG_UPDATED");
                        SkinnedMeshRenderer skinnedMeshRenderer = (transform2 != null) ? transform2.GetComponent<SkinnedMeshRenderer>() : null;
                        Transform transform3 = this.playerRoot.Find("model_RIG_UPDATED");
                        SkinnedMeshRenderer skinnedMeshRenderer2 = (transform3 != null) ? transform3.GetComponent<SkinnedMeshRenderer>() : null;
                        if (skinnedMeshRenderer != null && skinnedMeshRenderer2 != null)
                        {
                            skinnedMeshRenderer2.material = skinnedMeshRenderer.material;
                        }
                        string[] array = new string[]
                        {
                            "kneepad_LOW.L",
                            "kneepad_LOW.R",
                            "rollerblade_LOW.L",
                            "rollerblade_LOW.R",
                            "shoe_LOW.L",
                            "shoe_LOW.R"
                        };
                        foreach (string text in array)
                        {
                            Transform transform4 = transform.Find(text);
                            SkinnedMeshRenderer skinnedMeshRenderer3 = (transform4 != null) ? transform4.GetComponent<SkinnedMeshRenderer>() : null;
                            Transform transform5 = this.playerRoot.Find(text);
                            SkinnedMeshRenderer skinnedMeshRenderer4 = (transform5 != null) ? transform5.GetComponent<SkinnedMeshRenderer>() : null;
                            if (skinnedMeshRenderer3 != null && skinnedMeshRenderer4 != null)
                            {
                                skinnedMeshRenderer4.material = skinnedMeshRenderer3.material;
                            }
                        }
                    }
                }
            }
            if (this.PlayerCollision != null)
            {
                if (this.PlayerCollision.GetComponent<CollisionMP>() == null)
                {
                    this.PlayerCollision.AddComponent<CollisionMP>();
                }
            }
            else
            {
                PlayerCollisionHandler playerCollisionHandler = GameObject.FindObjectOfType<PlayerCollisionHandler>();
                if (playerCollisionHandler != null)
                {
                    this.PlayerCollision = playerCollisionHandler.gameObject;
                }
            }
            this.UpdateUsernameDisplay();
        }

        private void InitializePlayerComponents()
        {
            if (this.playerPhysics == null || this.playerModel == null)
            {
                Transform transform = base.transform.Find("PlayerMultiplayerColl");
                if (transform != null)
                {
                    MeshRenderer component = transform.GetComponent<MeshRenderer>();
                    if (component != null)
                    {
                        this.DisableSkinnedMeshRenderers();
                        component.gameObject.GetComponent<BoxCollider>().enabled = false;
                    }
                    PlayerController playerController = GameObject.FindObjectOfType<PlayerController>();
                    if (playerController != null)
                    {
                        this.playerPhysics = playerController.transform.Find("PlayerPhysics");
                        this.playerModel = playerController.transform.Find("PlayerModel");
                    }
                }
            }
        }

        private void SyncPlayerTransform()
        {
            base.transform.position = this.playerPhysics.position;
            base.transform.rotation = this.playerModel.rotation;
        }

        private void SyncPlayerLimbs()
        {
            RecoilTest componentInChildren = base.GetComponentInChildren<RecoilTest>();
            if (componentInChildren == null)
            {
                Debug.LogWarning("RecoilTest component not found in children.");
                return;
            }
            Transform[] componentsInChildren = componentInChildren.GetComponentsInChildren<Transform>();
            if (componentsInChildren == null || componentsInChildren.Length == 0)
            {
                Debug.LogWarning("No children found in RecoilTest.");
                return;
            }
            if (this.playerRoot == null)
            {
                Debug.LogWarning("Player root is null.");
                return;
            }
            Transform transform = this.playerRoot.Find("root");
            if (transform == null)
            {
                Debug.LogWarning("Root transform not found.");
                return;
            }
            Transform transform2 = transform.Find("root.x");
            if (transform2 == null)
            {
                Debug.LogWarning("root.x transform not found.");
                return;
            }
            Transform[] componentsInChildren2 = transform2.GetComponentsInChildren<Transform>();
            if (componentsInChildren2 == null || componentsInChildren2.Length == 0)
            {
                Debug.LogWarning("No child INR transforms found.");
                return;
            }
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                for (int j = 0; j < componentsInChildren2.Length; j++)
                {
                    if (componentsInChildren[i].gameObject.name == componentsInChildren2[j].gameObject.name)
                    {
                        componentsInChildren[i].position = componentsInChildren2[j].position;
                        componentsInChildren[i].rotation = componentsInChildren2[j].rotation;
                        break;
                    }
                }
            }
        }

        private void SyncOffsetEffectors()
        {
            OffsetEffector[] componentsInChildren = base.GetComponentsInChildren<OffsetEffector>();
            OffsetEffector[] componentsInChildren2 = this.playerModel.GetComponentsInChildren<OffsetEffector>();
            if (componentsInChildren.Length == componentsInChildren2.Length)
            {
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    componentsInChildren[i].transform.position = componentsInChildren2[i].transform.position;
                    componentsInChildren[i].transform.rotation = componentsInChildren2[i].transform.rotation;
                }
            }
        }

        // Token: 0x06000027 RID: 39 RVA: 0x00003168 File Offset: 0x00001368
        private void UpdateUsernameDisplay()
        {
            if (this.usernameText == null)
            {
                Transform transform = base.transform.Find("PlayerMultiplayerColl");
                if (transform != null)
                {
                    Transform transform2 = transform.Find("Canvas");
                    if (transform2 != null)
                    {
                        Transform transform3 = transform2.Find("Name");
                        if (transform3 != null)
                        {
                            this.usernameText = transform3.GetComponent<TMP_Text>();
                        }
                    }
                }
            }
            if (this.usernameText != null)
            {
                this.usernameText.text = this.PlayerUsername;
                if (this.usernameText.GetComponent<Billboard>() == null)
                {
                    this.usernameText.gameObject.AddComponent<Billboard>();
                }
            }
        }

        private void DisableSkinnedMeshRenderers()
        {
            if (this.cachedMeshRenderers == null)
            {
                this.cachedMeshRenderers = base.GetComponentsInChildren<SkinnedMeshRenderer>();
            }
            foreach (SkinnedMeshRenderer skinnedMeshRenderer in this.cachedMeshRenderers)
            {
                if (skinnedMeshRenderer.enabled)
                {
                    skinnedMeshRenderer.enabled = false;
                }
            }
        }

        [SerializeField]
        private string PlayerUsername = "Player";
        private Transform playerPhysics;
        private Transform playerModel;
        private TMP_Text usernameText;
        private SkinnedMeshRenderer[] cachedMeshRenderers;
        private Transform playerRoot;
        private GameObject joinNotif;
        private GameObject PlayerCollision;
    }
}
