import { Component } from '@angular/core';
import { PdfResize } from '../../features/tools/pdf-resize/pdf-resize';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [PdfResize],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home {
  subscribe() {
    window.location.href = environment.checkoutRecurrentSubscriptionUrl;
  }
}
