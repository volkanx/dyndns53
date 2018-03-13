import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { catchError, map, tap } from 'rxjs/operators';
import 'rxjs/add/operator/map';

@Injectable()
export class ExternalIpService {

  constructor(private http: HttpClient) { }

  // readonly url: string = 'http://checkip.myvirtualhome.net/';
  // readonly url: string = 'http://checkip.amazonaws.com';
  // readonly url: string = 'https://67ml6xrmha.execute-api.eu-west-1.amazonaws.com/dev';
  readonly url: string = 'https://8x6c5h83d9.execute-api.eu-west-2.amazonaws.com/prod';

  getIP(): Observable<any> {
    return this.http.get(this.url);
  }
}
