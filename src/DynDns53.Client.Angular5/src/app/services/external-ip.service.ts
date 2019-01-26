import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { catchError, map, tap } from 'rxjs/operators';
import 'rxjs/add/operator/map';

@Injectable()
export class ExternalIpService {

  constructor(private http: HttpClient) { }

  readonly url: string = 'https://46ebv5trsk.execute-api.eu-west-2.amazonaws.com/prod';

  getIP(): Observable<any> {
    return this.http.get(this.url);
  }
}
