namespace DimensionAdventurer.Players.Abilities
{
    /// <summary>
    /// The interface of controlling how the player perform movements
    /// </summary>
    public interface IPlayerMovementPerformer
    {
        void MoveLeft();
        void MoveRight();
        void Teleport(WorldPosition worldPosition);
    }
}