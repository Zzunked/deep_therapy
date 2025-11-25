using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


public class ActionDisplayer : MonoBehaviour
{
    [Header("Animation objects Settings")]
    [SerializeField] private GameObject _youDeadPrefab;
    [SerializeField] private SpriteRenderer _youDeadBackgroundRenderer;
    [SerializeField] private GameObject _blastPrefab;
    [SerializeField] private GameObject _shieldPrefab;
    [SerializeField] private GameObject _boomPrefab;
    [SerializeField] private GameObject _skdshhhPrefab;
    [SerializeField] private GameObject _tentaclePrefab;
    [SerializeField] private GameObject _crackPrefab;
    [SerializeField] private GameObject _digitPrefab;
    [SerializeField] private Sprite[] _digitSprites;

    [Header("Damage digits Settings")]
    [SerializeField] private float _digitSpacing = 0.47f;
    [SerializeField] private float _scaleMultiplier = 1.7f;
    [SerializeField] private float _fadeDuration = 1f;

    // Blink config
    [Header("Blink Settings")]
    [SerializeField] private int _enemyBlinkCount = 8;
    [SerializeField] private float _enemyBlinkingSpeed = 0.1f;
    [SerializeField] private SpriteRenderer _enemyRenderer;


    // Player card
    [Header("Players card Settings")]
    [SerializeField] private Transform _cardTransform;
    [SerializeField] private float _cardSpeed = 10f;
    private Vector2 _centerPosition;
    private Vector2 _rightCornerPosition;

    public int Damage { get; set ; }
    private readonly List<GameObject> _spawnedDigits = new List<GameObject>();

    // Animations positions
    // Crack
    private Vector3 _crackPos = new Vector3(0.92f, -3.44f, 0);

    // Tentacle
    private Vector3 _tentaclePos = new Vector3(2.89f, -3.44f, 0);

    // Shield
    private Vector3 _enemyShieldPos = new Vector3(0f, 2.14f, 0);
    private Vector3 _playerShieldPos = new Vector3(0f, -3.47f, 0);

    // Blast
    private Vector3 _blastBodyPos = new Vector3(-0.04f, 0.84f, 0);
    private Vector3 _blastHeadPos = new Vector3(-0.09f, 2.62f, 0);
    private Vector3 _blastEyesPos = new Vector3(-1.11f, 2.62f, 0);

    // YouDead
    private Vector3 _youDeadPos = new Vector3(0, 0, 0);

    // Skdshhh sign
    private Vector3 _skdshhhRightPos = new Vector3(2.91f, 2.45f, 0);
    private float _skdshhhRightRotZ = 11f;
    private Vector3 _skdshhhLeftPos = new Vector3(-3.54f, 2.29f, 0);
    private float _skdshhhLeftRotZ = 46.303f;

    // Boom sign
    private Vector3 _boomRightPos = new Vector3(2.8f, 1.97f, 0);
    private float _boomRightRotZ = -20.362f;
    private Vector3 _boomLeftPos = new Vector3(-3.1f, 1.75f, 0);
    private float _boomLeftRotZ = 33.5f;

    // Damage numbers
    private List<(float x, float y)> _playerDamageNumPos = new(){ (-0.96f, -3.29f), (0.3f, -1.88f), (2.21f, -1.69f) };
    private List<(float x, float y)> _enemyDamageNumPos = new() { (-2f, 0f), (0f, 0f), (2f, 0f) };
    private List<SignPrefabConfig> _signPrefabs;

    private void Awake()
    {
        List<SignPrefabConfig> signPrefabs = new()
        {
            new SignPrefabConfig(_boomPrefab, _boomRightPos, _boomRightRotZ),
            new SignPrefabConfig(_boomPrefab, _boomLeftPos, _boomLeftRotZ),
            new SignPrefabConfig(_skdshhhPrefab, _skdshhhRightPos, _skdshhhRightRotZ),
            new SignPrefabConfig(_skdshhhPrefab, _skdshhhLeftPos, _skdshhhLeftRotZ),
        };
        _signPrefabs = signPrefabs;

        _rightCornerPosition = _cardTransform.position;
        _centerPosition = new Vector2(0, _cardTransform.position.y);
    }

    public async Task ShowDamageOnEnemy(PlayersTarget target)
    {
        var animationTasks = new Task[4];
        GameObject blastGO = Instantiate(_blastPrefab);
        var blast = blastGO.GetComponent<Blast>();
        blast.DamageTcs = new TaskCompletionSource<bool>();
        blast.SignTcs = new TaskCompletionSource<bool>();

        switch (target)
        {
            case PlayersTarget.Eyes:
                blastGO.transform.position = _blastEyesPos;
                break;
            case PlayersTarget.Head:
                blastGO.transform.position = _blastHeadPos;
                break;
            case PlayersTarget.Body:
                blastGO.transform.position = _blastBodyPos;
                break;
            default:
                throw new ArgumentException("Unexpected target", nameof(target));
        }

        // Start blast animation
        animationTasks[0] = blast.PlayAnimation();

        // Show damage done to the enemy and start enemy to blink
        // as soon as damage phase trigger is fired
        await blast.DamageTcs.Task;
        animationTasks[1] = ShowDamageNumberOnEnemy();
        animationTasks[2] = BlinkEnemy();
        
        // Show damage sign on the enemy as soon as sign trigger is fired
        await blast.SignTcs.Task;
        animationTasks[3] = ShowDamageSignOnEnemy();

        // Wait for all animations to complete
        await Task.WhenAll(animationTasks);
    }

