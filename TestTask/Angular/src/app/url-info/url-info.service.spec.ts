import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { UrlInfoService } from './url-info.service';
import { Url } from '../shared/models/Url';
import { environment } from 'src/environments/environment';

describe('UrlInfoService', () => {
  let service: UrlInfoService;
  let httpTestingController: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [UrlInfoService]
    });
    service = TestBed.inject(UrlInfoService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpTestingController.verify();
  });

  it('should retrieve the URL by ID', () => {
    const id = 1;
    const apiUrl = environment.apiUrl;
    const mockUrl: Url = {
      id: id,
      shortUrl: 'abc123',
      fullUrl: 'https://example.com',
      createdByUserId: 'user123',
      createdAt: '2023-06-20T10:00:00'
    };

    service.getUrl(id).subscribe((url: Url) => {
      expect(url).toEqual(mockUrl);
    });

    const req = httpTestingController.expectOne(`${apiUrl}api/url/by-id/${id}`);
    expect(req.request.method).toBe('GET');
    req.flush(mockUrl);
  });
});
