import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StorekeeperHomePageComponent } from './storekeeper-home-page.component';

describe('StorekeeperHomePageComponent', () => {
  let component: StorekeeperHomePageComponent;
  let fixture: ComponentFixture<StorekeeperHomePageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StorekeeperHomePageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StorekeeperHomePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