    private async Task ShowDamageNumberOnEnemy()
    {
        int randInx = UnityEngine.Random.Range(0, _enemyDamageNumPos.Count);
        await ShowDamageNumber(_enemyDamageNumPos[randInx].x, _enemyDamageNumPos[randInx].y);
    }

    public async Task ShowShieldOnEnemy()
    {
        GameObject shieldGO = Instantiate(_shieldPrefab);
        Shield shield = shieldGO.GetComponent<Shield>();
        shieldGO.transform.position = _enemyShieldPos;

        await shield.PlayAnimation();
    }

    private async Task ShowDamageSignOnEnemy()
    {
        int randIdx = UnityEngine.Random.Range(0, _signPrefabs.Count);

        GameObject actionAnimationPrefab = _signPrefabs[randIdx].Prefab;
        Vector3 pos = _signPrefabs[randIdx].Pos;
        float rotZ = _signPrefabs[randIdx].RotZ;

        GameObject actionAnimationGO = Instantiate(actionAnimationPrefab);
        ActionAnimation actionAnimation = actionAnimationGO.GetComponent<ActionAnimation>();

        actionAnimationGO.transform.position = pos;
        actionAnimationGO.transform.rotation = Quaternion.Euler(0, 0, rotZ);

        await actionAnimation.PlayAnimation();
    }

    public async Task ShowDamageOnPlayer()
    {
        var animationTasks = new Task[3];
        GameObject tentacleGO = Instantiate(_tentaclePrefab);
        var tentacle = tentacleGO.GetComponent<Tentacle>();
        tentacleGO.transform.position = _tentaclePos;
        tentacle.DamageTcs = new TaskCompletionSource<bool>();
        tentacle.CrackTcs = new TaskCompletionSource<bool>();

        // Start tentacle animation
        animationTasks[0] =  tentacle.PlayAnimation();

        // Show damage done to player as soon as damage trigger is fired
        await tentacle.DamageTcs.Task;
        animationTasks[1] = ShowDamageNumberOnPlayer();

        // Show crack animation as soon as crack trigger is fired
        await tentacle.CrackTcs.Task;
        animationTasks[2] = ShowCrackOnPlayer();

        // Wait for all animations to complete
        await Task.WhenAll(animationTasks);
    }

    private async Task ShowCrackOnPlayer()
    {
        GameObject crackGO = Instantiate(_crackPrefab);
        Crack crack = crackGO.GetComponent<Crack>();
        crackGO.transform.position = _crackPos;
        
        await crack.PlayAnimation();
    }

    public async Task ShowShieldOnPlayer()
    {
        GameObject shieldGO = Instantiate(_shieldPrefab);
        Shield shield = shieldGO.GetComponent<Shield>();
        shieldGO.transform.position = _playerShieldPos;

        await shield.PlayAnimation();
    }
    
    private async Task ShowDamageNumberOnPlayer()
    {
        int randIdx = UnityEngine.Random.Range(0, _playerDamageNumPos.Count);
        await ShowDamageNumber(_playerDamageNumPos[randIdx].x, _playerDamageNumPos[randIdx].y);
    }

    private async Task ShowDamageNumber(float xPos, float yPos)
    {
        // Clean up old digits
        DestroyDigits();
        _spawnedDigits.Clear();

        // Convert number to string to extract digits
        char[] digits = Damage.ToString().ToCharArray();

        float startX = -((digits.Length - 1) * _digitSpacing) / 2f; // Center it
        for (int i = 0; i < digits.Length; i++)
        {
            int digitIndex = digits[i] - '0'; // Convert char to int (e.g. '3' → 3)
            Sprite sprite = _digitSprites[digitIndex];

            GameObject digitGO = Instantiate(_digitPrefab);
            digitGO.GetComponent<SpriteRenderer>().sprite = sprite;

            digitGO.transform.localPosition = new Vector2(xPos + startX + i * _digitSpacing, yPos);

            _spawnedDigits.Add(digitGO);
        }
        await ScaleAndFade();
    }

    public async Task ShowWinScreen()
    {
        Debug.Log("YOU WIN");
        await Awaitable.WaitForSecondsAsync(1);
    }

