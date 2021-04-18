import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManagerHomePageComponent } from './manager-home-page.component';

describe('ManagerHomePageComponent', () => {
  let component: ManagerHomePageComponent;
  let fixture: ComponentFixture<ManagerHomePageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ManagerHomePageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ManagerHomePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
