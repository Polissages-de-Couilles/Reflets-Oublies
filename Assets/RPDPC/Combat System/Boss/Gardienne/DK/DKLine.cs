using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class DKLine : MonoBehaviour
{
    ProjectileManager pm;
    GameObject spear;
    float spearSize;
    float speed;
    float startRadius;
    public List<DKSpear> spears = new List<DKSpear>();
    Vector2 offLimitAngles = new Vector2(0,180);
    List<Vector2> excludedAngles = new List<Vector2>();
    HoleValues holeValues;

    public void Init(ProjectileManager pm, GameObject spear, float spearSize, float speed, float startRadius, HoleValues holeValues)
    {
        this.pm = pm;
        this.spear = spear;
        this.spearSize = spearSize;
        this.speed = speed;
        this.startRadius = startRadius;
        this.holeValues = holeValues;
        PrepareHole();
        LaunchLine();
    }

    void PrepareHole()
    {
        switch (holeValues.mode) 
        { 
            case HoleMode.Set:
                excludedAngles = holeValues.setAngles;
                break;
            case HoleMode.Random:
                for (int i = 0; i < holeValues.randomNb; i++) 
                {
                    float randomAngle = Random.Range(holeValues.randomStartAngle.x, holeValues.randomStartAngle.y);
                    excludedAngles.Add(new Vector2(randomAngle, randomAngle + Random.Range(holeValues.randomAngleRange.x, holeValues.randomAngleRange.y)));
                }
                break;
        }
    }
    
    void LaunchLine()
    {
        foreach (float f in GetSpawnAngles(startRadius, spearSize))
        {
            spawnSpear(f, startRadius);
        }
        StartCoroutine(AddSpearOverTime());
    }

    IEnumerator AddSpearOverTime()
    {
        while (spears.Count > 0) 
        { 
            foreach (float f in GetSpawnAngles(getCurrentRadius(), spearSize))
            {
                Vector2 posrela = GetUnitOnCircle(f + 180, getCurrentRadius());
                if (!isThereASpearHere(transform.position + new Vector3(posrela.x, 0, posrela.y)) && !isAngleOffLimits(f))
                {
                    spawnSpear(f, getCurrentRadius());
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
        yield return null;
        Destroy(gameObject);
    }

    void spawnSpear(float angle, float radius)
    {
        foreach (Vector2 c in excludedAngles)
        {
            if (angle >= c.x && angle <= c.y) return;
        }

        GameObject spearInstance = Instantiate(spear);
        Vector2 relative = GetUnitOnCircle(angle + 180, radius);
        spearInstance.transform.position = transform.position + new Vector3(relative.x, 0, relative.y);
        spears.Add(spearInstance.GetComponent<DKSpear>());
        spearInstance.GetComponent<DKSpear>().Init(pm, (new Vector2(spearInstance.transform.position.x, spearInstance.transform.position.z) - new Vector2(transform.position.x, transform.position.z)).normalized, speed, spearSize, angle, this);
    }

    bool isThereASpearHere(Vector3 pos)
    {
        foreach (DKSpear spear in spears)
        {
            if (Vector3.Distance(pos, spear.transform.position) < spearSize)
            {
                return true;
            }
        }
        return false;
    }

    float getCurrentRadius()
    {
        return Vector3.Distance(new Vector3(spears[0].transform.position.x, 0, spears[0].transform.position.z), new Vector3(transform.position.x, 0, transform.position.z));
    }

    Vector2 GetUnitOnCircle(float angleDegrees, float radius)
    {
        float _x = 0;
        float _y = 0;
        float angleRadians = 0;
        Vector2 _returnVector;

        angleRadians = angleDegrees * Mathf.PI / 180.0f;

        _x = radius * Mathf.Cos(angleRadians);
        _y = radius * Mathf.Sin(angleRadians);

        _returnVector = new Vector2(_x, _y);

        return _returnVector;
    }

    public static int GetMaxSpawnableObjects(float radius, float objectWidth)
    {
        if (objectWidth <= 0 || radius <= 0 || objectWidth > 2 * radius)
        {
            Debug.LogWarning("Paramètres invalides.");
            return 0;
        }

        float angleRadians = 2f * Mathf.Asin(objectWidth / (2f * radius));
        int maxObjects = Mathf.FloorToInt(Mathf.PI / angleRadians);
        return maxObjects;
    }

    public static float[] GetSpawnAngles(float radius, float objectWidth)
    {
        int count = GetMaxSpawnableObjects(radius, objectWidth);
        if (count == 0) return new float[0];

        float angleStep = 180f / (count - 1);
        float[] angles = new float[count];

        for (int i = 0; i < count; i++)
        {
            angles[i] = i * angleStep;
        }

        return angles;
    }

    public void addAngleToOffLimit(float angleRadians)
    {
        if (angleRadians < 90f)
        {
            if (angleRadians > offLimitAngles.x) offLimitAngles.x = angleRadians;
        }
        else
        {
            if (angleRadians < offLimitAngles.y) offLimitAngles.y = angleRadians;
        }
    }

    bool isAngleOffLimits(float angleRadians) 
    { 
        return angleRadians < offLimitAngles.x || angleRadians > offLimitAngles.y;
    }
}
