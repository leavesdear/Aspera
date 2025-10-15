using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour
{
    //public Player player;

    public GameSceneSO firstRoom;

    public SceneLoadEventSO loadEventSO;

    public GameSceneSO sceneToGo;

    public Vector3 positionToGo;

    void Start()
    {
        //player = PlayerManger.instance.player;
        // player.teleport = this;
    }


    public void TriggerAction()
    {
        loadEventSO.RaiseLoadRequestEvent(sceneToGo, positionToGo, true);
    }

    public void DieBack()
    {
        loadEventSO.RaiseLoadRequestEvent(firstRoom, positionToGo, true);
    }
}
