import { HttpBackend, HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map } from "rxjs";

@Injectable({
  providedIn: 'root',
})

export class ConfigurationLoaderService {
  private configs: Configs | undefined;

  private http: HttpClient;

  constructor(private readonly httpHandler: HttpBackend) {
    this.http = new HttpClient(httpHandler);
  }

  init(): Promise<boolean> {
    return new Promise<boolean>((resolve, reject) => {
      this.http.get<Configs>('/assets/config/runtime.config.json').pipe(map(res => res))
        .subscribe(value => {
          this.configs = value;
          resolve(true);
        },
        (error) => {
          reject(error);
        });
    });
  }

  get config(): Configs | undefined {
    return this.configs;
  }

  get apiBaseUrl(): string {
    return this.configs?.ApiBaseURL || '';
  }
}

export interface Configs{
  EnvironmentType: string;
  ApiBaseURL: string;
  ClientId: string;
  Authority: string;
}
