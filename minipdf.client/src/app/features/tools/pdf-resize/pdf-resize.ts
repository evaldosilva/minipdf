import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { InputTextModule } from 'primeng/inputtext';
import { FormsModule } from '@angular/forms';
import { environment } from '../../../../environments/environment';

@Component({
  standalone: true,
  selector: 'app-pdf-resize',
  imports: [FormsModule, PanelModule, InputTextModule],
  templateUrl: './pdf-resize.html',
  styleUrl: './pdf-resize.css',
})
export class PdfResize {
  protected quality: number = 100;
  protected uploadedFiles: File[] = [];
  private readonly httpClient = inject(HttpClient);

  getFileName(file: File) {
    const filename = file.name + ' (' + this.getFileSize(file.size) + ')';
    return filename;
  }

  private getFileSize(size: number) {
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

    // In case of clear and re select items, activate onChange call
    (<HTMLInputElement>document.getElementById('file-upload')).value = '';
  }

  removeFileFromList(removedFile: File) {
    this.uploadedFiles?.splice(this.uploadedFiles?.indexOf(removedFile), 1);
  }

  public sendFiles() {
    const contentHeaders = new HttpHeaders({
      Authorization: 'Bearer ojfbgojfdbgjdfbg',
      enctype: 'multipart/form-data',
    });

    const formData = new FormData();
    this.uploadedFiles?.forEach((file) => {
      formData.append('file', file);
    });

    console.log('data sent: ' + this.uploadedFiles.length);

    this.httpClient
      .post(environment.apiUrl + 'PdfCompressor/compress', formData, {
        headers: contentHeaders,
      })
      .subscribe({
        next: (response) => console.log(response),
        error: (err) => console.log(err),
      });
  }
}
