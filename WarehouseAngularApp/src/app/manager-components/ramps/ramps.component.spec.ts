import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RampsComponent } from './ramps.component';

describe('RampsComponent', () => {
  let component: RampsComponent;
  let fixture: ComponentFixture<RampsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RampsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RampsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
