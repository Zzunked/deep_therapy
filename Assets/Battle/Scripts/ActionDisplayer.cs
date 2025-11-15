using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ActionDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject _blastPrefab;
    [SerializeField] private GameObject _shieldPrefab;
    [SerializeField] private GameObject _boomPrefab;
    [SerializeField] private GameObject _skdshhhPrefab;
    [SerializeField] private GameObject _tentaclePrefab;
    [SerializeField] private GameObject _crackPrefab; 
    [SerializeField] private GameObject _digitPrefab;
    [SerializeField] private Sprite[] _digitSprites;
    [SerializeField] private float _digitSpacing = 0.47f;
    [SerializeField] private float _scaleMultiplier = 1.7f;
    [SerializeField] private float _fadeDuration = 1.5f;

    public delegate void StartBlinking();

    public StartBlinking Blink;

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

    public IEnumerator ShowDamageOnEnemy()
    {
        GameObject blastGO = Instantiate(_blastPrefab);
        Blast blast = blastGO.GetComponent<Blast>();
        blastGO.transform.position = _blastPos;

        blast.BlastDamagePhase += ShowDamageNumberOnEnemy;
        blast.BlastDamagePhase += BlinkFromDamage;
        blast.BlastSignPhase += ShowDamageSignOnEnemy;

        yield return StartCoroutine(blast.PlayAnimationEnum());
    }

    private void ShowDamageNumberOnEnemy()
    {
        int randInx = Random.Range(0, _enemyDamageNumPos.Count);
        ShowDamageNumber(_enemyDamageNumPos[randInx].x, _enemyDamageNumPos[randInx].y);
    }

    public IEnumerator ShowShieldOnEnemy()
    {
        GameObject shieldGO = Instantiate(_shieldPrefab);
        Shield shield = shieldGO.GetComponent<Shield>();
        shieldGO.transform.position = _enemyShieldPos;

        yield return StartCoroutine(shield.PlayAnimationEnum());
    }

    private void ShowDamageSignOnEnemy()
    {
        int randIdx = Random.Range(0, _signPrefabs.Count);

        GameObject actionAnimationPrefab = _signPrefabs[randIdx].prefab;
        Vector3 pos = _signPrefabs[randIdx].pos;
        float rotZ = _signPrefabs[randIdx].rotZ;

        GameObject actionAnimationGO = Instantiate(actionAnimationPrefab);
        ActionAnimation actionAnimation = actionAnimationGO.GetComponent<ActionAnimation>();

        actionAnimationGO.transform.position = pos;
        actionAnimationGO.transform.rotation = Quaternion.Euler(0, 0, rotZ);
        actionAnimation.PlayAnimation();
    }

    public IEnumerator ShowDamageOnPlayer()
    {
        GameObject tentacleGO = Instantiate(_tentaclePrefab);
        Tentacle tentacle = tentacleGO.GetComponent<Tentacle>();

        tentacleGO.transform.position = _tentaclePos;
        // tentacle.TentacleCrackPhase += ShowCrackOnPlayer;
        tentacle.TentacleDamagePhase += ShowDamageNumberOnPlayer;

        yield return StartCoroutine(tentacle.PlayAnimationEnum());
        yield return StartCoroutine(ShowCrackOnPlayer());
    }

    private IEnumerator ShowCrackOnPlayer()
    {
        GameObject crackGO = Instantiate(_crackPrefab);
        Crack crack = crackGO.GetComponent<Crack>();
        Debug.Log("AAAAAAAAAAAAAAAAAAA");
        crackGO.transform.position = _crackPos;
        
        yield return StartCoroutine(crack.PlayAnimationEnum());
    }

    public IEnumerator ShowShieldOnPlayer()
    {
        GameObject shieldGO = Instantiate(_shieldPrefab);
        Shield shield = shieldGO.GetComponent<Shield>();
        shieldGO.transform.position = _playerShieldPos;

        yield return StartCoroutine(shield.PlayAnimationEnum());
    }
    
    private void ShowDamageNumberOnPlayer()
    {
        int randIdx = Random.Range(0, _playerDamageNumPos.Count);
        ShowDamageNumber(_playerDamageNumPos[randIdx].x, _playerDamageNumPos[randIdx].y);
    }

    private void ShowDamageNumber(float xPos, float yPos)
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
        StartCoroutine(ScaleAndFade());
    }

    private void DestroyDigits()
    {
        foreach (var digit in _spawnedDigits)
            Destroy(digit);
    }

    public IEnumerator ScaleAndFade()
    {
        if (_spawnedDigits == null || _spawnedDigits.Count == 0)
            yield break;

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

            yield return null;
        }

        // Clean up digits after fade
        DestroyDigits();
    }

    private void BlinkFromDamage()
    {
        Blink?.Invoke();
    }
}
