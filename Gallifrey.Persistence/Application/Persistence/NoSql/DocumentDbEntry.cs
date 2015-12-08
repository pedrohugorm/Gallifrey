using System;

namespace Gallifrey.Persistence.Application.Persistence.NoSql
{
    public class DocumentDbEntry<TModel>
    {
        private readonly Action _action;
        public EntryStateType State { private set; get; }
        public TModel Model { get; set; }

        public DocumentDbEntry(EntryStateType state, TModel model, Action action)
        {
            State = state;
            Model = model;
            _action = action;
        }

        public void ExecuteAction()
        {
            _action();
        }
    }
}