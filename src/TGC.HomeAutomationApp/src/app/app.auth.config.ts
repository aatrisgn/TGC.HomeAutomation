import {
  IPublicClientApplication,
  PublicClientApplication,
  InteractionType,
  BrowserCacheLocation,
  LogLevel,
} from '@azure/msal-browser';
import {
  MsalInterceptorConfiguration,
  MsalGuardConfiguration,
} from '@azure/msal-angular';
import { ConfigurationLoaderService } from './services/ConfigurationLoaderService';

export function loggerCallback(logLevel: LogLevel, message: string) {
  console.log(message);
}

export function MSALInstanceFactory(configService:ConfigurationLoaderService): IPublicClientApplication {
  return new PublicClientApplication({
    auth: {
      clientId: configService.config?.ClientId!, //environment.msalConfig.auth.clientId,
      authority: configService.config?.Authority, //environment.msalConfig.auth.authority,
      redirectUri: '/',
      postLogoutRedirectUri: '/',
    },
    cache: {
      cacheLocation: BrowserCacheLocation.SessionStorage,
    },
    system: {
      allowPlatformBroker: false, // Disables WAM Broker
      loggerOptions: {
        loggerCallback,
        logLevel: LogLevel.Warning,
        piiLoggingEnabled: false,
      },
    },
  });
}

export function MSALInterceptorConfigFactory(): MsalInterceptorConfiguration {
  const protectedResourceMap = new Map<string, Array<string>>();
  protectedResourceMap.set(
    "http://localhost:5298",//environment.apiConfig.uri,
    ["api://7e4b8c41-76fd-4f2e-84f7-fbf7541f685b/test"]//environment.apiConfig.scopes
  );

  return {
    interactionType: InteractionType.Redirect,
    protectedResourceMap,
  };
}

export function MSALGuardConfigFactory(): MsalGuardConfiguration {
  return {
    interactionType: InteractionType.Redirect,
    authRequest: {
      scopes: ["api://7e4b8c41-76fd-4f2e-84f7-fbf7541f685b/test"]//[...environment.apiConfig.scopes],
    },
    loginFailedRoute: '/login-failed',
  };
}
