import { Injectable } from '@angular/core';

@Injectable()
export class LocalStorageService {

  constructor() { }

  setData(key, val): void {
    if (window.localStorage) {
      window.localStorage.setItem(key, val);
    }
  }

  getData(key): string {
    if (window.localStorage) {
      return window.localStorage.getItem(key);
    }
    return null;
  }

}
