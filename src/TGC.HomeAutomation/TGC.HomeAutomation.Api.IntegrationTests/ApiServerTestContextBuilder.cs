using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using TGC.HomeAutomation.Api.IntegrationTests.Authentication;
using TGC.HomeAutomation.Api.IntegrationTests.Tests;
using TGC.HomeAutomation.Infrastructure.Authentication;

namespace TGC.HomeAutomation.Api.IntegrationTests;

public class ApiServerTestContextBuilder<T> where T : class
{
	private readonly List<Action<IServiceCollection>> _serviceConfigurations = new List<Action<IServiceCollection>>();
	private readonly List<Action<IConfigurationBuilder>> _configurations = new List<Action<IConfigurationBuilder>>();
	private readonly WebApplicationFactory<T> _fixture;

	private bool replaceAuthentication;
	private bool stubJwt;
	private bool stubApiKey;
	private bool useEmptyJwtHandler;
	private bool useEmptyApiKeyHandler;

	public ApiServerTestContextBuilder(WebApplicationFactory<T> fixture)
	{
		_fixture = fixture;
	}

	public ApiServerTestContextBuilder<T> AddServiceAction(Action<IServiceCollection> action)
	{
		_serviceConfigurations.Add(action);
		return this;
	}

	public ApiServerTestContextBuilder<T> AddScoped<TService, TImplementation>()
		where TService : class
		where TImplementation : class, TService
	{
		return AddServiceAction(services => services.AddScoped<TService, TImplementation>());
	}

	public ApiServerTestContextBuilder<T> AddTransient<TService, TImplementation>()
		where TService : class
		where TImplementation : class, TService
	{
		return AddServiceAction(services => services.AddTransient<TService, TImplementation>());
	}

	public ApiServerTestContextBuilder<T> AddSingleton<TService, TImplementation>()
		where TService : class
		where TImplementation : class, TService
	{
		return AddServiceAction(services => services.AddSingleton<TService, TImplementation>());
	}

	public ApiServerTestContextBuilder<T> AddSingleton<TService>(TService instance)
		where TService : class
	{
		return AddServiceAction(services => services.AddSingleton(instance));
	}

	public ApiServerTestContextBuilder<T> Replace<TService, TImplementation>(ServiceLifetime lifetime)
		where TService : class
		where TImplementation : class, TService
	{
		return AddServiceAction(services =>
			services.Replace(new ServiceDescriptor(typeof(TService), typeof(TImplementation), lifetime)));
	}

	public ApiServerTestContextBuilder<T> Replace<TService>(TService implementation, object? serviceKey = null)
		where TService : class
	{
		return AddServiceAction(services =>
			services.Replace(new ServiceDescriptor(typeof(TService), serviceKey, implementation)));
	}

	public ApiServerTestContextBuilder<T> AddConfigurationAction(Action<IConfigurationBuilder> action)
	{
		_configurations.Add(action);
		return this;
	}

	public ApiServerTestContextBuilder<T> ReplacePortalAuthWithEmpty()
	{
		replaceAuthentication = true;
		useEmptyJwtHandler = true;
		return this;
	}

	public ApiServerTestContextBuilder<T> ReplacePortalAuthWithStub()
	{
		replaceAuthentication = true;
		stubJwt = true;
		return this;
	}

	public ApiServerTestContextBuilder<T> ReplaceApiKeyAuthWithEmpty()
	{
		replaceAuthentication = true;
		useEmptyApiKeyHandler = true;
		return this;
	}

	public ApiServerTestContext<T> Build()
	{
		var newFixture = _fixture.WithWebHostBuilder(builder =>
		{
			builder.ConfigureAppConfiguration((hostContext, configurationBuilder) =>
				_configurations.ForEach(action => action.Invoke(configurationBuilder)));
			builder.ConfigureTestServices(services =>
				_serviceConfigurations.ForEach(action => action.Invoke(services)));
		});

		RegisterAuthenticationHandlers();

		return new ApiServerTestContext<T>(newFixture);
	}

	private void RegisterAuthenticationHandlers()
	{
		if (replaceAuthentication)
		{
			_serviceConfigurations.Add(services => RemoveAuthentication(services));

			if (useEmptyJwtHandler)
			{
				_serviceConfigurations.Add(services => NoJWTAuthenticationHandler.Register(services));
			}

			if (stubJwt)
			{
				_serviceConfigurations.Add(services => StubJWTAuthenticationHandler.Register(services));
			}

			if (!stubJwt && !useEmptyJwtHandler)
			{
				//Register orignal service since no stubbing -> Needs a bit of refactor before it can be supported.
			}

			if (useEmptyApiKeyHandler)
			{
				_serviceConfigurations.Add(services => NoAPIKeyAuthenticationHandler.Register(services));
			}

			if (stubApiKey)
			{
				//Register stub - Not sure if this is ever needed.
			}

			if (!useEmptyApiKeyHandler && !stubApiKey)
			{
				//Register orignal service since no stubbing
				_serviceConfigurations.Add(services =>
				{
					services.AddAuthentication(ApiKeyAuthSchemeOptions.DefaultScheme)
						.AddScheme<ApiKeyAuthSchemeOptions, ApiKeyAuthSchemeHandler>(
							ApiKeyAuthSchemeOptions.DefaultScheme,
							_ => { });
				});
			}
		}
	}

	private static IServiceCollection RemoveAuthentication(IServiceCollection services)
	{
		var jwtBearerDescriptor =
			services.Where(d => d.ServiceType == typeof(IConfigureOptions<JwtBearerOptions>)).ToList();
		foreach (var descriptor in jwtBearerDescriptor)
		{
			services.Remove(descriptor);
		}

		var authOptionsDescriptor = services
			.Where(d => d.ServiceType == typeof(IConfigureOptions<AuthenticationOptions>) && d.ServiceType != typeof(IConfigureOptions<ApiKeyAuthSchemeOptions>)).ToList();
		foreach (var descriptor in authOptionsDescriptor)
		{
			services.Remove(descriptor);
		}
		return services;
	}
}
