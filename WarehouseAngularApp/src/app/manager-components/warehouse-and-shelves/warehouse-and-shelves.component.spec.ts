import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseAndShelvesComponent } from './warehouse-and-shelves.component';

describe('WarehouseAndShelvesComponent', () => {
  let component: WarehouseAndShelvesComponent;
  let fixture: ComponentFixture<WarehouseAndShelvesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseAndShelvesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WarehouseAndShelvesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
