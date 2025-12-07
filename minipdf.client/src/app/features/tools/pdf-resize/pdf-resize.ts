import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { InputTextModule } from 'primeng/inputtext';
import { FormsModule } from '@angular/forms';
import { ProgressBarModule } from 'primeng/progressbar';
import { environment } from '../../../../environments/environment';
import { PdfCompressorResult } from '../../../core/models/PdfCompressorResult';
import { PdfDataResult } from '../../../core/models/PdfDataResult';

@Component({
  standalone: true,
  selector: 'app-pdf-resize',
  imports: [FormsModule, PanelModule, InputTextModule, ProgressBarModule],
  templateUrl: './pdf-resize.html',
  styleUrl: './pdf-resize.css',
})
export class PdfResize {
  protected isCompressing = signal<boolean>(false);

  protected compressionPercentage: number = 0;
  protected originalSize: string = '';
  protected compressedSize: string = '';
  protected publicURI: string = '';
  protected uploadedFileResults: PdfDataResult[] = [];

  protected quality: number = 50;
  protected maxSizeKb: number | undefined;
  protected uploadedFiles: File[] = [];

  private readonly httpClient = inject(HttpClient);

  getFileName(file: File) {
    const filename = file.name + ' (' + this.getFileSize(file.size) + ')';
    return filename;
  }

  public getFileSize(size: number) {
    return new Intl.NumberFormat().format(Math.ceil(size / 1024)) + ' KB';
  }

  onUpload() {
    let files = (<HTMLInputElement>document.getElementById('file-upload')).files || [];
    let fileExists: boolean = false;

    for (var i = 0; i < files.length; i++) {
      for (var x = 0; x < this.uploadedFiles?.length; x++)
        if (this.uploadedFiles[x].name == files[i].name) {
          fileExists = true;
          break;
        }

      if (!fileExists) this.uploadedFiles?.push(files[i]);

      fileExists = false;
    }
    this.clearDOMFileIpload();
  }

  private clearDOMFileIpload() {
    // In case of clear and re select items, activate onChange call
    (<HTMLInputElement>document.getElementById('file-upload')).value = '';
  }

  clearFileList() {
    this.clearDOMFileIpload();
    this.uploadedFiles = [];
  }

  clearCompressionResults() {
    this.compressionPercentage = 0;
    this.originalSize = '';
    this.compressedSize = '';
    this.publicURI = '';
    this.uploadedFileResults = [];
  }

  removeFileFromList(removedFile: File) {
    this.uploadedFiles?.splice(this.uploadedFiles?.indexOf(removedFile), 1);
  }

  public sendFiles() {
    if (this.uploadedFiles.length > 0) {
      this.clearCompressionResults();

      const contentHeaders = new HttpHeaders({
        Authorization: 'Bearer ojfbgojfdbgjdfbg',
        enctype: 'multipart/form-data',
      });

      const formData = new FormData();
      this.uploadedFiles?.forEach((file) => {
        formData.append('files', file, file.name);
      });

      console.log('data sent: ' + this.uploadedFiles.length);
      this.isCompressing.update((value) => (value = true));
      this.clearFileList();

      let _maxSizeKb = this.maxSizeKb ?? 0;
      if (isNaN(_maxSizeKb)) _maxSizeKb = 0;

      this.httpClient
        .post(environment.apiUrl + 'PdfCompressor/compress', formData, {
          headers: contentHeaders,
          params: { quality: this.quality, maxSizeKb: _maxSizeKb },
        })
        .subscribe({
          next: (response: PdfCompressorResult) => {
            console.log(response);

            this.publicURI =
              environment.apiUrl + 'PdfCompressor/document?identification=' + response.publicURI!;
            this.compressionPercentage = response.reductionPercentage!;
            this.originalSize = this.getFileSize(response.originalSize ?? 0);
            this.compressedSize = this.getFileSize(response.compressedSize ?? 0);
            this.uploadedFileResults = response.pdfDataResults!;
          },
          error: (err) => console.log(err),
        })
        .add(() => {
          this.isCompressing.update((value) => (value = false));
          window.location.href = this.publicURI;
        });
    }
  }
}