    public async Task ShowPlayerDeadScreen()
    {
        GameObject youDeadGO = Instantiate(_youDeadPrefab);
        YouDead youDead = youDeadGO.GetComponent<YouDead>();
        youDeadGO.transform.position = _youDeadPos;
        _youDeadBackgroundRenderer.sortingOrder = 4;

        await BackgroundFadeIn();

        await youDead.PlayAnimation();

        await Awaitable.WaitForSecondsAsync(5);

        Destroy(youDeadGO);
        _youDeadBackgroundRenderer.sortingOrder = -1;
    }

    private async Task BackgroundFadeIn()
    {
        float fadeDuration = 2f;
        float elapsed = 0f;
        Color startColor = Color.black;
        Color endColor = _youDeadBackgroundRenderer.color; // original sprite color

        // Temporarily set the sprite color to black
        _youDeadBackgroundRenderer.color = startColor;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            _youDeadBackgroundRenderer.color = Color.Lerp(startColor, endColor, t);
            await Task.Yield();
        }

        // Ensure exact final color
        _youDeadBackgroundRenderer.color = endColor;
    }

    private void DestroyDigits()
    {
        foreach (var digit in _spawnedDigits)
            Destroy(digit);
    }

    public async Task ScaleAndFade()
    {
        int count = _spawnedDigits.Count;

        // Cache transforms, renderers, and original positions
        List<Transform> digits = new List<Transform>(count);
        List<SpriteRenderer> renderers = new List<SpriteRenderer>(count);
        List<Vector3> initialPositions = new List<Vector3>(count);

        for (int i = 0; i < count; i++)
        {
            var t = _spawnedDigits[i].transform;
            digits.Add(t);
            renderers.Add(_spawnedDigits[i].GetComponent<SpriteRenderer>());
            initialPositions.Add(t.localPosition);
        }

        float elapsed = 0f;

        // Store initial and target scales
        float initialScale = digits[0].localScale.x;
        float targetScale = initialScale * _scaleMultiplier;

        // Compute the center X of the number
        float centerX = 0f;
        foreach (var pos in initialPositions)
            centerX += pos.x;
        centerX /= count;

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _fadeDuration);

            // Scale digits
            float currentScale = Mathf.Lerp(initialScale, targetScale, t);
            Vector3 scaleVec = Vector3.one * currentScale;
            foreach (var d in digits)
                d.localScale = scaleVec;

            // Fade out (start fading halfway)
            float alpha = 1f - Mathf.Clamp01((t - 0.5f) * 2f);
            foreach (var r in renderers)
            {
                if (r == null) continue;
                Color c = r.color;
                c.a = alpha;
                r.color = c;
            }

            // Dynamic spacing: scale distance from center
            float spacingFactor = 1f + (currentScale - initialScale) / initialScale;
            for (int i = 0; i < count; i++)
            {
                float offsetFromCenter = initialPositions[i].x - centerX;
                float newX = centerX + offsetFromCenter * spacingFactor;
                digits[i].localPosition = new Vector3(newX, initialPositions[i].y, initialPositions[i].z);
            }

            await Task.Yield();
        }

        // Clean up digits after fade
        DestroyDigits();
    }

    private async Task BlinkEnemy()
    {
        await BlinkAlpha(_enemyRenderer, _enemyBlinkCount, _enemyBlinkingSpeed);
    }

    private async Task BlinkAlpha(SpriteRenderer renderer, int blinkCount, float blinkingSpeed)
    {
        bool visible = true;

        for(int i = 0; i < blinkCount; i++)
        {
            Color c = renderer.color;
            c.a = visible ? 0f : 1f;
            renderer.color = c;

            visible = !visible;

            await Awaitable.WaitForSecondsAsync(blinkingSpeed);
        }

        // restore alpha
        Color final = renderer.color;
        final.a = 1f;
        renderer.color = final;
    }

    public async Task MoveCardToCenter()
    {
        Debug.Log("Card is moving towads center");

        await MoveCard(_centerPosition);

        Debug.Log("Card is in the center");
    }

    public async Task MoveCardToRight()
    {
        Debug.Log("Card is moving to right corner");

        await MoveCard(_rightCornerPosition);

        Debug.Log("Card is in the right corner");
    }

    private async Task MoveCard(Vector2 targetPosition)
    {
        while (Vector2.Distance(_cardTransform.position, targetPosition) > 0.05f)
        {
            _cardTransform.position = Vector2.MoveTowards(_cardTransform.position, targetPosition, _cardSpeed * Time.deltaTime);

            await Task.Yield();
        }

        // Snap to final position to avoid small offset
        _cardTransform.position = targetPosition;
    }

    public void SetCardDefaultPosition()
    {
        _cardTransform.position = _rightCornerPosition;
    }
}


struct SignPrefabConfig
{
    public GameObject Prefab;
    public Vector3 Pos;
    public float RotZ;

    public SignPrefabConfig(GameObject prefab, Vector3 pos, float rotZ)
    {
        this.Prefab = prefab;
        this.Pos = pos;
        this.RotZ = rotZ;
    }
}