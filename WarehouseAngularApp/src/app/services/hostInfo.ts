import { HttpHeaders } from "@angular/common/http";

export class HostInfo {
    defaultHostAddress: string = "http://localhost:64334";
    apiPrefix : string = "/api";
    tokenPath : string = "/oauth/token";
    warehouseController : string = "/warehouse";
    shelvesController : string = "/shelves";
    itemsController : string = "/item";
    receiptController : string = "/receipt";
    reportController : string = "/report";
    vehicleController : string = "/vehicle";
    loadController : string = "/load";
    userController : string = "/user";
    comapnyController : string = "/company";
    rampController : string = "/ramp";
    httpOptionsJson = {
      headers: new HttpHeaders({'Content-Type': 'application/json'})
    }
  }