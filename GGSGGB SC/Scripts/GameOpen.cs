using GorillaGoSmallGorillaGoBig.main;
using System;
using UnityEngine;

namespace GorillaGoSmallGorillaGoBig.Scripts
{
    internal class GameOpen
    {
        void Awake()
        {
            Plugin.Instance.SizeChanger.SetActive(true);
            Plugin.Instance.SizeChanger.transform.localScale = new Vector3(0, 0, 0);
            Plugin.Instance.SizeChanger.GetComponent<SizeChanger>().minScale = 0.03f;
            Debug.Log("Game Opened");
        }
    }
}
