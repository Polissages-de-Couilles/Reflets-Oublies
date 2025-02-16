using EditorCools;
using Firebase;
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class FirebaseManager : MonoBehaviour
{
    //private const string firebase_url = "https://rpdpc-a1f69-default-rtdb.europe-west1.firebasedatabase.app/";
    //private string GetURL(string url) => firebase_url + url + ".json";

    private const string USER_KEY = "USER";
    private FirebaseDatabase _database;
    public Action OnFirebaseInitialized;
    bool isFirebaseInit = false;
    bool isInitCompleted = false;

    private List<UserData> _activeUsers = new();

    bool isDataWriting = false;

    [SerializeField] GameObject _ghost;
    private Dictionary<int, GameObject> _ghostDic = new();

    public void Start()
    {
        OnFirebaseInitialized += OnFirebaseInit;
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => 
        {
            if(task.Exception != null)
            {
                Debug.LogError($"Failed to init Firebase with {task.Exception}");
                return;
            }

            Debug.Log("Firebase is init");
            _database = FirebaseDatabase.DefaultInstance;
            isFirebaseInit = true;
            OnFirebaseInitialized?.Invoke();
        });
    }

    private async void HandleDisconnect()
    {
        OnFirebaseInitialized -= OnFirebaseInit;
        Disconnect();
        await _database.GetReference(USER_KEY).Child(UserInstance.User.id.ToString()).RemoveValueAsync();
    }

    [Button]
    private void Disconnect()
    {
        _database.GetReference(USER_KEY).ChildChanged -= HandleChildChange;
        _database.GetReference(USER_KEY).ChildAdded -= HandleChildAdded;
        _database.GetReference(USER_KEY).ChildRemoved -= HandleChildRemoved;
    }

    public void OnDestroy()
    {
        HandleDisconnect();
    }

    public void OnApplicationQuit()
    {
        HandleDisconnect();
    }

    public void Update()
    {
        if (isFirebaseInit && !isDataWriting && isInitCompleted)
        {
            UpdateThisUserData();
        }

        if(isFirebaseInit && isInitCompleted)
        {
            
        }
    }

    private async void UpdateThisUserData()
    {
        isDataWriting = true;
        UserInstance.User.x = GameManager.Instance.Player.transform.position.x;
        UserInstance.User.y = GameManager.Instance.Player.transform.position.y;
        UserInstance.User.z = GameManager.Instance.Player.transform.position.z;
        await SetUserData(UserInstance.User);
        isDataWriting = false;
    }

    private async void OnFirebaseInit()
    {
        //await SetUserData(new(2, 1, 1, 1));

        //var data = await GetUserData("Ryan");
        //Debug.Log(data.name);

        var datas = await GetAllUserData();
        //foreach (var d in datas)
        //{
        //    Debug.Log($"{d.id}");
        //}
        System.Random rng = new System.Random();

        int id = rng.Next(0, 9);

        for (int i = 0; i < 1000000; i++)
        {
            id = int.Parse($"{rng.Next(0, 9)}{rng.Next(0, 9)}{rng.Next(0, 9)}{rng.Next(0, 9)}{rng.Next(0, 9)}{rng.Next(0, 9)}{rng.Next(0, 9)}{rng.Next(0, 9)}");
            Debug.Log(id);

            if (id != 0 && datas.Find(x => x.id == id) == null)
            {
                break;
            }
            else if (i >= 999999)
            {
                Debug.LogError("Max id Reach");
            }
        }
        
        //while (id == 0 || datas.Find(x => x.id == id) != null)
        //{
        //    id = Random.Range(10000000, 99999999);
        //    Debug.Log(id);
        //}

        _database.GetReference(USER_KEY).ChildChanged += HandleChildChange;
        _database.GetReference(USER_KEY).ChildAdded += HandleChildAdded;
        _database.GetReference(USER_KEY).ChildRemoved += HandleChildRemoved;

        _activeUsers.Clear();
        _activeUsers = datas.ToList();
        
        UserInstance.User = new(id, 0, 0, 0);
        
        await SetUserData(UserInstance.User);
        isInitCompleted = true;

        foreach (var user in _activeUsers)
        {
            CreateGhost(user);
        }
    }

    private void CreateGhost(UserData userData)
    {
        if (userData.id == UserInstance.User.id || userData.id == -1 || UserInstance.User.id == UserInstance.OldId) return;
        GameObject go;
        _ghostDic.Add(userData.id, go = Instantiate(_ghost));
        go.transform.position = new Vector3(userData.x, userData.y, userData.z);
    }

    private void HandleChildAdded(object sender, ChildChangedEventArgs e)
    {
        if (Application.isPlaying)
        {
            var user = JsonUtility.FromJson<UserData>(e.Snapshot.GetRawJsonValue());
            _activeUsers.Add(user);
            //Debug.Log(user.id);
            CreateGhost(user);
        }
    }

    private void HandleChildRemoved(object sender, ChildChangedEventArgs e)
    {
        if (Application.isPlaying)
        {
            var user = JsonUtility.FromJson<UserData>(e.Snapshot.GetRawJsonValue());
            if (!_ghostDic.ContainsKey(user.id)) return;
            _activeUsers.Remove(_activeUsers.Find(x => x.id == user.id));
            //Debug.Log(user.id);
            Destroy(_ghostDic[user.id]);
            _ghostDic.Remove(user.id);
        }
    }

    private void HandleChildChange(object sender, ChildChangedEventArgs e)
    {
        if (Application.isPlaying)
        {
            var user = JsonUtility.FromJson<UserData>(e.Snapshot.GetRawJsonValue());
            if (user.id == UserInstance.User.id || user.id == -1 || !_ghostDic.ContainsKey(user.id) || UserInstance.User.id == UserInstance.OldId) return;
            Debug.Log(user.id + " | " + UserInstance.User.id + " | " + UserInstance.OldId + " | " + (UserInstance.User.id == UserInstance.OldId));
            _ghostDic[user.id].transform.position = new(user.x, user.y, user.z);
        }
    }

    public async Task SetUserData(UserData userData)
    {
        await _database.GetReference(USER_KEY).Child(userData.id.ToString()).SetRawJsonValueAsync(JsonUtility.ToJson(userData));
    }

    public async Task<UserData> GetUserData(string key)
    {
        var dataSnapshot = await _database.GetReference(USER_KEY).Child(key).GetValueAsync();
        if (!dataSnapshot.Exists)
        {
            return null;
        }
        return JsonUtility.FromJson<UserData>(dataSnapshot.GetRawJsonValue());
    }

    public async Task<List<UserData>> GetAllUserData()
    {
        var dataList = await _database.GetReference(USER_KEY).OrderByChild("name").GetValueAsync();

        var list = new List<UserData>();
        foreach (var data in dataList.Children.Reverse())
        {
            var d = JsonUtility.FromJson<UserData>(data.GetRawJsonValue());
            list.Add(d);
        }
        return list;
    }

    //private void AddUser(string name, float x, float y, float z)
    //{
    //    var user = new UserData(name, x, y, z);
    //    RestClient.Put(GetURL($"USER/{name}"), user);
    //}

    //private async Task<UserData> GetUserData(string user)
    //{
    //    bool isDone = false;
    //    int maxIteration = 1000;
    //    int currentIteration = 0;

    //    RestClient.Get<UserData>(GetURL($"USER/{user}")).Then(reponse =>
    //    {
    //        isDone = true;
    //        return reponse;
    //    });

    //    while (!isDone && currentIteration < maxIteration)
    //    {
    //        currentIteration++;
    //        await Task.Delay(10);
    //    }

    //    await Task.Delay(10);
    //    if (!isDone)
    //    {
    //        Debug.LogError("Data is not receive");
    //    }
    //    return new("", 0, 0, 0);
    //}
}

public class UserData
{
    public int id;
    public float x;
    public float y;
    public float z;

    public UserData(int id, float x, float y, float z)
    {
        this.id = id;
        this.x = x;
        this.y = y;
        this.z = z;
    }
}

public static class UserInstance
{
    public static UserData User;
    public static int OldId;
}