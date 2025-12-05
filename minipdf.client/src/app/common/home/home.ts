import { Component } from '@angular/core';
import { PdfResize } from '../../features/tools/pdf-resize/pdf-resize';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [PdfResize],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home {
  subscribe() {
    window.location.href = 'https://buy.stripe.com/test_7sY00lgJc9699B6fcacMM00';
  }
}
