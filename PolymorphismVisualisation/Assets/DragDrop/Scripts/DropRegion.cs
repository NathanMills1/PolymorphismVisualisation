
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropRegion : MonoBehaviour, IDropHandler {

    public Image screenImage;
    public Image bottomScreenImage;
    public Image objectImage;
    public Image screenShadow;
    public GameObject topFade;
    public GameObject bottomFade;
    public GameObject parentTypeError;
    public QuestionManager questionManager;
    public AudioSource thudSound;
    public AudioSource paperSound;
    public TutorialManager tutorialManager;

    public Entity screenEntity { get; private set; }
    public Entity objectEntity { get; private set; }

    private TextMeshProUGUI[] objectMethods;
    private List<TextMeshProUGUI> screenNames = new List<TextMeshProUGUI>();
    private List<TextMeshProUGUI> fieldNames = new List<TextMeshProUGUI>();
    private Image[] shades;
    private bool faded = false;

    private Vector2 screenDropFromOffset = new Vector3(-40, 75);
    private Vector3 screenBasePosition;
    private Vector3 objectBasePosition;
    private Coroutine sheetSlideInRoutine;
    private Coroutine screenSheetErrorRoutine;
    private Coroutine fadeRoutine;

    void Start()
    {
        objectMethods = objectImage.gameObject.transform.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI text in objectMethods)
        {
            text.gameObject.SetActive(false);
        }
        TextMeshProUGUI[] labels = screenImage.gameObject.transform.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI label in labels)
        {
            if (label.gameObject.name.Contains("Field"))
            {
                fieldNames.Add(label);
            }
            else
            {
                screenNames.Add(label);
            }
            label.gameObject.SetActive(false);
        }
        shades = screenImage.gameObject.GetComponentsInChildren<Image>();

        screenBasePosition = screenImage.gameObject.GetComponent<RectTransform>().localPosition;
        objectBasePosition = objectImage.gameObject.GetComponent<RectTransform>().localPosition;
    }

    public void OnDrop(PointerEventData eventData) {
        if (eventData.pointerDrag != null) {
            if (eventData.pointerDrag.tag.Equals("Screen"))
            {
                placeScreen(eventData.pointerDrag.GetComponent<EntityRepresentation>().getRepresentation());
            }
            else if (eventData.pointerDrag.tag.Equals("Object"))
            {
                placeObject(eventData.pointerDrag.GetComponent<EntityRepresentation>().getRepresentation());
            }

            questionManager.updateQuestion();
        }
    }

    public void placeScreen(Entity screenEntity)
    {
        if(objectEntity == null)
        {
            this.screenEntity = screenEntity;
            this.screenImage.sprite = screenEntity.screenImage;
            this.screenImage.gameObject.SetActive(true);

            this.bottomScreenImage.sprite = screenEntity.screenImage;
            this.bottomScreenImage.gameObject.SetActive(true);
            
            this.screenShadow.sprite = screenEntity.shadow;
            this.screenShadow.gameObject.SetActive(true);
            updateScreenLabels(screenEntity);
            StartCoroutine(dropScreen());
            setFadePositions();
        }
        else //TODO have error appear stating cannot change screen type while object is placed
        {

        }
        
        
    }

    public void placeObject(Entity objectEntity)
    {
        parentTypeError.SetActive(false);

        if (screenEntity != null)
        {
            if(sheetSlideInRoutine != null)
            {
                StopCoroutine(sheetSlideInRoutine);
            }
            resetScreenSheetPositions();

            this.objectEntity = objectEntity;
            this.objectImage.sprite = objectEntity.objectImage;
            this.objectImage.gameObject.SetActive(true);
            objectEntity.updateFields(this.objectMethods, true);
            sheetSlideInRoutine = StartCoroutine(slideObjectIn());

        }
        else //TODO make message detailing to place screen first
        {

        }
    }

    public void adjustForActivitySection(int activitySection)
    {
        switch (activitySection)
        {
            case 1:
                adjustGameObjectPositions(-200);
                break;
            case 2: 
                adjustGameObjectPositions(-100);
                break;
            default:
                break;
        }
    }


    private void adjustGameObjectPositions(int yOffset)
    {
        Vector3 offset = new Vector3(0, yOffset, 0);
        screenBasePosition += offset;
        objectBasePosition += offset;

        GameObject[] dropComponents = new GameObject[] { screenImage.gameObject, bottomScreenImage.gameObject, objectImage.gameObject, screenShadow.gameObject};
        foreach(GameObject component in dropComponents)
        {
            component.GetComponent<RectTransform>().localPosition += offset;
        }
    }

    //Used to stop current animations, at set sheets back to initial placements
    private void resetScreenSheetPositions()
    {
        if(screenSheetErrorRoutine != null)
        {
            StopCoroutine(screenSheetErrorRoutine);
        }
        if(sheetSlideInRoutine != null)
        {
            StopCoroutine(sheetSlideInRoutine);
        }
        screenImage.rectTransform.localPosition = screenBasePosition;
        objectImage.rectTransform.localPosition = objectBasePosition;
        bottomScreenImage.rectTransform.localPosition = objectBasePosition + new Vector3(1, -1, 0);

    }

    public void checkForErrors()
    {
        if(objectEntity != null)
        {
            //set size of method error box to match screen size

            bool screenIsParent = objectEntity.determineIfChildOf(screenEntity);
            if (!screenIsParent && screenEntity.generation > objectEntity.generation)
            {
                parentTypeError.GetComponent<RectTransform>().sizeDelta = new Vector2(410, (screenEntity.height - objectEntity.height - 8));
                parentTypeError.transform.localPosition = new Vector3(0, objectImage.rectTransform.localPosition.y - (objectEntity.height), 0);
            }
            parentTypeError.SetActive(!screenIsParent && screenEntity.generation > objectEntity.generation);

            if (!screenIsParent && !screenEntity.determineIfChildOf(objectEntity))
            {
                //TODO error showing that they don't match up
            }
        }
        
    }

    private void updateScreenLabels(Entity screen)
    {
        for(int i = 0; i < 3; i++)
        {
            screenNames[i].gameObject.SetActive(screen.generation > i);
            fieldNames[i].gameObject.SetActive(screen.generation > i);
            shades[i].gameObject.SetActive(screen.generation > i);

            Entity currentScreen = screen;
            while (currentScreen.generation > i+1)
            {
                currentScreen = currentScreen.parent;
            }
            screenNames[i].text = currentScreen.identity.name;
            fieldNames[i].text = currentScreen.identity.field;
        }
        
    }

    public void clearSelected()
    {
        this.screenImage.gameObject.SetActive(false);
        this.bottomScreenImage.gameObject.SetActive(false);
        this.objectImage.gameObject.SetActive(false);
        this.parentTypeError.SetActive(false);
        this.screenShadow.gameObject.SetActive(false);
        if(fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        Color colour = this.topFade.GetComponent<Image>().color;
        Color invisColour = new Color(colour.r, colour.g, colour.b, 0);
        this.topFade.GetComponent<Image>().color = invisColour;
        this.bottomFade.GetComponent<Image>().color = invisColour;

        this.screenEntity = null;
        this.objectEntity = null;
        this.faded = false;

        foreach (TextMeshProUGUI method in objectMethods)
        {
            method.gameObject.SetActive(false);
        }

        questionManager.updateQuestion();
    }

    public IEnumerator dropScreen()
    {
        const float DURATION = 0.4f;
        const float STEP_SIZE = 0.05f;
        Vector2 screenEndPosition = screenBasePosition;
        float currentTime = 0;

        GameObject screen = screenImage.gameObject;
        GameObject bottomScreen = bottomScreenImage.gameObject;
        Vector2 screenStartPosition = (Vector2)screenBasePosition + screenDropFromOffset;

        while(currentTime <= DURATION)
        {
            float alpha = currentTime / DURATION;
            screen.transform.localPosition = (1 - alpha) * screenStartPosition + alpha * screenEndPosition;
            bottomScreen.transform.localPosition = (1 - alpha) * screenStartPosition + alpha * screenEndPosition - new Vector2(-9,9);
            screenShadow.color = new Color(55, 55, 55, 0.6f * alpha);

            currentTime += STEP_SIZE;

            yield return new WaitForSeconds(STEP_SIZE);
        }

        screen.transform.localPosition = screenEndPosition;
        bottomScreen.transform.localPosition = screenEndPosition - new Vector2(-9, 9);
        if (!GameManager.muted)
        {
            thudSound.Play();
        }
        
        questionManager.screenPlaced(screenEntity);

        if(GameManager.activeActivity == 1)
        {
            tutorialManager.actionMade(6);
        } else if(GameManager.activeActivity == 2)
        {
            if (screenEntity.generation == 2)
            {
                tutorialManager.actionMade(3);
            }
        }
    }

    public IEnumerator slideObjectIn()
    {
        const float DURATION = 0.6f;
        const float STEP_SIZE = 0.05f;
        Vector2 objectEndPosition = objectBasePosition;
        Vector2 objectStartPosition = objectBasePosition + new Vector3(400, 0, 0);
        float currentTime = 0;

        GameObject page = objectImage.gameObject;

        if (!GameManager.muted)
        {
            paperSound.Play();
        }
        while (currentTime <= DURATION)
        {
            float alpha = currentTime / DURATION;
            page.transform.localPosition = (1 - alpha) * objectStartPosition + alpha * objectEndPosition;

            currentTime += STEP_SIZE;

            yield return new WaitForSeconds(STEP_SIZE);
        }
        paperSound.Stop();

        page.transform.localPosition = objectEndPosition;
        checkForErrors();
        questionManager.objectPlaced(objectEntity);

        if (!faded)
        {
            fadeRoutine = StartCoroutine(fadeIn());
            faded = true;
        }

        if (!objectEntity.determineIfChildOf(screenEntity) && !screenEntity.determineIfChildOf(objectEntity))
        {
            screenSheetErrorRoutine = StartCoroutine(ejectBadSheet());
        }

        if (GameManager.activeActivity == 1)
        {
            tutorialManager.actionMade(7);
        } else if(GameManager.activeActivity == 2)
        {
            if(screenEntity.determineIfChildOf(objectEntity) && objectEntity.generation == 1)
            {
                tutorialManager.actionMade(4);
            } else if(objectEntity.determineIfChildOf(screenEntity) && objectEntity.generation == 2 && screenEntity.generation == 1)
            {
                tutorialManager.actionMade(6);
            }
        }


    }

    private void setFadePositions()
    {
        float topFadeHeight = System.Math.Abs(screenBasePosition.y) + screenEntity.height;
        float bottomFadeYPos = 0 - topFadeHeight;
        float bottomFadeHeight = 1000 - topFadeHeight;

        Vector2 topSize = topFade.gameObject.GetComponent<RectTransform>().sizeDelta;
        topFade.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(topSize.x, topFadeHeight);
        bottomFade.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector3(topSize.x, bottomFadeHeight, 0);
        bottomFade.transform.localPosition = new Vector3(0, bottomFadeYPos, 0);
    }

    private IEnumerator fadeIn()
    {

        Image topFadeImage = topFade.GetComponent<Image>();
        Image bottomFadeImage = bottomFade.GetComponent<Image>();
        Color finalColour = topFadeImage.color;
        Image[] fades = new Image[] { topFadeImage, bottomFadeImage };

        const float DURATION = 0.6f;
        const float STEP_SIZE = 0.05f;
        const float finalTransparency = 0.9f;
        float currentTime = 0;

        while (currentTime <= DURATION)
        {
            float currentTransparency = finalTransparency * (currentTime / DURATION);
            foreach (Image fade in fades)
            {
                fade.color = new Color(finalColour.r, finalColour.g, finalColour.b, currentTransparency);
            }

            currentTime += STEP_SIZE;

            yield return new WaitForSeconds(STEP_SIZE);
        }

    }

    private IEnumerator ejectBadSheet()
    {
        const float INITIAL_WAIT_PERIOD = 1.3f;
        const float MAX_OFFSET = 10f;
        const float NUMBER_OF_SHAKES = 5f;
        const float SHAKE_SPEED = 5f;

        yield return new WaitForSeconds(INITIAL_WAIT_PERIOD);

        int direction = 1;
        float shakeCount = 0;
        float currentOffset = 0;

        RectTransform screenTransform = screenImage.rectTransform;
        RectTransform objectTransform = objectImage.rectTransform;
        RectTransform bottomTransform = bottomScreenImage.rectTransform;

        while (shakeCount < NUMBER_OF_SHAKES)
        {
            Vector3 movementVector = new Vector3(SHAKE_SPEED * direction, 0, 0);
            screenTransform.localPosition += movementVector;
            objectTransform.localPosition += movementVector;
            bottomTransform.localPosition += movementVector;

            currentOffset += SHAKE_SPEED * direction;

            if(currentOffset * direction > MAX_OFFSET)
            {
                direction *= -1;
                shakeCount++;
            }

            yield return new WaitForSeconds(0.015f);
        }

        screenTransform.localPosition = screenBasePosition;
        objectTransform.localPosition = objectBasePosition;
        bottomTransform.localPosition = objectBasePosition + new Vector3(1, -1, 0);

        //TODO start unfade coroutine


        const float MAX_SHEET_MOVEMENT = 450f;
        const float SHEET_MOVE_TIME = 1f;
        const float DIVISIONS = 20f;

        float sheetMovement = 0;

        while(sheetMovement< MAX_SHEET_MOVEMENT)
        {
            float moveDistance = MAX_SHEET_MOVEMENT / DIVISIONS;
            objectTransform.localPosition += new Vector3(moveDistance, 0, 0);

            sheetMovement += moveDistance;
            yield return new WaitForSeconds(SHEET_MOVE_TIME/ DIVISIONS);
        }

        if (GameManager.activeActivity == 1)
        {
            tutorialManager.actionMade(8);
        }

    }



}
