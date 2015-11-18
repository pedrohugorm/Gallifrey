using System.Data.Entity;

namespace Gallifrey.SharedKernel.Application.Persistence.Repository
{
    public interface IHandleEntityChanged<TModel> where TModel : class
    {
        void OnEntityChanged(EntityChangedEvent<TModel> model);
    }

    public class EntityChangedEvent<TModel>
    {
        public EntityState State { set; get; }
        public TModel Model { set; get; }

        public EntityChangedEvent()
        {

        }

        public EntityChangedEvent(TModel model, EntityState state)
        {
            Model = model;
            State = state;
        }
    }
}