using System;
using UnityEngine;
using Realms.Sync;
using System.Linq;
using Realms;
using MongoDB.Bson;
using System.Data;


public class RealmController : MonoBehaviour
{
    static public RealmController Instance;
    private string _realmAppId = "application-0-mbfrqjx";
    private App _realmApp;
    private User _realmUser;
    private Realm _realm;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        intializeRealm();
        Instance = this;
    }

    private void intializeRealm()
    {
        var config = new AppConfiguration(_realmAppId);
        _realmApp = App.Create(config);
    }

    public async void Login()
    {
        var input = LoginSystemController.Instance.GetInputPlayerLogin();

        if (input.HasValue)
        {
            LoginSystemController.Instance.setErrorMessageLogin("");
            var (email, password) = input.Value;
            Debug.Log($"Email: {email}, Password: {password}");


            try
            {
                if (_realmUser == null)
                {
                    var credentials = Credentials.EmailPassword(email, password);
                    _realmUser = await _realmApp.LogInAsync(credentials);
                    LoginSystemController.Instance.setErrorMessageLogin("Login Successful");
                }
                else
                {
                    LoginSystemController.Instance.setErrorMessageLogin("Already Login!!");
                }
            }
            catch (Exception ex)
            {
                LoginSystemController.Instance.setErrorMessageLogin(ex.Message);
            }

            if (_realmUser != null)
            {
                var config = new FlexibleSyncConfiguration(_realmUser);
                _realm = await Realm.GetInstanceAsync(config);

                _realm.Subscriptions.Update(() =>
                {
                    var myItem = _realm.All<PlayerData>();
                    _realm.Subscriptions.Add(myItem);
                });

                await _realm.Subscriptions.WaitForSynchronizationAsync();
            }
            else
            {
                Debug.Log("Try agin Login!!");
            }

            if (_realm != null)
            {
                PlayerData playerData = GetOrCreateUserData();

                if (string.IsNullOrEmpty(playerData.PlayerName))
                {
                    LoginSystemController.Instance.SetPlayername();
                }
                else
                {
                    LoadScene.ChangeScene.Load("MainMenu");
                }
            }
        }
    }

    public async void Register()
    {
        var input = LoginSystemController.Instance.GetInputPlayerRegister();

        if (input.HasValue)
        {
            LoginSystemController.Instance.setErrorMessageRegister("");
            var (registerEmail, registerPassword) = input.Value;
            Debug.Log($"Email: {registerEmail}, Password: {registerPassword}");

            await _realmApp.EmailPasswordAuth.RegisterUserAsync(registerEmail, registerPassword);

            LoginSystemController.Instance.setErrorMessageRegister("Register successful");
            LoginSystemController.Instance.Login();
        }
    }

    private PlayerData GetOrCreateUserData()
    {
        PlayerData playerData = _realm.All<PlayerData>().Where(n => n.OwnerId == _realmUser.Id).FirstOrDefault();

        if (_realm != null && playerData == null)
        {
            _realm.Write(() =>
            {
                playerData = _realm.Add<PlayerData>(new PlayerData
                {
                    Id = ObjectId.GenerateNewId(),
                    PlayerName = "",
                    Role = "user",
                    Tokens = 0,
                    TopupGems = 0,
                    IsRoom = false,
                    IsRoomRoomname = "",
                    IsRoomColor = "",
                    IsRoomPercent = 0,
                    OwnerId = _realmUser.Id
                });
            });
        }
        return playerData;
    }

    private void setPlayerName(string name)
    {
        var existingUser = _realm.All<PlayerData>().FirstOrDefault(u => u.PlayerName == name);

        if (existingUser == null)
        {
            PlayerData playerData = GetOrCreateUserData();
            if (playerData != null && string.IsNullOrEmpty(playerData.PlayerName))
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    LoginSystemController.Instance.setErrorMessagePlayerName("Name can't space or empty!!");
                }
                else
                {
                    _realm.Write(() =>
                    {
                        playerData.PlayerName = name;
                    });
                    LoginSystemController.Instance.setErrorMessagePlayerName("");
                    LoadScene.ChangeScene.Load("MainMenu");
                }
            }
        }
        else
        {
            LoginSystemController.Instance.setErrorMessagePlayerName("Name is already exist!!");
        }
    }

    public void setPlayerNameButton()
    {
        var Playername = LoginSystemController.Instance.GetInputPlayerName();
        setPlayerName(Playername);
    }

    public void LoginWithGoogle()
    {
        LoginSystemController.Instance.SetPlayername();
    }

    public void changeRegisterPage()
    {
        LoginSystemController.Instance.Register();
    }

    public void changeLoginPage()
    {
        LoginSystemController.Instance.Login();
    }
}
