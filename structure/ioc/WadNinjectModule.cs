﻿
using Ninject.Modules;
using Ninject.Web.Common;
using data.repository;
using data.service;
using data.sql;
using structure.factory;
using structure.repository;
using structure.sql;

namespace structure.ioc
{
    public class WadNinjectModule : NinjectModule
    {
        public override void Load()
        {
            //DB's
            this.Bind<IDatabaseFactory>().To<DatabaseFactory>().InRequestScope();
            this.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();

            //Repo's
            this.Bind<IUserSessionRepository>().To<UserSessionRepository>().InRequestScope();
            this.Bind<IHtmlSnippetRepository>().To<HtmlSnippetRepository>().InRequestScope();
            this.Bind<IHtmlItemRepository>().To<HtmlItemRepository>().InRequestScope();

            //Serv's
            this.Bind<IUserSessionService>().To<UserSessionService>().InRequestScope();
            this.Bind<IMembershipService>().To<MembershipService>().InRequestScope();
            this.Bind<IHtmlSnippetService>().To<HtmlSnippetService>().InRequestScope();
            this.Bind<IHtmlItemService>().To<HtmlItemService>().InRequestScope();
        }
    }

}
