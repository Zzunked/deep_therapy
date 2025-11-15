using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ActionDisplayer : MonoBehaviour
{
    [SerializeField] private Blast _blastPrefab;
    [SerializeField] private GameObject[] _signsPrefabs;
    [SerializeField] private GameObject _shieldPrefab;
    [SerializeField] private GameObject _tentaclePrefab;
    [SerializeField] private GameObject _crackPrefab;
    [SerializeField] private GameObject _digitPrefab;
    [SerializeField] private Transform _playerCardTransform;
    [SerializeField] private Sprite[] _digitSprites;
    [SerializeField] private float _digitSpacing = 0.47f;
    [SerializeField] private float _scaleMultiplier = 1.7f;
    [SerializeField] private float _fadeDuration = 1.5f;

    private Vector3 _crackPos = new Vector3(0.92f, -3.44f, 0);
    private Vector3 _tentaclePos = new Vector3(2.89f, -3.44f, 0);
    private List<(float x, float y)> _playerDamageNumPos = new(){ (-0.96f, -3.29f), (0.3f, -1.88f), (2.21f, -1.69f) };
    private List<(float x, float y)> _enemyDamageNumPos = new(){ (-2f, 0f), (0f, 0f), (2f, 0f) };

    public int Damage { get; set ; }
    private readonly List<GameObject> _spawnedDigits = new List<GameObject>();

    private void OnDrawGizmos()
    {
        // Gizmos.color = Color.red;
        // Gizmos.DrawSphere(_blastOffset, 0.1f);

        // Gizmos.color = Color.blue;
        // Gizmos.DrawSphere(_signOffsetLeft, 0.1f);

        // Gizmos.color = Color.blueViolet;
        // Gizmos.DrawSphere(_signOffsetRight, 0.1f);
    }


    private IEnumerator PlayAnimation(string animation, Animator animator)
    {
        animator.Play(animation);

        yield return null;

        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        Debug.Log("Playing " + animation + " animation for " + info.length + "s");

        yield return new WaitForSeconds(info.length);
    }

    public IEnumerator ShowDamageOnEnemy()
    {

        Blast blast = Instantiate(_blastPrefab);

        blast.BlastDamagePhase += ShowDamageNumberOnEnemy;
        blast.BlastSignPhase += ShowDamageSignOnEnemy;

        yield return StartCoroutine(blast.PlayAnimationEnum());
    }

    private void ShowDamageNumberOnEnemy()
    {
        int randInx = Random.Range(0, _enemyDamageNumPos.Count);
        ShowDamageNumber(_enemyDamageNumPos[randInx].x, _enemyDamageNumPos[randInx].y);
    }

    public void ShowShieldOnEnemy()
    {

    }

    private void ShowDamageSignOnEnemy()
    {
        GameObject actionAnimationPrefab = GetSignToShow();
        GameObject actionAnimationGO = Instantiate(actionAnimationPrefab);
        ActionAnimation actionAnimation = actionAnimationGO.GetComponent<ActionAnimation>();
        actionAnimation.PlayAnimation();
    }

    private GameObject GetSignToShow()
    {
        int idx = Random.Range(0, _signsPrefabs.Length);

        return _signsPrefabs[idx];
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
        crackGO.transform.position = _crackPos;
        yield return StartCoroutine(crack.PlayAnimationEnum());
    }

    private void ShowShieldOnPlayer()
    {

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
}
