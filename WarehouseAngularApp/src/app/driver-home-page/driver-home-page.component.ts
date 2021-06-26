import { Component, OnInit } from '@angular/core';
import { DriversLoad } from '../Models/DriversLoad';
import { Result } from '../Models/Result';
import { TakeLoadByDriverParams } from '../Models/TakeLoadByDriverParams';
import { LoadService } from '../services/load.service';
import { VehicleService } from '../services/vehicle.service';
import { ReceiptService } from '../services/receipt.service';
import { ReportsService } from '../services/reports.service';
import { of, Subscription, timer } from "rxjs";
import { switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-driver-home-page',
  templateUrl: './driver-home-page.component.html',
  styleUrls: ['./driver-home-page.component.css'],
})
export class DriverHomePageComponent implements OnInit {
 vehicle : any;
 vehicles: any
 loads: any;
 driversLoad: DriversLoad;
 minutes: number = 0;
 subscription: Subscription | undefined;
  constructor(private vehicleService : VehicleService, private loadsService : LoadService, private receiptService : ReceiptService,
    private reportService : ReportsService) {
    this.vehicle = [];
    this.loads = [];
    this.driversLoad = new DriversLoad();

   }

  ngOnInit(): void {
    this.getDriversVehicle();
    this.getFreeVehicles();
    this.GetDriversLoad();
    this.getLoads();

    this.minutes =  30 * 1000; // on evry 30 seconds

    this.subscription = timer(0, this.minutes)
      .pipe(
        switchMap(() => {
          return this.loadsService.getUnloadedLoads('')
            .pipe();
        }),
      )
      .subscribe(data => {
        this.loads = data;
      });

  }

  getDriversVehicle()
  {
    this.vehicleService.getDriversVehicle().subscribe((data)=> {
      this.vehicle = data;
      console.log(this.vehicle,"vehicle");
    })
  }

  getFreeVehicles()
  {
    this.vehicleService.getFreeVehicles().subscribe((data) => {
      this.vehicles = data;
      console.log(this.vehicles,"vehicles");
    })
  }

  takeVehicleByDriver(registration : string)
  {
    this.vehicleService.giveVehicleToDriver(registration).subscribe((data) => {
      this.getDriversVehicle();
    })
  }

  freeVehicle(registration: string)
  {
    this.vehicleService.freeVehicle(registration).subscribe((data) => {
      if(data.Success)
      {
        this.vehicle = null;
        window.alert(data.Message);
        this.getLoads();
        this.GetDriversLoad();
        this.getFreeVehicles();
       
      }
    })
  }

  getLoads()
  {
    this.loadsService.getUnloadedLoads('').subscribe((data) => {
      this.loads = data;
    })
  }

  takeLoad(id: string, rampFree: string)
  {
    if(rampFree === 'false')
    {
      window.alert('Ramp is taken. Please wait for ramp to get free and try again')
    }
    if(window.confirm('Are you sure you want to take the load ' + id + '?'))
    {
    var params = new TakeLoadByDriverParams();
    params.Email = sessionStorage['email'];
    params.LoadId = id;
    this.loadsService.takeLoadByDriver(params).subscribe(data =>
      {
        var result = data as Result;
        window.alert(result.Message);
        this.GetDriversLoad();
      });
    }
  }

  GetDriversLoad()
  {
    this.loadsService.getDriversLoad(sessionStorage['email']).subscribe(data => {
      if(data != null)
      {
        this.driversLoad.Id = data.Id;
        this.driversLoad.Ramp = data.Ramp;
        this.driversLoad.Warehouse = data.Warehouse;
        this.driversLoad.ReceiptId = data.ReceiptId;
        this.driversLoad.ReportId = data.ReportId;
      }
    });
  }

  LoadFinished(receiptId : string | undefined)
  {
    var params = new TakeLoadByDriverParams();
    params.LoadId = this.driversLoad.Id;
    this.loadsService.finishLoading(params).subscribe(data =>
      {
        window.alert(data.Message);
        this.GetReceiptPdf(receiptId);
        this.driversLoad.Id = '';
        this.getLoads();
      });
  }

  GetReceiptPdf(receiptId : string | undefined): void {
    this.receiptService.getReceiptFile(receiptId)
        .subscribe(x => {
            // It is necessary to create a new blob object with mime-type explicitly set
            // otherwise only Chrome works like it should
            var newBlob = new Blob([x], { type: "application/pdf" });

            // IE doesn't allow using a blob object directly as link href
            // instead it is necessary to use msSaveOrOpenBlob
            if (window.navigator && window.navigator.msSaveOrOpenBlob) {
                window.navigator.msSaveOrOpenBlob(newBlob);
                return;
            }

            // For other browsers: 
            // Create a link pointing to the ObjectURL containing the blob.
            const data = window.URL.createObjectURL(newBlob);

            var link = document.createElement('a');
            link.href = data;
            link.download = "Receipt" + receiptId + ".pdf";
            // this is necessary as link.click() does not work on the latest firefox
            link.dispatchEvent(new MouseEvent('click', { bubbles: true, cancelable: true, view: window }));

            setTimeout(function () {
                // For Firefox it is necessary to delay revoking the ObjectURL
                window.URL.revokeObjectURL(data);
                link.remove();
            }, 100);

        });
}

GetReportPdf(reportId : string | undefined): void {
  this.reportService.getReportFile(reportId)
      .subscribe(x => {
          // It is necessary to create a new blob object with mime-type explicitly set
          // otherwise only Chrome works like it should
          var newBlob = new Blob([x], { type: "application/pdf" });

          // IE doesn't allow using a blob object directly as link href
          // instead it is necessary to use msSaveOrOpenBlob
          if (window.navigator && window.navigator.msSaveOrOpenBlob) {
              window.navigator.msSaveOrOpenBlob(newBlob);
              return;
          }

          // For other browsers: 
          // Create a link pointing to the ObjectURL containing the blob.
          const data = window.URL.createObjectURL(newBlob);

          var link = document.createElement('a');
          link.href = data;
          link.download = "Report" + reportId + ".pdf";
          // this is necessary as link.click() does not work on the latest firefox
          link.dispatchEvent(new MouseEvent('click', { bubbles: true, cancelable: true, view: window }));

          setTimeout(function () {
              // For Firefox it is necessary to delay revoking the ObjectURL
              window.URL.revokeObjectURL(data);
              link.remove();
          }, 100);

      });
}
   

}
