
using System;
using structure.sql;

namespace structure.factory
{
    public interface IDatabaseFactory : IDisposable
    {
        WadDbContext Get();
    }

    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private WadDbContext _dataContext;
        public WadDbContext Get()
        {
            return _dataContext ?? (_dataContext = new WadDbContext());
        }
        protected override void DisposeCore()
        {
            if (_dataContext != null)
            {
                _dataContext.Dispose();
                _dataContext = null;
            }
        }
    }

}
