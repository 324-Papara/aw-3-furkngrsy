using Autofac;
using Para.Data.UnitOfWork;
using FluentValidation;
using Para.Bussiness.Validations;
using Para.Bussiness.Cqrs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Para.Data.Context;
using Para.Data.DapperRepository;


namespace Para.Bussiness.DependencyResolvers
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            builder.RegisterType<CustomerRepository>().SingleInstance();


            builder.RegisterAssemblyTypes(typeof(CreateCustomerCommand).Assembly)
               .AsImplementedInterfaces();

            builder.Register(c =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<ParaDbContext>();
                var connectionString = c.Resolve<IConfiguration>().GetConnectionString("MsSqlConnection");
                optionsBuilder.UseSqlServer(connectionString);
                return new ParaDbContext(optionsBuilder.Options);
            }).AsSelf().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(CustomerValidator).Assembly)
                   .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                   .AsImplementedInterfaces();


        }
    }
}
