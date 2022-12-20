using System;
using System.Reactive.Subjects;
using DataMiningForShoppingBasket.Interfaces;
using DynamicData;

namespace DataMiningForShoppingBasket.Common
{
    public class DefaultNotifier<TEntity, TId> : INotifier<TEntity, TId>
        where TEntity : class, IHavingId<TId>, new()
    {
        private readonly Subject<IChangeSet<TEntity, TId>> _previewChangeSubject;

        private static DefaultNotifier<TEntity, TId> _instance;
        private static readonly TEntity Locker = new TEntity();

        private DefaultNotifier()
        {
            _previewChangeSubject = new Subject<IChangeSet<TEntity, TId>>();
        }

        public IObservable<IChangeSet<TEntity, TId>> Changes => _previewChangeSubject;

        public static DefaultNotifier<TEntity, TId> GetInstance()
        {
            if (_instance != null)
                return _instance;

            lock (Locker)
            {
                return _instance ?? (_instance = new DefaultNotifier<TEntity, TId>());
            }
        }
        
        /// <summary>
        /// Сообщаем об изменении объекта или новом объекте <see cref="TEntity"/>
        /// </summary>
        /// <param name="entity"></param>
        /// <remarks>Т.к. подписчиком является <see cref="IObservableCache{TObject,TKey}"/>,
        /// она опирается на метод <see cref="ChangeAwareCache{TObject,TKey}.AddOrUpdate"/>,
        /// то делить эти истории мы не будем </remarks>
        public virtual void NotifyAdd(TEntity entity)
        {
            _previewChangeSubject.OnNext(new ChangeSet<TEntity, TId>
            {
                new Change<TEntity, TId>(ChangeReason.Add, entity.Id, entity)
            });
        }
        
        public virtual void NotifyUpdate(TEntity entity)
        {
            _previewChangeSubject.OnNext(new ChangeSet<TEntity, TId>
            {
                new Change<TEntity, TId>(ChangeReason.Update, entity.Id, entity, entity)
            });
        }

        public virtual void NotifyDelete(TId entityId)
        {
            _previewChangeSubject.OnNext(new ChangeSet<TEntity, TId>
            {
                new Change<TEntity, TId>(ChangeReason.Remove, entityId, default)
            });
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool dispose)
        {
            if (!dispose)
                return;

            _previewChangeSubject?.Dispose();
        }
    }
}
