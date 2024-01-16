import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListAndRegistersComponent } from './list-and-registers.component';

describe('ListAndRegistersComponent', () => {
  let component: ListAndRegistersComponent;
  let fixture: ComponentFixture<ListAndRegistersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ListAndRegistersComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListAndRegistersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
