using catHomerun.Utils;
using HomerunKitty;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : SingletonGlobal<PlayerManager>
{
    public Vector3 spawnPos = new Vector3(0, 10f, 0);
    public GameObject playerPrefab;
    List<InputDevice> inputDevices = new List<InputDevice>();
    Dictionary<InputDevice, GameObject> devicePlayerMap = new Dictionary<InputDevice, GameObject>();
    public Cinemachine.CinemachineTargetGroup targetGroup;

    private void Awake()
    {
        foreach (var device in InputSystem.devices)
        {
            AddPlayer(device);
        }

        InputSystem.onDeviceChange += OnDeviceChanged;
    }

    private void Start()
    {
        SpawnPlayers();
    }

    public void SpawnPlayers()
    {
        foreach (var device in inputDevices)
        {
            if (device.name == "Mouse")
                continue;

            Debug.Log("Spawn player for device " + device);
            GameObject playerInstance = Instantiate(playerPrefab);
            playerInstance.transform.position = spawnPos;
            playerInstance.GetComponentInChildren<PlayerController>().GroundPlayer();
            devicePlayerMap.Add(device, playerInstance);
            InputManagerSystem playerInput = playerInstance.GetComponentInChildren<InputManagerSystem>();
            playerInput.Init();
            playerInput.AddDevice(device);
            if (device.name == "Keyboard")
            {
                foreach (var device2 in inputDevices)
                {
                    if (device.name == "Mouse")
                        playerInput.AddDevice(device2);
                }
            }
            targetGroup.AddMember(playerInstance.transform, 1, 5);
        }
    }

    public void DespawnPlayer(GameObject playerInstance)
    {
        targetGroup.RemoveMember(playerInstance.transform);
        Destroy(playerInstance);
    }

    void AddPlayer(InputDevice device)
    {
        if (inputDevices.Contains(device))
        {
            Debug.LogError($"{device} already registered");
        } 
        
        inputDevices.Add(device);
    }

    void RemovePlayer(InputDevice device)
    {
        inputDevices.Remove(device);

        if (!devicePlayerMap.ContainsKey(device))
            return;

        DespawnPlayer(devicePlayerMap[device]);
        Destroy(devicePlayerMap[device]);
        devicePlayerMap.Remove(device);
    }

    void OnDeviceChanged(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Added:
                AddPlayer(device);
                break;
            case InputDeviceChange.Removed:
                RemovePlayer(device);
                break;
        }
    }
}
