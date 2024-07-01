using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBarController : MonoBehaviour
{
    public Transform healthBarTrans;
    private UIDocument healthBarDocument;

    private ProgressBar HealthBar;
    private CharacterBase character;
    [Header("防御BuffUI")]
    private VisualElement DefenceElement;
    private Label DefenceText;
    [Header("力量BuffUI")]
    private VisualElement StrengthBuffElement;
    private Label StrengthBuffElementAmount;
    public Sprite StrengthbuffSprite;
    public Sprite StrengthDebuffSprite;

    [Header("敌人状态图标")]
    public VisualElement IntentElement;
    public Label IntentAmount;
    private Enemy enemy;
    private void Awake()
    {
        character= GetComponent<CharacterBase>();
        enemy= GetComponent<Enemy>();
        InitHealthBar();
    }
    private void OnEnable()
    {
        InitHealthBar();
    }
    private void Update()
    {
        UpdateHealthBar();
    }
    public void MovePositionToWorld(VisualElement element,Vector3 worldPos,Vector2 size) 
    {
        Rect rect =RuntimePanelUtils.CameraTransformWorldToPanelRect(element.panel, worldPos, size, Camera.main);
        element.transform.position = rect.position;

    }
    public void InitHealthBar() 
    {
        healthBarDocument = GetComponent<UIDocument>();
        HealthBar = healthBarDocument.rootVisualElement.Q<ProgressBar>("HealthBar");
        HealthBar.highValue = character.MaxHP;//设置血量条文本最大值  便于UI比例适配

        DefenceElement=HealthBar.Q<VisualElement>("Defence");
        DefenceText=DefenceElement.Q<Label>("DefenceAmount");
        DefenceElement.style.display=DisplayStyle.None;

        StrengthBuffElement=HealthBar.Q<VisualElement>("StrengthBuff");
        StrengthBuffElementAmount=StrengthBuffElement.Q<Label>("StrengthBuffAmount");
        StrengthBuffElement.style.display=DisplayStyle.None;

        IntentElement= HealthBar.Q<VisualElement>("EnemyIntentBuff");
        IntentAmount = HealthBar.Q<Label>("IntentCount");
        IntentElement.style.display=DisplayStyle.None;

        MovePositionToWorld(HealthBar, healthBarTrans.position, Vector2.zero);
    }
    public void UpdateHealthBar() 
    {
        if (character.isDead)
        {
            HealthBar.style.display = DisplayStyle.None;
            return;
        }
        if (HealthBar!=null)
        {
            HealthBar.title = $"{character.Currenthp}/{character.MaxHP}";//更新血量条文本显示
            HealthBar.value = character.Currenthp; //更新血量条UI显示
            HealthBar.RemoveFromClassList("highHealth");
            HealthBar.RemoveFromClassList("mediumHealth");
            HealthBar.RemoveFromClassList("lowHealth");
            var percentage = (float)character.Currenthp / (float)character.MaxHP;
            if (percentage<0.3f)
            {
                HealthBar.AddToClassList("lowHealth");

            }
            else if (percentage < 0.6f)
            {
                HealthBar.AddToClassList("mediumHealth");
            }
            else 
            {
                HealthBar.AddToClassList("highHealth");
            }
            DefenceElement.style.display=character.Defence.currentValue>0?DisplayStyle.Flex:DisplayStyle.None;
            DefenceText.text=character.Defence.currentValue.ToString();

            StrengthBuffElement.style.display=character.buffRound.currentValue>0?DisplayStyle.Flex:DisplayStyle.None;
            StrengthBuffElement.style.backgroundImage=character.baseStrength>1?new StyleBackground( StrengthbuffSprite):new StyleBackground( StrengthDebuffSprite);
            StrengthBuffElementAmount.text=character.buffRound.currentValue.ToString();

        }
    }
    public void SetIntentElement()
    {
        IntentElement.style.display = DisplayStyle.Flex;
        IntentElement.style.backgroundImage = new StyleBackground(enemy.currentAction.IntentIcon);
        var value= enemy.currentAction.enemyEffect.value;
        if (enemy.currentAction.enemyEffect.GetType()==typeof(DamageEffect))//如果是攻击特效
        {
            value = Mathf.RoundToInt(enemy.currentAction.enemyEffect.value* enemy.baseStrength);
        }
        IntentAmount.text=value.ToString();
    }
    public void HideIntentElement()
    {
        IntentElement.style.display = DisplayStyle.None;
    }
}
