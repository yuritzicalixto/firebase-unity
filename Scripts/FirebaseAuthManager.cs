using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Auth;
using Firebase.Database;

public class FirebaseAuthManager : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public Button loginButton;
    public Button registerButton;
    public TextMeshProUGUI messageText;

    private FirebaseAuth auth;
    private DatabaseReference databaseRef;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            auth = FirebaseAuth.DefaultInstance;
            databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        });

        loginButton.onClick.AddListener(() => StartCoroutine(Login(emailInput.text, passwordInput.text)));
        registerButton.onClick.AddListener(() => StartCoroutine(Register(emailInput.text, passwordInput.text)));
    }

    IEnumerator Register(string email, string password)
    {
        var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => registerTask.IsCompleted);

        if (registerTask.Exception != null)
        {
            messageText.text = "❌ Error: " + registerTask.Exception.Message;
        }
        else
        {
            FirebaseUser user = registerTask.Result.User;  // 🔹 Aquí se corrige el error
            User newUser = new User(user.UserId, email);
            string json = JsonUtility.ToJson(newUser);

            databaseRef.Child("users").Child(user.UserId).SetRawJsonValueAsync(json);

            messageText.text = "✅ Registro exitoso.";
        }
    }

    IEnumerator Login(string email, string password)
    {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            messageText.text = "❌ Error: " + loginTask.Exception.Message;
        }
        else
        {
            FirebaseUser user = loginTask.Result.User;  // 🔹 Aquí se corrige el error
            messageText.text = "✅ Login exitoso.";
            PlayerPrefs.SetString("UserID", user.UserId);
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }
}

[System.Serializable]
public class User
{
    public string userId;
    public string email;

    public User(string userId, string email)
    {
        this.userId = userId;
        this.email = email;
    }
}
