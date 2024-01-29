using catHomerun.Utils;
using HomerunKitty;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : SingletonGlobal<PlayerManager>, DefaultInput.ICharacterActions
{
    public DefaultInput input;

    public Vector3 spawnPos = new Vector3(0, 10f, 0);
    //public GameObject playerPrefab;
    List<InputDevice> inputDevices = new List<InputDevice>();
    Dictionary<InputDevice, InputManagerSystem> devicePlayerMap = new Dictionary<InputDevice, InputManagerSystem>();
    public Cinemachine.CinemachineTargetGroup targetGroup;
    public List<PlayerPointsManager> players = new List<PlayerPointsManager>();
    public List<GameObject> playerPrefabs = new List<GameObject>();

    protected override void Awake()
    {
        base.Awake();

        input = new DefaultInput();
        input.Character.SetCallbacks(this);

        foreach (var device in InputSystem.devices)
        {
            AddPlayer(device);
        }

        InputSystem.onDeviceChange += OnDeviceChanged;
    }

    private void Start()
    {
        //SpawnPlayers();
    }

    public void Reset()
    {
        devicePlayerMap.Clear();
    }


    protected void OnEnable()
    {
        input.Enable();
    }

    protected void OnDisable()
    {
        input.Disable();
    }

    public void SpawnPlayer()
    {
        targetGroup = FindFirstObjectByType<Cinemachine.CinemachineTargetGroup>();
        GameObject playerInstance = Instantiate(playerPrefabs[Random.Range(0, playerPrefabs.Count)]);
        playerInstance.transform.position = spawnPos;
        playerInstance.GetComponentInChildren<PlayerController>().GroundPlayer();
        InputManagerSystem playerInput = playerInstance.GetComponentInChildren<InputManagerSystem>();
        playerInput.Init(input);
        targetGroup.AddMember(playerInstance.transform, 1, 5);
        var points = playerInstance.GetComponentInChildren<PlayerPointsManager>();
        players.Add(points);

        ScoreText[] scores = FindObjectsByType<ScoreText>(FindObjectsSortMode.InstanceID);
        points.scoreText = scores[0];
        points.playerid = 0;

        foreach (var device in inputDevices)
        {
            devicePlayerMap.Add(device, playerInput);
            playerInput.AddDevice(device);
        }
    }

    public void SpawnPlayers()
    {
        targetGroup = FindFirstObjectByType<Cinemachine.CinemachineTargetGroup>();
        ScoreText[] scores = FindObjectsByType<ScoreText>(FindObjectsSortMode.InstanceID);

        int i = 0;
        int numPlayers = inputDevices.Count;
        float spawnAngleOffsetPerPlayer = 360f / (float)numPlayers;
        
        foreach (var device in inputDevices)
        {
            if (device.name == "Mouse")
                continue;
            Debug.Log("Spawn player for device " + device);


            float spawnAngle = spawnAngleOffsetPerPlayer * (float)i;
            Vector3 addPos = Quaternion.Euler(0, spawnAngle, 0) * (1.5f * Vector3.right);
            Vector3 spawnPosition = spawnPos + addPos;

            GameObject playerInstance = Instantiate(playerPrefabs[Random.Range(0, playerPrefabs.Count)], spawnPosition, Quaternion.identity);
            //playerInstance.GetComponent<Rigidbody>().position = spawnPosition;

            playerInstance.GetComponentInChildren<PlayerController>().GroundPlayer();
            InputManagerSystem playerInput = playerInstance.GetComponentInChildren<InputManagerSystem>();
            devicePlayerMap.Add(device, playerInput);
            playerInput.Init(input);
            playerInput.AddDevice(device);
            var points = playerInstance.GetComponentInChildren<PlayerPointsManager>();
            points.scoreText = scores[i];
            points.playerid = i;
            players.Add(points);

            if (device.name == "Keyboard")
            {
                foreach (var device2 in inputDevices)
                {
                    if (device2.name == "Mouse")
                    {
                        devicePlayerMap.Add(device2, playerInput);
                        playerInput.AddDevice(device2);
                    }
                }
            }
            targetGroup.AddMember(playerInstance.transform, 1, 5);
            i++;
        }
    }

    public void Clear()
    {
        players.Clear();
        devicePlayerMap.Clear();
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

        //DespawnPlayer(devicePlayerMap[device]);
        Destroy(devicePlayerMap[device]);
        devicePlayerMap.Remove(device);
    }

    InputManagerSystem GetInput(InputDevice device)
    {
        if (!devicePlayerMap.TryGetValue(device, out InputManagerSystem sys))
            return null;
        return sys;
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

    void DefaultInput.ICharacterActions.OnMove(InputAction.CallbackContext context) //=> move = context.ReadValue<Vector2>();
    {
        InputManagerSystem sys = GetInput(context.control.device);
        if (sys == null)
            return;

        sys.move = context.ReadValue<Vector2>();
    }
    void DefaultInput.ICharacterActions.OnAim(InputAction.CallbackContext context)
    {
        InputManagerSystem sys = GetInput(context.control.device);
        if (sys == null)
            return;
        sys.aim = context.ReadValue<Vector2>();
        sys.mouseAim = true;
    }
    void DefaultInput.ICharacterActions.OnLook(InputAction.CallbackContext context)
    {
        InputManagerSystem sys = GetInput(context.control.device);
        if (sys == null)
            return;
        sys.aim = context.ReadValue<Vector2>();
        sys.mouseAim = false;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        InputManagerSystem sys = GetInput(context.control.device);
        if (sys == null)
            return;
        sys.jump = context.ReadValue<float>() > 0.5f;
    }
}
