import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-default-homepage',
  templateUrl: './default-homepage.component.html',
  styleUrls: ['./default-homepage.component.css']
})
export class DefaultHomepageComponent implements OnInit {
  images = [944, 1011, 984].map((n) => `https://picsum.photos/id/${n}/900/500`);
  constructor() { }

  ngOnInit(): void {
  }

}
