using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Linq;
using System.Collections.Generic;
using IAP;

public class FirebaseManager : MonoBehaviour
{
    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;    
    public FirebaseUser User;
    public DatabaseReference DBreference;

    //Login variables
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;

    //Register variables
    [Header("Register")]
    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;
    public TMP_Text warningRegisterText;

    //User Data variables
    [Header("UserData")]
    public TMP_InputField usernameField;
    public TMP_InputField xpField;
    public TMP_InputField killsField;
    public TMP_InputField deathsField;
    public GameObject scoreElement;
    public Transform scoreboardContent;
    public TMP_Text scoreText;


    public static FirebaseManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Debug.Log("Instance already exists, destroying object!");
            //StartCoroutine(AddLog("Instance already exists, destroying object!"));
            Destroy(this);
        }

        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                InitializeFirebase();

            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);

                StartCoroutine(AddErrorLog("Could not resolve all Firebase dependencies: " + dependencyStatus, 1));
            }
        });


        StartCoroutine(SetupPurchaseIttems());
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //StartCoroutine(AddLog("Setting up Firebase Auth"));
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;

    }

    public IEnumerator SetupPurchaseIttems()
    {
        while (DBreference is null)
        {
            yield return null;
        }
        
        {
            foreach (var item in InAppManager.instance.PurchaseItems)
            {
                var DBItemTask = DBreference.Child("Products").Child(item.ID).Child("cost").SetValueAsync(item.cost);
                DBreference.Child("Products").Child(item.ID).Child("product_type_id").SetValueAsync(item.productTypeId);

                yield return new WaitUntil(predicate: () => DBItemTask.IsCompleted);

                //if (DBItemTask.Exception == null)
                {
                    //DataSnapshot snapshot = DBItemTask.Result;
                    //item.cost = int.Parse(snapshot.Value.ToString());
                }
            }
        }
    }

    public IEnumerator AddOptions()
    {
        while (DBreference is null)
        {
            yield return null;
        }

        var DBTask = DBreference.Child("Options").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        var count = DBTask.Result.ChildrenCount;

        var DBItemTask = DBreference.Child("Options")
            .Child((count).ToString())
            .Child("music")
            .SetValueAsync(true);

        DBreference.Child("Options")
             .Child((count).ToString())
             .Child("vibrations")
             .SetValueAsync(true);

        yield return new WaitUntil(predicate: () => DBItemTask.IsCompleted);

        //if (DBItemTask.Exception == null)
        {
            //DataSnapshot snapshot = DBItemTask.Result;
            //item.cost = int.Parse(snapshot.Value.ToString());
        }
    }

    public IEnumerator AddLevel(int seed)
    {
        while (DBreference is null)
        {
            yield return null;
        }

        var DBTask = DBreference.Child("Levels").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        var count = DBTask.Result.ChildrenCount;

        DBreference.Child("Levels")
            .Child((count).ToString())
            .Child("seed")
            .SetValueAsync(seed);

        var DBItemTask = DBreference.Child("Users").Child(User.UserId).Child("player_id").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBItemTask.IsCompleted);

        DBreference.Child("Levels")
             .Child((count).ToString())
             .Child("player_id")
             .SetValueAsync(DBItemTask.Result);

        

        //if (DBItemTask.Exception == null)
        {
            //DataSnapshot snapshot = DBItemTask.Result;
            //item.cost = int.Parse(snapshot.Value.ToString());
        }
    }

    public IEnumerator AddPlayer()
    {
        while (DBreference is null)
        {
            yield return null;
        }

        var DBTask = DBreference.Child("Players").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        var count = DBTask.Result.ChildrenCount;

        DBreference.Child("Players")
            .Child((count).ToString())
            .Child("status_id")
            .SetValueAsync("online");
       


        //if (DBItemTask.Exception == null)
        {
            //DataSnapshot snapshot = DBItemTask.Result;
            //item.cost = int.Parse(snapshot.Value.ToString());
        }
    }

    public IEnumerator AddUserInfo()
    {
        while (DBreference is null)
        {
            yield return null;
        }

        var DBTask = DBreference.Child("UsersInfo").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        var count = DBTask.Result.ChildrenCount;

        var DBItemTask = DBreference.Child("UsersInfo")
            .Child((count).ToString())
            .Child("username")
            .SetValueAsync(User.DisplayName);

        DBreference.Child("UsersInfo")
             .Child((count).ToString())
             .Child("email")
             .SetValueAsync(User.Email);

        DBreference.Child("UsersInfo")
             .Child((count).ToString())
             .Child("optios_id")
             .SetValueAsync(count);

        yield return new WaitUntil(predicate: () => DBItemTask.IsCompleted);

        //if (DBItemTask.Exception == null)
        {
            //DataSnapshot snapshot = DBItemTask.Result;
            //item.cost = int.Parse(snapshot.Value.ToString());
        }
    }

    public IEnumerator AddScore(int score)
    {
        while (DBreference is null)
        {
            yield return null;
        }

        var DBTask = DBreference.Child("Scores").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        var count = DBTask.Result.ChildrenCount;

        DBreference.Child("Scores")
            .Child((count).ToString())
            .Child("total_count")
            .SetValueAsync(score);

        var DBItemTask = DBreference.Child("Users").Child(User.UserId).Child("player_id").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBItemTask.IsCompleted);

        DBreference.Child("Scores")
             .Child((count).ToString())
             .Child("player_id")
             .SetValueAsync(DBItemTask.Result);

        //if (DBItemTask.Exception == null)
        {
            //DataSnapshot snapshot = DBItemTask.Result;
            //item.cost = int.Parse(snapshot.Value.ToString());
        }
    }

    public IEnumerator AddTransaction(string productId)
    {
        while (DBreference is null)
        {
            yield return null;
        }

        var DBTask = DBreference.Child("Transactions").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        var count = DBTask.Result.ChildrenCount;

        DBreference.Child("Transactions")
            .Child((count).ToString())
            .Child("product_id")
            .SetValueAsync(productId);

        var DBItemTask = DBreference.Child("Users").Child(User.UserId).Child("player_id").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBItemTask.IsCompleted);

        DBreference.Child("Transactions")
             .Child((count).ToString())
             .Child("player_id")
             .SetValueAsync(DBItemTask.Result);

        DBreference.Child("Transactions")
             .Child((count).ToString())
             .Child("status_id")
             .SetValueAsync(1);
        //if (DBItemTask.Exception == null)
        {
            //DataSnapshot snapshot = DBItemTask.Result;
            //item.cost = int.Parse(snapshot.Value.ToString());
        }
    }

    public IEnumerator AddLog(string description)
    {
        while (DBreference is null)
        {
            yield return null;
        }

        var DBTask = DBreference.Child("ActionLogs").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        var count = DBTask.Result.ChildrenCount;

        var DBItemTask = DBreference.Child("ActionLogs")
            .Child((count).ToString())
            .Child("description")
            .SetValueAsync(description);

        DBreference.Child("ActionLogs")
            .Child((count).ToString())
            .Child("user_id")
            .SetValueAsync(User.UserId);

        yield return new WaitUntil(predicate: () => DBItemTask.IsCompleted);

        //if (DBItemTask.Exception == null)
        {
            //DataSnapshot snapshot = DBItemTask.Result;
            //item.cost = int.Parse(snapshot.Value.ToString());
        }                    
    }

    public IEnumerator AddWarningLog(string description)
    {
        while (DBreference is null)
        {
            yield return null;
        }

        var DBTask = DBreference.Child("WarningLogs").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        var count = DBTask.Result.ChildrenCount;

        var DBItemTask = DBreference.Child("WarningLogs")
            .Child((count).ToString())
            .Child("description")
            .SetValueAsync(description);

        DBreference.Child("WarningLogs")
            .Child((count).ToString())
            .Child("user_id")
            .SetValueAsync(User.UserId);

        yield return new WaitUntil(predicate: () => DBItemTask.IsCompleted);

        //if (DBItemTask.Exception == null)
        {
            //DataSnapshot snapshot = DBItemTask.Result;
            //item.cost = int.Parse(snapshot.Value.ToString());
        }
    }

    public IEnumerator AddErrorLog(string description, int errorCode)
    {
        while (DBreference is null)
        {
            yield return null;
        }

        var DBTask = DBreference.Child("ErrorLogs").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        var count = DBTask.Result.ChildrenCount;

        var DBItemTask = DBreference.Child("ErrorLogs")
            .Child((count).ToString())
            .Child("description")
            .SetValueAsync(description);

        DBreference.Child("ErrorLogs")
            .Child((count).ToString())
            .Child("user_id")
            .SetValueAsync(User.UserId);
        
        DBreference.Child("ErrorLogs")
            .Child((count).ToString())
            .Child("user_id")
            .SetValueAsync(User.UserId);
       
        DBreference.Child("ErrorLogs")
           .Child((count).ToString())
           .Child("error_code")
           .SetValueAsync(errorCode);

        yield return new WaitUntil(predicate: () => DBItemTask.IsCompleted);

        //if (DBItemTask.Exception == null)
        {
            //DataSnapshot snapshot = DBItemTask.Result;
            //item.cost = int.Parse(snapshot.Value.ToString());
        }
    }

    public void ClearLoginFeilds()
    {
        emailLoginField.text = "";
        passwordLoginField.text = "";
    }
    public void ClearRegisterFeilds()
    {
        usernameRegisterField.text = "";
        emailRegisterField.text = "";
        passwordRegisterField.text = "";
        passwordRegisterVerifyField.text = "";
    }

    //Function for the login button
    public void LoginButton()
    {
        //Call the login coroutine passing the email and password
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }
    //Function for the register button
    public void RegisterButton()
    {
        //Call the register coroutine passing the email, password, and username
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
    }
    //Function for the sign out button
    public void SignOutButton()
    {
        auth.SignOut();
        UIManager.instance.LoginScreen();
        ClearRegisterFeilds();
        ClearLoginFeilds();
    }
    //Function for the save button
    public void SaveDataButton()
    {
       // StartCoroutine(UpdateUsernameAuth(usernameField.text));
        //StartCoroutine(UpdateUsernameDatabase(usernameField.text));

        StartCoroutine(UpdateScore(int.Parse(xpField.text)));
    }

    public void UpdateData(int score)
    {
        // StartCoroutine(UpdateUsernameAuth(usernameField.text));
        //StartCoroutine(UpdateUsernameDatabase(usernameField.text));

        StartCoroutine(UpdateScore(score));
    }

    //Function for the scoreboard button
    public void ScoreboardButton()
    {        
        StartCoroutine(LoadScoreboardData());
    }

    private IEnumerator Login(string _email, string _password)
    {
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");

            StartCoroutine(AddWarningLog($"Failed to register task with {LoginTask.Exception}"));

            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            warningLoginText.text = message;
        }
        else
        {
            //User is now logged in
            //Now get the result
            User = LoginTask.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
            StartCoroutine(AddLog($"User signed in successfully: {User.DisplayName} ({User.Email})"));
            warningLoginText.text = "";
            confirmLoginText.text = "Logged In";
            StartCoroutine(LoadUserData());

            yield return new WaitForSeconds(2);

            usernameField.text = User.DisplayName;
            UIManager.instance.UserDataScreen(); // Change to user data UI
            confirmLoginText.text = "";
            ClearLoginFeilds();
            ClearRegisterFeilds();
        }


        
    }

    private IEnumerator Register(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            //If the username field is blank show a warning
            warningRegisterText.text = "Missing Username";
        }
        else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            //If the password does not match show a warning
            warningRegisterText.text = "Password Does Not Match!";
        }
        else
        {
            //Call the Firebase auth signin function passing the email and password
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                }
                warningRegisterText.text = message;
            }
            else
            {
                //User has now been created
                //Now get the result
                User = RegisterTask.Result;

                if (User != null)
                {
                    //Create a user profile and set the username
                    UserProfile profile = new UserProfile { DisplayName = _username };

                    //Call the Firebase auth update user profile function passing the profile with the username
                    var ProfileTask = User.UpdateUserProfileAsync(profile);
                    //Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        warningRegisterText.text = "Username Set Failed!";
                    }
                    else
                    {
                        //Username is now set
                        //Now return to login screen
                        UIManager.instance.LoginScreen();
                        warningRegisterText.text = "";
                        ClearRegisterFeilds();
                        ClearLoginFeilds();
                    }
                }
            }
        }
    }

    private IEnumerator UpdateUsernameAuth(string _username)
    {
        //Create a user profile and set the username
        UserProfile profile = new UserProfile { DisplayName = _username };

        //Call the Firebase auth update user profile function passing the profile with the username
        var ProfileTask = User.UpdateUserProfileAsync(profile);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

        if (ProfileTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
        }
        else
        {
            //Auth username is now updated
        }
    }

    private IEnumerator UpdateUsernameDatabase(string _username)
    {
        //Set the currently logged in user username in the database
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("username").SetValueAsync(_username);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Database username is now updated
        }
    }
    private IEnumerator UpdateScore(int _xp)
    {
        var DBGetTask = DBreference.Child("Users").Child(User.UserId).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBGetTask.IsCompleted);

        if (DBGetTask.Exception == null 
            && DBGetTask.Result.Child("score").Value != null
            &&float.Parse(DBGetTask.Result.Child("score").Value.ToString()) >= _xp)
        {
            yield break;
        }
        //Set the currently logged in user xp
        var DBTask = DBreference.Child("Users").Child(User.UserId).Child("score").SetValueAsync(_xp);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            StartCoroutine(AddWarningLog($"Failed to register task with {DBTask.Exception}"));
        }
        else
        {
        }
    }

    private IEnumerator LoadUserData()
    {
        //Get the currently logged in user data
        var DBTask = DBreference.Child("Users").Child(User.UserId).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            StartCoroutine(AddWarningLog($"Failed to register task with {DBTask.Exception}"));
        }
        else if (DBTask.Result.Value == null || DBTask.Result.Child("score").Value == null)
        {
            //No data exists yet
            scoreText.text = "0";
            
            UIManager.instance.TurnOffMenu();
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;
            scoreText.text = snapshot.Child("score").Value.ToString();
            //xpField.text = snapshot.Child("score").Value.ToString();
            //killsField.text = snapshot.Child("kills").Value.ToString();
            //deathsField.text = snapshot.Child("deaths").Value.ToString();

            UIManager.instance.TurnOffMenu();
        }
    }

    private IEnumerator LoadScoreboardData()
    {
        //Get all the Users data ordered by kills amount
        var DBTask = DBreference.Child("Users").OrderByChild("score").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            StartCoroutine(AddWarningLog($"Failed to register task with {DBTask.Exception}"));
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            //Destroy any existing scoreboard elements
            foreach (Transform child in scoreboardContent.transform)
            {
                Destroy(child.gameObject);
            }

            int ind = 0;
            //Loop through every Users UID
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                string username = childSnapshot.Child("username").Value.ToString();
                int score = int.Parse(childSnapshot.Child("score").Value.ToString());
                ind++;
                //Instantiate new scoreboard elements
                GameObject scoreboardElement = Instantiate(scoreElement, scoreboardContent);
                scoreboardElement.GetComponent<ScoreElement>()
                    .NewScoreElement(
                    username,
                    ind, 
                    score);
            }

            //Go to scoareboard screen
            UIManager.instance.ScoreboardScreen();
        }
    }
}
