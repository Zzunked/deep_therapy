using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;


public class ActionDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject _blastPrefab;
    [SerializeField] private GameObject[] _signsPrefabs;
    [SerializeField] private GameObject _shieldPrefab;
    [SerializeField] private GameObject _tentaclePrefab;
    [SerializeField] private GameObject _crackPrefab;
    [SerializeField] private GameObject _digitPrefab;
    [SerializeField] private Transform _playerCardTransform;
    [SerializeField] private Sprite[] _digitSprites;
    [SerializeField] private float _digitSpacing = 0.5f;
    [SerializeField] private float _startScale = 0.2f;
    [SerializeField] private float _endScale = 0.35f;
    [SerializeField] private float _fadeDuration = 1f;
    [SerializeField] Vector2 _blastOffset;
    [SerializeField] Vector2 _signOffsetLeft;
    [SerializeField] Vector2 _signOffsetRight;
    private int _damage;

    private readonly List<GameObject> _spawnedDigits = new List<GameObject>();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_blastOffset, 0.1f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(_signOffsetLeft, 0.1f);

        Gizmos.color = Color.blueViolet;
        Gizmos.DrawSphere(_signOffsetRight, 0.1f);
        
    }


    private IEnumerator PlayAnimation(string animation, Animator animator)
    {
        animator.Play(animation);

        yield return null;

        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        Debug.Log("Playing " + animation + " animation for " + info.length + "s");

        yield return new WaitForSeconds(info.length);
    }

    public IEnumerator ShowDamageOnEnemy(int damage)
    {
        _damage = damage;

        GameObject blastGO = Instantiate(_blastPrefab);
        var blastAnimator = blastGO.GetComponent<Animator>();
        var blast = blastGO.GetComponent<Blast>();

        blast.BlastDamagePhase += ShowDamageNumberOnEnemy;
        blast.BlastSignPhase += ShowDamageSignOnEnemy;

        yield return StartCoroutine(PlayAnimation("Blast", blastAnimator));
    }

    public void ShowShieldOnEnemy()
    {

    }

    private void ShowDamageSignOnEnemy()
    {
        GameObject signGO = GetSignToShow();
        var actionAnimation = signGO.GetComponent<ActionAnimation>();
        actionAnimation.PlayAnimation();
    }
    
    private Vector2 GetSignOffset()
    {
        Vector2[] signOffsets = { _signOffsetLeft, _signOffsetRight };
        int idx = Random.Range(0, signOffsets.Length);

        return signOffsets[idx];
    }

    private GameObject GetSignToShow()
    {
        int idx = Random.Range(0, _signsPrefabs.Length);

        return Instantiate(_signsPrefabs[idx]);
    }

    private void ShowDamageNumberOnEnemy()
    {

    }

    private void PlaySignAnimation()
    {
        // string clip;
        // Animator animator;
        // Transform transform;

        // int[] positionCoef = { 1, -1 };
        // string[] animationClip = { "Boom", "Skdshhh" };

        // int posIdx = UnityEngine.Random.Range(0, positionCoef.Length);
        // int clipIdx = UnityEngine.Random.Range(0, animationClip.Length);

        // clip = animationClip[clipIdx];

        // if (clip == "Boom")
        // {
        //     animator = _boomAnimator;
        //     transform = _boomTransform;
        //     Vector3 rot = transform.eulerAngles;
        //     rot.z *= (float)positionCoef[posIdx];
        //     transform.eulerAngles = rot;
        // }
        // else
        // {
        //     animator = _skdshhhAnimator;
        //     transform = _skdshhhTransform;
        // }

        // transform.position = new Vector2(transform.position.x * positionCoef[posIdx], transform.position.y);

        // animator.Play(animationClip[clipIdx]);
    }

    public void ShowNumber(int number)
    {
        // Clean up old digits
        DestroyDigits();
        _spawnedDigits.Clear();

        // Convert number to string to extract digits
        char[] digits = number.ToString().ToCharArray();

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

    public void ShowTentacleOnPlayer()
    {

    }

    public void ShowShieldOnPlayer()
    {

    }
    
    public void ShowCrackOnPlayer()
    {
        
    }
}
