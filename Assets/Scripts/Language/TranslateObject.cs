using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TranslateObject : MonoBehaviour
{
    public abstract void ChangeLanguageTo(Language language);
}

public enum Language { English = 0, Portuguese = 1}
