using System.Collections.Generic;
using UnityEngine;


public class FlockManager : MonoBehaviour
{
    public float Speed = 5f;
    public float AvoidanceDistance = 2f;
    public float DetectRadius = 10f;
    public List<Bird> birdsInFlock;

    private Vector2 followPoint;
    private Vector2 _currentVelocity;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            followPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            TargetSpriteAnimate.Instance.transform.position = followPoint;
        }
        foreach (Bird bird in birdsInFlock)
        {
            StayOnScreenTeleport(bird);
            Vector2 moveTowards = CalculateMove(bird);
            bird.Move(Speed * Time.deltaTime * moveTowards.normalized);
        }
    }
    Vector2 CalculateMove(Bird bird)
    {
        Bird[] birds = GetNeighbours(bird);
        Vector2 movePoint = 4 * Cohesion(bird, birds) + 3 * Alignment(birds) + 1 * Separation(bird, birds) + FollowPoint(bird);
        return movePoint / 3;
    }
    Bird[] GetNeighbours(Bird bird)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(bird.transform.position, DetectRadius);
        List<Bird> neighbours = new();

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject == bird.gameObject) continue;
            collider.TryGetComponent(out Bird neighbour);
            if (neighbour != null) neighbours.Add(neighbour);
        }
        return neighbours.ToArray();
    }
    Vector2 Cohesion(Bird bird, Bird[] birds)
    {
        if (birds == null || birds.Length == 0) return Vector2.zero;
        Vector2 centerOfMass = Vector2.zero;
        foreach (Bird neighbour in birds)
        {
            centerOfMass += (Vector2)neighbour.transform.position;
        }
        centerOfMass /= birds.Length;
        centerOfMass -= (Vector2)bird.transform.position;
        if (float.IsNaN(_currentVelocity.x) || float.IsNaN(_currentVelocity.y)) _currentVelocity = Vector2.zero;
        centerOfMass = Vector2.SmoothDamp(bird.transform.up, centerOfMass, ref _currentVelocity, 5f);
        return centerOfMass.normalized;
    }
    Vector2 Alignment(Bird[] birds)
    {
        if (birds == null || birds.Length == 0) return Vector2.zero;
        Vector2 averageDirection = Vector2.zero;
        foreach (Bird neighbour in birds)
        {
            averageDirection += (Vector2) neighbour.transform.up;
        }
        averageDirection /= birds.Length;
        return averageDirection.normalized;
    }
    Vector2 Separation(Bird bird, Bird[] birds)
    {
        if (birds == null || birds.Length == 0) return Vector2.zero;
        Vector2 moveAway = Vector2.zero;
        int nAvoid = 0;
        foreach (Bird neighbour in birds)
        {
            if (Vector2.Distance(neighbour.transform.position, bird.transform.position) < AvoidanceDistance)
            {
                nAvoid++;
                moveAway += (Vector2)(bird.transform.position - neighbour.transform.position);
            }
        }
        if (nAvoid > 0) moveAway /= nAvoid;
        return moveAway.normalized;
    }
    Vector2 FollowPoint(Bird bird)
    {
        Vector2 direction = followPoint - (Vector2)bird.transform.position;
        return direction.normalized;
    }
    void StayOnScreenTeleport(Bird bird)
    {
        if (bird.transform.position.x > 20) bird.transform.position = new Vector3(-20f, bird.transform.position.y, 0f);
        if (bird.transform.position.x < -20) bird.transform.position = new Vector3(20f, bird.transform.position.y, 0f);
        if (bird.transform.position.y > 11) bird.transform.position = new Vector3(bird.transform.position.x, -11f, 0f);
        if (bird.transform.position.y < -11) bird.transform.position = new Vector3(bird.transform.position.x, 11f, 0f);
    }
}