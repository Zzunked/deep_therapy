using UnityEngine;
using System.Collections;
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
    [SerializeField] private int _enemyBlinkCount = 11;
    [SerializeField] private float _enemyBlinkingSpeed = 0.1f;
    [SerializeField] private SpriteRenderer _enemyRenderer;

    public int Damage { get; set ; }
    private readonly List<GameObject> _spawnedDigits = new List<GameObject>();

    // Animations positions
    // Crack
    private Vector3 _crackPos = new Vector3(0.92f, -3.44f, 0);

    // Tentacle
    private Vector3 _tentaclePos = new Vector3(2.89f, -3.44f, 0);

    // Shield
    private Vector3 _enemyShieldPos = new Vector3(0f, 1.65f, 0);
    private Vector3 _playerShieldPos = new Vector3(0f, -3.47f, 0);

    // Blast
    private Vector3 _blastPos = new Vector3(-0.04f, 1.54f, 0);

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
    private List<(GameObject prefab, Vector3 pos, float rotZ)> _signPrefabs;


    private void Awake()
    {
        List<(GameObject prefab, Vector3 pos, float rotZ)> signPrefabs = new()
        {
            (_boomPrefab, _boomRightPos, _boomRightRotZ),
            (_boomPrefab, _boomLeftPos, _boomLeftRotZ),
            (_skdshhhPrefab, _skdshhhRightPos, _skdshhhRightRotZ),
            (_skdshhhPrefab, _skdshhhLeftPos, _skdshhhLeftRotZ)
        };
        _signPrefabs = signPrefabs;
    }

    public async Task ShowDamageOnEnemy()
    {
        var animationTasks = new Task[4];
        GameObject blastGO = Instantiate(_blastPrefab);
        var blast = blastGO.GetComponent<Blast>();
        blastGO.transform.position = _blastPos;
        blast.DamageTcs = new TaskCompletionSource<bool>();
        blast.SignTcs = new TaskCompletionSource<bool>();

        // Start blast animation
        animationTasks[0] = blast.PlayAnimationAndWait();

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
        int randInx = Random.Range(0, _enemyDamageNumPos.Count);
        await ShowDamageNumber(_enemyDamageNumPos[randInx].x, _enemyDamageNumPos[randInx].y);
    }

    public async Task ShowShieldOnEnemy()
    {
        GameObject shieldGO = Instantiate(_shieldPrefab);
        Shield shield = shieldGO.GetComponent<Shield>();
        shieldGO.transform.position = _enemyShieldPos;

        await shield.PlayAnimationAndWait();
    }

    private async Task ShowDamageSignOnEnemy()
    {
        int randIdx = Random.Range(0, _signPrefabs.Count);

        GameObject actionAnimationPrefab = _signPrefabs[randIdx].prefab;
        Vector3 pos = _signPrefabs[randIdx].pos;
        float rotZ = _signPrefabs[randIdx].rotZ;

        GameObject actionAnimationGO = Instantiate(actionAnimationPrefab);
        ActionAnimation actionAnimation = actionAnimationGO.GetComponent<ActionAnimation>();

        actionAnimationGO.transform.position = pos;
        actionAnimationGO.transform.rotation = Quaternion.Euler(0, 0, rotZ);

        await actionAnimation.PlayAnimationAndWait();
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
        animationTasks[0] =  tentacle.PlayAnimationAndWait();

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
        
        await crack.PlayAnimationAndWait();
    }

    public IEnumerator ShowShieldOnPlayer()
    {
        GameObject shieldGO = Instantiate(_shieldPrefab);
        Shield shield = shieldGO.GetComponent<Shield>();
        shieldGO.transform.position = _playerShieldPos;

        yield return StartCoroutine(shield.PlayAnimationEnum());
    }
    
    private async Task ShowDamageNumberOnPlayer()
    {
        int randIdx = Random.Range(0, _playerDamageNumPos.Count);
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

        await youDead.PlayAnimationAndWait();

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
}
