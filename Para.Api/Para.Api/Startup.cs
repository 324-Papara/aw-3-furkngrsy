using System.Reflection;
using System.Text.Json.Serialization;
using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Para.Api.Middleware;
using Para.Api.Service;
using Para.Bussiness;
using Para.Bussiness.Cqrs;
using Para.Bussiness.Validations;
using Para.Bussiness.DependencyResolvers;
using Para.Data.Context;
using Para.Data.UnitOfWork;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation;

namespace Para.Api;

public class Startup
{
    public IConfiguration Configuration;
    
    public Startup(IConfiguration configuration)
    {
        this.Configuration = configuration;
    }
    
    
    public void ConfigureServices(IServiceCollection services)
    {

        services.AddControllers()
        .AddFluentValidation(fv =>
        {
            fv.RegisterValidatorsFromAssemblyContaining<CustomerValidator>();
            fv.RegisterValidatorsFromAssemblyContaining<CustomerAddressValidator>();
            fv.RegisterValidatorsFromAssemblyContaining<CustomerDetailValidator>();
            fv.RegisterValidatorsFromAssemblyContaining<CustomerPhoneValidator>();
        });

        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.WriteIndented = true;
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
        });
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Para.Api", Version = "v1" });
        });

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MapperConfig());
        });
        services.AddSingleton(config.CreateMapper());

    }

    public void ConfigureContainer(ContainerBuilder builder)
    {
        // Autofac ile yapýlandýrmalarý buraya taþýyoruz.
        // AutofacBusinessModule sýnýfý, tüm baðýmlýlýklarý burada yapýlandýracaktýr.
        builder.RegisterModule(new AutofacBusinessModule());
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Para.Api v1"));
        }


        app.UseMiddleware<HeartbeatMiddleware>();
        app.UseMiddleware<ErrorHandlerMiddleware>();
        app.UseMiddleware<LoggerMiddleware>();

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        
        app.Use((context,next) =>
        {
            if (!string.IsNullOrEmpty(context.Request.Path) && context.Request.Path.Value.Contains("favicon"))
            {
                return next();
            }
            
            var service1 = context.RequestServices.GetRequiredService<CustomService1>();
            var service2 = context.RequestServices.GetRequiredService<CustomService2>();
            var service3 = context.RequestServices.GetRequiredService<CustomService3>();

            service1.Counter++;
            service2.Counter++;
            service3.Counter++;

            return next();
        });
        
        app.Run(async context =>
        {
            var service1 = context.RequestServices.GetRequiredService<CustomService1>();
            var service2 = context.RequestServices.GetRequiredService<CustomService2>();
            var service3 = context.RequestServices.GetRequiredService<CustomService3>();

            if (!string.IsNullOrEmpty(context.Request.Path) && !context.Request.Path.Value.Contains("favicon"))
            {
                service1.Counter++;
                service2.Counter++;
                service3.Counter++;
            }

            await context.Response.WriteAsync($"Service1 : {service1.Counter}\n");
            await context.Response.WriteAsync($"Service2 : {service2.Counter}\n");
            await context.Response.WriteAsync($"Service3 : {service3.Counter}\n");
        });
    }
}