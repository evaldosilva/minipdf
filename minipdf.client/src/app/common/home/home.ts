import { Component, inject, OnInit, signal } from '@angular/core';
import { PdfResize } from '../../features/tools/pdf-resize/pdf-resize';
import { environment } from '../../../environments/environment';
import { Footer } from '../footer/footer';
import { AppUserService } from '../../core/services/app-user';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [PdfResize, Footer],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {
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
