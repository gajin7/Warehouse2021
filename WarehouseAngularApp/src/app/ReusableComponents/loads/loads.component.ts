import { Component, OnInit } from '@angular/core';
import { DocumentService } from 'src/app/services/document.service';
import { LoadService } from 'src/app/services/load.service';

@Component({
  selector: 'app-loads',
  templateUrl: './loads.component.html',
  styleUrls: ['./loads.component.css']
})
export class LoadsComponent implements OnInit {
  loads: any;
  loadTypes = ['Unloaded','Loaded','Loading'];
  currentLoadType : string | undefined = 'Unloaded';
  loadKeyWord : string = '';
  constructor(private loadService : LoadService, private documentService : DocumentService) { }

  ngOnInit(): void {
    this.getLoad();
  }

  getUnloadedLoads()
  {
    this.loadService.getUnloadedLoads('all').subscribe((data) => {
      this.loads = data;
    })
  }

  getLoadedLoads()
  {
    this.loadService.getLoadedLoads('all').subscribe((data) => {
      this.loads = data;
    })
  }

  getLoadingLoads()
  {
    this.loadService.getLoadingLoads('all').subscribe((data) => {
      this.loads = data;
    })
  }

  getLoad()
  {
    if(this.currentLoadType === 'Unloaded')
    {
      this.getUnloadedLoads();
    }
    if(this.currentLoadType === 'Loaded')
    {
      this.getLoadedLoads();
    }
    if(this.currentLoadType === 'Loading')
    {
      this.getLoadingLoads();
    }
  }

  changeLoadType (event: any) {
   
    this.currentLoadType = event.target.value;
    this.getLoad();
    
  }

  GetReceiptPdf(receiptId : string): void {
    this.documentService.GetReceiptPdf(receiptId);
}

LoadSearch()
{
  this.loadService.getLoadsById(this.loadKeyWord).subscribe((data) => {
    this.loads = data;
  });
}

clearLoadSearch()
{
  this.loadKeyWord = '';
  this.getLoad()
}

updateLoadSearch(e : any) {
  this.loadKeyWord = e.target.value; 
}

}
