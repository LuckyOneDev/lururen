namespace Lururen.Client.EntityComponentSystem.Generic
{
    /// <summary>
    /// Basic component whitch should be bound to IEntity.
    /// </summary>
    public interface IComponent<T> : IDisposable where T : IComponent<T>
    {
        /// <summary>
        /// Entity this component is bound to. 
        /// Should always be not null except for when component is being disposed.
        /// </summary>
        public Entity? Entity { get; set; }

        /// <summary>
        /// Init method is called only once after component is successufly registered in corresponding system.
        /// </summary>
        /// <param name="entity"></param>
        public void Init(ISystem<T> system);

        /// <summary>
        /// Called every time corresponding system needs to update this component.
        /// In general case it is called every frame.
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Update(double deltaTime);

        /// <summary>
        /// Determines if component should be considered by corresponding system.
        /// Set to true when component is successfully registered.
        /// </summary>
        public void SetActive(bool state)

        /// <summary>
        /// Determines if component should be considered by corresponding system.
        /// Set to true when component is successfully registered.
        /// </summary>
        public bool IsActive();
    }
}