using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour {
    public Image image;
    public void SetActive(bool isActive)
    {
        image.raycastTarget = isActive;
    }
}
