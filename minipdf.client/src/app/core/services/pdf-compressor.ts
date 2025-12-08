import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class PdfcompressorService {
  private readonly httpClient = inject(HttpClient);

  public Compress(formData: FormData, quality: number, maxSizeKb: number | undefined) {
    const contentHeaders = new HttpHeaders({
      Authorization: 'Bearer ojfbgojfdbgjdfbg',
      enctype: 'multipart/form-data',
    });

    let _maxSizeKb = maxSizeKb ?? 0;
    if (isNaN(_maxSizeKb)) _maxSizeKb = 0;

    return this.httpClient.post(environment.apiUrl + 'PdfCompressor/compress', formData, {
      headers: contentHeaders,
      params: { quality: quality, maxSizeKb: _maxSizeKb },
    });
  }
}
