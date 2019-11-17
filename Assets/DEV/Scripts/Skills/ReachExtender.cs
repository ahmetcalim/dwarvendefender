using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachExtender : MonoBehaviour
{
    [Tooltip("Extension piece for weapon.")]
    public GameObject ExtensionPrefab;
    [Tooltip("Size of extension piece.")]
    public float ExtensionSize = 0.1f;

    [Tooltip("Weapon's head and tail references for range manipulation.")]
    public Transform WeaponHead, WeaponTail;
    [Tooltip("Offset vectors for weapon's head and tail.")]
    public Vector3 WeaponHeadOffset, WeaponTailOffset;
    [Tooltip("Direction from head to tail.")]
    public Vector3 WorkDirection;



    float size;
    public void UpdateRange(float r, Vector3 basePose)
    {
         int pieceCount = Mathf.CeilToInt(r) + 2;
        /* WeaponHeadOffset = WeaponHeadOffset * transform.localScale.x;
        // WeaponTailOffset = WeaponTailOffset * transform.localScale.x;
         //ExtensionSize = ExtensionSize * transform.localScale.x;
        
         WorkDirection = transform.TransformDirection(WorkDirection); // Transform work direction to global.
         Vector3 referencePosition = transform.localPosition; // Might integrate this to basePose
         if (pieceCount % 2 == 0) referencePosition -= WorkDirection.normalized * (ExtensionSize / 2.0f); // if pieceCount is even, go up by half.
         referencePosition -= WorkDirection.normalized * ((Mathf.CeilToInt(pieceCount / 2.0f) - 1) * ExtensionSize); // Go up by pieceCount/2 (rounded up) - 1

         for (int i = 0; i < pieceCount; i++)
         {
             if (i == 0) WeaponHead.localPosition = referencePosition; // Set head position if starting.
             else if (i == pieceCount - 1) WeaponTail.position = referencePosition; // Set tail position if ending.
             else
             {
                 GameObject ext = Instantiate(ExtensionPrefab,transform); // Instantiate new extension.
                 ext.transform.localPosition += referencePosition; // Position it at the reference.
                 ext.transform.localRotation = transform.localRotation;
                 ext.transform.localScale = WeaponHead.localScale; 
             }
             referencePosition += WorkDirection.normalized * ExtensionSize; // Move down the work direction.
         }
         // Setup the head and tail offsets.
         WeaponHead.position += transform.TransformVector(WeaponHeadOffset);
         WeaponTail.position += transform.TransformVector(WeaponTailOffset);*/
        for (int i = 0; i < pieceCount; i++)
        {
            size += ExtensionSize;
            Transform extCopy = Instantiate(ExtensionPrefab, transform).transform;
            extCopy.transform.localPosition = Vector3.zero;
            extCopy.transform.localPosition = ExtensionPrefab.transform.localPosition;
            extCopy.transform.localRotation = ExtensionPrefab.transform.localRotation;
            extCopy.transform.localPosition -= new Vector3(0f, 0f, size);
            WeaponHead.transform.localPosition -= new Vector3(0f, 0f, ExtensionSize);
        }
    }
}
