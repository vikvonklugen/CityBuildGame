using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private UnityEngine.Object[] buildingTypes;
    private UnityEngine.Object[] resourceTypes;

    public InputManager inputManager;
    public HeroManager heroManager;

    public GameObject buildPanel;
    public GameObject upgradePanel;
    public GameObject eventPanel;
    public GameObject eventResultPanel;
    public GameObject topLevelHUD;
    public GameObject heroHireScreen;
    public GameObject heroSelectScreen;
    public GameObject gridHolder;

    public Sprite emptyTileSprite;

    private Button upgradeButton;
    private Button destroyButton;

    public Sprite[] combatModifierIcons;
    private int heroHireScreenIndex = 0;
    private Transform resourceBar;
    private Transform buildingListContent;
    private Image clock;
    public static bool clockAnimationFinished;

    private Color negativeColor = new Color(249, 119, 34, 255) / 255f;
    private Color positiveColor = new Color(209, 205, 61, 255) / 255f;
    private Color neutralColor = new Color(237, 239, 166, 255) / 255f;

    private TextMeshProUGUI[] resourceText = new TextMeshProUGUI[4];
    private TextMeshProUGUI[] growthText = new TextMeshProUGUI[4];

    private System.Random random = new System.Random();


    public static event Action ResourcesChangedEvent = delegate { };

    void Start()
    {

        resourceBar = topLevelHUD.transform.GetChild(0);
        clock = topLevelHUD.transform.GetChild(2).gameObject.GetComponent<Image>();

        buildingListContent = buildPanel.transform.GetChild(1).GetChild(0).GetChild(0);

        upgradeButton = upgradePanel.transform.GetChild(1).gameObject.GetComponent<Button>();
        destroyButton = upgradePanel.transform.GetChild(2).gameObject.GetComponent<Button>();

        destroyButton.onClick.AddListener(() => DestroyBuilding());
        upgradeButton.onClick.AddListener(() => UpgradeBuilding());

        buildingTypes = Resources.LoadAll("Data/Buildings", typeof(Building));

        RectTransform rectTransform = buildingListContent.GetComponent<RectTransform>();
        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, 550 - (Mathf.Ceil(buildingTypes.Length / 2f) * 500f));

        foreach (Building buildingtype in buildingTypes)
        {
            GameObject building = (GameObject)Instantiate(Resources.Load("BuildingUIButtonItem"), buildingListContent);
            GameObject buildingButton = building.transform.GetChild(1).gameObject;
            GameObject buildingName = building.transform.GetChild(2).gameObject;
            GameObject buildingInfo = building.transform.GetChild(3).gameObject;

            if (buildingtype.buildingSprite != null)
            {
                buildingButton.GetComponent<Image>().sprite = buildingtype.buildingSprite;
            }
            if (buildingtype.name != null)
            {
                buildingName.GetComponent<TextMeshProUGUI>().text = buildingtype.name;
            }
            if (buildingtype.buildingInfo != null)
            {
                buildingInfo.GetComponent<TextMeshProUGUI>().text = buildingtype.buildingInfo + "\nCost: " + buildingtype.buildingCost;
            }

            buildingButton.GetComponent<Button>().onClick.AddListener(() => BuildBuilding(buildingtype));
        }

        AddResourceIndicators();

        for (int i = 0; i < 4; i++)
        {
            resourceText[i] = topLevelHUD.transform.GetChild(0).GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            growthText[i] = topLevelHUD.transform.GetChild(0).GetChild(i).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        }

        UpdateHeroSelectScreen();
        UpdateHeroHireScreen();
    }

    public void BuildBuilding(Building building)
    {
        BuildingController buildingController = InputManager.selectedTile.GetComponent<BuildingController>();
        if (buildingController.buildable && GameManager.resources[AResource.Type.Materials] >= building.buildingCost)
        {
            StartCoroutine(AddResources(new AResource.ResourceBundle(AResource.Type.Materials, -building.buildingCost)));
            buildingController.buildable = false;
            buildingController.building = building;
            buildingController.productionPerTick = building.resourceProducedPerTick;
            buildingController.returnedMaterialsOnDestroy += (int)Mathf.Floor(building.buildingCost / 2);
            InputManager.selectedTile.GetComponent<SpriteRenderer>().sprite = building.buildingSprite;

            if (building.producedResource != AResource.Type.Seconds && building.producedResource != AResource.Type.None)
            {
                GameManager.resourceGrowth[building.producedResource] += buildingController.productionPerTick;
            }

            UpdateHUD();
            buildPanel.SetActive(false);
            UpdateUpgradeMenu(buildingController);
            upgradePanel.SetActive(true);
        }
    }

    public string DestroyBuilding(bool randomBuilding = false)
    {
        BuildingController buildingController;

        if (randomBuilding)
        {
            BuildingController[] buildingControllers = gridHolder.GetComponentsInChildren<BuildingController>();
            List<BuildingController> buildingTiles = new List<BuildingController>();
            foreach (BuildingController controller in buildingControllers)
            {
                if (controller.building != null)
                {
                    buildingTiles.Add(controller);
                }
            }

            if (buildingTiles.Count == 0)
            {
                Debug.Log("No buildings to destroy");
                return null;
            }
            
            buildingController = buildingTiles[random.Next(0, buildingTiles.Count - 1)];
            buildingController.GetComponentInParent<SpriteRenderer>().sprite = null;
        }
        else
        {
            buildingController = InputManager.selectedTile.GetComponent<BuildingController>();
            StartCoroutine(AddResources(new AResource.ResourceBundle(AResource.Type.Materials, buildingController.returnedMaterialsOnDestroy)));
            InputManager.selectedTile.GetComponent<SpriteRenderer>().sprite = null;
        }

        Building building = buildingController.building;
        buildingController.building = null;
        buildingController.buildable = true;
        buildingController.level = 0;

        if (building.producedResource != AResource.Type.Seconds && building.producedResource != AResource.Type.None)
        {
            GameManager.resourceGrowth[building.producedResource] -= buildingController.productionPerTick;
        }

        UpdateHUD();
        upgradePanel.SetActive(false);
        inputManager.Deselect();

        if (GameManager.currentEvent != null)
        {
            eventPanel.SetActive(true);
        }

        return building.name;
    }

    public void UpgradeBuilding()
    {
        BuildingController buildingController = InputManager.selectedTile.GetComponent<BuildingController>();
        Building building = buildingController.building;

        if (GameManager.resources[AResource.Type.Materials] >= building.upgrades[buildingController.level].materialCost)
        {
            StartCoroutine(AddResources(new AResource.ResourceBundle(AResource.Type.Materials, -building.upgrades[buildingController.level].materialCost)));
            buildingController.productionPerTick += building.upgrades[buildingController.level].productionBoost;
            buildingController.returnedMaterialsOnDestroy += (int)Mathf.Floor(building.upgrades[buildingController.level].materialCost / 2f);

            if (building.producedResource != AResource.Type.Seconds && building.producedResource != AResource.Type.None)
            {
                GameManager.resourceGrowth[building.producedResource] += building.upgrades[buildingController.level].productionBoost;
            }

            buildingController.level++;
            UpdateUpgradeMenu(buildingController);
            UpdateHUD();
        }
    }

    public void ActivateBuildMenu(RaycastHit2D hit)
    {
        upgradePanel.SetActive(false);
        buildPanel.SetActive(false);
        GameObject tile = hit.collider.gameObject;
        Debug.Log(tile.name);
        BuildingController buildingController = tile.GetComponent<BuildingController>();
        if (buildingController.unlockedForBuilding && InputManager.selectedTile != tile)
        {
            // Activate build menu if nothing is built yet
            if (buildingController.building == null)
            {
                buildPanel.SetActive(true);
            }
            else
            {
                UpdateUpgradeMenu(buildingController);
                upgradePanel.SetActive(true);
            }

            if (GameManager.currentEvent != null)
            {
                eventPanel.SetActive(false);
            }

            inputManager.Select(hit);
        }
        else
        {
            if (GameManager.currentEvent != null)
            {
                eventPanel.SetActive(true);
            }

            inputManager.Deselect();
        }
    }

    void UpdateUpgradeMenu(BuildingController buildingController)
    {
        GameObject upgradeTextPanel = upgradePanel.transform.GetChild(0).gameObject;
        TextMeshProUGUI buildingName = upgradeTextPanel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI buildingLevel = upgradeTextPanel.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI productionInfo = upgradeTextPanel.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI upgradeInfo = upgradeTextPanel.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();

        buildingName.text = buildingController.building.name;
        if (buildingController.building.producedResource != AResource.Type.None)
        {
            productionInfo.text = buildingController.productionPerTick.ToString() + " " + buildingController.building.producedResource + "/tick";
        }
        else
        {
            productionInfo.text = "";
        }

        if (buildingController.building.upgrades.Length > 0 && buildingController.level < buildingController.building.upgrades.Length)
        {
            if (buildingController.building.upgrades[buildingController.level].materialCost <= GameManager.resources[AResource.Type.Materials])
            {
                upgradeButton.interactable = true;
            }
            else
            {
                upgradeButton.interactable = false;
            }

            buildingLevel.text = "Level " + (buildingController.level + 1).ToString();
            upgradeInfo.text = "Next upgrade: " + buildingController.building.upgrades[buildingController.level].description + "\nCost: " + buildingController.building.upgrades[buildingController.level].materialCost + " materials";
        }
        else
        {
            buildingLevel.text = "Max Level";
            upgradeInfo.text = buildingController.building.buildingInfo;
            upgradeButton.interactable = false;
        }
    }

    public void DisplayEvent()
    {
        TextMeshProUGUI eventTextPanel = eventPanel.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        eventTextPanel.text = GameManager.currentEvent.description;
    }

    public void UpdateHUD()
    {
        string modifier;
        for (int i = 0; i < 4; i++)
        {
            if (GameManager.resourceGrowth[(AResource.Type)i] < 0)
            {
                modifier = "";
                growthText[i].color = negativeColor;
            }
            else
            {
                modifier = "+";
                growthText[i].color = neutralColor;
            }
            resourceText[i].text = GameManager.resources[(AResource.Type)i].ToString();
            growthText[i].text = modifier + GameManager.resourceGrowth[(AResource.Type)i].ToString();
        }
    }

    public void AddResourceIndicators()
    {
        resourceTypes = Resources.LoadAll("Data/Resources", typeof(AResource));
        foreach (AResource resourceType in resourceTypes)
        {
            GameObject resourceIndicator = (GameObject)Instantiate(Resources.Load("ResourceIndicator"), resourceBar);
            resourceIndicator.name = resourceType.resource.ToString();
            TextMeshProUGUI resourceAmount = resourceIndicator.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            Image resourceIcon = resourceIndicator.transform.GetChild(1).gameObject.GetComponent<Image>();
            Image resourceBorder = resourceIndicator.transform.GetChild(3).gameObject.GetComponent<Image>();

            GameManager.resources[resourceType.resource] = resourceType.startAmount;

            resourceAmount.text = GameManager.resources[resourceType.resource].ToString();
            resourceIcon.sprite = resourceType.resourceIcon;
            resourceBorder.sprite = resourceType.resourceBorder;
        }
    }

    public IEnumerator AddResources(AResource.ResourceBundle resourceBundle)
    {
        int increment = 1;
        if (resourceBundle.resourceAmount < 0)
        {
            resourceText[(int)resourceBundle.resourceType].color = negativeColor;
            increment = -increment;
        }
        else
        {
            resourceText[(int)resourceBundle.resourceType].color = positiveColor;
        }

        for (int i = 0; i < Mathf.Abs(resourceBundle.resourceAmount); i++)
        {
            resourceText[(int)resourceBundle.resourceType].fontSize++;

            if (GameManager.resources[resourceBundle.resourceType] + increment >= 0)
            {
                GameManager.resources[resourceBundle.resourceType] += increment;
            }
            else
            {
                resourceText[(int)resourceBundle.resourceType].fontSize--;
                break;
            }

            GameManager.uiController.UpdateHUD();

            yield return new WaitForSeconds(0.025f);

            resourceText[(int)resourceBundle.resourceType].fontSize--;

            yield return new WaitForSeconds(0.075f);
        }

        resourceText[(int)resourceBundle.resourceType].color = neutralColor;
    }

    public void UpdateHeroHireScreen()
    {
        Transform buttons = heroHireScreen.transform.GetChild(5);
        Button backButton = buttons.GetChild(1).gameObject.GetComponent<Button>();
        Button nextButton = buttons.GetChild(0).gameObject.GetComponent<Button>();
        Button hireButton = buttons.GetChild(4).gameObject.GetComponent<Button>();

        Transform hero = heroHireScreen.transform.GetChild(4);
        TextMeshProUGUI heroName = heroHireScreen.transform.GetChild(6).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        Image heroSprite = hero.GetChild(1).gameObject.GetComponent<Image>();
        TextMeshProUGUI heroInfo = heroHireScreen.transform.GetChild(8).GetChild(4).GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI heroCost = heroHireScreen.transform.GetChild(9).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

        heroHireScreenIndex = Mathf.Clamp(heroHireScreenIndex, 0, Mathf.Clamp(HeroManager.recruitableHeroes.Count - 1, 0, int.MaxValue));

        if (HeroManager.recruitableHeroes.Count != 0)
        {
            HeroManager.Hero _hero = HeroManager.recruitableHeroes[heroHireScreenIndex];
            heroName.text = _hero.heroName.name + "\nthe " + _hero.type.heroType;
            heroSprite.enabled = true;
            heroSprite.sprite = _hero.type.icon;
            heroInfo.text = _hero.heroName.description;
            string luxuries;
            if (_hero.heroName.recruitmentCost == 1)
            {
                luxuries = " Luxury item";
            }
            else
            {
                luxuries = " Luxuries";
            }
            heroCost.text = "Cost: " + _hero.heroName.recruitmentCost + luxuries;

            if (GameManager.currentEvent != null)
            {
                heroSprite.transform.GetChild(0).gameObject.SetActive(true);
                heroSprite.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = GetCombatModifierIcon(_hero);
            }
            else
            {
                heroSprite.transform.GetChild(0).gameObject.SetActive(false);
            }

            if (GameManager.resources[AResource.Type.Luxuries] < _hero.heroName.recruitmentCost)
            {
                hireButton.interactable = false;
            }
            else
            {
                hireButton.interactable = true;
            }
        }
        else
        {
            heroName.text = "No Heroes\nAvailable";
            heroCost.text = "No hero selected";
            heroInfo.text = "No hero selected";
            heroSprite.enabled = false;
            hireButton.interactable = false;
        }

        if (heroHireScreenIndex == 0)
        {
            backButton.interactable = false;
        }
        else
        {
            backButton.interactable = true;
        }

        if (heroHireScreenIndex == Mathf.Clamp(HeroManager.recruitableHeroes.Count - 1, 0, int.MaxValue))
        {
            nextButton.interactable = false;
        }
        else
        {
            nextButton.interactable = true;
        }
    }

    public void UpdateHeroSelectScreen()
    {
        Transform buttons = heroSelectScreen.transform.GetChild(5);
        Button backButton = buttons.GetChild(1).gameObject.GetComponent<Button>();
        Button nextButton = buttons.GetChild(0).gameObject.GetComponent<Button>();
        Button useButton = buttons.GetChild(3).gameObject.GetComponent<Button>();

        Transform hero = heroSelectScreen.transform.GetChild(4);
        TextMeshProUGUI heroName = heroSelectScreen.transform.GetChild(6).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        Image heroSprite = hero.GetChild(1).gameObject.GetComponent<Image>();
        TextMeshProUGUI heroInfo = heroSelectScreen.transform.GetChild(8).GetChild(4).GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI heroStrength = heroSelectScreen.transform.GetChild(9).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

        GameManager.heroSelectScreenIndex = Mathf.Clamp(GameManager.heroSelectScreenIndex, 0, Mathf.Clamp(HeroManager.recruitedHeroes.Count - 1, 0, int.MaxValue));

        if (HeroManager.recruitedHeroes.Count != 0)
        {
            HeroManager.Hero _hero = HeroManager.recruitedHeroes[GameManager.heroSelectScreenIndex];
            heroName.text = _hero.heroName.name + "\nthe " + _hero.type.heroType;
            heroSprite.enabled = true;
            heroSprite.sprite = _hero.type.icon;
            heroInfo.text = _hero.heroName.description;
            heroStrength.text = "Strength: " + _hero.strength + " / Mentality: " + _hero.mentality;

            if (GameManager.currentEvent != null)
            {
                useButton.interactable = true;
                heroSprite.transform.GetChild(0).gameObject.SetActive(true);
                heroSprite.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = GetCombatModifierIcon(_hero);
            }
            else
            {
                useButton.interactable = false;
                heroSprite.transform.GetChild(0).gameObject.SetActive(false);
            }

            if (_hero.injured)
            {
                useButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Injured";
                useButton.interactable = false;
            }
            else
            {
                useButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Use";
            }
        }
        else
        {
            heroSprite.transform.GetChild(0).gameObject.SetActive(false);
            heroName.text = "No Heroes\nHired";
            heroInfo.text = "No hero selected";
            heroStrength.text = "No hero selected";
            heroSprite.enabled = false;
            useButton.interactable = false;
        }

        if (GameManager.heroSelectScreenIndex == 0)
        {
            backButton.interactable = false;
        }
        else
        {
            backButton.interactable = true;
        }

        if (GameManager.heroSelectScreenIndex == Mathf.Clamp(HeroManager.recruitedHeroes.Count - 1, 0, int.MaxValue))
        {
            nextButton.interactable = false;
        }
        else
        {
            nextButton.interactable = true;
        }
    }

    public void FinalizeEvent(string eventResultText)
    {
        eventPanel.SetActive(false);
        heroSelectScreen.SetActive(false);

        eventResultPanel.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = eventResultText;
        eventResultPanel.SetActive(true);
    }

    Sprite GetCombatModifierIcon(HeroManager.Hero hero)
    {
        Sprite combatModifierIcon;
        CharacterFaction enemyFaction = GameManager.currentEvent.enemyType;
        CharacterFaction heroFaction = hero.type.faction;

        if (heroFaction.strongAgainst == enemyFaction)
        {
            combatModifierIcon = combatModifierIcons[2];
        }
        else if (heroFaction.weakAgainst == enemyFaction)
        {
            combatModifierIcon = combatModifierIcons[0];
        }
        else
        {
            combatModifierIcon = combatModifierIcons[1];
        }
        return combatModifierIcon;
    }

    public void HireHero()
    {
        StartCoroutine(AddResources(new AResource.ResourceBundle(AResource.Type.Luxuries, -HeroManager.recruitableHeroes[heroHireScreenIndex].heroName.recruitmentCost)));
        HeroManager.recruitedHeroes.Add(HeroManager.recruitableHeroes[heroHireScreenIndex]);
        HeroManager.recruitableHeroes.Remove(HeroManager.recruitableHeroes[heroHireScreenIndex]);
        GameManager.resourceGrowth[AResource.Type.Food]--;
        UpdateHUD();
        UpdateHeroSelectScreen();
    }

    public void NextHeroHire()
    {
        heroHireScreenIndex++;
        UpdateHeroHireScreen();
    }

    public void PreviousHeroHire()
    {
        heroHireScreenIndex--;
        UpdateHeroHireScreen();
    }

    public void NextHeroSelect()
    {
        GameManager.heroSelectScreenIndex++;
        UpdateHeroSelectScreen();
    }

    public void PreviousHeroSelect()
    {
        GameManager.heroSelectScreenIndex--;
        UpdateHeroSelectScreen();
    }

    public IEnumerator SetClock(float target, float increment)
    {
        float _target = Mathf.Clamp(target + increment, 0, 1);
        float _increment = increment;
        while (clock.fillAmount < _target)
        {
            clock.fillAmount += _increment;
            _increment += increment / 5f;
            yield return new WaitForEndOfFrame();
        }
        while (clock.fillAmount > target)
        {
            clock.fillAmount -= _increment / 5f;
            yield return new WaitForEndOfFrame();
        }
        clockAnimationFinished = true;
    }

    public IEnumerator ResetClock()
    {
        while (clock.fillAmount > 0)
        {
            clock.fillAmount -= 0.05f;
            yield return new WaitForEndOfFrame();
        }
    }
}