import { Component } from '@angular/core';
import { PdfResize } from '../../features/tools/pdf-resize/pdf-resize';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [PdfResize],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home {}
