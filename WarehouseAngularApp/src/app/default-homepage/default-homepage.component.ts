import { Component, OnInit } from '@angular/core';
import { Subscription, timer } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { NumberOfOrdersService } from '../services/numberOfOrders.service';

@Component({
  selector: 'app-default-homepage',
  templateUrl: './default-homepage.component.html',
  styleUrls: ['./default-homepage.component.css']
})
export class DefaultHomepageComponent implements OnInit {
  images = [944, 1011, 984].map((n) => `https://picsum.photos/id/${n}/900/500`);
  numOfOrdersToday : string = '';
  numOfOrdersAllTime : string = ''
  minutes: number = 0;
  subscription: Subscription | undefined;
  constructor(private numberOfOrdersService : NumberOfOrdersService) { }

  ngOnInit(): void {
    this.scheduleNumberOfOrdersAllTIme();
    this.scheduleNumberOfOrdersToday();
  }

  scheduleNumberOfOrdersAllTIme()
  {
    this.minutes =  5 * 1000; // on evry 30 seconds

    this.subscription = timer(0, this.minutes)
      .pipe(
        switchMap(() => {
          return this.numberOfOrdersService.getNumberOfOrdersAllTime()
            .pipe();
        }),
      )
      .subscribe(data => {
        this.numOfOrdersAllTime = data as string;
      });
  }

  scheduleNumberOfOrdersToday()
  {
    this.minutes =  5 * 1000; // on evry 30 seconds

    this.subscription = timer(0, this.minutes)
      .pipe(
        switchMap(() => {
          return this.numberOfOrdersService.getNumberOfOrdersToday()
            .pipe();
        }),
      )
      .subscribe(data => {
        this.numOfOrdersToday = data as string;
      });
  }
}
