import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

@Injectable({
  providedIn: 'root',
})
export class ConfigurationLoaderService {
  private configs: Configs | undefined;

  constructor(private http:HttpClient) {}

  loadConfig(): Promise<any> {
    return this.http.get<Configs>('/assets/config/runtime.config.json').toPromise().then((config: Configs | undefined) => {
      this.configs = config;
      return config;
    });
  }

  get apiBaseUrl(): string {
    return this.configs?.ApiBaseURL || '';
  }
}

export interface Configs{
  EnvironmentType: string;
  ApiBaseURL: string;
}
