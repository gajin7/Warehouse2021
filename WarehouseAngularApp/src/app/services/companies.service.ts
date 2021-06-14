import { Host, Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';

import { Observable, pipe, of } from 'rxjs';
import { HostInfo } from '../services/hostInfo';
import { NewItem } from '../Models/NewItem';
import { Company } from '../Models/Company';


@Injectable({
  providedIn: 'root',
})
export class CompaniesService {
  isLoggedin = false;
  hostInfo : HostInfo = new HostInfo();


  constructor(private http: HttpClient) { }

  getCompanies()
  {
    return this.http.get(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.comapnyController + '/getCompanies',{ 'headers': { 'Access-Control-Allow-Origin': 'http://localhost:4200' } });
  }

  addCompany(company : Company)
  {
    return this.http.post(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.comapnyController + '/addCompany',company,{ 'headers': { 'Access-Control-Allow-Origin': 'http://localhost:4200' } });
  }

  changeCompany(company : Company)
  {
    return this.http.post(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.comapnyController + '/changeCompany',company,{ 'headers': { 'Access-Control-Allow-Origin': 'http://localhost:4200' } });
  }

  deleteCompany(pib : string | undefined)
  {
    return this.http.delete(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix + this.hostInfo.comapnyController + '/removeCompany?companyPib=' + pib,{ 'headers': { 'Access-Control-Allow-Origin': 'http://localhost:4200' } });
  }

  getAllCompaniesSearch(keyWord: string | undefined): Observable<any> {
    return this.http.get(this.hostInfo.defaultHostAddress + this.hostInfo.apiPrefix +
       this.hostInfo.comapnyController + '/getAllCompaniesSearch?keyWord=' + keyWord);
  }

  private handle(error: any) {
    return of (error.message);
  }
}
