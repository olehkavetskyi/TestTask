import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute, convertToParamMap } from '@angular/router';
import { of } from 'rxjs';
import { UrlInfoComponent } from './url-info.component';
import { UrlInfoService } from './url-info.service';
import { Url } from '../shared/models/Url';
import { environment } from 'src/environments/environment';

describe('UrlInfoComponent', () => {
  let component: UrlInfoComponent;
  let fixture: ComponentFixture<UrlInfoComponent>;
  let urlInfoService: UrlInfoService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UrlInfoComponent],
      providers: [
        {
          provide: ActivatedRoute,
          useValue: {
            paramMap: of(convertToParamMap({ id: '1' }))
          }
        },
        {
          provide: UrlInfoService,
          useValue: {
            getUrl: jasmine.createSpy('getUrl').and.returnValue(of({ id: 1, shortUrl: 'abc123', fullUrl: 'https://example.com' } as Url))
          }
        }
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UrlInfoComponent);
    component = fixture.componentInstance;
    urlInfoService = TestBed.inject(UrlInfoService);
  });

  it('should fetch the URL details on component initialization', () => {
    fixture.detectChanges();

    expect(urlInfoService.getUrl).toHaveBeenCalledWith(1);
    expect(component.url).toEqual({ id: 1, shortUrl: 'abc123', fullUrl: 'https://example.com' } as Url);
    expect(component.url?.shortUrl).toBe(environment.apiUrl + 'r/abc123');
  });
});
