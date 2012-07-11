using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Backend.Facade.Interfaces;
using Backend.DataContext;
using Backend.Facade.Implementations;

namespace Backend
{
    public class UnitOfWork : IDisposable
    {
        private RouletteContext ctx = new RouletteContext();

        private IRouletteFacade rouletteFacade;

        public IRouletteFacade RouletteSrvc
        {
            get
            {
                if (rouletteFacade == null)
                {
                    rouletteFacade = new RouletteFacade(ctx);
                }
                return rouletteFacade;
            }
        }

        #region Resource removable pattern
        private bool isDisposed = false;

        private void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    ctx.Dispose();
                }
            }
            this.isDisposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnitOfWork()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
