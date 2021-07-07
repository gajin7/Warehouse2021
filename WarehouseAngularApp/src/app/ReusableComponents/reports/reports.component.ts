import { Component, OnInit } from '@angular/core';
import { DocumentService } from 'src/app/services/document.service';
import { ReportsService } from 'src/app/services/reports.service';

@Component({
  selector: 'app-reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.css']
})
export class ReportsComponent implements OnInit {
  reports : any;
  orderDate = 'asc';
  orderType = 'desc';

  constructor(private reportService : ReportsService, private documetService : DocumentService) { }

  ngOnInit(): void {
    this.getReports();
  }

  getReports()
  {
    this.reportService.getReports('').subscribe((data) => {
      this.reports = data;
    });
  }

  SortByDate()
  {
    this.reportService.getReportsSortByDate('',this.orderDate).subscribe((data) => {
      this.reports = data;
      if(this.orderDate === 'asc')
      {
        this.orderDate = 'desc';
      }
      else
      {
        this.orderDate = 'asc';
      }
    });
  }

  SortByType()
  {
    this.reportService.getReportsSortByType('',this.orderType).subscribe((data) => {
      this.reports = data;
      if(this.orderType === 'asc')
      {
        this.orderType = 'desc';
      }
      else
      {
        this.orderType = 'asc';
      }
    });
  }

  GetReportPdf(reportId : string | undefined): void {
    this.documetService.GetReportPdf(reportId);
  }

}
