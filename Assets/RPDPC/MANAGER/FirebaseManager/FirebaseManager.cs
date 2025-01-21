using Firebase;
using Firebase.Database;
using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class FirebaseManager : MonoBehaviour
{
    //private const string firebase_url = "https://rpdpc-a1f69-default-rtdb.europe-west1.firebasedatabase.app/";
    //private string GetURL(string url) => firebase_url + url + ".json";

    private const string USER_KEY = "USER";
    private FirebaseDatabase _database;
    public Action OnFirebaseInitialized;

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
            OnFirebaseInitialized?.Invoke();
        });
    }

    private async void OnFirebaseInit()
    {
        await SetUserData(new("Ryan", 1, 1, 1));

        var data = await GetUserData("Ryan");
        Debug.Log(data.name);

        var datas = await GetAllUserData();
        foreach (var d in datas)
        {
            Debug.Log($"{d.name}");
        }
    }

    public async Task SetUserData(UserData userData)
    {
        await _database.GetReference(USER_KEY).Database.GetReference(userData.name).SetRawJsonValueAsync(JsonUtility.ToJson(userData));
    }

    public async Task<UserData> GetUserData(string key)
    {
        var dataSnapshot = await _database.GetReference(USER_KEY).Database.GetReference(key).GetValueAsync();
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
    public string name;
    public float x;
    public float y;
    public float z;

    public UserData(string name, float x, float y, float z)
    {
        this.name = name;
        this.x = x;
        this.y = y;
        this.z = z;
    }
}