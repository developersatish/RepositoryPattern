using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Infrastructure
{
    public class UnitOfWork:IDisposable
    {
        private MyContext _context;

        public UnitOfWork()
        {
            this._context = new MyContext();
        }
    
        public IRepository<T> Get<T>() where T : class
        {
            return new Repository<T>(_context);
        }
        public void Commit()
        {
            _context.SaveChanges();
        }

        private bool dispoded = false;
        public virtual void Dispose(bool disposing)
        {
            if (!this.dispoded)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.dispoded = true;
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}