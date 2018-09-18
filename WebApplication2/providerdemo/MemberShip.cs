using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.providerdemo
{
    public class LogManager
    {
        private readonly LogProvider provider;
        public LogManager(LogProvider provider)
        {
            this.provider = provider;
        }

        public virtual int Get()
        {
            return provider.Get();
        }

        public virtual void Change()
        {
            provider.Change();
        }

        public virtual void Create()
        {
            provider.Create();
        }
    }

    public abstract class LogProvider
    {
        public abstract int Get();
        public abstract void Change();
        public abstract void Create();
    }

    public class SqlMemberShipProvider: LogProvider
    {
        public override int Get()
        {
            throw new NotImplementedException();
        }

        public override void Change()
        {
            throw new NotImplementedException();
        }

        public override void Create()
        {
            throw new NotImplementedException();
        }
    }

    public class ActiveDirectiveMemberShipProvider: LogProvider
    {
        public override int Get()
        {
            throw new NotImplementedException();
        }

        public override void Change()
        {
            throw new NotImplementedException();
        }

        public override void Create()
        {
            throw new NotImplementedException();
        }
    }
}
