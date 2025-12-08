import { Component, inject, OnInit, signal } from '@angular/core';

import { environment } from '../../../../environments/environment';
import { AppUserService } from '../../../core/services/app-user';
import { PdfResize } from '../pdf-resize/pdf-resize';
import { Footer } from '../../../common/footer/footer';

@Component({
  selector: 'app-tools-area',
  standalone: true,
  imports: [PdfResize, Footer],
  templateUrl: './tools-area.html',
  styleUrl: './tools-area.css',
})
export class ToolsArea implements OnInit {
  private readonly appUserService = inject(AppUserService);
  protected remainingCompressions = signal<number>(0);

  ngOnInit(): void {
    this.appUserService.getRemainingCompressions().subscribe({
      next: (response) => {
        this.remainingCompressions.set(response.valueOf());
      },
      error: (err) => {
        console.log(err);
      },
    });
  }

  getRemainingCompressions() {
    this.appUserService.getRemainingCompressions().subscribe({
      next: (response) => {
        this.remainingCompressions.update((value) => (value = response.valueOf()));
      },
      error: (err) => {
        console.log(err);
      },
    });
  }

  subscribe() {
    window.location.href = environment.checkoutRecurrentSubscriptionUrl;
  }
}
