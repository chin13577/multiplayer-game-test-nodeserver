using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{

    public static SkillManager instance = null;

    public Transform aoeTransform;
    public Transform singleTargetTransform;
    Transform skillTransform;
    public PlayerController player;
    void Awake()
    {
        instance = this;
    }
    private void OnEnable()
    {
        ControllerManager.instance.OnSkillBtnPress += OnSkillBtnPress;
    }
    private void OnDisable()
    {
        ControllerManager.instance.OnSkillBtnPress -= OnSkillBtnPress;
    }
    void OnPlayerCreated(PlayerController playerController)
    {
        if (playerController.isLocalPlayer)
        {
            player = playerController;
        }
    }
    void OnSkillBtnPress(bool isPress, int skill)
    {
        if (isPress)
        {
            Skill.SkillType type = player.skill[skill].skillType;
            if (type == Skill.SkillType.Single)
            {
                aoeTransform.gameObject.SetActive(false);
                singleTargetTransform.gameObject.SetActive(true);
            }
            else if (type == Skill.SkillType.AOE)
            {
                aoeTransform.gameObject.SetActive(true);
                singleTargetTransform.gameObject.SetActive(false);
            }
        }
    }
    void OnSkillBtnDrag(Vector2 value)
    {

    }
    IEnumerator UpdateMagicRingTransform()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();

        }
    }
}
