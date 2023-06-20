import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { RedirectComponent } from './redirect.component';
import { RedirectService } from './redirect.service';
import { of, throwError } from 'rxjs';

describe('RedirectComponent', () => {
  let component: RedirectComponent;
  let fixture: ComponentFixture<RedirectComponent>;
  let redirectService: RedirectService;
  let locationSpy: jasmine.SpyObj<Location>;

  beforeEach(async () => {
    locationSpy = jasmine.createSpyObj<Location>('Location', ['assign']);
    
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      declarations: [RedirectComponent],
      providers: [
        RedirectService,
        { provide: Location, useValue: locationSpy }
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RedirectComponent);
    component = fixture.componentInstance;
    redirectService = TestBed.inject(RedirectService);
    locationSpy.assign.calls.reset();
  });

  it('should redirect to the full URL when the short URL is valid', fakeAsync(() => {
    const mockResponse = { fullUrl: 'https://example.com' };
    spyOn(redirectService, 'getFullUrl').and.returnValue(of(mockResponse));

    component['redirectToFullUrl']('abc123');
    tick();

    expect(locationSpy.assign).toHaveBeenCalledWith(mockResponse.fullUrl);
  }));

  it('should handle error when retrieving full URL', fakeAsync(() => {
    const mockError = 'Failed to retrieve full URL';
    spyOn(redirectService, 'getFullUrl').and.returnValue(throwError(mockError));
    spyOn(console, 'error');

    component['redirectToFullUrl']('invalidUrl');
    tick();

    expect(console.error).toHaveBeenCalledWith('Failed to retrieve full URL:', mockError);
    expect(locationSpy.assign).not.toHaveBeenCalled();
  }));
});
