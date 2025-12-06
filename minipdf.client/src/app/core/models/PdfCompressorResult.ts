import { PdfDataResult } from './PdfDataResult';

export class PdfCompressorResult {
  compressedSize?: number;
  originalSize?: number;
  reductionPercentage?: number;
  publicURI?: string;
  pdfDataResults?: Array<PdfDataResult>;
}
