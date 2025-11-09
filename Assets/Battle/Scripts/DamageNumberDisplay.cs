using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class DamageNumberDisplay : MonoBehaviour
{
    [Header("Digit Sprites 0–9 in order")]
    [SerializeField] private Sprite[] _digitSprites;

    [Header("Digit Prefab (with SpriteRenderer)")]
    [SerializeField] private GameObject _digitPrefab;

    [Header("Settings")]
    [SerializeField] private float _digitSpacing = 0.5f;
    // [SerializeField] private float _digitScale = 0.2f;
    [SerializeField] private float _startScale = 0.2f;
    [SerializeField] private float _endScale = 0.35f;
    [SerializeField] private float _fadeDuration = 1f;
    // private int _damageValue = 0;

    // public int DamageValue
    // {
    //     get { return _damageValue; }
    //     set { _damageValue = value; }
    // } 

    private readonly List<GameObject> _spawnedDigits = new List<GameObject>();

    // private void OnEnable()
    // {
    //     BattleEyesEnemy.OnBlastDamagePhase += ShowNumber;
    // }

    // private void OnDisable()
    // {
    //     BattleEyesEnemy.OnBlastDamagePhase -= ShowNumber;
    // }

    public void ShowNumber(int damage)
    {
        // Clean up old digits
        DestroyDigits();
        _spawnedDigits.Clear();

        // Convert number to string to extract digits
        char[] digits = damage.ToString().ToCharArray();

        float startX = -((digits.Length - 1) * _digitSpacing) / 2f; // Center it
        for (int i = 0; i < digits.Length; i++)
        {
            int digitIndex = digits[i] - '0'; // Convert char to int (e.g. '3' → 3)
            Sprite sprite = _digitSprites[digitIndex];

            GameObject digitGO = Instantiate(_digitPrefab, transform);
            digitGO.GetComponent<SpriteRenderer>().sprite = sprite;

            Vector3 pos = transform.position + new Vector3(startX + i * _digitSpacing, 0f, 0f);
            digitGO.transform.position = pos;

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
        float scaleCoeff = 1.4f;

        // Cache the original local positions of each digit so we can re-space them dynamically
        List<Transform> digits = new List<Transform>();
        foreach (Transform child in transform)
            digits.Add(child);

        float elapsed = 0f;
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _fadeDuration);

            // Scale up
            float currentScale = Mathf.Lerp(_startScale, _endScale, t);
            transform.localScale = Vector3.one * currentScale;

            // Fade out (start fading halfway through)
            float alpha = 1f - Mathf.Clamp01((t - 0.5f) * 2f);

            foreach (var r in renderers)
            {
                Color c = r.color;
                c.a = alpha;
                r.color = c;
            }

            // Adjust spacing dynamically based on scale
            float spacing = scaleCoeff * currentScale / _endScale; // keeps digits apart proportionally
            float startX = -((digits.Count - 1) * spacing) / 2f;

            for (int i = 0; i < digits.Count; i++)
            {
                digits[i].localPosition = new Vector3(startX + i * spacing, 0f, 0f);
            }

            yield return null;
        }

        // Destroy after fade
        DestroyDigits();
    }
}
