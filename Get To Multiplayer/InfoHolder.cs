using System;
using UnityEngine;

// Token: 0x02000005 RID: 5
public class InfoHolder : MonoBehaviour
{

    public void StartServer()
    {
    }

    public void JoinServer()
    {
    }

    public void SetTagBool(bool yes)
    {
        this.Tag = yes;
    }

    public void SetCasualBool(bool yes)
    {
        this.Casual = yes;
    }

    public bool Tag;

    public bool Casual;
}
