import { ComponentFixture, TestBed } from '@angular/core/testing';
import { UrlsTableComponent } from './urls-table.component';
import { UrlsTableService } from './urls-table.service';
import { ToastrService } from 'ngx-toastr';
import { of, throwError } from 'rxjs';
import { Url } from '../shared/models/Url';
import { User, Roles } from '../shared/models/User';
import { environment } from 'src/environments/environment';

describe('UrlsTableComponent', () => {
  let component: UrlsTableComponent;
  let fixture: ComponentFixture<UrlsTableComponent>;
  let urlsTableService: UrlsTableService;
  let toastrService: ToastrService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UrlsTableComponent],
      providers: [UrlsTableService, ToastrService]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UrlsTableComponent);
    component = fixture.componentInstance;
    urlsTableService = TestBed.inject(UrlsTableService);
    toastrService = TestBed.inject(ToastrService);
  });

  it('should initialize component and fetch current user and URLs', () => {
    const mockCurrentUser: User = {
      id: 'user123',
      email: 'john.doe@example.com',
      role: Roles.Admin
    };
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

    spyOn(urlsTableService, 'getCurrentUser').and.returnValue(of(mockCurrentUser));
    spyOn(urlsTableService, 'getShortUrls').and.returnValue(of(mockUrls));

    fixture.detectChanges();

    expect(component.currentUser).toEqual(mockCurrentUser);
    expect(component.urls).toEqual(mockUrls);
    expect(urlsTableService.getCurrentUser).toHaveBeenCalled();
    expect(urlsTableService.getShortUrls).toHaveBeenCalled();
  });

  it('should add a new URL', () => {
    const mockFullUrl = 'https://example.com';
    const mockSuccessResponse = { success: true };

    spyOn(urlsTableService, 'addUrl').and.returnValue(of(mockSuccessResponse));
    spyOn(component, 'getUrls');

    component.fullUrl = mockFullUrl;
    component.addUrl();

    expect(urlsTableService.addUrl).toHaveBeenCalledWith(mockFullUrl);
    expect(component.getUrls).toHaveBeenCalled();
  });

  it('should handle error when adding a duplicate URL', () => {
    const mockFullUrl = 'https://example.com';
    const mockErrorMessage = 'This url is already registered!';

    spyOn(urlsTableService, 'addUrl').and.returnValue(throwError(mockErrorMessage));
    spyOn(toastrService, 'error');

    component.fullUrl = mockFullUrl;
    component.addUrl();

    expect(urlsTableService.addUrl).toHaveBeenCalledWith(mockFullUrl);
    expect(toastrService.error).toHaveBeenCalledWith(mockErrorMessage);
  });

  it('should remove a URL', () => {
    const mockUrl: Url = {
      id: 1,
      shortUrl: 'abc123',
      fullUrl: 'https://example.com',
      createdByUserId: 'user123',
      createdAt: '2023-06-20T10:00:00'
    };

    spyOn(urlsTableService, 'removeUrl').and.returnValue(of(null));

    component.urls = [mockUrl];
    component.remove(mockUrl.id);

    expect(urlsTableService.removeUrl).toHaveBeenCalledWith(mockUrl.id);
    expect(component.urls.length).toBe(0);
  });

  it('should remove all URLs', () => {
    spyOn(urlsTableService, 'removeAll').and.returnValue(of(null));

    component.urls = [
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
    component.removeAll();

    expect(urlsTableService.removeAll).toHaveBeenCalled();
    expect(component.urls.length).toBe(0);
  });

  it('should determine if user is an admin', () => {
    component.currentUser = { id: 'user123', email: 'john.doe@example.com', role: Roles.Admin };

    expect(component.isAdmin()).toBe(true);

    component.currentUser.role = Roles.Regular;

    expect(component.isAdmin()).toBe(false);

    component.currentUser = undefined;

    expect(component.isAdmin()).toBe(false);
  });

  it('should determine if a URL can be removed', () => {
    const adminUser: User = { id: 'user123', email: 'john.doe@example.com', role: Roles.Admin };
    const regularUser: User = { id: 'user456', email: 'jane.doe@example.com', role: Roles.Regular };
    const mockUrl: Url = {
      id: 1,
      shortUrl: 'abc123',
      fullUrl: 'https://example.com',
      createdByUserId: 'user123',
      createdAt: '2023-06-20T10:00:00'
    };

    component.currentUser = adminUser;
    expect(component.ableToRemove(mockUrl)).toBe(true);

    component.currentUser = regularUser;
    expect(component.ableToRemove(mockUrl)).toBe(false);

    component.currentUser = undefined;
    expect(component.ableToRemove(mockUrl)).toBe(false);

    mockUrl.createdByUserId = 'user456';
    component.currentUser = regularUser;
    expect(component.ableToRemove(mockUrl)).toBe(true);

    component.currentUser = adminUser;
    expect(component.ableToRemove(mockUrl)).toBe(true);
  });

  it('should fetch URLs', () => {
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

    spyOn(urlsTableService, 'getShortUrls').and.returnValue(of(mockUrls));

    component.getUrls();

    expect(urlsTableService.getShortUrls).toHaveBeenCalled();
    expect(component.urls).toEqual(mockUrls);
  });
});
