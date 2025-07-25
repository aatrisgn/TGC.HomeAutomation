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
      clientId: configService.config?.ClientId!,
      authority: configService.config?.Authority,
      redirectUri: window.location.origin,
      postLogoutRedirectUri: window.location.origin,
    },
    cache: {
      cacheLocation: BrowserCacheLocation.SessionStorage,
    },
    system: {
      allowPlatformBroker: false, // Disables WAM Broker
      loggerOptions: {
        loggerCallback,
        logLevel: LogLevel.Error,
        piiLoggingEnabled: false,
      },
    },
  });
}

export function MSALInterceptorConfigFactory(configService:ConfigurationLoaderService): MsalInterceptorConfiguration {
  const protectedResourceMap = new Map<string, Array<string>>();
  protectedResourceMap.set(
    configService.apiBaseUrl,
    ["api://7e4b8c41-76fd-4f2e-84f7-fbf7541f685b/test"]
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
      scopes: ["api://7e4b8c41-76fd-4f2e-84f7-fbf7541f685b/test"]
    },
    loginFailedRoute: '/login-failed',
  };
}
