import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { UrlsTableService } from './urls-table.service';
import { Roles, User } from '../shared/models/User';
import { environment } from '../../environments/environment';
import { Url } from '../shared/models/Url';

describe('UrlsTableService', () => {
  let service: UrlsTableService;
  let httpTestingController: HttpTestingController;
  const apiUrl = environment.apiUrl;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [UrlsTableService]
    });
    service = TestBed.inject(UrlsTableService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpTestingController.verify();
  });

  it('should retrieve the current user', () => {
    const mockUser: User = {
      id: 'user123',
      email: 'john.doe@example.com',
      role: Roles.Admin
    };

    service.getCurrentUser().subscribe((user: User) => {
      expect(user).toEqual(mockUser);
    });

    const req = httpTestingController.expectOne(`${apiUrl}api/user/current-user`);
    expect(req.request.method).toBe('GET');
    req.flush(mockUser);
  });

  it('should retrieve the short URLs', () => {
    const mockUrls: Url[] = [
      {
        id: 1,
        shortUrl: 'abc123',
        fullUrl: 'https://example.com',
        createdByUserId: 'user123',
        createdAt: '2023-06-20T10:00:00'
      },
      {
        id: 2,
        shortUrl: 'def456',
        fullUrl: 'https://example.org',
        createdByUserId: 'user456',
        createdAt: '2023-06-21T12:00:00'
      }
    ];

    service.getShortUrls().subscribe((urls: Url[]) => {
      expect(urls).toEqual(mockUrls);
    });

    const req = httpTestingController.expectOne(`${apiUrl}api/url`);
    expect(req.request.method).toBe('GET');
    req.flush(mockUrls);
  });

  it('should remove a URL', () => {
    const urlId = 1;

    service.removeUrl(urlId).subscribe();

    const req = httpTestingController.expectOne(`${apiUrl}api/url/${urlId}`);
    expect(req.request.method).toBe('DELETE');
    req.flush({});
  });

  it('should remove all URLs', () => {
    service.removeAll().subscribe();

    const req = httpTestingController.expectOne(`${apiUrl}api/url`);
    expect(req.request.method).toBe('DELETE');
    req.flush({});
  });

  it('should add a new URL', () => {
    const mockFullUrl = 'https://example.com';
    const mockSuccessResponse = { success: true };

    service.addUrl(mockFullUrl).subscribe((response: any) => {
      expect(response).toEqual(mockSuccessResponse);
    });

    const req = httpTestingController.expectOne(`${apiUrl}api/url/`);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual({ fullUrl: mockFullUrl });
    req.flush(mockSuccessResponse);
  });
});
