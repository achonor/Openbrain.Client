using UnityEditor;

[InitializeOnLoad]
public class GlobalConfig
{
    static GlobalConfig()
    {
        PlayerSettings.Android.keystorePass = "Lull0618";
        PlayerSettings.Android.keyaliasName = "openbrain";
        PlayerSettings.Android.keyaliasPass = "Lull0618";
    }
}