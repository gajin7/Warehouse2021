import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-driver-home-page',
  templateUrl: './driver-home-page.component.html',
  styleUrls: ['./driver-home-page.component.css']
})
export class DriverHomePageComponent implements OnInit {
  date : Date | undefined;
  constructor() { }

  ngOnInit(): void {
  }

  getDateAndTime() : void
  {
    this.date = new Date();
  }
   

}
