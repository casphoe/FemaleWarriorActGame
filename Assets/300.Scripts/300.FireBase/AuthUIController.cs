using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Firebase.Auth;

public class AuthUIController : MonoBehaviour
{
   [Header("Panels")]
    public GameObject panelSignIn, panelSignUp, panelForgot, panelHUD, loadingBlocker;

    [Header("SignIn UI")]
    public TMP_InputField inEmail;
    public TMP_InputField inPassword;
    public Toggle inShowPw;
    public Toggle inRememberEmail;

}
