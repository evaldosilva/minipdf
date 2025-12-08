import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AppUserService {
  private readonly httpClient = inject(HttpClient);

  getRemainingCompressions() {
    return this.httpClient.get<number>(environment.apiUrl + 'AppUSer/RemainingConvertions');
  }
}
