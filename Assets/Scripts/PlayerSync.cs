using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class PlayerSync : NetworkBehaviour
{
    [SyncVar]
    private Vector3 syncPosition;
    public float lerpSpeed=20;
    // Start is called before the first frame update
    void Start()
    {
        //if (isLocalPlayer)
        //{
        //    syncPosition = transform.position;
        //}
        
    }
    void FixedUpdate()
    {
        LerpPosition();
        TransmitPosition();
    }

    void LerpPosition()
    {
        if (!isLocalPlayer)
        {
            transform.position = Vector3.Lerp(transform.position, syncPosition, Time.deltaTime * lerpSpeed);
        }
    }
    [Client]
    void TransmitPosition()
    {
        if (isLocalPlayer)
        {
            CmdSendPosition(transform.position);
        }
    }

    [Command]
    void CmdSendPosition(Vector3 pos)
    {
        syncPosition = pos;
    }
}
