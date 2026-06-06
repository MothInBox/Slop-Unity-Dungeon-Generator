using UnityEngine;

[System.Serializable]
public class RequiredRoomEntry
{
    [SerializeField] private RoomTypeEntry RoomTypeEntry;
    [Tooltip("The minimum quantity of this room type that must be included. Max quantity is determined by the Room Type Entry Limit field.")]
    [SerializeField][Range(0, 255)] private byte minQuantity;
    [Tooltip("The minimum depth at which this room can be placed. The higher the depth the higher chance of being placed until min is met.")]
    [SerializeField][Range(0, 255)] private byte minDepth;
    [Tooltip("The maximum depth at which this room can be placed. The higher the depth the higher chance of being placed until min is met.")]
    [SerializeField][Range(0, 255)] private byte maxDepth;
    [Tooltip("The chance to place this room after the minimum depth is met.")]
    [SerializeField][Range(0, 255)] private byte chanceAfterMinDepthIsMet;

}