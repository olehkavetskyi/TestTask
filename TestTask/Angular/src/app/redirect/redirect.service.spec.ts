import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { RedirectService } from './redirect.service';

describe('RedirectService', () => {
  let service: RedirectService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [RedirectService]
    });
    service = TestBed.inject(RedirectService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should retrieve the full URL for a valid short URL', () => {
    const shortUrl = 'abc123';
    const mockResponse = { fullUrl: 'https://example.com' };

    service.getFullUrl(shortUrl).subscribe(response => {
      expect(response).toEqual(mockResponse);
    });

    const req = httpMock.expectOne(`${service.apiUrl}api/url/${shortUrl}`);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('should handle error when retrieving full URL', () => {
    const shortUrl = 'invalidUrl';
    const mockError = 'Failed to retrieve full URL';

    service.getFullUrl(shortUrl).subscribe(
      () => {
        fail('Expected error to be thrown');
      },
      error => {
        expect(error).toEqual(mockError);
      }
    );

    const req = httpMock.expectOne(`${service.apiUrl}api/url/${shortUrl}`);
    expect(req.request.method).toBe('GET');
    req.error(new ErrorEvent('network error'), { status: 500, statusText: 'Internal Server Error' });
  });
});
